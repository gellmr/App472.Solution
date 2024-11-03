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
            detailHeadTable: $(options.detailTableHeadClass),
            detailTable: $(options.detailTableClass),

            readyShipDropDownBtn: $(options.readyShipDropDownBtnClass),
            readyShipDropDownLinks: $(options.readyShipDropDownLinksClass),

            //billingAddressInput: $(options.billingAddressInputClass),
            //shippingAddressInput: $(options.shippingAddressInputClass),

            productRows: $(options.productRowsClass),
            quantityInputs: $(options.quantityInputsClass),

            detailTotalQuantity: $(options.detailTotalQuantityClass),
            detailTotalCost: $(options.detailTotalCostClass),

            EnableListeners: function () {
                page.readyShipDropDownBtn.on("click", options.readyShipDropDownLinksClass, page.ReadyShipLinkClick);

                //page.billingAddressInput.on("focus", page.AddressFocus);
                //page.billingAddressInput.on("blur", page.AddressBlur);

                //page.shippingAddressInput.on("focus", page.AddressFocus);
                //page.shippingAddressInput.on("blur", page.AddressBlur);

                //page.detailHeadTable.on('keyup', options.billingAddressInputClass, page.AddressKeyup);
                //page.detailHeadTable.on('keyup', options.shippingAddressInputClass, page.AddressKeyup);

                page.quantityInputs.on("focus", page.QuantityFocus);
                page.quantityInputs.on("blur", page.QuantityBlur);

                page.detailTable.on('keyup', options.quantityInputsClass, page.QuantityKeyup);
                page.detailTable.on('click', options.deleteButtonsClass, page.ProductLineClickX);
            },

            DisableListeners: function () {
                page.readyShipDropDownBtn.off("click");

                //page.billingAddressInput.off("focus");
                //page.billingAddressInput.off("blur");

                //page.shippingAddressInput.off("focus");
                //page.shippingAddressInput.off("blur");

                //page.detailHeadTable.off('keyup');

                page.quantityInputs.off("focus");
                page.quantityInputs.off("blur");

                page.detailTable.off('keyup');
                page.detailTable.off('click');
            },

            // User focused the Quantity field
            QuantityFocus: function (event) {
                if (event) {
                    var inputField = $(event.currentTarget); inputField.removeClass("disabled"); inputField.closest("tr").removeClass("disabled");
                }
            },

            // User blurred the Quantity field
            QuantityBlur: function (event) {
                if (event) {
                    page.DisableListeners();
                    page.UpdateLine(event);
                }
            },

            // User pressed RETURN while editing Quantity field
            QuantityKeyup: function (event) {
                if (event && event.which === 13) {
                    event.preventDefault();
                    event.currentTarget.blur();
                }
            },

            // See
            // https://stackoverflow.com/questions/38760368/jquery-ajax-security-concerns
            UpdateLine: function (event) {
                var trow = $(event.currentTarget).closest("tr");
                var qty = $(event.currentTarget).val();
                var params = {
                    OrderID: page.detailTable.data("orderid"),
                    ProductID: trow.data("productid"),
                    NewQty: qty
                };
                console.log("Try to update product " + params.ProductID + " -> new qty: " + qty);
                $.ajax({
                    url: "/AdminUserOrder/ProductLineUpdate",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    success: function (result) {
                        // Server has accepted our post data, and sent back info to update the current page.
                        var QuantityTotal = parseInt(result.QuantityTotal);
                        var PriceTotal = Number.parseFloat(result.PriceTotal);
                        $(detailTotalQuantity).html(QuantityTotal);
                        $(detailTotalCost).html(PriceTotal);
                        // See
                        // https://stackoverflow.com/questions/921789/how-to-loop-through-a-plain-javascript-object-with-the-objects-as-members
                        var Rows = result.Rows;
                        for (var key in Rows) {
                            if (!Rows.hasOwnProperty(key)) continue; // skip loop if the property is from prototype
                            var obj = Rows[key];
                            var ProductID = obj["ProductID"];
                            var ProductName = obj["ProductName"];
                            var UnitPrice = obj["UnitPrice"];
                            var Quantity = obj["Quantity"];
                            var Cost = obj["Cost"];
                            var Category = obj["Category"];
                            var tableRow = $(page.detailTable).find('[data-productid="' + ProductID + '"]');
                            // update the table row values
                            tableRow.children("td.isProductID").html(ProductID);
                            tableRow.children("td.isProductName").find("a").html(ProductName)
                            tableRow.children("td.isUnitPrice").find("a").html(UnitPrice.toFixed(2));
                            tableRow.children("td.isQuantity").find("input.mgAjaxText").html(Quantity);
                            tableRow.children("td.isCost").html(Cost.toFixed(2));
                            tableRow.children("td.isCategory").html(Category);
                        }
                        // update summary info
                        $(page.detailTable).find("#detailTotalQuantity").html(QuantityTotal);
                        $(page.detailTable).find("#detailTotalCost").html("$" + PriceTotal.toFixed(2));
                        page.EnableListeners();
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
                            window.location.reload();
                        }
                    }
                });
            },

            // User clicked one of the ReadyToShip links inside the dropdown
            ReadyShipLinkClick: function (event) {
                var dropDownBtn = page.readyShipDropDownBtn;
                var links = page.readyShipDropDownLinks;
                links.removeClass("active");
                $(event.currentTarget).addClass("active");
                var text = $(event.currentTarget).html();
                dropDownBtn.find(".dropdown-toggle").html(text);
                var shipStatus = $(event.currentTarget).data("statuscode")

                var params = {
                    OrderID: page.detailTable.data("orderid"),
                    OrderStatus: shipStatus,
                };
                $.ajax({
                    url: "/AdminUserOrder/SetShipping",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            debugger;
                            window.location.reload();
                        }
                    }
                });
            },
            // --------------------------------------
            //AddressFocus: function (event) {
            //    if (event) {
            //        var inputField = $(event.currentTarget); inputField.removeClass("disabled"); inputField.closest("tr").removeClass("disabled");
            //    }
            //},
            //AddressBlur: function (event) {
            //    if (event) {
            //        page.DisableListeners();
            //        page.UpdateAddress(event);
            //    }
            //},
            //AddressKeyup: function (event) {
            //    if (event && event.which === 13) {
            //        event.preventDefault();
            //        event.currentTarget.blur();
            //        page.AddressBlur(event);
            //    }
            //},
            //UpdateAddress: function (event) {
            //},
            // --------------------------------------
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
    cardBackgroundClass:        ".card-body.adminUserOrderDetail",
    detailTableHeadClass:        "table.adminUserOrderDetail.detailHead",
    detailTableClass:            "table.adminUserOrderDetail.detailBody",

    readyShipDropDownBtnClass:   "#mgReadyShip",
    readyShipDropDownLinksClass: ".dropdown-item",

    billingAddressInputClass:  "input#BillingAddress",
    shippingAddressInputClass: "input#ShippingAddress",

    productRowsClass:          "tr",
    quantityInputsClass:       "input.mgAjaxText",
    deleteButtonsClass:        "button.mgDeleteX",

    detailTotalQuantityClass:  "#detailTotalQuantity",
    detailTotalCostClass:      "#detailTotalCost",
};
var page = $.adminUserOrderDetail(options);
jQuery(document).ready(page.ready);
