(function ($) {
    // This file uses the Module Pattern
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    //
    $.adminOrderIndex = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({
                //'duration': 500
            }, options),
            // --------------------------------------
            // member variables
            sortingColumnsRow: $(options.sortingColumnsRowClass),
            columnButtons: $(options.columnButtonsClass),
            ColumnButtonClicked: function (event) {
                debugger;
                event.preventDefault(); //stop default behaviour
                var formHref = page.sortingColumnsRow.data("href");
                var recent = page.sortingColumnsRow.data("recent");
                var column = $(event.currentTarget);
                var ascending = column.data("asc"); // get value
                var sortby = column.data("sortby");
                var isAsc = ascending.toString().toLowerCase() === "true"; // get boolean value
                if (sortby == recent) {
                    isAsc = !(isAsc); // negate
                }
                formHref = formHref + "?SortBy=" + sortby + "&SortAscend=" + isAsc + "&Recent=" + recent; // query string
                window.location.href = formHref;
            },
            EnableListeners: function () {
                page.columnButtons.on("click", page.ColumnButtonClicked);
            },
            DisableListeners: function () {
                page.columnButtons.off("click");
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
    sortingColumnsRowClass: "tr.mgAccTable",
    columnButtonsClass: "a.mg-th-link"
};
var page = $.adminOrderIndex(options);
jQuery(document).ready(page.ready);
