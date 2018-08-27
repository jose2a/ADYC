var assert = chai.assert;
var expect = chai.expect;
//var jQuery = require('jquery');

describe('constructor', function () {
    

    it('should call TableHelper.init', function () {
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
});