using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<List<TaskRecord>> GetTaskTreeById(int id);       

        Task<List<TaskRecord>> GetAllTasks();

        Task AddNewTask(string name, string desc, string performer, int estimate, int? parentId);

        Task DeleteTask(int id);

        Task UpdateTask(int id, string name, string desc, string performer, int estimate, int? factualEstimate);

        Task UpdateTaskStatus(int taskId, int statusId, int? factualEstimate);
    }
}
