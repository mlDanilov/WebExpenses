'use strict'
var purchApp = angular.module('purchApp', []);
purchApp.controller("purchaseController", function ($scope, $http, Items, Groups, Shops, Purchases) {
    //Код покупки
    $scope.Id;

    //Кол-во
    $scope.Count = 0.0;
    //Цена
    $scope.Price = 0.0;
    //Дата
    $scope.Date = new Date();

    //Получить объект с группами товаров 
    //и текущей группой
    $scope.getGroups = function (currentGroupId) {
        Groups.getAll().then(function success(response) {
            console.log('$scope.getGroups => Groups.getAll успешно');
            //Список групп
            let groupList = response.data.GroupList;
            let groups = {
                Items: groupList,
                SelectedItem: undefined
            };
            //В качестве текущей первую из массива
            if (groupList.length > 0)
                groups.SelectedItem = groupList.find(g => g.Id == currentGroupId);

            //Установить в контроллер
            $scope.Groups = groups;
        }, function error(response) {
            console.log('$scope.getGroups => Groups.getAll ошибка' + response.data);
            console.log(response);
        });


    }

    //Получить объект с магазинами
    //и текущим магазином
    $scope.getShops = function (currentShopId) {
        Shops.getAll().then(function success(response) {
            console.log('$scope.getShops => Shops.getAll успешно');
            //Список групп
            let shopList = response.data.Shops;
            let shopListExt = [];

            //Заполняем массив объектов shopListExt с новым property DisplayName
            shopList.forEach(
                function (sh) {
                    shopListExt.push(
                        {
                            Id : sh.Id,
                            Name : sh.Name,
                            DisplayName : sh.Name + ' - ' + sh.Address
                        });
                }
                );

            let shops = {
                Items: shopListExt,
                SelectedItem: undefined
            };
            //В качестве текущей первую из массива
            if (shopList.length > 0)
                shops.SelectedItem = shopListExt.find(s => s.Id == currentShopId);

            //Установить в контроллер
            $scope.Shops = shops;
        }, function error(response) {
            console.log('$scope.getShops => Shops.getAll ошибка' + response.data);
            console.log(response);
        });


    }

    //Получить объект с магазинами
    //и текущим магазином
    $scope.getItems = function (currentGroupId, currentItemId) {
        Items.GetItemsByGId(currentGroupId).then(function success(response) {
            console.log('$scope.GetItemsByGId => Items.GetItemsByGId успешно');
            //Список товаров
            let itemList = response.data.ItemList;

            //Заполняем массив объектов shopListExt с новым property DisplayName
            let items = {
                Items: itemList,
                SelectedItem: undefined
            };
            //В качестве текущей первую из массива
            if (itemList.length > 0) {
                if (currentItemId)
                    items.SelectedItem = itemList.find(i => i.Id == currentItemId);
                else
                    items.SelectedItem = itemList[0];
            }

            //Установить в контроллер
            $scope.Items = items;
        }, function error(response) {
            console.log('$scope.getItems => Items.GetItemsByGId ошибка' + response.data);
            console.log(response);
        });


    }

    $scope.init = function (model) {
        $scope.Id = model.Id;
        $scope.Count = model.Count;
        $scope.Price = model.Price;
        //{0:yyyy-MM-dd}
        $scope.Date = new Date(model.Date);

        $scope.getGroups(model.Item.GId);
        if (model.Shop != null)
            $scope.getShops(model.Shop.Id);
        else
            $scope.getShops();

        $scope.getItems(model.Item.GId, model.Item.Id);
    }

    $scope.initEmpty = function () {

        $scope.getGroups();
        $scope.getShops();
        $scope.getItems();
    }

    //текущая выбранная группа изменилась
    $scope.groupChanged = function () {

        let currentGroup = $scope.Groups.SelectedItem;
        if (currentGroup)
            $scope.getItems(currentGroup.Id);

        //if ($scope.Items.Items && $scope.Items.Items.length > 0)
        //    $scope.Items.SelectedItem = $scope.Items.Items[0];
    }

    //Список групп и текущая выбранная
    $scope.Groups = {
        Items: [],
        SelectedItem: undefined

    }
    //Список магазинов и текущая выбранная
    $scope.Shops = {
        Items: [],
        SelectedItem: undefined

    }

    //Список магазинов и текущая выбранная
    $scope.Items = {
        Items: [],
        SelectedItem: undefined

    }

    //Создать новую покупку
    $scope.CreatePurchase = function () {
        console.log('CreatePurchase');

        let shop_id = null;
        if ($scope.Shops.SelectedItem != undefined)
            shop_id = $scope.Shops.SelectedItem.Id;

        //Создать покупку
        Purchases.createPurchase(
            $scope.shop_id,
            $scope.Items.SelectedItem.Id,
            $scope.Date,
            $scope.Count,
            $scope.Price
            ).then(
          function success(response) {
              console.log('/Purchase/CreatePurchase успех');
              console.log(response);
              window.location.href = "/Purchases";
          },
          function error(response) {
              console.log('/Purchase/CreatePurchase ошибка');
              console.log(response);
          }
          )
        
    }

    //Редактировать текущую покупку
    $scope.EditPurchase = function () {
        //EditPurchase
        let shop_id = null;
        if ($scope.Shops.SelectedItem != undefined)
            shop_id = $scope.Shops.SelectedItem.Id;

        //редактировать покупку
        Purchases.editPurchase(
            $scope.Id,
            shop_id,
            $scope.Items.SelectedItem.Id,
            $scope.Date,
            $scope.Count,
            $scope.Price            
          ).then(
          function success(response) {
              console.log('/Purchase/EditPurchase успех');
              console.log(response);
              window.location.href = "/Purchases";
          },
          function error(response) {
              console.log('/Purchase/EditPurchase ошибка');
              console.log(response);
          }
          )
    }

    ////Редактировать текущую покупку
    //$scope.DeletePurchase = function (purchId) {

    //    $http(
    //      {
    //          method: 'POST',
    //          url: '/Purchase/DeletePurchase',
    //          params: {
    //              purchId_: purchId
    //          }
    //      }
    //      ).then(
    //      function success(response) {
    //          console.log('/Purchase/DeletePurchase успех');
    //          console.log(response);
    //          window.location.href = "/Purchases";
    //      },
    //      function error(response) {
    //          console.log('/Purchase/DeletePurchase ошибка');
    //          console.log(response);
    //      }
    //      )
    //}
});