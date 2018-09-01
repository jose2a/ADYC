var assert = chai.assert;
var expect = chai.expect;

describe('adyc.table.utils', function () {

    describe('constructor', function () {


        it('should call TableHelper function', function () {
            var tableHelperInit = sinon.stub(TH$, 'init');

            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: true,
                addDeleteEvent: true
            });

            tableHelperInit.restore();
            sinon.assert.calledOnce(tableHelperInit);
        });

        it('should call TableHelper.init', function () {
            var addAjaxStopListener = sinon.stub(TH$.prototype, 'addAjaxStopListener');
            var addEvents = sinon.stub(TH$.prototype, 'addEvents');

            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: true,
                addDeleteEvent: true
            });

            addAjaxStopListener.restore();
            addEvents.restore();

            sinon.assert.calledOnce(addAjaxStopListener);
            sinon.assert.calledOnce(addEvents);
        });
    });

    describe('trashCourseAjax', function () {
        var table;

        beforeEach(function () {
            table = $("<table id='courses'>");
            var tr = "<tr>" +
                "<td>1</td>" +
                "<td>Gym</td>" +
                "<td>External</td>" +
                "<td><a href='courses/edit/1' class='btn btn-xs btn-primary'>" +
                "<i class='glyphicon glyphicon-edit'></i> Edit" +
                "</a>" +
                "<div class='btn-group btn-group-xs'>" +
                "<a href='courses/trash/1' data-id='1' class='btn btn-danger js-trash' data-name='Are you sure you want to trash Gym?'><i class='glyphicon glyphicon-remove'></i> Trash</a>" +
                "<a href='#' class='btn btn-danger dropdown-toggle' data-toggle='dropdown'><span class='caret'></span></a>" +
                "<ul class='dropdown-menu'>" +
                "<li>" +
                "<a href='courses/delete/1' data-id='1' class='js-delete' data-name='Are you sure you want to delete Gym FOREVER?'>" +
                "<i class='glyphicon glyphicon-remove'></i> Delete" +
                "</a>" +
                "</li>" +
                "</ul>" +
                "</div>" +
                "</td>" +
                "</tr>";
            table.append(tr);
            $(document.body).append(table);
        });

        it('"should call courses/trash/1 on the server', function () {
            var bootboxCfrm = sinon.stub(bootbox, 'confirm');
            bootboxCfrm.withArgs('Are you sure you want to trash Gym?', sinon.stub().returns(true));

            var addAjaxStopListener = sinon.stub(TH$.prototype, 'addAjaxStopListener');

            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: false,
                addDeleteEvent: false
            });

            $("#courses a.js-trash")[0].click();

            bootboxCfrm.restore();
            sinon.assert.calledOnce(bootboxCfrm);
        });

        afterEach(function () {
            table.remove();
            table = null;
        });
    });
});