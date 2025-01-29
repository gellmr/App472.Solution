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
            // Email input
            mgAccTableRow: $(options.mgAccTableRowClass),
            mgAccTableEmailCellInput: $(options.mgAccTableEmailCellInputClass),

            emailRestoreVal: "",
            emailRestoreEl: null,

            Reset: function () {
                page.DisableListeners();
                page.EnableListeners();
                page.emailRestoreVal = "";
                page.emailRestoreEl = null;
            },

            EnableListeners: function () {
                page.lockedOutDropDownBtns.on("click", options.lockedOutDropDownLinksClass, page.LockedOutClick);
                // Email input
                page.mgAccTableEmailCellInput.on("focus", page.EmailFocus);                
            },
            DisableListeners: function () {
                page.lockedOutDropDownBtns.off("click");
                // Email input
                page.mgAccTableRow.off("keyup");
                page.mgAccTableEmailCellInput.off("focus");
                page.mgAccTableEmailCellInput.off("blur");
            },

            // User focused the Email field
            EmailFocus: function (event) {
                console.log("\nFocus " + $(event.currentTarget).val());
                page.DisableListeners();
                page.emailRestoreVal = $(event.currentTarget).val();
                page.emailRestoreEl = $(event.currentTarget);
                page.mgAccTableEmailCellInput.on("blur", page.EmailBlur);
                page.mgAccTableRow.on("keyup", options.mgAccTableEmailCellInputClass, page.EmailKeyup);
            },

            // User blurred the Email field
            EmailBlur: function (event) {
                console.log("  Blur " + $(event.currentTarget).val());
                page.mgAccTableRow.off("keyup");
                page.UpdateEmail(event);
            },

            // User pressed RETURN while editing Email field
            EmailKeyup: function (event) {
                if (event && event.which === 13) {
                    event.preventDefault();
                    console.log("  Keyup " + $(event.currentTarget).val());
                    page.mgAccTableEmailCellInput.off("blur");
                    $(event.currentTarget).blur();
                    page.UpdateEmail(event);
                }
            },

            UpdateEmail: function (event) {
                console.log("    UpdateEmail " + $(event.currentTarget).val());
                var uid = $(event.currentTarget).closest("tr").data("uid");
                var isGuest = $(event.currentTarget).closest("tr").data("isguest");
                var userId = isGuest ? null : parseInt(uid);
                var gid = isGuest ? uid : null;
                var params = {
                    UserID: userId,
                    GuestID: gid,
                    Email: $(event.currentTarget).val(),
                    IsGuest: isGuest
                };
                $.ajax({
                    url: "/AdminUserAcc/UpdateEmail",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            if (!jqXHR.success) {
                                page.emailRestoreEl.val(page.emailRestoreVal);
                                console.log("     failed " + jqXHR.errorMessage);
                            } else {
                                console.log("     success");
                            }
                            page.Reset();
                        }
                    }
                });
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
                page.Reset();
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

    mgAccTableRowClass: "tr.mgAccTable",
    mgAccTableEmailCellInputClass: "td.mgAccEmailCell input",
};
var page = $.adminUserAccIndex(options);
jQuery(document).ready(page.ready);
