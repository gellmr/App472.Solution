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

            // Convert ASP.NET JSON date format into milliseconds
            // See https://davidsekar.com/javascript/converting-json-date-string-date-to-date-object
            GetDateFromAspNetFormat: function(date) {
                const re = /-?\d+/;
                const m = re.exec(date);
                var tenDigits = parseInt(m[0], 10);
                var dayJsTimestamp = dayjs(tenDigits); // use dayJs to include the timezone.
                return dayJsTimestamp;
            },

            LockedOutClick: function (event) {
                var dropDownBtn = $(event.currentTarget).closest(".mgLockedOutBtn");
                var uid = dropDownBtn.data("userid");
                var links = dropDownBtn.find(options.lockedOutDropDownLinksClass);
                links.removeClass("active");
                $(event.currentTarget).addClass("active");
                var text = $(event.currentTarget).html();
                dropDownBtn.find(".dropdown-toggle").html(text);
                var params = {
                    UserID: uid,
                    Lock: (text.toLowerCase() == 'yes')
                };
                // DO AJAX CALL TO UPDATE RECORD.
                $.ajax({
                    url: "/AdminUserAcc/LockedOutUpdate",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    myuid: uid,
                    success: function (result) {
                        var lockoutElement = $("td#lockoutUTC-" + this.myuid);
                        var accessFailElement = $("td#accessFailed-" + this.myuid);
                        debugger;
                        var utc = result.LockoutEndDateUtc;
                        var attempts = result.Attempts;
                        if (utc != null) {
                            var dayjstime = page.GetDateFromAspNetFormat(result.LockoutEndDateUtc);
                            var lockoutEndDateUtc = dayjstime.format('DD/MM/YYYY h:mm:ss A'); // See https://day.js.org/docs/en/display/format
                            lockoutElement.html(lockoutEndDateUtc);
                            accessFailElement.html(attempts);
                        } else {
                            lockoutElement.html("");
                            accessFailElement.html(attempts);
                        }
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
