
// Partial rendering on ToDoEntry/Index

$(function () {

    $('#ToDoEntriesCardsContent').on('click', '.pages-buttons-wrapper a', function () {
        var url = $(this).attr('href');

        $('#ToDoEntriesCardsContent').load(url)

        return false;
    })
})