(function () {
	'use strict';

	angular
		.module('app')
		.factory('AuthenticationService', AuthenticationService);

	AuthenticationService.$inject = ['$http', '$rootScope'];
	function AuthenticationService($http, $rootScope) {
		var service = {};

		service.Login = Login;
		service.Logout = Logout;
		service.SetCredentials = SetCredentials;
		service.ClearCredentials = ClearCredentials;
		service.CheckRegistration = CheckRegistration;
		service.Register = Register;
		service.GetUser = GetUser;

		return service;

		function Login(username, password, callback, errorCallback) {
			var loginData = {
				grant_type: 'password',
				email: encodeURIComponent(username),
				password: encodeURIComponent(password)
			};

			$http({
					method: 'POST',
					url: '/token',
					data: "email=" + encodeURIComponent(username)
						+ "&password=" + encodeURIComponent(password)
						+ "&grant_type=password",
					headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
				})
			.then(
				function success(response) {
					callback(response.data);
				},
				function error(response) {
					errorCallback(response.data);
				});
		}

		function SetCredentials(username, token) {
			$rootScope.globals = {
				currentUser: {
					username: username,
					access_token: token
				}
			};

			// set default auth header for http requests
			$http.defaults.headers.common['Authorization'] = 'Bearer ' + token;

			sessionStorage.setItem("globals", JSON.stringify($rootScope.globals));
		}

		function ClearCredentials() {
			$rootScope.globals = {};
			sessionStorage.removeItem('globals');
			$http.defaults.headers.common.Authorization = 'Bearer';
		}

		function CheckRegistration(token, callback, errorCallback) {
			var request = {
				method: 'GET',
				url: '/getlogin',
				headers: {
					'Authorization': 'Bearer ' + token
				}
			};

			$http(request).then(
				function success(response) {
					callback(response.data);
				},
				function error() {
					errorCallback(response.data);
				}
			);
		}

		function Register(userName, password, confirmPassword, callback, errorCallback) {
			var userData = {
				Email: userName,
				Password: password,
				ConfirmPassword: confirmPassword
			};

			$http({
					method: 'POST',
					url: '/register',
					data: userData,
					headers: { 'Content-Type': 'application/json' }
				})
				.then(
					function success(response) {
						callback(response.data);
					},
					function error(response) {
						errorCallback(response.data);
				});
		}

		function GetUser(callback, errorCallback) {
			$http({
					method: 'GET',
					url: '/getlogin'
				})
				.then(
					function success(response) {
						callback(response.data);
					},
					function error(response) {
						errorCallback(response);
					});
		}

		function Logout(callback, errorCallback) {
			$http({
					method: 'GET',
					url: '/logout'
				})
				.then(
					function success(response) {
						callback(response.data);
					},
					function error(response) {
						errorCallback(response);
					});
		}
	}
})();