export { DelWrong, AddWrong, WrongStatus, OkStatus }

const DelWrong = (...selectors) => {
    selectors.forEach(selector => {
        if(selector.classList.contains("wrong_input")) {
            selector.classList.remove("wrong_input");
        } 
    });
}

const AddWrong = (...selectors) => {
    selectors.forEach(selector => {
        if(!selector.classList.contains("wrong_input")) {
            selector.classList.add("wrong_input");
        }
    });
}

const WrongStatus = (selector) => {
    if(selector.classList.contains("wrong_status")) {
        selector.classList.remove("wrong_status");
    } 
}
 
const OkStatus = (selector) => {
    if(!selector.classList.contains("wrong_status")) {
        selector.classList.add("wrong_status");
    }
}