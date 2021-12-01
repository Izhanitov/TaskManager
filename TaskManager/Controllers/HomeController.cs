using Microsoft.AspNetCore.Mvc;
using TaskManager.BLL.Interfaces;
using System.Threading.Tasks;
using TaskManager.ViewModels;
using TaskManager.Services;
using System.Collections.Generic;
using System;

namespace TaskManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITransferService transferService;

        public HomeController(ITransferService transferService)
        {
            this.transferService = transferService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetTasksList()
        {
            try
            {
                var taskList = await transferService.GetTaskList();

                var taskListVM = new List<AllTaskViewModel>();

                MapperService.MapAllTasksDTOtoVM(taskList, taskListVM);

                return new JsonResult(taskListVM);
            }
            catch(Exception)
            {
                return new NotFoundResult();
            }
        }

        public async Task<IActionResult> GetTaskTreeById(int id)
        {
            try
            {
                var taskTree = await transferService.GetTaskTreeById(id);

                var taskViewModels = new List<TaskViewModel>();

                MapperService.MapTreeDTOtoViewModel(taskTree, taskViewModels);

                return new JsonResult(taskViewModels);
            }
            catch(Exception)
            {
                return new NotFoundResult();
            }
        }

        public async Task<IActionResult> GetTaskById(int id)
        {
            try
            {
                var taskRecord = await transferService.GetTaskById(id);

                var taskViewModel = new TaskViewModel();

                MapperService.MapTaskDTOtoViewModel(taskRecord, taskViewModel);

                return new JsonResult(taskViewModel);
            }
            catch(Exception)
            {
                return new NotFoundResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendTask([FromBody] object jsonObj)
        {
            try
            {   
                await transferService.SetTask(jsonObj);

                return new OkResult();
            }
            catch(Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await transferService.DeleteTask(id);

                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTask([FromBody] object jsonObj)
        {
            try
            {
                await transferService.UpdateTask(jsonObj);

                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] object jsonObj)
        {
            try
            {
                bool check = await transferService.UpdateTaskStatus(jsonObj);

                if (check)
                {
                    return Ok("Статус изменен успешно.");
                }
                else
                {
                    return Ok("Ошибка: одна из подзадач не завершена!");
                }
            }
            catch
            {
                return new BadRequestResult();
            }
        }

    }
}
