'use strict';
purchApp.factory('Items', function ($http) {
    var _items = [];
    var _selectedGroupId = undefined;

    return {

        //ItemList: undefined,

        //Список товаров текущей группы
        getItems: function () {
            return _items;
        },


        //Перезагрузить текущий список товаров по группе
        ReloadItemsByGId: function (gId_) {

            $http({
                method: 'GET',
                url: "/Item/GetItemListByGroupId",
                params: { groupId_: gId_ }
            }).then(function success(response) { 
                console.log('/Item/GetItemListByGroupId успех');
                _items = response.data.ItemList;

            }, function error(response) {
                console.log('/Item/GetItemListByGroupId ошибка');

            }
            );
        },
        //Создать карточку товара в указанной группе товаров
        CreateNewItemCard: function (groupId_) {

            $http({
                method: 'GET',
                url: "/Item/GetItemListByGroupId",
                params: { groupId_: gId_ }
            }).then(function success(response) {
                console.log('/Item/GetItemListByGroupId успех');
                _items = response.data.ItemList;

            }, function error(response) {
                console.log('/Item/GetItemListByGroupId ошибка');

            }
            );
        },
        //Удалить карточку товара
        DeleteItemCard: function (itemId_) {
            return $http({
                method: 'POST',
                url: "/Item/DeleteItemCard",
                params: { itemId_: itemId_ }
            });
        },
        //Редактировать карточку товара
        EditItemCard : function (itemId_){
            return $http({
                method: 'POST',
                url: "/Item/EditItemCard",
                params: { itemId_: itemId_ }
            });
        }

    }
});