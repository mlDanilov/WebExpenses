'use strict';
var purchApp = angular.module('purchApp');
purchApp.filter('purchasesByPeriod', function () {
    return function (purchases, BDate, EDate) {
        return purchases.filter(p => p.Date >= BDate && p.Date <= EDate);
    }
})