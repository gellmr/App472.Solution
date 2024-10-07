(function ($) {

    // The module pattern, in javascript. See...
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    // https://www.google.com.au/books/edition/Learning_JavaScript_Design_Patterns/L46fX62D5qYC?hl=en&gbpv=1&dq=Learning+JavaScript+Design+Patterns&printsec=frontcover
    //
    $.adminUserOrderDetail = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({
                //'animated': true,
                //'duration': 500,
                //'direction': 'left'
            }, options),

            GetParams: function (currentTarget) {
                var table = $(currentTarget).closest("table");
                var row = $(currentTarget).closest("tr");
                var params = { OrderID: table.data("orderid"), ProductID: row.data("productid") };
                return params;
            },

            DisableAll: function (event) {
                $("input").addClass("disabled");
                $("tr").addClass("disabled");
                $(".card-body").addClass("disabled");
                $(event.currentTarget).removeClass("disabled");
                $(event.currentTarget).closest("tr").removeClass("disabled");
            },

            EnableAll: function (event) {
                $(".disabled").removeClass("disabled");
            },

            // User focused the Quantity field
            QuantityFocus: function (event) {
                if (event) {
                    page.DisableAll(event);
                }
            },

            // User blurred the Quantity field
            QuantityBlur: function (event) {
                if (event) {
                    var params = page.GetParams(event.currentTarget);
                    var qty = $(this).val();
                    page.UpdateLine(params.ProductID, qty, $(event.target));
                }
            },

            // User pressed RETURN while editing Quantity field
            QuantityKeyup: function (event) {
                if (event && event.which === 13) {
                    var params = page.GetParams(event.currentTarget);
                    var qty = $(this).val();
                    page.UpdateLine(params.ProductID, qty, $(event.target));
                }
            },

            // See
            // https://stackoverflow.com/questions/38760368/jquery-ajax-security-concerns
            UpdateLine: function (productID, newQty, inputElement) {
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
                    statusCode: {
                        200: function (jqXHR) {
                            //window.location.reload();
                            page.EnableAll();
                        }
                    }
                });
            },

            // User clicked X button to remove a Product line from the Order
            ProductLineClickX: function (e) {
                var params = page.GetParams(e.currentTarget);
                $.ajax({
                    url: "/AdminUserOrder/DeleteProduct",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            //window.location.reload();
                        }
                    }
                });
            },

            // Page ready, attach event listeners
            ReadyJs: function () {
                var detailTable = $('table.adminUserOrderDetail');
                var quantityInputs = $("input.mgAjaxText");
                quantityInputs.on("focus", page.QuantityFocus);
                quantityInputs.on("blur", page.QuantityBlur);
                detailTable.on('keyup', 'input.mgAjaxText', page.QuantityKeyup);
                detailTable.on('click', 'button.mgDeleteX', page.ProductLineClickX);
            },
        };

        return {
            ready: page.ReadyJs
        };
    };
})(jQuery);

var options = {};
var page = $.adminUserOrderDetail(options);
jQuery(document).ready(page.ready);
