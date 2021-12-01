export default class SendService {
    constructor(mainUrl) {
        this.mainUrl = mainUrl;            
    }        

    sendRequest(url, options) {
        return fetch(`${this.mainUrl + url}`, options);
    }

    getRequest(url) {
        return this.sendRequest(url);
    }
    
    deleteRequest(url) {
        return this.sendRequest(url, {method: "DELETE"});
    }
      
    postRequest(url, sendObject) {
        return this.sendRequest(url, {method: "POST",
                                        body: JSON.stringify(sendObject),
                                        headers: {
                                                    'Content-type': 'application/json'
        }});
    }

    LoadAllTasks() {
        return this.getRequest(`/GetTasksList`);
    }
    
    LoadTaskTreeById(id) {
        return this.getRequest(`/GetTaskTreeById/${id}`);
    }

    LoadTaskById(id) {
        return this.getRequest(`/GetTaskById/${id}`);
    }

    UpdateTaskStatus(sendObject) {
        return this.postRequest(`/UpdateTaskStatus`, sendObject);
    }

    UpdateTask(sendObject) {
        return this.postRequest(`/UpdateTask`, sendObject);
    }

    DeleteTask(id) {
        return this.deleteRequest(`/DeleteTask/${id}`);
    }

    SendData(sendObject) {
        return this.postRequest(`/SendTask`, sendObject);
    }
};