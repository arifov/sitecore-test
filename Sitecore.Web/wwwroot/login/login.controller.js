(function () {
	'use strict';

	angular
		.module('app')
		.controller('LoginController', LoginController);

	LoginController.$inject = ['LocationService', 'AuthenticationService', 'FlashService'];
	function LoginController(LocationService, AuthenticationService, FlashService) {
		var vm = this;

		vm.login = login;
		vm.register = register;

		(function initController() {
			// reset login status
			AuthenticationService.ClearCredentials();

			checkLocationHash();
		})();

		function login() {
			vm.dataLoading = true;
			AuthenticationService.Login(vm.username, vm.password,
				function success(response) {
					AuthenticationService.SetCredentials(response.email, response.access_token);
					LocationService.RedirectToHomeLocation();
				}, function error(response) {
					FlashService.Error(response.Error[0]);
					vm.dataLoading = false;
			});

		};

		function register() {
			LocationService.RedirectToRegister();
		}

		function checkLocationHash() {
			if (location.hash) {
				var hash = location.hash.split('access_token=');
				if (hash.length >= 2) {
					var accessToken = hash[1].split('&')[0];
					if (accessToken) {
						AuthenticationService.CheckRegistration(accessToken, function success(response) {
							if (response != '') {
								successLogin(response.UserName, accessToken);
							}
						},
						function error(response) {
							FlashService.Error(response.Message);
						});
					}
				}
			}
		}

		function successLogin(userName, accessToken) {
			AuthenticationService.SetCredentials(userName, accessToken);
			LocationService.RedirectToHomeLocation();
		}
	}

})();
