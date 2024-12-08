// bootstrap popover
const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]')
const popoverList = [...popoverTriggerList].map(popoverTriggerEl => new bootstrap.Popover(popoverTriggerEl));

// hamburger menu
document.getElementById('mobile-hamburger-menu').addEventListener('click', function () {
    document.getElementById('aside-nav-primary').classList.toggle('d-none');
});

document.getElementById('mobile-close-menu').addEventListener('click', function () {
    document.getElementById('aside-nav-primary').classList.add('d-none');
});

function checkDate() {
    let startDateField = document.getElementById("startDate");
    let endDateField = document.getElementById("endDate");
    let warningMessage = document.getElementById("mondayWarning");
    let currentDate = new Date().setHours(0, 0, 0, 0);

    let date = new Date(startDateField.value);
    let dayNumber = date.getDay();
    document.getElementById("prognosisCreateButton").disabled = (dayNumber !== 1 || date < currentDate);

    if (dayNumber !== 1) {
        warningMessage.classList.remove("d-none");
    } else {
        warningMessage.classList.add("d-none");
    }

    if (!(dayNumber !== 1 || date < currentDate)) {
        date.setDate(date.getDate() + 6);
        //formatten voor dd-mm-yyyy (met 07 ipv 7)
        endDateField.textContent = ('0' + date.getDate()).slice(-2) + '-' +
            ('0' + (date.getMonth() + 1)).slice(-2) + '-' +
            date.getFullYear();
    }
    else {
        endDateField.textContent = 'dd-mm-yyyy';
    }
}