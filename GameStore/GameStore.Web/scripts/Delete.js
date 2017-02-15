$(document)
        .ready(function() {
            var isCanceled = false;
            $("#cancel")
                .on("click",
                    function() {
                        isCanceled = true;
                        $.ajax({
                            url: AppUrlSettings.CommentLoadUrl,
                            type: "GET",
                            datatype: "html",
                            success: function (result) {
                                $('.dialog').remove();
                                $("#results").html(result);
                            }
                        });
                    });
            animate({
                duration: 5000,
                timing: function(timeFraction) {
                    return timeFraction;
                },
                draw: function(progress) {
                    elem.style.width = progress * 100 + '%';
                }
            });
            var commentId = document.getElementById('CommentValueId').value;
            window.setTimeout(
                function() {
                    if (!isCanceled) {
                        $.ajax({
                            url: AppUrlSettings.CommentDeleteUrl,
                            type: "POST",
                            data: "commentId=" + commentId,
                            datatype: "html",
                            success: function(result) {
                                $('.dialog').remove();
                                $("#results").html(result);
                            }
                        });
                    }
                },
                5000);

        });

function animate(options) {

    var start = performance.now();

    requestAnimationFrame(function animate(time) {
        var timeFraction = (time - start) / options.duration;
        if (timeFraction > 1) timeFraction = 1;

        var progress = options.timing(timeFraction);

        options.draw(progress);

        if (timeFraction < 1) {
            requestAnimationFrame(animate);
        }

    });
}
