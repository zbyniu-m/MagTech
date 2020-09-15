$(function() {
    var PlaceHolderHereElement = $('#PlaceHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            PlaceHolderHereElement.html(data);
            PlaceHolderHereElement.find('.modal').modal('show');
        })
    })

    PlaceHolderHereElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            PlaceHolderHereElement.find('.modal').modal('hide');
        })
    })
})