import SendService from "./services/sendservice";
import { ShowElems, HideElems, AllowedButtons, DisabledButtons, CleanInput } from "./services/availability";
import { DelWrong, AddWrong, WrongStatus, OkStatus } from "./services/highlightwrongs";

'use strict';

document.addEventListener('DOMContentLoaded', () => {     
    
    ///Селекторы    
    const taskList = document.querySelector(`.task_list`);
    
    const selectedtask = document.querySelector(`.selected_task`);
    const taskChilds = document.querySelector(`[data-task-childs]`);
    const editTaskPanel = document.querySelector(`[data-edit-panel]`);
    const subtasksPanel = document.querySelector(`[data-subtasks-panel]`);
    const dataMenuPanel = document.querySelector(`[data-menu-panel]`);
    const dataButtonPanel = document.querySelector(`[data-button-panel]`);
    const statusBar = document.querySelector(`.status_bar`);

    const selTaskData = document.querySelector(`[data-selected-task]`);

    const modalForm = document.querySelector(`.modal`);
    const modalTitle = document.querySelector(`.modal_title`);
    const miniModal = document.querySelector(`.mini_modal`);
    
    const panelDeleteButton = document.querySelector(`[data-delete-task]`);
    const panelAddkButton = document.querySelector(`[data-add-subtask]`);
    const panelEditButton = document.querySelector(`[data-edit-task]`);
    const panelSaveButton = document.querySelector(`[data-save-task]`);
    const panelCancelButton = document.querySelector(`[data-cancel-changes]`);
    
    const beginButton = document.querySelector(`[data-status-begin]`);
    const pauseButton = document.querySelector(`[data-status-pause]`);
    const resumeButton = document.querySelector(`[data-status-resume]`);
    const endButton = document.querySelector(`[data-status-end]`);
    const statusPanel = document.querySelector(`[data-status-panel]`);

    const taskName = document.querySelector(`[name="taskName"]`);
    const taskDesc = document.querySelector(`[name="taskDescription"]`);
    const taskPerformer = document.querySelector(`[name="taskPerformer"]`);
    const taskEstimate = document.querySelector(`[name="estimate"]`);
    const taskParent = document.querySelector(`[name="parent"]`);
    const confirmButton = document.querySelector(`[data-confirm]`);
    const closeModalForm = document.querySelector(`.modal_close`);
    const cancelButton = document.querySelector(`[ data-cancel]`);

    const factualEstimate = document.querySelector(`[name="factualEstimate"]`);
    const miniConfirmBtn = document.querySelector(`[data-mini-confirm]`);
    const miniCancelBtn = document.querySelector(`[data-mini-cancel]`);
    
    const dataSelName = document.querySelector(`[data-sel-taskName]`);
    const dataSelPerformer = document.querySelector(`[data-sel-taskPerformer]`);
    const dataSelDesc = document.querySelector(`[data-sel-taskDescription]`);
    const dataSelEstimateSub = document.querySelector(`[data-sel-estimateSubTasks]`)
    const dataSelRegDate = document.querySelector(`[data-sel-taskRegDate]`);
    const dataSelEstimate = document.querySelector(`[data-sel-estimate]`);
    const dataSelFactualSub = document.querySelector(`[data-sel-factualSubtasks]`);
    const dataSelFactual = document.querySelector(`[data-sel-factualEstimate]`);
    const dataSelStatusName = document.querySelector(`[data-sel-taskStatusName]`);
    const dataSelEstSummary = document.querySelector(`[data-sel-estimateSummary]`);
    const dataSelFactualSummary = document.querySelector(`[data-sel-factualSummary]`);

    const endedPanel = document.querySelector(`[data-ended-panel]`);
    const endedTime = document.querySelector(`[data-sel-completeddate]`);

    const updateName = document.querySelector(`[name="updateName"]`);
    const updateDesc = document.querySelector(`[name="updateDescription"]`);
    const updatePerformer = document.querySelector(`[name="updatePerformer"]`);
    const updateEstimate = document.querySelector(`[name="updateEstimate"]`);
    const updateFactual = document.querySelector(`[name="updateFactual"]`);

    //Получение ключа (номера) задачи
    const GetKey = (selector) => selector.getAttribute("key");
    
    const mainUrl = `${document.location.origin}/Home`;   
    const sendService = new SendService(mainUrl);
   
    //Панель статусов    
    const SelStatusPanel = (status) => {
        if (status == 0) {
            ShowElems(beginButton, statusPanel); 
            HideElems(pauseButton, resumeButton, endButton); 
        }
        if (status == 1) {
            ShowElems(pauseButton, endButton, statusPanel); 
            HideElems(beginButton, resumeButton); 
        }
        if (status == 2) {
            ShowElems(resumeButton, statusPanel); 
            HideElems(pauseButton, beginButton, endButton); 
        }
        if (status == 3) {
            HideElems(statusPanel, resumeButton, pauseButton, beginButton, endButton); 
        }        
    };

    //Вывод сообщения в статус бар
    const InsertStatus = (statusCode, message) => {
        if(statusCode == 200) {
            statusBar.innerText = `${message}`;
            WrongStatus(statusBar);
        } else {
            statusBar.innerText = `${message}`;
            OkStatus(statusBar);
        }
    }

    ///Отправка статуса
    const SendStatus = (taskId, statusId, ended = false) => {
        let sendObject = {
            taskid: {},
            statusid: {},
            factualestimate: {}
        }

        sendObject.taskid = taskId;
        sendObject.statusid = statusId;
        
        if(ended == true) {
            sendObject.factualestimate = factualEstimate.value;
        } else {
            sendObject.factualestimate = "";
        }

        sendService.UpdateTaskStatus(sendObject)
        .then(responce => {            
            if(responce.ok) { 
                LoadTask(taskId);          
                InsertStatus(200, `Статус обновлен успешно...`);   
            } else {
                throw new Error(`Задача не может быть завершена - одна из подзадач незавершена...`);
            }
        })    
        .catch(error => InsertStatus(9000, error));           
    };

    //Обработчик панели статусов
    statusPanel.addEventListener('click', (event) => {
        const id = GetKey(selTaskData);
        if(event.target && event.target === beginButton) {            
            SendStatus(id, 1);
        }
        if(event.target && event.target === resumeButton) {
            SendStatus(id, 1);
        }
        if(event.target && event.target === pauseButton) {
            SendStatus(id, 2);
        }
        if(event.target && event.target === endButton) {
            ShowElems(miniModal);
            miniModal.classList.add("open_modal");
         }
    });    

    //Скорытие null
    const HideNulls = (value) => {        
        value.replace("null", "не завершена");
    }

    //Форма завершения задачи и отправки фактического времени работы
    factualEstimate.addEventListener('input', e => {
        const numsRegExpr = /^0|\D/;
        Verification(e.target, numsRegExpr, miniConfirmBtn);
    });

    miniConfirmBtn.addEventListener('click', () => {       
        const id = GetKey(selTaskData);
        if(factualEstimate.value != "") {
            SendStatus(id, 3, true);
            CloseMiniModal();
        } else {
            AddWrong(factualEstimate);
            DisabledButtons(miniConfirmBtn);            
        }
    });

    const CloseMiniModal = () => {
        CleanInput(factualEstimate);
        HideElems(miniModal);
        miniModal.classList.remove("open_modal");
        DelWrong(factualEstimate);
    }

    miniCancelBtn.addEventListener('click', () => {
        CloseMiniModal();
    })

    //Загрузка списка задач
    const LoadTasks = () => {
        sendService.LoadAllTasks()
        .then(response => response.json())
        .then(elems => renderTasks(elems.$values));
    }
    
    LoadTasks();

    //Отрисовка списка задач
    const renderTasks = (elems) => {
        taskList.innerHTML = "";
        elems.forEach(elem => {            
            if(elem.parentTaskID == null) {
                taskList.innerHTML += `<div id="#${elem.taskID}"><button class="btn_expand child_button">></button><div class="listElem" key="${elem.taskID}">${elem.taskName}</div></div>`;
                renderChilds(elems, elem.taskID);
            }
        })                    
    };   
    
    const renderChilds = (elems, parentId) => {
        elems.forEach(elem => {
            if(elem.parentTaskID === parentId) {                
                const parent = document.querySelector(`[id="#${parentId}"]`);
                parent.innerHTML += `<div id="#${elem.taskID}" class="child_node hidden"><button class="btn_expand child_button">></button><div class="listElem" key="${elem.taskID}">${elem.taskName}</div></div>`;
                parent.firstChild.classList.remove("child_button");
                renderChilds(elems, elem.taskID);                
            }
        })
    };

    taskList.addEventListener('click', (event) => {
        if(event.target && event.target.classList.contains("listElem")) {            
            LoadTask(GetKey(event.target))            
        }
        if(event.target && event.target.classList.contains("btn_expand")) {
            if(event.target.classList.contains("btn_narrow")) {
                event.target.innerText = ">";
                event.target.classList.remove("btn_narrow");                
                const childs = event.target.parentNode.querySelectorAll(".child_node");
                           
                childs.forEach(child => {
                    child.classList.add("hidden");
                    const subchilds = child.querySelectorAll(".btn_expand");
                    subchilds.forEach(subchild => {
                        subchild.classList.remove("btn_narrow");
                        subchild.innerText = ">";
                    })                                     
                });                    
            }
            else {
                event.target.innerText = '∨';                
                event.target.classList.add("btn_narrow");
                const childs = event.target.parentNode.childNodes;                
                childs.forEach(child => child.classList.remove("hidden"));
            }             
        }     
    });

    dataMenuPanel.addEventListener('click', event => {
        if(event.target && event.target.classList.contains("btn_add")) {
            OpenSendForm();
        }
        if(event.target && event.target.classList.contains("btn_refresh")) {
            LoadTasks();
            alert("загружены");
        }  
    });

    //Просмотр выбранной задачи
    const LoadTask = (id) => {
        sendService.LoadTaskTreeById(id)        
        .then(responce => {
            if(responce.ok) {                
                return responce.json();   
            } else {
                throw new Error(`Задача загружена некорректно...`);
            }
        })            
        .then(elems => {            
            renderTask(elems.$values, id);
            InsertStatus(200, `Задача загружена успешно...`);
        })
        .catch(error => InsertStatus(9000, error))
    };
    
    const renderTask = (elems, id) => {
        taskChilds.innerHTML = "";

        ShowElems(selectedtask);
        HideElems(editTaskPanel);

        if(elems.length > 1) {
            ShowElems(subtasksPanel);
        } else {
            HideElems(subtasksPanel);
        }

        elems.forEach(elem => {
            if(elem.taskID == id) {
                RenderParentTask(elem);
            }
             
            if(elem.parentTaskID == id) {                
                RenderChildTask(elem);
            } 
        });

        AllowedButtons(panelDeleteButton, panelAddkButton, panelEditButton);
        DisabledButtons(panelSaveButton, panelCancelButton);
    };

    const RenderParentTask = (elem) => {
        dataSelName.innerText = `${elem.taskName}`;
        selTaskData.setAttribute("key", `${elem.taskID}`)
        dataSelPerformer.innerText = `${elem.taskPerformer}`;
        dataSelDesc.innerText = `${elem.taskDescription}`;
        dataSelEstimateSub.innerText = `${elem.estimate}`;

        let taskReg = elem.taskRegDate;         
        taskReg = taskReg.replace("T", " ");
        taskReg = taskReg.split(".", 1);

        dataSelRegDate.innerText = `${taskReg}`;
        dataSelEstimate.innerText = `${elem.estimateSubTasks || ''}`;
        dataSelFactualSub.innerText = `${elem.factualEstimateSubTasks || ''}`;        
        dataSelStatusName.innerText = `${elem.taskStatusName}`;
        dataSelEstSummary.innerText = `${elem.summuryEstimate || ''}`;
        dataSelFactual.innerText = `${elem.factualEstimate || ''}`;
        dataSelFactualSummary.innerText = `${elem.summuryFactual || ''}`;
                           
        if (elem.taskStatusID == 3) {
            let endTime = elem.completeDate;         
            endTime = endTime.replace("T", " ");
            endTime = endTime.split(".", 1);
            endedTime.innerText = `${endTime}`; 
            ShowElems(endedPanel); 
        }   
        else {
            endedTime.innerText = ``;
            HideElems(endedPanel); 
        }
                           
        SelStatusPanel(elem.taskStatusID);
    };

    const RenderChildTask = (elem) => {

        taskChilds.innerHTML += `<div class="row">
                                    <div class="col-lg-3">${elem.taskName}</div>
                                    <div class="col-lg-2">${elem.taskPerformer}</div>
                                    <div class="col-lg-2">${elem.taskStatusName}</div>
                                    <div class="col-lg-5">
                                        <div class="row">
                                            <div class="col-lg-3">${elem.estimate}</div>
                                            <div class="col-lg-3">${elem.estimateSubTasks || ''}</div>
                                            <div class="col-lg-3">${elem.factualEstimate || ''}</div>
                                            <div class="col-lg-3">${elem.factualEstimateSubTasks || ''}</div>
                                        </div>
                                    </div> 
                                    <div class="col-12 separator"></div>                                    
                                </div>`
    }

    //Загрузка задачи для редактирования
    const LoadTaskForEdit = (id) => {    
        sendService.LoadTaskById(id)
        .then(responce => responce.json())
        .then(elem => {
            RenderEditTask(elem);
        });
    };

    //Отрисовка задачи для редактировани
    const RenderEditTask = (task) => {
        ShowElems(editTaskPanel);
        HideElems(selectedtask);

        updateName.value = `${task.taskName}`;
        updateDesc.value = `${task.taskDescription}`;
        updatePerformer.value = `${task.taskPerformer}`;
        updateEstimate.value = `${task.estimate}`;
        updateFactual.value = `${task.factualEstimate}`;

        AllowedButtons(panelSaveButton, panelCancelButton);
        DisabledButtons(panelDeleteButton, panelAddkButton, panelEditButton);
        HideElems(statusPanel); 
    };

    //Обновление задачи после редактирования
    const UpdateTask = (id) => {
        let task = {
            id: {},
            name: {},
            desc: {},
            performer: {},
            estimate: {},
            factualestimate: {}
        }

        task.id = id;
        task.name = updateName.value;
        task.desc = updateDesc.value;
        task.performer = updatePerformer.value;
        task.estimate = updateEstimate.value;
        task.factualestimate = updateFactual.value;

        sendService.UpdateTask(task)
            .then(() => {
                                //alert(responce.status);
                                LoadTask(id);
                                LoadTasks();
                            });                                                  

    };

    //Панель задачи
    dataButtonPanel.addEventListener('click', (event) => {
        const id = GetKey(selTaskData);
        if(event.target && event.target === panelDeleteButton && !event.target.classList.contains("disabled")) {
            const id = GetKey(selTaskData);
            DelTask(id);
        }
        if(event.target && event.target === panelAddkButton && !event.target.classList.contains("disabled")) {
            OpenSendForm(id);
        }
        if(event.target && event.target === panelEditButton && !event.target.classList.contains("disabled")) {
            LoadTaskForEdit(id);
        }
        if(event.target && event.target === panelSaveButton && !event.target.classList.contains("disabled")) {
            UpdateTask(id);
        }
        if(event.target && event.target === panelCancelButton && !event.target.classList.contains("disabled")) {
            LoadTask(id);
        }     
    });
 
    //Удаление задачи
    const DelTask = (id) => {
        sendService.DeleteTask(id)
        .then(responce => {
            if(responce.ok) {                
                InsertStatus(200, `Задача успешно удалена...`);   
            } else {
                throw new Error(`Задача загружена некорректно...`);
            }
        })    
        .then(() => {
            LoadTasks();
            HideElems(statusPanel, selectedtask, editTaskPanel);             
            DisabledButtons(panelDeleteButton, panelAddkButton, panelEditButton, panelSaveButton, panelCancelButton);
        })
        .catch(error => InsertStatus(9000, error));
    };

    //Работа с формой Добавление задачи
    modalForm.addEventListener('click', (event) => {
        if(event.target && (event.target === cancelButton || event.target === closeModalForm)) {
            CloseModal();
        }

    }); 

    ///Обработка клика в форме
    confirmButton.addEventListener('click', () => {        
        let checkInput = true;
        
        let task = {
            name: {},
            desc: {},
            performer: {},
            estimate: {},
            parent: {}
        }

        if(taskName.value != "") {
            task.name = taskName.value;
        } else {
            checkInput = false;
            AddWrong(taskName);
        }

        if(taskDesc.value != "") {
            task.desc = taskDesc.value;
        } else {
            checkInput = false;
            AddWrong(taskDesc);
        }

        if(taskPerformer.value != "") {
            task.performer = taskPerformer.value;
        } else {
            checkInput = false;
            AddWrong(taskPerformer);
        }

        if(taskEstimate.value != "") {
            task.estimate = taskEstimate.value;
        } else {
            checkInput = false;
            AddWrong(taskEstimate);
        }
        
        task.parent = taskParent.value;

        if(checkInput) {
            SendData(task);
            CloseModal();
        }
    });

    //Верефикация в форме
    const VerificationForm = () => {
        const textRegExpr = /\<|\>|\[|\]/m;
        const numsRegExpr = /^0|\D/;

        taskEstimate.addEventListener('input', e => {            
            Verification(e.target, numsRegExpr, confirmButton);
        })

        taskDesc.addEventListener('input', e => {            
            Verification(e.target, textRegExpr, confirmButton);
        })

        taskPerformer.addEventListener('input', e => {            
            Verification(e.target, textRegExpr, confirmButton);
        })

        taskName.addEventListener('input', e => {            
            Verification(e.target, textRegExpr, confirmButton);
        })
    }

    const Verification = (target, regExpr, button) => {

        if(regExpr.test(target.value)) {
            DisabledButtons(button);
            AddWrong(target);            
        } else {
            AllowedButtons(button);
            DelWrong(target);                   
        }
    }

    //Очистка и закрытие формы добавления задачи
    const CloseModal = () => {
        CleanInput(taskName, taskDesc, taskPerformer, taskEstimate, taskParent);
        DelWrong(taskName, taskDesc, taskPerformer, taskEstimate, taskParent);
        modalForm.classList.remove("open_modal");
    }

    //Открытие формы добавления задачи
    const OpenSendForm = (parent = null) => {
        modalForm.classList.add("open_modal");
        taskParent.value = parent;
        if(parent == null) {
            modalTitle.innerText = "Добавить задачу";
        } else {
            modalTitle.innerText = "Добавить подзадачу";
        }
        VerificationForm();
    };

    const SendData = (sendObject) => {
        sendService.SendData(sendObject)                                            
        .then(responce => {
            if(responce.ok) {                
                InsertStatus(200, `Задача добавлена успешно...`);   
            } else {
                throw new Error(`Ошибка при добавлении задачи...`);
            }
        })    
        .then(() => {                
            LoadTasks();
        })
        .catch(error => InsertStatus(9000, error));            
    };
});

