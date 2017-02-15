$(document)
    .ready(function() {

        InitDeleteComment();
        $(".viewDialog")
            .on("click",
                function(e) {
                    e.preventDefault();
                    $("<div id='dialogContent'></div>")
                        .addClass("dialog")
                        .appendTo("body")
                        .load(this.href)
                        .dialog({
                                title: $(this).attr("data-dialog-title"),
                                close: function() { $(this).remove() },
                                position: { my: "center", at: "center", of: "#commentPosition" },
                                modal: true,
                                buttons: {
                                    "Save": function() {
                                        $.ajax({
                                            url: AppUrlSettings.CommentAddUrl,
                                            type: "POST",
                                            data: $('form').serialize(),
                                            datatype: "json",
                                            success: function(result) {
                                                $("#dialogContent").html(result);
                                            }
                                        });
                                    }
                                }
                            }
                        );
                });
        $(".close")
            .on("click",
                function(e) {
                    e.preventDefault();
                    $(this).closest(".dialog").dialog("close");
                });
    });

function InitDeleteComment() {
    $(".viewDialogDelete")
        .on("click",
            function(e) {
                e.preventDefault();
                var commentId = document.getElementById('CommentValueId').value;
                $("<div id='dialogContent'></div>")
                    .addClass("dialog")
                    .appendTo("body")
                    .dialog({
                            title: $(this).attr("data-dialog-title"),
                            close: function() { $(this).remove() },
                            position: { my: "center", at: "center", of: "#commentPosition" },
                            modal: true
                        })
                        .load(
                            $.ajax({
                                url: AppUrlSettings.CommentDeleteUrl,
                                type: "GET",
                                data: "commentId=" + commentId,
                                datatype: "html",
                                success: function (result) {
                                    $("#dialogContent").html(result);
                                }
                            })
                        );
            });
    $(".close")
        .on("click",
            function(e) {
                e.preventDefault();
                $(this).closest(".dialog").dialog("close");
            });
};