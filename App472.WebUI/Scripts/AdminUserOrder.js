(function () {
    var removeLineFromUserOrderDetail = function (e) {
        var params = {
            ProductId: e.currentTarget.attributes["data-ProductID"].nodeValue,
            OrderId: e.currentTarget.attributes["data-OrderID"].nodeValue
        };
        $.ajax({
            url: "/AdminUserOrder/DeleteProduct",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            statusCode: {
                200: function (jqXHR) {
                    debugger;
                    window.location.reload();
                }
            },
            complete: function (xhr, textStatus) {
                debugger;
            }
        });
    }
    var AdminUserOrderReadyJs = function (e) {
        $('table.adminUserOrderDetail').on('click','button.mgDeleteX',removeLineFromUserOrderDetail);
    };
    jQuery(document).ready(AdminUserOrderReadyJs);
})();