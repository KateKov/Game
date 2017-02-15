$(document).ready(function () {
    setTimeout(function () {
        $.ajax({
            type: 'GET',
            url: AppUrlBasketSettings.BasketUrl,
            contentType: 'application/html; charset=utf-8',
            datatype: "json",
            success: function (result) {
                $("#basketInfo").html(result);
            }
        });
        $('.dialog').remove();
    }, 1000);
})
