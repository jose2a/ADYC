// Global module
var adycUtils = (function (jQ) {

    if (!jQ) {
        throw 'jQuery was not loaded.';
    }
 
    function getInfoMsg(text) {
        var msgHtml = '<div " class="alert alert-info alert-dismissible">';
        msgHtml += '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>';
        msgHtml += '<b><i class="icon fa fa-info"></i> Info!</b> ';
        msgHtml += '<span>' + text + '</span>';
        msgHtml += '</div>';

        return msgHtml;
    }

    function getWarningMsg(text) {
        var msgHtml = '<div " class="alert alert-warning alert-dismissible">';
        msgHtml += '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>';
        msgHtml += '<b><i class="icon fa fa-warning"></i> Warning!</b> ';
        msgHtml += '<span>' + text + '</span>';
        msgHtml += '</div>';

        return msgHtml;
    }

    function getSuccessMsg(text) {
        var msgHtml = '<div " class="alert alert-success alert-dismissible">';
        msgHtml += '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>';
        msgHtml += '<b><i class="icon fa fa-check"></i> Great!</b> ';
        msgHtml += '<span>' + text + '</span>';
        msgHtml += '</div>';

        return msgHtml;
    }

    function getErrorMsg(text) {
        var msgHtml = '<div " class="alert alert-danger alert-dismissible">';
        msgHtml += '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>';
        msgHtml += '<b><i class="icon fa fa-ban"></i> Error!</b> ';
        msgHtml += '<span>' + text + '</span>';
        msgHtml += '</div>';

        return msgHtml;
    }
 
    return {
        MessageTypeEnum : {
            INFO: 1,
            WARNING: 2,
            SUCCESS: 3,
            ERROR: 4
        },

        showMsg: function (text, type, messageDiv) {
            if (text === undefined || text.trim() === "") {
                throw "Text is required."
            }

            if (type === undefined) {
                throw "Message type is required."
            }

            if (messageDiv === undefined || messageDiv.trim() === "") {
                throw "Div to insert message is required."
            }

            var msg = "";

            if (type === this.MessageTypeEnum.INFO) {
                msg = getInfoMsg(text);
            }
            else if (type === this.MessageTypeEnum.WARNING) {
                msg = getWarningMsg(text);
            }
            else if (type === this.MessageTypeEnum.SUCCESS) {
                msg = getSuccessMsg(text);
            }
            else if (type === this.MessageTypeEnum.ERROR) {
                msg = getErrorMsg(text);
            }
            
            $("#" + messageDiv).html(msg);
        }
    }
 
// Pull in jQuery
})(jQuery);

window.UtilsHelper = window.UH$ = adycUtils;