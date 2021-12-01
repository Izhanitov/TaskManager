﻿using System;


namespace TaskManager.BLL.DataTransferObjects
{
    public class TaskRecordDTO
    {
        public int TaskID { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public string TaskPerformer { get; set; }

        public DateTime TaskRegDate { get; set; }

        public int TaskStatusID { get; set; }

        public string TaskStatusName { get; set; }

        public DateTime? CompleteDate { get; set; }

        public int? ParentTaskID { get; set; }

        public int Estimate { get; set; }

        public int EstimateSubTasks { get; set; }

        public int? FactualEstimate { get; set; }

        public int? FactualEstimateSubTasks { get; set; }
    }
}
