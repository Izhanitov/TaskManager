using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.BLL.DataTransferObjects;

namespace TaskManager.BLL.Interfaces
{
    public interface ITransferService
    {
        Task<List<TaskRecordDTO>> GetTaskTreeById(int taskId);
        Task<TaskRecordDTO> GetTaskById(int taskId);

        Task<List<AllTasksDTO>> GetTaskList();

        Task SetTask(object newTask);
        Task DeleteTask(int id);
        Task UpdateTask(object updateTask);

        Task<bool> UpdateTaskStatus(object taskStatus);      
    
    }
}
