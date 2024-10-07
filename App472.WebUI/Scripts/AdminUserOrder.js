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

            // member variables
            cardBackground: $(options.cardBackgroundClass),
            detailTable: $(options.detailTableClass),
            productRows: $(options.productRowsClass),
            quantityInputs: $(options.quantityInputsClass),

            EnableListeners: function () {
                page.quantityInputs.on("focus", page.QuantityFocus);
                page.quantityInputs.on("blur", page.QuantityBlur);
                page.detailTable.on('keyup', options.quantityInputsClass, page.QuantityKeyup);
                page.detailTable.on('click', options.deleteButtonsClass, page.ProductLineClickX);
            },

            DisableListeners: function () {
                page.quantityInputs.off("focus");
                page.quantityInputs.off("blur");
                page.detailTable.off('keyup');
                page.detailTable.off('click');
            },

            EnableFields: function (event) {
                // remove disabled class
                //page.cardBackground.removeClass("disabled");
                //page.productRows.removeClass("disabled");
                //page.quantityInputs.removeClass("disabled");
            },

            DisableFields: function () {
                // add disabled class
                //page.cardBackground.addClass("disabled");
                //page.productRows.addClass("disabled");
                //page.quantityInputs.addClass("disabled");
            },

            // User focused the Quantity field
            QuantityFocus: function (event) {
                if (event) {
                    page.DisableFields();
                    var inputField = $(event.currentTarget); inputField.removeClass("disabled"); inputField.closest("tr").removeClass("disabled");
                }
            },

            // User blurred the Quantity field
            QuantityBlur: function (event) {
                if (event) {
                    page.DisableFields();
                    page.DisableListeners();
                    page.UpdateLine(event);
                }
            },

            // User pressed RETURN while editing Quantity field
            QuantityKeyup: function (event) {
                if (event && event.which === 13) {
                    event.preventDefault();
                    event.currentTarget.blur();
                    //page.QuantityBlur(event);
                }
            },

            // See
            // https://stackoverflow.com/questions/38760368/jquery-ajax-security-concerns
            UpdateLine: function (event) {
                var row = $(event.currentTarget).closest("tr");
                var qty = $(event.currentTarget).val();
                var params = {
                    OrderID: page.detailTable.data("orderid"),
                    ProductID: row.data("productid"),
                    NewQty: qty
                };
                console.log("Try to update product " + params.ProductID + " -> new qty: " + qty);
                debugger;
                $.ajax({
                    url: "/AdminUserOrder/ProductLineUpdate",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            //window.location.reload();
                            page.EnableFields();
                            page.EnableListeners();
                        }
                    }
                });
            },

            // User clicked X button to remove a Product line from the Order
            ProductLineClickX: function (event) {
                var params = {
                    OrderID: page.detailTable.data("orderid"),
                    ProductID: $(event.currentTarget).closest("tr").data("productid")
                };
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
                page.EnableListeners();
            },
        };

        return {
            ready: page.ReadyJs
        };
    };
})(jQuery);

var options = {
    // Css selectors
    cardBackgroundClass: ".card-body.adminUserOrderDetail",
    detailTableClass: "table.adminUserOrderDetail",
    productRowsClass: "tr",
    quantityInputsClass: "input.mgAjaxText",
    deleteButtonsClass: "button.mgDeleteX",
};
var page = $.adminUserOrderDetail(options);
jQuery(document).ready(page.ready);
