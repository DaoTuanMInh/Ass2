using Assignment2.DTOs;
using Assignment2.Models;

namespace Assignment2.Service.Interface
{
    public interface IWorkItemService
    {
        public Task<bool> HealChecK();
        public Task<WorkItem> AddWorkItem(AddWorkItem workItem);
        public Task<bool> Delete(long id);

        public Task<ItemDetailsDto> GetItemDetails(long id);

    }
}
