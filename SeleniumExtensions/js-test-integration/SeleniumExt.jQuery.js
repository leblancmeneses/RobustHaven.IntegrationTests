$(function() {
	$(document).ajaxStart(function () {
		window.runningAjaxCount++;
	});

	$(document).ajaxStop(function () {
		window.runningAjaxCount--;
	});
});
 