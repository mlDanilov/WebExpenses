'use strict';
purchApp.factory('Shops', function ($http) {

    let _selectedShopId = undefined;

    return {
        //Получить список групп
        getAll: function () {
            return $http({
                method: 'GET',
                url: "/Shop/GetList"
            });
        },

      

        //Установить текущий выбранный магазин
        setSelectedShopId: function (shopId) {
            _selectedShopId = shopId;
        },
        //Получить текущий выбранный магазин
        getSelectedShopId: function () {
            return _selectedShopId;
        },
        
        //Создать магазин
        createShop : function (shopName_, shopAddress_){
            return $http(
           {
               method: 'POST',
               url: '/Shop/CreateNewShop',
               params: {
                   name_: shopName_,
                   address_: shopAddress_
               }
           }
           );
        },

        //Редактировать магазин
        editShop : function (shopId_, shopName_, shopAddress_){
            return $http(
            {
                method: 'POST',
                url: '/Shop/EditShopCard',
                params: {
                    id_: shopId_,
                    name_: shopName_,
                    address_: shopAddress_
                }
            }
            );

        },

        //Удалить магазин
        deleteShop: function (shopId_) {
            return $http({
                method: 'POST',
                url: "/Shop/DeleteShopById",
                params: { shopId_: shopId_ }
            });
        }
      

    }
});
