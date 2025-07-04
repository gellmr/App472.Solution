﻿(function ($) {
    // This file uses the Module Pattern
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    //
    $.productList = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({}, options),

            // Member variables
            searchInput: $(options.searchInputId),
            renderBody: $(options.renderBodyClass),
            xClearBtn: $(options.xClearBtnClass),
            hostandpath: options.hostandpath, // eg "mikegelldemo.live" | "mikegelldemo.live/Soccer" | "mikegelldemo.live/Soccer/page2"
            searching: false,

            ClearSearchClicked: function (event) {
                page.searchInput.val(""); // Clear the search input
                page.SearchProducts(event, true);
            },
            SearchInputClicked: function (event) {
                event.preventDefault();
                if (page.searchInput.val() == "Search") {
                    page.searchInput.val(""); // Clear the search input
                }
                page.xClearBtn.hide();
            },
            SearchInputBlur: function (event) {
                page.SearchProducts(event);
            },
            SearchInputKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) {
                    page.SearchProducts(event);
                } else if (event && event.which === 27) {
                    page.Reset();
                    page.searchInput.val("");
                    page.searchInput.blur();
                }
            },
            SearchProducts: function (event, clear=false)
            {
                if (page.searching) {
                    event.preventDefault(); return;
                }
                page.searching = true; // Prevent race condition
                page.DisableListeners();

                // Get the search term eg "soccer"
                var ct = $(event.target);
                console.log("SearchProducts " + ct.val());
                var searchString = ct.val(); // "soccer ball"
                var debug = false;
                if (!debug) {
                    searchString = encodeURIComponent(searchString);
                }
                var searchString = (searchString) ? ("search=" + searchString) : ""; // search=soccer ball
                var clearStr     = (clear == true) ? ("clear=true") : "";
                var amp = (searchString) && (clearStr) ? "&" : ""; // Null and empty string are both falsey values in javascript.

                // Construct url including our query string
                // Note - we have to encode the URL so it can be put into a GET string.
                // https://stackoverflow.com/questions/332872/encode-url-in-javascript
                var productsUrl = page.hostandpath + "?" + searchString + amp + clearStr; // "mikegelldemo.live/Soccer/page2?search=soccer%20ball&clear%3Dfalse"
                $.ajax({
                    url: productsUrl,
                    type: 'GET',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    statusCode: {
                        200: function (jqXHR) {
                            console.log("(success)");
                            var pageFragment = jqXHR.responseText;
                            $(".mg-renderbody").html(pageFragment);
                            page.Reset();
                        },
                        400: function (jqXHR) {
                            console.log("400 error");
                            page.Reset();
                            page.searchInput.val(""); // Clear the search input
                        },
                        500: function (jqXHR) {
                            console.log("500 error");
                            page.Reset();
                            page.searchInput.val(""); // Clear the search input
                        }
                    }
                });
            },

            EnableListeners: function () {
                page.searchInput.on("click", page.SearchInputClicked);
                page.searchInput.on("blur", page.SearchInputBlur);
                page.renderBody.on("keyup", options.searchInputId, page.SearchInputKeyup);
                page.xClearBtn.on("click", page.ClearSearchClicked);
            },
            DisableListeners: function () {
                page.searchInput.off("click");
                page.searchInput.off("blur");
                page.renderBody.off("keyup");
                page.xClearBtn.off("click");
            },
            // --------------------------------------
            // Page ready, attach event listeners
            ReadyJs: function () {
                page.Reset();
            },
            Reset: function () {
                page.searchInput = $(page.options.searchInputId);
                page.renderBody  = $(page.options.renderBodyClass);
                page.xClearBtn   = $(page.options.xClearBtnClass);
                page.hostandpath = page.options.hostandpath;
                page.searching = false;
                page.DisableListeners();
                page.EnableListeners();
            }
        };
        return {
            ready: page.ReadyJs
        };
    };
})(jQuery);

var options = {
    searchInputId: "#productSearchInput",
    renderBodyClass: ".mg-renderbody",
    hostandpath: $(".mg-curr-category").data("hostandpath"),
    xClearBtnClass: ".search-x",
};
var page = $.productList(options);
jQuery(document).ready(page.ready);
