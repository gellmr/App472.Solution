(function ($) {
    // This file uses the Module Pattern
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    //
    $.productList = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({
                //'duration': 500
            }, options),
            // --------------------------------------
            // member variables
            searchInput: $(options.searchInputId),
            renderBody: $(options.renderBodyClass),

            SearchInputClicked: function (event) {
                event.preventDefault(); //stop default behaviour
                if (page.searchInput.val() == "Search") {
                    page.searchInput.val(""); // clear the search input
                }
            },
            SearchInputBlur: function (event) {
                page.SearchProducts(event);
            },
            SearchInputKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) {
                    page.SearchProducts(event);
                }
            },
            SearchProducts: function (event) {
                debugger;
                var ct = $(event.target);
                console.log("SearchProducts " + ct.val());
                // TODO - ajax call
            },

            EnableListeners: function () {
                page.searchInput.on("click", page.SearchInputClicked);
                page.searchInput.on("blur", page.SearchInputBlur);
                page.renderBody.on("keyup", options.searchInputId, page.SearchInputKeyup);
            },
            DisableListeners: function () {
                page.searchInput.off("click");
            },
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
    searchInputId: "#productSearchInput",
    renderBodyClass: ".mg-renderbody"
};
var page = $.productList(options);
jQuery(document).ready(page.ready);
