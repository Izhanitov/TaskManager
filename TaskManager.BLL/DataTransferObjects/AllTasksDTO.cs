

namespace TaskManager.BLL.DataTransferObjects
{
    public class AllTasksDTO
    {
        public int TaskID { get; set; }
        public string TaskName { get; set; }
        public int? ParentTaskID { get; set; }
    }
}
