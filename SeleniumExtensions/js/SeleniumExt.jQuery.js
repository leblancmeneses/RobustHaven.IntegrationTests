(function($) {
	if (typeof $ != 'undefined') {
		$(function() {
			$(document).ajaxStart(function() {
				window.runningAjaxCount++;
			});

			$(document).ajaxStop(function() {
				window.runningAjaxCount--;
			});
		});
	}
})(window.jQuery); 
 