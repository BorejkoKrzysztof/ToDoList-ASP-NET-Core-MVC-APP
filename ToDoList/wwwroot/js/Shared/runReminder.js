﻿document.addEventListener("DOMContentLoaded", RunReminder)



function RunReminder() {

    if (sessionStorage.getItem('reminder') == null) {

        MakeReminderRequest()
    }

    // run comparing toDoEntry duedate every 25 seconds.
    window.setInterval(CompareTime, 25000)
}


// Prepare request for reminder
function MakeReminderRequest() {

    var getUrl = window.location;
    var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[0];


    var xhttp = new XMLHttpRequest();
    xhttp.open("POST", baseUrl + 'ToDoEntry/Reminder')
    xhttp.setRequestHeader("Content-Type", "application/json");

    xhttp.onload = () => {

        const reminderInfo = JSON.parse(xhttp.response)

        if (reminderInfo.value !== null) {
            console.log(reminderInfo.value.toDoEntryTitle)
            SetItemsInSessionStorage(reminderInfo)
        }
        else {
            sessionStorage.removeItem('reminder')
            sessionStorage.removeItem('titleRmr')
            sessionStorage.removeItem('dateRmr')
            sessionStorage.removeItem('5mIsReminded')
            sessionStorage.removeItem('30mIsReminded')
        }
    }

    xhttp.send()
}

function SetItemsInSessionStorage(reminderObject) {

    if (reminderObject.value.toDoEntryTitle != null) {
        sessionStorage.setItem('reminder', 'false')
        sessionStorage.setItem('titleRmr', reminderObject.value.toDoEntryTitle)
        sessionStorage.setItem('dateRmr', reminderObject.value.toDoEntryDueDate)
        sessionStorage.setItem('5mIsReminded', 'false')
        sessionStorage.setItem('30mIsReminded', 'false')
    }
}

function CompareTime() {

    const dueDate = new Date(sessionStorage.getItem('dateRmr'))

    const now = new Date()

    if ((now.getHours() == dueDate.getHours() && dueDate.getMinutes() === now.getMinutes()) || dueDate.getTime() < now.getTime()) {
        sessionStorage.setItem('5mIsReminded', 'false')
        sessionStorage.setItem('30mIsReminded', 'false')
        MakeReminderRequest()
        return
    }

    var remaining = Math.abs(dueDate.getTime() - new Date().getTime())

    if (sessionStorage.getItem('5mIsReminded') == 'false') {
        if (remaining <= 5 * 60 * 1000) {
            DisplayAlert(5)
            sessionStorage.setItem('5mIsReminded', 'true')
            return
        }
    }

    if (sessionStorage.getItem('30mIsReminded') == 'false' && sessionStorage.getItem('5mIsReminded') == 'false') {
        if (remaining <= 30 * 60 * 1000) {
            DisplayAlert(30)
            sessionStorage.setItem('30mIsReminded', 'true')
            return
        }
    }
}

function DisplayAlert(minutes) {
    const title = sessionStorage.getItem('titleRmr')

    alert(`${title} task starts in less than ${minutes} minutes!!!`)
}