using System.Collections.Generic;
using TaskManager.BLL.DataTransferObjects;
using TaskManager.ViewModels;


namespace TaskManager.Services
{
    static class MapperService
    {
        public static void MapTaskDTOtoViewModel(TaskRecordDTO taskRecord, TaskViewModel taskViewModel)
        {           

            taskViewModel.TaskID = taskRecord.TaskID;

            taskViewModel.TaskName = taskRecord.TaskName;

            taskViewModel.TaskDescription = taskRecord.TaskDescription;

            taskViewModel.TaskPerformer = taskRecord.TaskPerformer;

            taskViewModel.TaskRegDate = taskRecord.TaskRegDate;

            taskViewModel.TaskStatusID = taskRecord.TaskStatusID;

            taskViewModel.TaskStatusName = taskRecord.TaskStatusName;

            taskViewModel.CompleteDate = taskRecord.CompleteDate;

            taskViewModel.ParentTaskID = taskRecord.ParentTaskID;

            taskViewModel.Estimate = taskRecord.Estimate;

            taskViewModel.EstimateSubTasks = taskRecord.EstimateSubTasks;

            taskViewModel.FactualEstimate = taskRecord.FactualEstimate;

            taskViewModel.FactualEstimateSubTasks = taskRecord.FactualEstimateSubTasks;

            taskViewModel.SummuryEstimate = taskViewModel.Estimate + taskViewModel.EstimateSubTasks;

            taskViewModel.SummuryFactual = taskViewModel.FactualEstimate + taskViewModel.FactualEstimateSubTasks;

        }


        public static void MapTreeDTOtoViewModel(List<TaskRecordDTO> taskRecordDTOs, List<TaskViewModel> taskViewModels) {
            foreach (var taskDTO in taskRecordDTOs)
            {
                TaskViewModel taskViewModel = new TaskViewModel();

                MapTaskDTOtoViewModel(taskDTO, taskViewModel);

                taskViewModels.Add(taskViewModel);
            }
        }

        public static void MapAllTasksDTOtoVM(List<AllTasksDTO> allTasksDTO , List<AllTaskViewModel> allTasksView)
        {
            foreach (var task in allTasksDTO)
            {
                var allTask = new AllTaskViewModel();

                allTask.TaskID = task.TaskID;

                allTask.TaskName = task.TaskName;

                allTask.ParentTaskID = task.ParentTaskID;

                allTasksView.Add(allTask);
            }
        }
    }
}
