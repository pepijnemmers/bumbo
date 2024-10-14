﻿// hamburger menu
document.getElementById('mobile-hamburger-menu').addEventListener('click', function () {
    document.getElementById('aside-nav-primary').classList.toggle('d-none');
});

document.getElementById('mobile-close-menu').addEventListener('click', function () {
    document.getElementById('aside-nav-primary').classList.add('d-none');
});

//Prognoses details
document.getElementById('prognosisEditButton').addEventListener('click', function () {
    var form = document.getElementById("form");
    var overview = document.getElementById("overview");
    var prognosisEditButton = document.getElementById("prognosisEditButton");

    if (form.style.display === "none") {
        overview.style.display = "none";
        form.style.display = "block";
        prognosisEditButton.textContent = "ANNULEREN";
    } else {
        form.style.display = "none";
        overview.style.display = "flex";
        prognosisEditButton.textContent = "BEWERKEN";
    }
});

function updateHours(id) {
    let employeesInput = document.getElementById(id + '-employees');
    let hoursInput = document.getElementById(id + '-hours');

    let employees = employeesInput.value;
    hoursInput.value = employees * 8;
}

function updateEmployees(id) {
    let hoursInput = document.getElementById(id + '-hours');
    let employeesInput = document.getElementById(id + '-employees');
    let hours = hoursInput.value;
    employeesInput.value = hours / 8;
}