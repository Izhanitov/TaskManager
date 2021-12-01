export { ShowElems, HideElems, AllowedButtons, DisabledButtons, CleanInput }

//Сокрытие и открытие элементов
const ShowElems = (...selectors) => {
    selectors.forEach(selector => {
        if(selector.classList.contains("hidden")) {
            selector.classList.remove("hidden");
        };
})};   

const HideElems = (...selectors) => {
    selectors.forEach(selector => {
        if(!selector.classList.contains("hidden")) {
            selector.classList.add("hidden");
        };
    }
)};

//Управление доступностью кнопок
const AllowedButtons = (...selectors) => {
    selectors.forEach(selector => {
        if(selector.classList.contains("disabled")) {
            selector.classList.remove("disabled");
        };
    });
};

const DisabledButtons = (...selectors) => {
    selectors.forEach(selector => {
        if(!selector.classList.contains("disabled")) {
            selector.classList.add("disabled");
        };
    })
};   

//Очистка инпутов
const CleanInput = (...selectors) => {
    selectors.forEach(selector => {
        selector.value = "";
    });
}

