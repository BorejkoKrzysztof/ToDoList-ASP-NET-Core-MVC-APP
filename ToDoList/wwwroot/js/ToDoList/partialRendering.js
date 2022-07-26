$(function () {

    $('#ToDoListTable').on('click', '#PagesLinks a', function () {
        var url = $(this).attr('href');

        $('#ToDoListTable').load(url)

        return false;
    })
})