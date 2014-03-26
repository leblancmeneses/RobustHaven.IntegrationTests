//http://stackoverflow.com/questions/4231580/get-if-browser-is-busy

window.runningAjaxCount = 0;

function isCallingAjax() {
	return window.runningAjaxCount > 0;
}

function isBrowserBusy() {
	return document.readyState != "complete" || isCallingAjax();
}