(function ($) {
    // This file uses the Module Pattern
    // https://alistapart.com/article/the-design-of-code-organizing-javascript/
    //
    $.adminUserAccIndex = function (options) {
        var page = {
            // Merge the passed options, into our internal options
            options: $.extend({ //'duration': 500
            }, options),
            // --------------------------------------

            // member variables - page elements
            ajaxErrors:            $(options.ajaxErrorsEl),
            mgAccTableRow:         $(options.mgAccTableRowClass),
            emailCellInput:        $(options.emailCellInputClass),
            lockedOutDropDownBtns: $(options.lockedOutDropDownBtnClass),

            // member variables - for logic
            focusColor: "unset", // "#9fefff",
            blurColor: "unset", // "rgb(255 243 122)",
            keyupColor: "unset", // "#98ea98",
            waitingColor: "lightgray",
            errorColor: "#ff8686", // "#ff8686",
            successColor: "#98ea98", // "#ff8686",

            Reset: function () {
                //console.log("Reset");
                page.DisableListeners();
                $(':focus').blur(); // blur anything that has focus
                page.EnableListeners();
                page.emailCellInput.each(function (index, element) {
                    element.InputRestoreValue = $(element).val();
                });
            },

            EnableListenersEmail: function (emailInput) {
                var selector = "input#" + emailInput.attr("id");
                page.mgAccTableRow.on("focusout", selector, page.EmailBlur);
                page.mgAccTableRow.on("keyup", selector, page.EmailKeyup);
            },
            DisableListenersEmail: function (emailInput) {
                var selector = "input#" + emailInput.attr("id");
                page.mgAccTableRow.off("focusout", selector, page.EmailBlur);
                page.mgAccTableRow.off("keyup", selector, page.EmailKeyup);
            },

            EnableListeners: function () {
                //console.log("EnableListeners");
                page.mgAccTableRow.on("focusin", options.emailCellInputClass, page.EmailFocus);
                page.lockedOutDropDownBtns.on("click", options.lockedOutDropDownLinksClass, page.LockedOutClick);
            },
            DisableListeners: function () {
                page.emailCellInput.off("mousedown");
                page.mgAccTableRow.off("keyup");
                page.mgAccTableRow.off("focusin");
                page.mgAccTableRow.off("focusout");
                page.lockedOutDropDownBtns.off("click");
            },
            EmailFocus: function (event) {
                var ct = $(event.target);
                ct.css("background-color", page.focusColor);
                //console.log("Focus " + ct.val());
                ct[0].InputRestoreValue = ct.val();
                page.EnableListenersEmail(ct);
            },
            EmailBlur: function (event) {
                var ct = $(event.target);
                ct.css("background-color", page.blurColor);
                //console.log("  Blur " + ct.val());
                page.UpdateEmail(event);
            },
            EmailKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) { // User pressed RETURN while editing Email field
                    ct.css("background-color", page.keyupColor);
                    event.preventDefault();
                    //console.log("  Keyup " + ct.val());
                    page.UpdateEmail(event);
                }
            },
            UpdateEmail: function (event) {
                var ct = $(event.target);
                page.DisableListenersEmail(ct);
                if (ct.val() == ct[0].InputRestoreValue) {
                    page.ajaxErrors.html("");
                    //console.log("    No change");
                    return;
                }
                //console.log("    UpdateEmail " + ct.val());
                var uid = ct.closest("tr").data("uid");
                var isGuest = ct.closest("tr").data("isguest");
                var userId = isGuest ? null : parseInt(uid);
                var gid = isGuest ? uid : null;
                var params = {
                    UserID: userId,
                    GuestID: gid,
                    Email: ct.val(),
                    IsGuest: isGuest
                };
                ct.css("background-color", page.waitingColor);
                ct.attr("disabled", "disabled");
                $.ajax({
                    url: "/AdminUserAcc/UpdateEmail",
                    type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            ct.removeAttr('disabled');
                            if (jqXHR.success) {
                                console.log("     Success email: " + jqXHR.email);
                                page.ajaxErrors.html("");
                                ct.css("background-color", page.successColor);
                                ct.blur();
                            } else {
                                page.ajaxErrors.html(jqXHR.errors[0]);
                                //console.log("     M: " + jqXHR.errorMessage);
                                ct.css("background-color", page.errorColor);
                                ct.val(ct[0].InputRestoreValue);
                            }
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
    ajaxErrorsEl: "#ajaxErr",
    mgAccTableRowClass: "tr.mgAccTable",
    emailCellInputClass: "td.mgAccEmailCell input",
    lockedOutDropDownBtnClass: ".mgLockedOutBtn",
    lockedOutDropDownLinksClass: ".dropdown-item",
};
var page = $.adminUserAccIndex(options);
jQuery(document).ready(page.ready);
