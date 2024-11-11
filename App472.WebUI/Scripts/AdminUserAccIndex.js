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
            EnableListeners: function (){},
            DisableListeners: function () { },

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
};
var page = $.adminUserAccIndex(options);
jQuery(document).ready(page.ready);
