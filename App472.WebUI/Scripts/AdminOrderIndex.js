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
            // Member variables
            ordersTable:       $(options.ordersTableClass),
            sortingColumnsRow: $(options.sortingColumnsRowClass),
            columnButtons:     $(options.columnButtonsClass),
            highlightRow:      $(options.highlightRowClass),

            ColumnSortButtonClicked: function (event) {
                event.preventDefault();
                var cell = $(event.target);
                var table = cell.closest("table");
                var formHref = table.data("href");
                var recent = table.data("recent");
                var sortby = cell.data("sortby");
                var ascending = cell.data("asc");
                var isAsc = ascending.toString().toLowerCase() === "true"; // Get boolean value
                if (sortby == recent) {
                    isAsc = !(isAsc); // Negate
                }
                formHref = formHref + "?SortBy=" + sortby + "&SortAscend=" + isAsc + "&Recent=" + recent; // Query string
                window.location.href = formHref;
            },
            HighlightRowClicked: function (event) {
                event.preventDefault();
                var table = $(event.currentTarget);
                var cell = $(event.target);
                var row = cell.closest("tr");
                var id = parseInt(row.data("orderid"));
                var hostandpath = row.data("href");
                var detailHref = hostandpath + "/" + id.toString();
                window.location.href = detailHref;
            },

            EnableListeners: function () {
                page.sortingColumnsRow.on("click", page.columnButtons, page.ColumnSortButtonClicked);
                page.highlightRow.on("click", page.HighlightRowClicked);
            },
            DisableListeners: function () {
                page.sortingColumnsRow.off("click");
                page.highlightRow.off("click");
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
    ordersTableClass:       "#ordersTable",
    sortingColumnsRowClass: "tr.mgAccTable.mgTableHeader",
    columnButtonsClass:     "a.mg-th-link",
    highlightRowClass:      "tr.mgAccTable.mgHighlightRow"
};
var page = $.adminOrderIndex(options);
jQuery(document).ready(page.ready);
