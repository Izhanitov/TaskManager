using System;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DAL.Models
{
    public class TaskRecord
    {
        [Key]
        public int TaskID { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public string TaskPerformer { get; set; }

        public DateTime TaskRegDate { get; set; }

        public int TaskStatusID { get; set; }

        public TaskStatus TaskStatus { get; set; }            

        public DateTime? CompleteDate { get; set; }

        public int? ParentTaskID { get; set; }
        
        public TaskRecord ParentTask { get; set; }

        public int Estimate { get; set; }

        public int? FactualEstimate { get; set; }
    }
}
