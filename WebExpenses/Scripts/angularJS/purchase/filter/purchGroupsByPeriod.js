'use strict';
var purchApp = angular.module('purchApp');
purchApp.filter('purchGroupsByPeriod', function () {
    return function (purchGroups, BDate, EDate) {
        let res = [];
        if (!purchGroups)
            return res;

        purchGroups.filter(
            function (pG) {
                let purchs = [];
                purchs = pG.Purchases.filter(p => p.Date >= BDate && p.Date <= EDate);
                if (purchs != undefined && purchs.length != 0)
                    res.push(pG);
            }
                //p.Date >= BDate && p.Date <= EDate
            );
        return res;
    }
})