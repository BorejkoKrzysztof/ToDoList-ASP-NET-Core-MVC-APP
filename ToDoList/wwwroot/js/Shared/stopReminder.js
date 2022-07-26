if ($('#logoutButton').length > 0) {
    $('#logoutButton').on('click', StopReminder)
}



function StopReminder() {

    if (sessionStorage.getItem('reminder') != null) {
        sessionStorage.removeItem('reminder')
        sessionStorage.removeItem('titleRmr')
        sessionStorage.removeItem('dateRmr')
    }
}
