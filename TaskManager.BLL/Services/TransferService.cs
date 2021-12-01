using System;
using System.Collections.Generic;
using TaskManager.BLL.DataTransferObjects;
using Newtonsoft.Json;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using System.Threading.Tasks;

namespace TaskManager.BLL.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITaskRepository taskRepository;
                
        public TransferService(ITaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<List<TaskRecordDTO>> GetTaskTreeById (int taskId)
        {
            List<TaskRecordDTO> taskTree = new List<TaskRecordDTO>();

            var tasks = await taskRepository.GetTaskTreeById(taskId);

            if (tasks.Count == 0)
                throw new Exception("Non found resource");

            MapperService.MapTaskTreeDALToDTO(tasks, taskTree);
            
            CountService.CalcTreeEstimates(taskTree);

            return taskTree;
        }

        public async Task<TaskRecordDTO> GetTaskById (int taskId)
        {
            var taskTree = await GetTaskTreeById(taskId);

            if (taskTree.Count == 0)
                throw new Exception("Non found resource");

            TaskRecordDTO taskRecord = taskTree.Find(elem => elem.TaskID == taskId);

            return taskRecord;
        }
        
        public async Task<List<AllTasksDTO>> GetTaskList ()
        {
            List<AllTasksDTO> taskList = new List<AllTasksDTO>();

            var tasks = await taskRepository.GetAllTasks();

            if (tasks.Count == 0)
                throw new Exception("Non found resource");

            MapperService.MapAllTasksList(tasks, taskList);

            return taskList;
        }

        public async Task SetTask (object newTask)
        {
            dynamic responce = JsonConvert.DeserializeObject(newTask.ToString());

            string name = responce.name;
            string desc = responce.desc;
            string performer = responce.performer;
            string strEstimate = responce.estimate;
            int estimate = Int32.Parse(strEstimate);
            string strParent = responce.parent;
            int? parent;

            if (strParent == "") 
            {
                parent = null;
            } 
            else 
            { 
                parent = Int32.Parse(strParent); 
            }

            await taskRepository.AddNewTask(name, desc, performer, estimate, parent);
        }

        public async Task DeleteTask (int id)
        {
            await taskRepository.DeleteTask(id);
        }

        public async Task UpdateTask (object updateTask)
        {
            dynamic responce = JsonConvert.DeserializeObject(updateTask.ToString());

            string strResponce = responce.id;
            int id = Int32.Parse(strResponce);
            string name = responce.name;
            string desc = responce.desc;
            string performer = responce.performer;
            string strEstimate = responce.estimate;
            int estimate = Int32.Parse(strEstimate);
            string strFactualEst = responce.factualestimate;
            int? factualEstimate = 0;

            if (strFactualEst == "" || strFactualEst == "null")
            {
                factualEstimate = null;
            } 
            else
            {
                factualEstimate = Int32.Parse(strFactualEst);
            }                      

            await taskRepository.UpdateTask(id, name, desc, performer, estimate, factualEstimate);
        }

        public async Task<bool> UpdateTaskStatus(object taskStatus)
        {
            dynamic responce = JsonConvert.DeserializeObject(taskStatus.ToString());

            string strTaskId = responce.taskid;
            string strStatusId = responce.statusid;

            int taskId = Int32.Parse(strTaskId);
            int statusId = Int32.Parse(strStatusId);

            int? factualEstimate = 0;

            if (statusId == 3)
            {
                string strFactualEst = responce.factualestimate;               

                if (strFactualEst == "" || strFactualEst == "null")
                {
                    return false;
                }
                else
                {
                    factualEstimate = Int32.Parse(strFactualEst);
                }

                if (await CheckSubTasksStatus(taskId))
                {
                    await taskRepository.UpdateTaskStatus(taskId, statusId, factualEstimate);
                    return true;
                }
                else
                {
                    return false;
                }
            } else
            {
                await taskRepository.UpdateTaskStatus(taskId, statusId, factualEstimate);
                return true;
            }            
        }

        private async Task<bool> CheckSubTasksStatus(int taskId)
        {
            List<TaskRecordDTO> taskTree = new List<TaskRecordDTO>();

            var tasks = await taskRepository.GetTaskTreeById(taskId);

            foreach(var task in tasks)
            {
                if(task.TaskID != taskId && task.TaskStatusID != 3)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
