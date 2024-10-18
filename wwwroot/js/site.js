// hamburger menu
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
        prognosisEditButton.textContent = "annuleren";
    } else {
        form.style.display = "none";
        overview.style.display = "flex";
        prognosisEditButton.textContent = "bewerken";
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

function checkDate() {
    let startDateField = document.getElementById("startDate");
    let endDateField = document.getElementById("endDate");
    let currentDate = new Date().setHours(0, 0, 0, 0);

    let date = new Date(startDateField.value);
    let dayNumber = date.getDay();
    document.getElementById("prognosisCreateButton").disabled = (dayNumber != 1 || date < currentDate);

    if (!(dayNumber != 1 || date < currentDate)) {
        date.setDate(date.getDate() + 6);
        let formattedDate = ('0' + date.getDate()).slice(-2) + '-' +
            ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
            date.getFullYear(); //formatten voor dd-mm-yyyy (met 07 ipv 7)

        endDateField.textContent = formattedDate;
    }
    else {
        endDateField.textContent = 'dd-mm-yyyy';
    }
}