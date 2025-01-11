(function ($) {
    // This file uses the Module Pattern
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    //
    $.adminUserAccIndex = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({
                //'duration': 500
            }, options),
            // --------------------------------------

            // member variables

            lockedOutDropDownBtns: $(options.lockedOutDropDownBtnClass),

            EnableListeners: function () {
                page.lockedOutDropDownBtns.on("click", options.lockedOutDropDownLinksClass, page.LockedOutClick);
            },
            DisableListeners: function () {
                page.lockedOutDropDownBtns.on("click");
            },

            LockedOutClick: function (event) {
                debugger;
                var dropDownBtn = $(event.currentTarget).closest(".mgLockedOutBtn");
                var links = dropDownBtn.find(options.lockedOutDropDownLinksClass);
                links.removeClass("active");
                $(event.currentTarget).addClass("active");
                var text = $(event.currentTarget).html();
                dropDownBtn.find(".dropdown-toggle").html(text);
                var params = {
                    UserID: dropDownBtn.data("userid"),
                    Lock: (text.toLowerCase() == 'yes')
                };
                // DO AJAX CALL TO UPDATE RECORD.
                $.ajax({
                    url: "/AdminUserAcc/LockedOutUpdate",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    success: function (result) {
                        debugger;
                    }
                });
                // END AJAX CALL
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
    lockedOutDropDownBtnClass: ".mgLockedOutBtn",
    lockedOutDropDownLinksClass: ".dropdown-item",
};
var page = $.adminUserAccIndex(options);
jQuery(document).ready(page.ready);
