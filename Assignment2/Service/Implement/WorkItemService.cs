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
    public static string FormatId(int id)
    {
        return id.ToString("D6");
    }
    public async Task<WorkItem> AddWorkItem(AddWorkItem workItem)
    {
        //var Inputproject = workItem.ProjectCode.Select(x => x.ToUpper()).Distinct().ToList() ?? new List<string>();
        var projectcode = await _db.Projects.FirstOrDefaultAsync(p => p.Code == workItem.ProjectCode);
        if (projectcode == null)
        {
            throw new Exception("Không thấy projectcode");
        }

        var assigneeId = await _db.Developers.FirstOrDefaultAsync(u => u.Id == workItem.AssigneeId);
        if (assigneeId == null)
        {
            throw new Exception("Không thấy assignee");

        }
        var Inputlabels = workItem.Labels.Select(x => x.ToUpper()).Distinct().ToList() ?? new List<string>();
        foreach (var label in Inputlabels)
        {
            var labels = await _db.Labels.FirstOrDefaultAsync(t => t.Name.ToUpper() == label);
            if (labels == null)
            {
                labels = new Label
                { 
                    Name = label 
                };
                _db.Labels.Add(labels);
                await _db.SaveChangesAsync();
            }
        }
        await _db.SaveChangesAsync();

        var newWorkItem = new AddWorkItem
        {
            Title = workItem.Title,
            Description = workItem.Description,
            ProjectCode = projectcode.Code,
            AssigneeId = assigneeId.Id,
            Priority = workItem.Priority,
            DueAt = DateTime.Now,
            Labels = Inputlabels
        };
        _db.Add(newWorkItem);
        await _db.SaveChangesAsync();

        var WorkItem = new WorkItem
        {
            Code = "WT-" + DateTime.Now.Year,

            
        };



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
            .Where(x => x.WorkItems.Any(w => w.Id == id))
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
}
