"use strict";

const navbar = document.querySelector("nav");
const tasksTable = document.querySelector(".table");
const tasksTableParent = tasksTable.closest(".col-6");

function handleResize() {
    if (window.innerWidth < 980) {
        navbar.classList.remove("container");
        navbar.classList.add("container-fluid");
        tasksTableParent.classList.remove("col-6");
        tasksTableParent.classList.add("col-12");
    }
    else {
        navbar.classList.add("container");
        navbar.classList.remove("container-fluid");
        tasksTableParent.classList.remove("col-12");
        tasksTableParent.classList.add("col-6");
    }
}

window.addEventListener("DOMContentLoaded", function () {
    handleResize();
});
window.addEventListener("resize", handleResize);