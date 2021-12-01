using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TaskManager.DAL.EF;
using TaskManager.DAL.Models;
using TaskManager.DAL.Interfaces;
using System.Threading.Tasks;

namespace TaskManager.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        TaskManagerContext db;

        public TaskRepository(TaskManagerContext context)
        {
            db = context;
        }

        public async Task<List<TaskRecord>> GetTaskTreeById(int id)
        {
            List<TaskRecord> taskTree = await db.TaskRecord
                                             .Where(p => p.TaskID == id)
                                             .Include(p => p.TaskStatus)                                         
                                             .ToListAsync();

            List<TaskRecord> subTree = new List<TaskRecord>();

            await GetChildren(id, subTree);

            taskTree.AddRange(subTree);

            return taskTree;
        }

        private async Task GetChildren(int parentId, List<TaskRecord> subTree)
        {
            var childs = await db.TaskRecord
                         .Where(p => p.ParentTaskID == parentId)
                         .Include(p => p.TaskStatus)
                         .ToListAsync();

            subTree.AddRange(childs);

            foreach (var child in childs)
            {
                await GetChildren(child.TaskID, subTree);
            }
        }

        public async Task<List<TaskRecord>> GetAllTasks()
        {
            var tasks = await db.TaskRecord
                         .Include(p => p.TaskStatus)                         
                        .ToListAsync();
            return tasks;
        }

        public async Task AddNewTask(string name, string desc, string performer, int estimate, int? parentId)
        {
            TaskRecord taskRecord = new TaskRecord();
            taskRecord.TaskName = name;
            taskRecord.TaskDescription = desc;
            taskRecord.TaskPerformer = performer;
            taskRecord.Estimate = estimate;
            taskRecord.ParentTaskID = parentId;
            taskRecord.TaskStatusID = 0;
            taskRecord.TaskRegDate = DateTime.Now;

            await db.AddAsync(taskRecord);
            await db.SaveChangesAsync();
        }

        public async Task DeleteTask(int id)
        {
            var deleted = await db.TaskRecord.FirstAsync(p => p.TaskID == id);

            if (deleted.ParentTaskID == null)
            {
                await DeleteParentIdInChilds(id, null);
            }
            else
            {
                await DeleteParentIdInChilds(id, deleted.ParentTaskID);
            }

            db.Remove(deleted);
            db.SaveChanges();
        }

        async Task DeleteParentIdInChilds(int deletedId, int? parentId)
        {
            var checkedChilds = await db.TaskRecord
                         .Where(p => p.ParentTaskID == deletedId)                                 
                         .ToListAsync();

            if (parentId == null)
            {
                if (checkedChilds.Count > 0)
                {
                    foreach (var child in checkedChilds)
                    {
                        child.ParentTaskID = null;
                    }
                }
            }
            else
            {
                if (checkedChilds.Count > 0)
                {
                    foreach (var child in checkedChilds)
                    {
                        child.ParentTaskID = parentId;
                    }
                }
            }

        }

        public async Task UpdateTask(int id, string name, string desc, string performer, int estimate, int? factualEstimate)
        {
            TaskRecord taskRecord = await db.TaskRecord.FirstAsync(p => p.TaskID == id);

            taskRecord.TaskName = name;
            taskRecord.TaskDescription = desc;
            taskRecord.TaskPerformer = performer;
            taskRecord.Estimate = estimate;
            taskRecord.FactualEstimate = factualEstimate;

            db.TaskRecord.Update(taskRecord);

            db.SaveChanges();
        }

        public async Task UpdateTaskStatus(int taskId, int statusId, int? factualEstimate)
        {
            TaskRecord taskRecord = await db.TaskRecord.FirstAsync(p => p.TaskID == taskId);

            taskRecord.TaskStatusID = statusId;            

            if(statusId == 3)
            {
                taskRecord.FactualEstimate = factualEstimate;

                taskRecord.CompleteDate = DateTime.Now;
            }

            db.TaskRecord.Update(taskRecord);

            await db.SaveChangesAsync();
        }
    }
}

