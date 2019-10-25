'use strict'
var purchApp = angular.module('purchApp');
let menuUrl = "/Menu/Index#!shops"
purchApp.controller("shopController", function ($scope, $http, Shops) {

    $scope.init = function (model) {
        $scope.Id = model.Id;
        $scope.Name = model.Name;
        $scope.Address = model.Address;
    }

    //Код магазина
    $scope.Id;
    //Название
    $scope.Name;
    //адрес магазина
    $scope.Address;


    //Редактировать карточку магазина
    $scope.EditShop = function () {
        console.log('EditShop');
        Shops.editShop($scope.Id, $scope.Name, $scope.Address).then(
            function success(response) {
                console.log('/Shop/EditShopCard успех');
                console.log(response);
                window.location.href = menuUrl;
            },
            function error(response) {
                console.log('/Shop/EditShopCard ошибка');
                console.log(response);
            }
            );
    }

    //Create новую карточку магазина
    $scope.CreateShop = function () {
        console.log('CreateShop');
        Shops.createShop($scope.Name, $scope.Address).then(
            function success(response) {
                console.log('/Shop/CreateShop успех');
                console.log(response);
                window.location.href = menuUrl;
            },
            function error(response) {
                console.log('/Shop/CreateShop ошибка');
                console.log(response);
            }
            );
    }


});