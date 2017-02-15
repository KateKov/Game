$(document).ready(function () {
    $("#commentslink")
        .on("click",
            function() {
                $.ajax({
                    url: AppUrlSettings.CommentLoadUrl,
                    type: "GET",
                    data: $("Key").serialize(),
                    success: function(result) {
                        $("#results").html(result);
                    }
                });
            });  
});
