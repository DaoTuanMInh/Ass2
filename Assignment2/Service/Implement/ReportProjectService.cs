using Assignment2.Common;
using Assignment2.DTOs;
using Assignment2.Models;
using Assignment2.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Service.Implement
{
    public class ReportProjectService : IReportProjectService
    {
        private readonly Ass2Context _db;
        public ReportProjectService(Ass2Context db)
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
        public async Task<respone<ReportProjectDto>> ReportProject(int minItems)
        {
            var countdata = await _db.Projects
                .Select(p => new 
                {
                    ProjectCode = p.Code,
                    ProjectName = p.Name,
                    TotalWorkItems = p.WorkItems
                    .Count(p => !p.IsDeleted),
                    OpenWorkItems = p.WorkItems.Count(p => !p.IsDeleted && p.Status != "Done" && p.Status != "Cancelled"),
                    OverdueItems = p.WorkItems.Count(p => !p.IsDeleted && p.Status != "Done" && p.Status != "Cancelled" && p.DueAt != null && p.DueAt < DateTime.UtcNow),
                    DoneItems = p.WorkItems.Count(p => !p.IsDeleted && p.Status == "Done" && p.CompletedAt != null),
                    TimeDone = p.WorkItems
                        .Where(w => !w.IsDeleted && w.Status == "Done" && w.CompletedAt != null)
                        .Select(w => new 
                        { 
                            w.CreatedAt, 
                            w.CompletedAt 
                        })
                        .ToList()
                })
                .Where(p => p.TotalWorkItems >= minItems)
                .ToListAsync();
            var Data = countdata.Select(p => new ReportProjectDto
            {
                ProjectCode = p.ProjectCode,
                ProjectName = p.ProjectName,
                TotalItems = p.TotalWorkItems,
                OpenItems = p.OpenWorkItems,
                OverdueItems = p.OverdueItems,
                DoneItems = p.DoneItems,
                AverageCompletionHours = p.TimeDone.Count > 0 ?
                                             // lấy giá tị date time            đổi sang giờ
                    p.TimeDone.Average(w => (w.CompletedAt!.Value - w.CreatedAt).TotalHours) : 0
            }).ToList();

            var response = new respone<ReportProjectDto>
            {
                TraceId = Guid.NewGuid().ToString(),
                Status = 200,
                Message = "Thành công",
                Data = Data
            };
            return response;
        }
    }
}
