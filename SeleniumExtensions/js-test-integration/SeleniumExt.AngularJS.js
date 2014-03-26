angular.module('SeleniumExt', [])
	.factory('SeleniumExtAjaxCounterHttpInterceptor', ['$q', function ($q) {
		return {
			'request': function(config) {
				window.runningAjaxCount++;
				return config || $q.when(config);
			},

			'response': function(response) {
				window.runningAjaxCount--;
				return response || $q.when(response);
			},

			'responseError': function(rejection) {
				window.runningAjaxCount--;
				return $q.reject(rejection);
			}
		};
	}])
	.config(['$httpProvider', '$provide', function ($httpProvider, $provide) {
		$httpProvider.interceptors.push('SeleniumExtAjaxCounterHttpInterceptor');
	}]);