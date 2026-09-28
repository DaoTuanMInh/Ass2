using Assignment2.Common;
using Assignment2.DTOs;

namespace Assignment2.Service.Interface
{
    public interface IReportProjectService
    {
        public Task<respone<ReportProjectDto>> ReportProject(int minItems);
    }
}
