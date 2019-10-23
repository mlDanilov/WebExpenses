'use strict';
purchApp.factory('Purchases', function ($http) {

    let _groupedPurchases = undefined;

    return {

        GroupedPurchases: _groupedPurchases,

        //Создать закупку
        createPurchase: function (
            shop_id_,
            item_id_,
            date_,
            count_,
            price_
            ) {
            return $http({
              method: 'POST',
              url: '/Purchase/CreatePurchase',
              params: {
                  Shop_Id: shop_id_,
                  Item_Id: item_id_,
                  Date: date_,
                  Count: count_,
                  Price: price_
              }
          });
        },

        //Редактировать покупку
        editPurchase: function (
            id_,
            shop_id_,
            item_id_,
            date_,
            count_,
            price_) {
            return $http(
            {
                method: 'POST',
                url: '/Purchase/EditPurchase',
                params: {
                    Id : id_,
                    Shop_Id: shop_id_,
                    Item_Id: item_id_,
                    Date: date_,
                    Count: count_,
                    Price: price_
                }
            }
            );

        },

        //Удалить покупку
        deletePurchase: function (purchaseId_) {
            return $http({
                method: 'DELETE',
                url: "/Purchase/DeletePurchase",
                params: { purchaseId_ }
            });
        },


        getPurchasesByPeriod: function (bDate, eDate) {
            var config = {
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                }
            };
            return $http({
                method: 'GET',
                config: config,
                url: "/Purchase/GetPurchaseGroupsByBeginAndEndDates",
                params: {
                    bDate_: bDate, 
                    eDate_: eDate
                },
                //Костыль!
                transformResponse: function (data, getHeaders, status) {
                    var obj = angular.fromJson(data);
                    //Костыль!
                    obj.forEach(gp => gp.Purchases.forEach(p => 
                    {
                        //Ещё раз костыль!
                        let date = new Date(p.DateStr);
                        p.Date = date;
                    }
                        ));
                    return obj;
                }
                });
        }
    }
});
