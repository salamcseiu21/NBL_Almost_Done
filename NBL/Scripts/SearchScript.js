var app = angular.module('Homeapp', []);
app.controller('HomeController', function ($scope, $http) {

    $http.get('/Home/GetClients').then(function (d) {

        $scope.clients = d.data;

    }, function (error) {

        alert('failed');

    });
   

});