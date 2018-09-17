var assert = chai.assert;
var expect = chai.expect;

describe('adyc.table.utils', function () {

    describe('constructor', function () {


        it('should call TableHelper init function', function () {
            // arrange
            var tableHelperInit = sinon.stub(TH$, 'init');

            // act
            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: true,
                addDeleteEvent: true
            });
            
            // assert
            sinon.assert.calledOnce(tableHelperInit);

            tableHelperInit.restore();
        });

        it('should call TableHelper addAjaxStopListener and addEvents', function () {
            // arrange
            var addAjaxStopListener = sinon.stub(TH$.prototype, 'addAjaxStopListener');
            var addEvents = sinon.stub(TH$.prototype, 'addEvents');

            // act
            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: true,
                addDeleteEvent: true
            });

            // assert
            sinon.assert.calledOnce(addAjaxStopListener);
            sinon.assert.calledOnce(addEvents);

            addAjaxStopListener.restore();
            addEvents.restore();
        });
    });

    describe('trashCourseAjax', function () {
        var table;
        var tr;

        beforeEach(function () {
            table = $("<table id='courses'>");
            tr = "<tr>" +
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

        it('"should call link.click() event and bootbox.confirm() should be called', function () {
            // arrange
            var bootboxCfrm = sinon.stub(bootbox, 'confirm').returns(true);

            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: false,
                addDeleteEvent: false
            });

            // act
            $("#courses a.js-trash")[0].click();

            // assert
            sinon.assert.calledOnce(bootboxCfrm);

            bootboxCfrm.restore();
        });

        it('"should call trashCourseAjax', function (done) {
            var ajaxStub = sinon.stub($, "ajax").yieldsTo("success", "");

            var tableHelper = TH$({
                table: "courses",
                messageDiv: "#msg-div",
                addTrashEvent: true,
                addRestoreEvent: false,
                addDeleteEvent: false
            });

            tableHelper.trashCourseAjax("/courses/trash/2", $("#courses").closest("tr"));

            done();
            
            // assertions 
            sinon.assert.calledOnce(ajaxStub);
            ajaxStub.restore();
        });

        afterEach(function () {
            table.remove();
            table = null;
            tr = null;
        });
    });
});