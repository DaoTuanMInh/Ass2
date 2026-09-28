using Assignment2.DTOs;
using Assignment2.Models;
using Assignment2.Service.Interface;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
namespace Assignment2.Service.Interface;

public class WorkItemService : IWorkItemService
{
    private readonly Ass2Context _db;
    public WorkItemService(Ass2Context db)
    {
        _db = db;
    }
    public async Task<bool> HealChecK()
    {
        try
        {
            return await _db.WorkItems.AnyAsync();
        }
        catch
        {
            return false;
        }
    }
    public async Task<bool> Delete(long id)
    {
        var workitemID = await _db.WorkItems
            .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        if(workitemID == null)
        {
            return false;
        }
        workitemID.IsDeleted = true;
        workitemID.DeletedAt = DateTime.Now;
        workitemID.UpdatedAt = DateTime.Now;
            
        await _db.SaveChangesAsync();
        return true;


    }
  
    public async Task<WorkItemDto> AddWorkItem (AddWorkItem workItem)
    {
        var projectcode = await _db.Projects.Select(p =>p.Code).FirstOrDefaultAsync(p => p == workItem.ProjectCode);
        if (projectcode == null)
        {
            throw new Exception("Không thấy projectcode");
        }

        var assigneeId = await _db.Developers.Select(u => u.Id).FirstOrDefaultAsync(u => u == workItem.AssigneeId);
        if (assigneeId == null)
        {
            throw new Exception("Không thấy assignee");

        }
        using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            
            var Inputlabels = workItem.Labels
                .Select(x => x.Trim().ToLower())
                .Distinct()
                .ToArray();

            var labe = _db.Labels
                .Where(l => Inputlabels.Contains(l.Name.ToLower()))
                .Select(l => l.Id)
                .ToList();

            var getProducId = _db.Projects
                .Where(p => p.Code == workItem.ProjectCode)
                .Select(p => p.Id)
                .FirstOrDefault();

            var count = _db.WorkItems
                .AsNoTracking()
                .Count() + 1;


            var newWorkItem = new WorkItem
            {
                Code = $"WI-{DateTime.Now.Year}-{count.ToString("D6")}",
                Title = workItem.Title,
                Description = workItem.Description, 
                Status = "Todo",
                Priority = workItem.Priority,
                ProjectId = getProducId,
                AssigneeId = assigneeId,
                DueAt = workItem.DueAt,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                CompletedAt = null,
                IsDeleted = false,
                DeletedAt = null,
            };
            _db.WorkItems.Add(newWorkItem);
            await _db.SaveChangesAsync();

            List<WorkItemLabel> workItemLabels = labe
                .Select(
                    x => new WorkItemLabel()
                    {   
                        LabelId = x,
                        WorkItemId = newWorkItem.Id
                    }
                )
                .ToList();

            _db.WorkItemLabels.AddRange(workItemLabels);
            await _db.SaveChangesAsync();

            var a = new WorkItemDto()
            { 
                Id = newWorkItem.Id,
                Code = newWorkItem.Code,
                Title = newWorkItem.Title,
                Description = newWorkItem.Description,
                Status = newWorkItem.Status,
                Priority = newWorkItem.Priority,
                ProjectId = newWorkItem.ProjectId,
                AssigneeId = newWorkItem.AssigneeId,
                DueAt = newWorkItem.DueAt,
                CreatedAt = newWorkItem.CreatedAt,
                UpdatedAt = newWorkItem.UpdatedAt,
                CompletedAt = newWorkItem.CompletedAt,
                IsDeleted = newWorkItem.IsDeleted,
                DeletedAt = newWorkItem.DeletedAt,
            };

            await transaction.CommitAsync();

            return a;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Lỗi khi thêm work item: {ex.Message}");
        }

    }

    public async Task<ItemDetailsDto> GetItemDetails(long id)
    { 
        var items = await _db.WorkItems.FirstOrDefaultAsync(w => w.Id == id);
        if(items == null)
        {
            throw new InvalidOperationException("Item khong ton tai");
        }

        var itemDetails = new WorkitemDetails
        {
            Id = items.Id,
            Code = items.Code,
            Title = items.Title,
            Description = items.Description,
            Status = items.Status,
            priority = items.Priority,
            DueAt = items.DueAt,
            CreatedAt = items.CreatedAt,
            UpdatedAt = items.UpdatedAt,
            CompletedAt = items.CompletedAt
        };
        var project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == items.ProjectId);
        var projectdetails = new ProjectDetails
        {
            Code = project.Code,
            Name = project.Name
        };
        var assignee = await _db.Developers.FirstOrDefaultAsync(a => a.Id == items.AssigneeId);
        var assigneeDetails = new AssigneeDetail
        {
            Id = assignee.Id,
            Code = assignee.Code,
            FullName = assignee.FullName
        };

        var labels =  _db.Labels
            //.Where(x => x.WorkItems.Any(w => w.Id == id))
            .Select(x => x.Name)
            .OrderBy(name => name)
            .ToList();

        var history = _db.WorkItemHistories
            .Where(h => h.WorkItemId == id)
            .OrderBy(h => h.CreatedAt)
            .ThenBy(h => h.Id)
            .Select(h => new HistoryDetails
            {
            FromStatus = h.FromStatus,
            ToStatus = h.ToStatus,
            Note = h.Note,
            ChangedBy = h.ChangedBy,
            CreatedAt = h.CreatedAt
            })
            .ToList();

        var res = new ItemDetailsDto
        {
            InforItems  = new List<WorkitemDetails> { itemDetails },
            Projects = new List<ProjectDetails> { projectdetails },
            Assignee = new List<AssigneeDetail> { assigneeDetails },
            Labels = labels.Select(label => new Label
            { 
                Name = label 
            })
            .ToList(),
            History = history,
        };
        return res;


    }
    public async Task<bool> AssigItem(long id, AssigneeItem assigneeItem)
    {
        var workitem = await _db.WorkItems.FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted);
        if (workitem == null)
        {
            throw new InvalidOperationException("Item khong ton tai");
        }
        var assignee = await _db.Developers.FirstOrDefaultAsync(a => a.Id == assigneeItem.AssigneeId);
        if (assignee == null)
        {
            throw new InvalidOperationException("Nhan vien khong ton tai");
        }

        workitem.AssigneeId = assigneeItem.AssigneeId;
        workitem.UpdatedAt = DateTime.Now;

        var history = new WorkItemHistory
        {
            WorkItemId = workitem.Id,
            FromStatus = workitem.Status,
            ToStatus = workitem.Status,
            Note = string.IsNullOrWhiteSpace(assigneeItem.Note) ? null : assigneeItem.Note,
            ChangedBy = assignee.Code,
            CreatedAt = DateTime.Now
        };
        _db.WorkItemHistories.Add(history);
        await _db.SaveChangesAsync();

        return true;

    }

    public async Task<object> GetWorkItemsList(WorkItemFilterDto filter)
    {
        var query = _db.WorkItems
            .Include(w => w.Project) // lay thong tin project
            .Include(w => w.Assignee)
            .Where(w => !w.IsDeleted);
        if (!string.IsNullOrWhiteSpace(filter.keyword))
            query = query.Where(w => w.Title
            .ToLower()
            //Contains: tìm item theo keyword nhap vao
            .Contains(filter.keyword.Trim().ToLower()));
        if (!string.IsNullOrWhiteSpace(filter.status))
        {
            var statuses = filter.status
                .Split(',') // cat chuoi thanh mang cac trang thai
                .Select(s => s.Trim()) // xoa khoang trang o dau va cuoi
                .ToList();
            query = query.Where(w => statuses.Contains(w.Status));
        }
        if (!string.IsNullOrWhiteSpace(filter.priority))
            query = query.Where(w => w.Priority == filter.priority.Trim());
        if (!string.IsNullOrWhiteSpace(filter.projectCode))
            query = query.Where(w => w.Project.Code == filter.projectCode.Trim());
        if (filter.assigneeId.HasValue)
            query = query.Where(w => w.AssigneeId == filter.assigneeId.Value);
        if (filter.overdue)
        {
            var now = DateTime.Now;
            query = query.Where(w => w.DueAt != null && w.DueAt < now && w.Status != "Done" && w.Status != "Cancelled");
        }

        var total = await query.CountAsync();

        string sortRaw = string.IsNullOrWhiteSpace(filter.sort) ? "-createdAt" : filter.sort;
        bool isDesc = sortRaw.StartsWith("-");
        string sortField = isDesc ? sortRaw.Substring(1) : sortRaw;
        if (sortField == "priority")
        {
            if (isDesc)
                query = query.OrderByDescending(w => w.Priority == "Urgent" ? 4 : w.Priority == "High" ? 3 : w.Priority == "Normal" ? 2 : 1).ThenBy(w => w.Id);
            else
                query = query.OrderBy(w => w.Priority == "Urgent" ? 4 : w.Priority == "High" ? 3 : w.Priority == "Normal" ? 2 : 1).ThenBy(w => w.Id);
        }
        else if (sortField == "dueAt")
        {
            if (isDesc)
                query = query.OrderBy(w => w.DueAt == null ? 1 : 0).ThenByDescending(w => w.DueAt).ThenBy(w => w.Id);
            else
                query = query.OrderBy(w => w.DueAt == null ? 1 : 0).ThenBy(w => w.DueAt).ThenBy(w => w.Id);
        }
        else
        {
            if (isDesc) query = query.OrderByDescending(w => w.CreatedAt).ThenBy(w => w.Id);
            else query = query.OrderBy(w => w.CreatedAt).ThenBy(w => w.Id);
        }

        var items = await query
            .Skip((filter.page - 1) * filter.pageSize)
            .Take(filter.pageSize)
            .Select(w => new
            {
                id = w.Id,
                code = w.Code,
                title = w.Title,
                status = w.Status,
                priority = w.Priority,
                projectCode = w.Project.Code,
                projectName = w.Project.Name,
                assigneeId = w.AssigneeId,
                assigneeName = w.Assignee != null ? w.Assignee.FullName : null,
                dueAt = w.DueAt,
                createdAt = w.CreatedAt,
                updatedAt = w.UpdatedAt,
                labels = w.WorkItemLabels.Select(l => l.label.Name).OrderBy(n => n).ToList()
            })
            .ToListAsync();
        return new
        {
            page = filter.page,
            pageSize = filter.pageSize,
            total = total,
            items = items
        };
    }
}
