
$(document)
      .ready(function () {

          $.ajaxSetup({ cache: false });

          $(".viewDialog")
              .on("click",
                  function (e) {
                      e.preventDefault();
                      $("<div id='dialogContent'></div>")
                          .addClass("dialog")
                          .appendTo("body")
                          .load(this.href)
                          .dialog({
                              title: $(this).attr("data-dialog-title"),
                              close: function () { $(this).remove() },
                              modal: true,
                              buttons: {
                                  "Save": function () {
                                      $.ajax({
                                          url: AppUrlSettings.CommentUrl,
                                          type: "POST",
                                      data: $('form').serialize(),
                                      datatype: "json",
                                      success: function (result) {

                                          $("#dialogContent").html(result);
                                      }
                                  });
                  }
          }
});
$(".close")
    .on("click",
        function (e) {
            e.preventDefault();
            $(this).closest(".dialog").dialog("close");
        });
});
});