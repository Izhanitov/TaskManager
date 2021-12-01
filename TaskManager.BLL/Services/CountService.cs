using System.Collections.Generic;
using TaskManager.BLL.DataTransferObjects;
using System.Linq;

namespace TaskManager.BLL.Services
{
    public static class CountService
    {
        public static void CalcTreeEstimates(List<TaskRecordDTO> taskTree)
        {
            List<int> countedTasks = new List<int>();

            int estimateSubtasks = 0;
            int? factualSubtasks = 0;

            if(taskTree.Count == 1)
            {
                return;
            }

            foreach(var task in taskTree)
            {
                var childs = taskTree.FindAll(elem => elem.ParentTaskID == task.TaskID) 
                    .ToList();

                //Ищем элементы, на которые никто не ссылается

                if (childs.Count == 0 && task.ParentTaskID != null)
                {
                    CalcTasksChain(task, taskTree, countedTasks, estimateSubtasks, factualSubtasks);
                }
            }
        }

        private static void CalcTasksChain(TaskRecordDTO child, List<TaskRecordDTO> taskTree, List<int> countedTasks, int estimateSubtasks, int? factualSubtasks)
        {            
            if(child.ParentTaskID != null)
            {
                var parent = taskTree.Single(elem => elem.TaskID == child.ParentTaskID);

                if(!countedTasks.Contains(child.TaskID))
                {                    
                    estimateSubtasks = child.Estimate + child.EstimateSubTasks;


                    //Если child task еще не завершена и затраченное время не указано
                    if (child.FactualEstimate == null)
                    {
                        factualSubtasks = child.FactualEstimateSubTasks;
                    }
                    else
                    {
                        factualSubtasks = child.FactualEstimate + child.FactualEstimateSubTasks;
                    }

                    //factualSubtasks = child.FactualEstimate + child.FactualEstimateSubTasks;                    

                    parent.EstimateSubTasks += estimateSubtasks;
                    parent.FactualEstimateSubTasks += factualSubtasks;
                    countedTasks.Add(child.TaskID);                    
                }
                else
                {
                    parent.EstimateSubTasks += estimateSubtasks;
                    parent.FactualEstimateSubTasks += factualSubtasks;
                }

                bool checkUpperTask = true;
                                
                if(taskTree.Find(elem => elem.TaskID == parent.ParentTaskID) == null)
                {
                    checkUpperTask = false;                    
                }

                if (parent.ParentTaskID != null && checkUpperTask)
                {
                    CalcTasksChain(parent, taskTree, countedTasks, estimateSubtasks, factualSubtasks);
                }               
            }          

        }
    }
}
