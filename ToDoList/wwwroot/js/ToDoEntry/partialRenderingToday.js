
// Partial rendering on ToDoEntry/ReadTodayItems

$(function () {

    $('#ToDoEntriesContent').on('click','.pages-buttons-wrapper a', function () {
        var url = $(this).attr('href');

        $('#ToDoEntriesContent').load(url)

        return false;
    })
})