'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("itemListController", function ($scope, $http,
    //$modalInstance,
    Items, Groups) {

    $scope.Items = Items;
   

    //Создать новый товар
    $scope.CreateNewItem = function () {
        
        var selectedGroupId = Groups.getSelectedGroupId();
        console.log('CreateNewItem, selectedGroupId = ' + selectedGroupId);
        //Items.CreateNewItemCard(selectedGroupId);
        window.location.href = "/Item/CreateItemCard?gId_=" + selectedGroupId;
    }

    //Удалить существующий товар
    $scope.DeleteItem = function (item) {
        $scope.Items.DeleteItemCard(item.Id).then(
            function success(response){
                console.log("DeleteItem Успех: " + response.data);
                Items.ReloadItemsByGId(item.GId);
            },
            function error(response) {
                console.log("DeleteItem ошибка: " + response.data);
            }
        );
    }

    //Удалить существующий товар
    $scope.EditItem = function (item) {
        
        window.location.href = "/Item/EditItemCard?itemId_=" + item.Id;
        //$scope.Items.EditItemCard(item.Id).then(
        //    function success(response) {
        //        console.log("DeleteItem Успех: " + response.data);
        //        //Items.ReloadItemsByGId(item.GId);
        //    },
        //    function error(response) {
        //        console.log("DeleteItem ошибка: " + response.data);
        //    }
        //);
    }

});