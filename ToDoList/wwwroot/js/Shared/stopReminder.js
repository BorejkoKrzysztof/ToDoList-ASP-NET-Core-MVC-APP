if ($('#logoutButton').length > 0) {
    $('#logoutButton').on('click', StopReminder)
}


// delete variables from session storage
function StopReminder() {

    if (sessionStorage.getItem('reminder') != null) {
        sessionStorage.removeItem('reminder')
        sessionStorage.removeItem('titleRmr')
        sessionStorage.removeItem('dateRmr')
        sessionStorage.removeItem('5mIsReminded')
        sessionStorage.removeItem('30mIsReminded')
    }
}
