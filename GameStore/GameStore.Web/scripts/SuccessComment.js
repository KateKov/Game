$(document)
      .ready(function () {
          var key = document.getElementById('Key');
          get(AppUrlCommentSettings.CommentUrl, key)
              .then(function (result) {
                  $('.dialog').remove();
                  $("#results").html(result);               
                  },
                  function (error) { console.log(error); });
        
    });


function get(url, key) {
    return new Promise(function (succeed, fail) {
        var request = new XMLHttpRequest();
        request.open("Get", AppUrlCommentSettings.CommentUrl, true);
        request.addEventListener("load",
            function () {
                if (request.status < 400) {
                    window.setTimeout(
                        function () {
                            succeed(request.responseText);
                        },
                        2000);
                }
                else {
                    fail(new Error("Request failed: " + request.statusText));
                }
            });
        request.addEventListener("error",
            function () {
                fail(new Error("Network error"));
            });
        request.send(key);
    });
};