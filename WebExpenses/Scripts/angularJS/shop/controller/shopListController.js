'use strict'
var purchApp = angular.module('purchApp',[]);
purchApp.controller("shopListController", function ($scope, $http, Shops) {


    //Получить объект с группами товаров 
    $scope.getShops = function (currentShopId) {
        Shops.getAll().then(function success(response) {
            console.log('$scope.getShops => Shops.getAll успешно');
            //Список групп
            let shopList = response.data.Shops;
            let current = response.data.Current;
            let shops = {
                Items: shopList,
                SelectedItem: current
            };
            //В качестве текущей первую из массива
            if (shopList.length > 0)
                shops.SelectedItem = shopList.find(g => g.Id == currentShopId);

            //Установить в контроллер
            $scope.Shops = shops;
        }, function error(response) {
            console.log('$scope.getGroups => Groups.getAll ошибка' + response.data);
            console.log(response);
        });


    }

    //Установить текующую группу
    $scope.setSelected = function(shop)
    {
        $scope.Shops.SelectedItem = shop;

        Shops.setSelectedShopId(shop.Id);

        //console.log('preved!!');
    }

    //Установить магазин, над которым в данный момент находится курсор мыши
    $scope.setOver = function (shop) {
        $scope.Shops.ItemOver = shop;
    }

    //Редактировать группу
    $scope.EditShop = function () {
        console.log('EditShops');
        let shopId = Shops.getSelectedShopId();
        window.location.href = "/Shop/EditShop?shopId_=" + shopId;
    }

    
    //Создать новую группу товаров
    $scope.CreateNewShop = function () {
        console.log('CreateNewShops');
        window.location.href = "/Shop/CreateShop";
    }

    //Создать новую группу товаров
    $scope.DeleteShop = function (shop) {
        Shops.deleteShop(shop.Id).then(
            function success(response) {
            console.log("DeleteShop " + shop.Id + " успех " + response.data);
            $scope.getShops(0); //Перезагрузить список групп и установить текущую группу
        }, function error(response) {
            console.log("DeleteShop ошибка: " + response.data);
        });
        
        //window.location.href = "/Group/CreateGroupCard?idParent_=0";
    }

    

    $scope.getShops();

    //Список групп и текущая выбранная
    $scope.Shops  = {
        Items: [],
        ItemOver: undefined,
        SelectedItem: undefined
        //ItemOver: undefined
        
    }
});