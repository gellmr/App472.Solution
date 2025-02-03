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
            ajaxErrors: $(options.ajaxErrorsEl),
            mgAccTableRow: $(options.mgAccTableRowClass),
            userCellInput: $(options.userCellInputClass),
            emailCellInput: $(options.emailCellInputClass),
            phoneCellInput: $(options.phoneCellInputClass),
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
                page.userCellInput.each(page.SetRestoreValue);
                page.emailCellInput.each(page.SetRestoreValue);
                page.phoneCellInput.each(page.SetRestoreValue);
            },
            SetRestoreValue: function (index, element) {
                element.InputRestoreValue = $(element).val();
            },
            EnableListeners: function () {
                //console.log("EnableListeners");
                page.mgAccTableRow.on("focusin", options.userCellInputClass,  page.UsernameFocus);
                page.mgAccTableRow.on("focusin", options.emailCellInputClass, page.EmailFocus);
                page.mgAccTableRow.on("focusin", options.phoneCellInputClass, page.PhoneFocus);
                page.lockedOutDropDownBtns.on("click", options.lockedOutDropDownLinksClass, page.LockedOutClick);
            },
            DisableListeners: function () {
                page.mgAccTableRow.off("keyup");
                page.mgAccTableRow.off("focusin");
                page.mgAccTableRow.off("focusout");
                page.lockedOutDropDownBtns.off("click");
            },
            // ----------------------------------------------------------------
            EnableListenersUsername: function (selector) {
                page.mgAccTableRow.on("focusout", selector, page.UsernameBlur); page.mgAccTableRow.on("keyup", selector, page.UsernameKeyup);
            },
            DisableListenersUsername: function (selector) {
                page.mgAccTableRow.off("focusout", selector, page.UsernameBlur); page.mgAccTableRow.off("keyup", selector, page.UsernameKeyup);
            },
            EnableListenersEmail: function (selector) {
                page.mgAccTableRow.on("focusout", selector, page.EmailBlur); page.mgAccTableRow.on("keyup", selector, page.EmailKeyup);
            },
            DisableListenersEmail: function (selector) {
                page.mgAccTableRow.off("focusout", selector, page.EmailBlur); page.mgAccTableRow.off("keyup", selector, page.EmailKeyup);
            },
            EnableListenersPhone: function (selector) {
                page.mgAccTableRow.on("focusout", selector, page.PhoneBlur); page.mgAccTableRow.on("keyup", selector, page.PhoneKeyup);
            },
            DisableListenersPhone: function (selector) {
                page.mgAccTableRow.off("focusout", selector, page.PhoneBlur); page.mgAccTableRow.off("keyup", selector, page.PhoneKeyup);
            },
            // ----------------------------------------------------------------
            UsernameFocus: function (event) {
                var ct = $(event.target); ct.css("background-color", page.focusColor);
                console.log("UsernameFocus " + ct.val());
                ct[0].InputRestoreValue = ct.val(); page.EnableListenersUsername("input#" + ct.attr("id"));
            },
            UsernameBlur: function (event) {
                var ct = $(event.target); ct.css("background-color", page.blurColor);
                console.log("  UsernameBlur " + ct.val());
                page.UpdateUsername(event);
            },
            UsernameKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) {
                    console.log("  UsernameKeyup " + ct.val());
                    ct.css("background-color", page.keyupColor); event.preventDefault();
                    page.UpdateUsername(event);
                }
            },
            UpdateUsername: function (event) {
                var ct = $(event.target);
                page.DisableListenersUsername("input#" + ct.attr("id"));
                if (ct.val() == ct[0].InputRestoreValue) {
                    page.ajaxErrors.html("");
                    return;
                }
                console.log("    UpdateUsername " + ct.val());
                var uid = ct.closest("tr").data("uid");
                var isGuest = ct.closest("tr").data("isguest");
                var userId = isGuest ? null : parseInt(uid);
                var gid = isGuest ? uid : null;
                var params = { UserID: userId, GuestID: gid, Username: ct.val(), IsGuest: isGuest };
                ct.css("background-color", page.waitingColor);
                ct.attr("disabled", "disabled");
                $.ajax({
                    url: "/AdminUserAcc/UpdateUsername", type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", data: JSON.stringify(params),
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
                                console.log("     M: " + jqXHR.errorMessage);
                                ct.css("background-color", page.errorColor);
                                ct.val(ct[0].InputRestoreValue);
                            }
                        }
                    }
                });
            },
            // ----------------------------------------------------------------
            EmailFocus: function (event) {
                var ct = $(event.target); ct.css("background-color", page.focusColor);
                console.log("EmailFocus " + ct.val());
                ct[0].InputRestoreValue = ct.val(); page.EnableListenersEmail("input#" + ct.attr("id"));
            },
            EmailBlur: function (event) {
                var ct = $(event.target); ct.css("background-color", page.blurColor);
                console.log("  EmailBlur " + ct.val());
                page.UpdateEmail(event);
            },
            EmailKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) { // User pressed RETURN while editing Email field
                    ct.css("background-color", page.keyupColor); event.preventDefault(); //console.log("  Keyup " + ct.val());
                    page.UpdateEmail(event);
                }
            },
            UpdateEmail: function (event) {
                var ct = $(event.target);
                page.DisableListenersEmail("input#" + ct.attr("id"));
                if (ct.val() == ct[0].InputRestoreValue) {
                    page.ajaxErrors.html(""); //console.log("    No change");
                    return;
                }
                console.log("    UpdateEmail " + ct.val());
                var uid = ct.closest("tr").data("uid");
                var isGuest = ct.closest("tr").data("isguest");
                var userId = isGuest ? null : parseInt(uid);
                var gid = isGuest ? uid : null;
                var params = { UserID: userId, GuestID: gid, Email: ct.val(), IsGuest: isGuest };
                ct.css("background-color", page.waitingColor);
                ct.attr("disabled", "disabled");
                $.ajax({
                    url: "/AdminUserAcc/UpdateEmail", type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", data: JSON.stringify(params),
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
            // ----------------------------------------------------------------
            PhoneFocus: function (event) {
                var ct = $(event.target); ct.css("background-color", page.focusColor);
                console.log("PhoneFocus " + ct.val());
                ct[0].InputRestoreValue = ct.val(); page.EnableListenersPhone("input#" + ct.attr("id"));
            },
            PhoneBlur: function (event) {
                var ct = $(event.target); ct.css("background-color", page.blurColor);
                console.log("  PhoneBlur " + ct.val());
                page.UpdatePhone(event);
            },
            PhoneKeyup: function (event) {
                var ct = $(event.currentTarget);
                if (event && event.which === 13) {
                    console.log("  PhoneKeyup " + ct.val());
                    ct.css("background-color", page.keyupColor); event.preventDefault();
                    page.UpdatePhone(event);
                }
            },
            UpdatePhone: function (event) {
                var ct = $(event.target);
                page.DisableListenersPhone("input#" + ct.attr("id"));
                if (ct.val() == ct[0].InputRestoreValue) {
                    page.ajaxErrors.html("");
                    return;
                }
                console.log("    UpdatePhone " + ct.val());
                var uid = ct.closest("tr").data("uid");
                var isGuest = ct.closest("tr").data("isguest");
                var userId = isGuest ? null : parseInt(uid);
                var gid = isGuest ? uid : null;
                var params = { UserID: userId, GuestID: gid, Phone: ct.val(), IsGuest: isGuest };
                ct.css("background-color", page.waitingColor);
                ct.attr("disabled", "disabled");
                $.ajax({
                    url: "/AdminUserAcc/UpdatePhone", type: 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json", data: JSON.stringify(params),
                    statusCode: {
                        200: function (jqXHR) {
                            ct.removeAttr('disabled');
                            if (jqXHR.success) {
                                console.log("     Success phone: " + jqXHR.phone);
                                page.ajaxErrors.html("");
                                ct.css("background-color", page.successColor);
                                ct.blur();
                            } else {
                                page.ajaxErrors.html(jqXHR.errors[0]);
                                console.log("     M: " + jqXHR.errorMessage);
                                ct.css("background-color", page.errorColor);
                                ct.val(ct[0].InputRestoreValue);
                            }
                        }
                    }
                });
            },
            // ----------------------------------------------------------------
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
    userCellInputClass: "td.mgAccUserCell input",
    emailCellInputClass: "td.mgAccEmailCell input",
    phoneCellInputClass: "td.mgAccPhoneCell input",
    lockedOutDropDownBtnClass: ".mgLockedOutBtn",
    lockedOutDropDownLinksClass: ".dropdown-item",
};
var page = $.adminUserAccIndex(options);
jQuery(document).ready(page.ready);
