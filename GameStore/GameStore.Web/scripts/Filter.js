$(document)
    .ready(function() {
        var formOriginalData = $("#filter").serialize();
        $("#filter input[type=\"button\"]")
            .click(function() {
                if ($("#filter-form").serialize() != formOriginalData) {
                    ajaxFilter();
                } else {
                    return false;
                }
                return false;
            });

    });

function ChangeButton(e) {
    document.getElementById('Page').value = e;
    ajaxFilter();
}

var ajaxFilter = function() {
    $.ajax({
            type: 'GET',
            url: AppUrlSettings.GamesListUrl,
            contentType: 'application/html; charset=utf-8',
            data: $("#filter").serialize(),
            datatype: "html"
        })
        .done(function(result) {
            restoreUrl(this.url);
            $('#gamesList').html(result);
        });
}

function restoreUrl(url) {
    var query = url.split("?");
    var newurl = "/games?" + query[1];
    window.history.pushState("object or string", "Title", newurl);
}