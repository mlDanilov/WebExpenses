'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("purchaseListController", function ($scope, Purchases) {

    $scope.BDate = undefined;
    $scope.EDate = undefined;

    //Если поменялся текущий год, то перезагрузаем список групп
    $scope.$on('yearsChanges', function (event, data) {
        
        $scope.BDate = data.bDate;
        $scope.EDate = data.eDate;
        $scope.getPurchGroupsByBeginAndEndDates(data.bDate, data.eDate);
        $scope.purchaseSelected = null;
        $scope.purchaseOver = null;
    });

    //Если поменялся текущий год, то перезагрузаем список групп
    $scope.$on('beginAndEndDatesChanged', function (event, data) {
        $scope.BDate = data.bDate;
        $scope.EDate = data.eDate;
        $scope.purchaseSelected = null;
        $scope.purchaseOver = null;
    });

    //Покупка, над которой находится курсор мыши
    $scope.purchaseOver = undefined;
    //Выделенная покупка
    $scope.purchaseSelected = undefined;

    //Установить текующую покупку
    $scope.setSelected = function (purchase) {
        $scope.purchaseSelected = purchase;
        //console.log('setSelected purchId=' + purchase.Id);
    }

    //Установить покупку, над которой в данный момент находится курсор мыши
    $scope.setOver = function (purchase) {
        $scope.purchaseOver = purchase;
        //console.log('setOver purchId=' + purchase.Id);
    }
   
    //$scope.GroupedPurchases = [
    //    {
    //        Group: { Id : 1, Name : "Спиртное"},
    //        Purchases: [
    //            {
    //                Id: 1,
    //                Item: { Id: 13, Name: "Водка" },
    //                Shop: { Id: 8, Name: "Кировский" },
    //                Price: 30, Count: 3, Date: new Date("2019-10-01")
    //            },
    //            {
    //                Id: 2,
    //                Item: { Id: 253, Name: "Белебеевская" },
    //                Shop: { Id: 8, Name: "Кировский" },
    //                Price: 70, Count: 2, Date: new Date("2019-10-01")
    //            },
    //            {
    //                Id: 8,
    //                Item: { Id: 934, Name: "На березовых бруньках" },
    //                Shop: { Id: 22, Name: "Монетка" },
    //                Price: 100, Count: 2, Date: new Date("2019-10-02")
    //            },
    //            {
    //                Id: 18,
    //                Item: { Id: 44, Name: "Златые берега" },
    //                Shop: { Id: 22, Name: "Монетка" },
    //                Price: 200, Count: 1, Date: new Date("2019-10-03")
    //            }
    //            ,
    //            {
    //                Id: 18,
    //                Item: { Id: 44, Name: "Златые берега" },
    //                Shop: { Id: 3, Name: "Алкомаркет" },
    //                Price: 190, Count: 2, Date: new Date("2019-10-05")
    //            }
    //        ]
    //    },
    //    {
    //        Group: { Id: 2, Name: "Хмельное" },
    //        Purchases: [
    //            {
    //                Id: 13,
    //                Item: { Id: 435, Name: "Жигулёвское" },
    //                Shop: { Id: 22, Name: "Монетка" },
    //                Price: 90, Count: 3, Date: new Date("2019-10-01")
    //            },
    //            {
    //                Id: 34,
    //                Item: { Id: 345, Name: "Нефильтрованное" },
    //                Shop: { Id: 22, Name: "Монетка" },
    //                Price: 80, Count: 2, Date: new Date("2019-10-01")
    //            },
    //            {
    //                Id: 754,
    //                Item: { Id: 879, Name: "Балтика жопа" },
    //                Shop: { Id: 9, Name: "Райт" },
    //                Price: 75, Count: 2, Date: new Date("2019-10-02")
    //            },
    //            {
    //                Id: 125,
    //                Item: { Id: 135, Name: "Златый базант" },
    //                Shop: { Id: 9, Name: "Райт" },
    //                Price: 150, Count: 3, Date: new Date("2019-10-03")
    //            }
    //        ]
    //    }
    //];

    //Взять все покупки, отсортированные по группам из репозитория
    $scope.getPurchGroupsByBeginAndEndDates = function (bDate, eDate) {
        Purchases.getPurchasesByPeriod(bDate, eDate).then(
            function successCallback(response) {
                console.log('getPurchasesByPeriod успех');
                Purchases.GroupedPurchases = response.data;
                //$scope.GroupedPurchases = response.data;
            },
            function errorCallback(response) {
                console.log('getPurchasesByPeriod ошибка');
                
            }
            );
    }

    $scope.getGroupedPurchases = function () {
        return Purchases.GroupedPurchases;
    }

    //$scope.getPurchGroupsByBeginAndEndDates($scope.BDate, $scope.EDate);

    $scope.calcGroupSum = function (groupPurch) {
        let sum = 0.0;

        (groupPurch.Purchases.filter(p => p.Date >= $scope.BDate && p.Date <= $scope.EDate)
          ).forEach(
            gp => sum += (gp.Count * gp.Price)
            );
        return sum;
    }
  
    //Собираем в кучу все группы товаров в покупках
    $scope.getGroupsFromPurchases = function () {
        let groups = [];

        for (let i = 0; i < $scope.Purchases.length; i++) {
            let group = $scope.Purchases[i].Item.Group;

            if (!groups.includes(group))
                groups.push(group);
        }

        return groups;
    }

    //Берем покупки по группе товаров
    $scope.getPurchasesbyGroup = function (group) {

        let purchases = $scope.Purchases.filter(p => p.Item.Group == group);

        return purchases;
    }

   
    //Удалить покупку
    $scope.CreatePurchaseCard = function () {
        window.location.href = "/Purchase/CreatePurchaseCard";
    }

    //Редактировать покупку
    $scope.EditPurchaseCard = function (purchase) {
        console.log('EditPurchaseCard');
        let purchId = purchase.Id
        window.location.href = "/Purchase/EditPurchaseCard?purchaseId_=" + purchId;
    }
    //Удалить покупку
    $scope.DeletePurchaseCard = function (purchase) {
        let purchId = purchase.Id

        //window.location.href = "/Purchase/DeletePurchaseView?purchaseId_=" + purchId;
        Purchases.deletePurchase(purchId).then(
            function successCallback(response) {
                console.log('DeletePurchaseCard purchId=' + purchId + ' успех');
                //Костыль!
                $scope.getPurchGroupsByBeginAndEndDates($scope.BDate, $scope.EDate);
            },
            function errorCallback(response) {
                console.log('DeletePurchaseCard purchId=' + purchId + ' ошибка');
                console.log(response);
            });
    }


});