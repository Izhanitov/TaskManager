using System.Collections.Generic;
using TaskManager.DAL.Models;
using TaskManager.BLL.DataTransferObjects;


namespace TaskManager.BLL.Services
{
    static class MapperService
    {
        public static void MapTaskTreeDALToDTO (List<TaskRecord> taskEntities, List<TaskRecordDTO> taskDTOs)
        {            
            foreach(var task in taskEntities)
            {
                var taskDTO = new TaskRecordDTO();

                taskDTO.TaskID = task.TaskID;

                taskDTO.TaskName = task.TaskName;

                taskDTO.TaskDescription = task.TaskDescription;

                taskDTO.TaskPerformer = task.TaskPerformer;

                taskDTO.TaskRegDate = task.TaskRegDate;

                taskDTO.TaskStatusID = task.TaskStatusID;

                taskDTO.TaskStatusName = task.TaskStatus.TaskStatusName;

                taskDTO.CompleteDate = task.CompleteDate;

                taskDTO.ParentTaskID = task.ParentTaskID;

                taskDTO.Estimate = task.Estimate;

                taskDTO.EstimateSubTasks = 0;

                taskDTO.FactualEstimate = task.FactualEstimate;

                taskDTO.FactualEstimateSubTasks = 0;

                taskDTOs.Add(taskDTO);
            }
        }


        public static void MapAllTasksList(List<TaskRecord> taskEntities, List<AllTasksDTO> allTasks)
        {
            foreach(var task in taskEntities)
            {
                var allTask = new AllTasksDTO();

                allTask.TaskID = task.TaskID;

                allTask.TaskName = task.TaskName;

                allTask.ParentTaskID = task.ParentTaskID;

                allTasks.Add(allTask);
            }
        }

    }
}
