using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManager.DAL.Models
{
    public class TaskStatus
    {
        [Key]
        public int TaskStatusID { get; set; }

        public string TaskStatusName { get; set; }

        public List<TaskRecord> TaskRecords { get; set; }

        public TaskStatus ()
        {
            TaskRecords = new List<TaskRecord>();
        }
    }
}
