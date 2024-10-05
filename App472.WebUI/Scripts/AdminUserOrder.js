(function () {
    var getParams = function (currentTarget) {
        var table = $(currentTarget).closest("table");
        var row = $(currentTarget).closest("tr");
        var params = { OrderID: table.data("orderid"), ProductID: row.data("productid") };
        return params;
    }

    var updateUserOrderDetailQuantity = function (event) {
        if (event && event.which === 13) {
            var params = getParams(event.currentTarget);
            var qty = $(this).val();
            updateProductLine(params.ProductID, qty, $(event.target));
        }
    }

    var userOrderDetailQuantityFocus = function (event) {
        if (event) {
            $("input").addClass("disabled");
            $("tr").addClass("disabled");
            $(".card-body").addClass("disabled");
            $(event.currentTarget).removeClass("disabled");
            $(event.currentTarget).closest("tr").removeClass("disabled");
        }
    }

    var userOrderDetailQuantityBlur = function (event){
        if (event) {
            var params = getParams(event.currentTarget);
            var qty = $(this).val();
            updateProductLine(params.ProductID, qty, $(event.target));
            $(".disabled").removeClass("disabled");
        }
    }

    // See
    // https://stackoverflow.com/questions/38760368/jquery-ajax-security-concerns
    //
    var updateProductLine = function (productID, newQty, inputElement) {
        console.log("Try to update product " + productID + " -> new qty: " + newQty);
        var table = $(inputElement).closest("table");
        var params = {
            ProductID: productID,
            OrderID: table.data("orderid"),
            NewQty: newQty
        };
        $.ajax({
            url: "/AdminUserOrder/ProductLineUpdate",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            statusCode: { 200: function (jqXHR) { window.location.reload(); } }
        });
    };

    var removeLineFromUserOrderDetail = function (e) {
        var params = getParams(e.currentTarget);
        $.ajax({
            url: "/AdminUserOrder/DeleteProduct",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(params),
            statusCode: { 200: function (jqXHR) { window.location.reload(); }}
        });
    }
    var AdminUserOrderReadyJs = function (e) {
        $('table.adminUserOrderDetail').on('keyup','input.mgAjaxText',updateUserOrderDetailQuantity);
        $("input.mgAjaxText").on("focus", userOrderDetailQuantityFocus);
        $("input.mgAjaxText").on("blur", userOrderDetailQuantityBlur);
        $('table.adminUserOrderDetail').on('click', 'button.mgDeleteX', removeLineFromUserOrderDetail);
    };
    jQuery(document).ready(AdminUserOrderReadyJs);
})();