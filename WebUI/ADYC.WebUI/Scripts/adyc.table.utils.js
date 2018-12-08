; (function (global, $, UH$) {

    var TableHelper = function (options) {
        if (!$) {
            throw 'jQuery was not loaded.';
        }

        if (!UH$) {
            throw 'ADYC.Utils.Helper was not loaded.';
        }

        return new TableHelper.init(options);
    }

    TableHelper.init = function (options) {
        if (!options.table) {
            throw 'Missing table id.';
        }

        if (!options.messageDiv) {
            throw 'Missing message div.';
        }

        this.table = options.table;
        this.messageDiv = options.messageDiv;
        this.options = options;

        this.addAjaxStopListener();
        this.addEvents();
    }

    TableHelper.prototype = {
        self: undefined,
        isTableModified: false,

        addEvents: function () {
            console.log("adding events");

            if (this.options.addTrashEvent === true) {
                this.addTrashEventToLinks();
            }
            if (this.options.addRestoreEvent === true) {
                this.addRestoreEventToLinks();
            }
            if (this.options.addDeleteEvent === true) {
                this.addDeleteEventToLinks();
            }
        },

        addTrashEventToLinks: function () {
            self = this;
            $("#" + this.table + " a.js-trash").unbind("click");
            $("#" + this.table + " a.js-trash").on("click", this.trashCourse);
        },

        addRestoreEventToLinks: function () {
            self = this;
            $("#" + this.table + " a.js-restore").unbind("click");
            $("#" + this.table + " a.js-restore").on("click", this.restoreCourse);
        },

        addDeleteEventToLinks: function () {
            self = this;
            $("#" + this.table + " a.js-delete").unbind("click");
            $("#" + this.table + " a.js-delete").on("click", this.deleteCourse);
        },

        trashCourse: function (e) {
            e.preventDefault();

            var link = $(this);
            var tr = link.closest("tr");

            bootbox.confirm(link.attr("data-name"), function (result) {
                if (result) {
                    $.ajax({
                        url: link.attr("href"),
                        method: "GET",
                        success: function (data) {
                            tr.replaceWith(data);
                            self.isTableModified = true;

                            UH$.showMsg("Your changes have been saved successfully.", UH$.MessageTypeEnum.SUCCESS, self.messageDiv);
                        },
                        error: function (data) {
                            UH$.showMsg(data.statusText, UH$.MessageTypeEnum.WARNING, self.messageDiv);
                        }
                    });
                }
            });
        },

        restoreCourse: function (e) {
            e.preventDefault();

            var link = $(this);
            var tr = link.closest("tr");

            $.ajax({
                url: link.attr("href"),
                method: "GET",
                success: function (data) {
                    tr.replaceWith(data);
                    self.isTableModified = true;

                    UH$.showMsg("Your changes have been saved successfully.", UH$.MessageTypeEnum.SUCCESS, self.messageDiv);
                },
                error: function (data) {
                    UH$.showMsg(data.statusText, UH$.MessageTypeEnum.WARNING, self.messageDiv);
                }
            });
        },

        deleteCourse: function (e) {
            e.preventDefault();

            var link = $(this);
            var tr = link.closest("tr");

            bootbox.confirm(link.attr("data-name"), function (result) {
                if (result) {

                    $.ajax({
                        url: link.attr("href"),
                        method: "GET",
                        success: function (data) {
                            tr.remove();
                            self.isTableModified = true;

                            UH$.showMsg("Your changes have been saved successfully.", UH$.MessageTypeEnum.SUCCESS, self.messageDiv);
                        },
                        error: function (data) {
                            UH$.showMsg(data.statusText, UH$.MessageTypeEnum.WARNING, self.messageDiv);
                        }
                    });
                }
            });
        },

        addAjaxStopListener: function () {
            self = this;

            $(document).ajaxStop(function () {
                if (self.isTableModified === true) {
                    self.isTableModified = !self.isTableModified;
                    self.addEvents();
                }
            });
        }
    };

    TableHelper.init.prototype = TableHelper.prototype;
    global.TableHelper = global.TH$ = TableHelper;

}(window, jQuery, UH$));