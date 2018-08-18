var bindingFunc = null;

$(document).ready(function () { 


$(document).ajaxStart(function () {
    console.log("Done ajax");
});

$(document).ajaxStop(function () {
    console.log("Finished ajax");
});
});

var setBindingFunction = function (fn) {
	bindingFunc = fn;
}

var bindTrashLink = function (table) {
	$("#" + table + " a.js-trash").unbind("click");
	$("#" + table + " a.js-trash").on("click", trashCourse);
};

var bindRestoreLink = function (table) {
	$("#" + table + " a.js-restore").unbind("click");
	$("#" + table + " a.js-restore").on("click", restoreCourse);
};

var bindDeleteLink = function (table) {
	$("#" + table + " a.js-delete").unbind("click");
	$("#" + table + " a.js-delete").on("click", deleteCourse);
};

var trashCourse = function (e) {
	e.preventDefault();

	var link = $(this);
	var tr = link.closest("tr");

	var courseId = link.attr("data-id");

	bootbox.confirm(link.attr("data-name"), function (result) {
		if (result) {
			$.ajax({
				url: link.attr("href"),
				method: "GET",
				success: function (data) {
					tr.replaceWith(data);
					bindingFunc();
				},
				error: function (data) {
					createErrorMsg(data.statusText);
				}
			});
		}
	});
}

var restoreCourse = function (e) {
	e.preventDefault();

	var link = $(this);
	var tr = link.closest("tr");

	$.ajax({
		url: link.attr("href"),
		method: "GET",
		success: function (data) {
			tr.replaceWith(data);
			bindingFunc();
		},
		error: function (data) {
			createErrorMsg(data.statusText);
		}
	});
}

var deleteCourse = function (e) {
	e.preventDefault();

	var link = $(this);
	var tr = link.closest("tr");

	var courseId = link.attr("data-id");

	bootbox.confirm(link.attr("data-name"), function (result) {
		if (result) {

			$.ajax({
				url: link.attr("href"),
				method: "GET",
				success: function (data) {
					tr.remove();
					bindingFunc();
				},
				error: function (data) {
					createErrorMsg(data.statusText);
				}
			});
		}
	});
}

var createErrorMsg = function (text) {
	var msgHtml = '<div id="Msg" class="alert alert-dismissible alert-info">';
	msgHtml += '<button type="button" class="close" data-dismiss="alert">&times;</button>';
	msgHtml += '<span id="MsgTxt">' + text + '</span>';
	msgHtml += '</div>';

	$("#msg-div").html(msgHtml);
}