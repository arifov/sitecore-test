(function () {
	'use strict';

	angular
		.module('app')
		.controller('HomeController', HomeController);

	HomeController.$inject = ['$rootScope', '$scope', 'LocationService', 'AuthenticationService'];
	function HomeController($rootScope, $scope, LocationService, AuthenticationService) {
		var vm = this;

		vm.logout = logout;
		getLogin();

		function getLogin() {
			AuthenticationService.GetUser(function success(userName) {
				vm.userName = userName;
			});
		}

		function logout() {
			AuthenticationService.Logout(function success() {
				AuthenticationService.ClearCredentials();
				LocationService.RedirectToLogin();
			});
		}
	}

})();
