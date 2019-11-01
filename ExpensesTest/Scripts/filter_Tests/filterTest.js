'use strict';
describe("Real Purchase Controller Test", function () {

    let fPurchByPeriod;
    let fPurchGroupsByPeriod;

    beforeEach(angular.mock.module("purchApp"));

    beforeEach(angular.mock.inject(function ($filter) {
        //fPurchGroupsByPeriod = $filter("purchGroupsByPeriod");
        fPurchByPeriod = $filter("purchasesByPeriod");
    }));


    it("Фильтрация покупок по периоду", function () {

        console.log("purchases=");
        console.log(purchases);
            //let bDate = new Date(2017, 10, 3);
            //let eDate = new Date(2017, 10, 18);
            //let res = fPurchByPeriod(purchases, bDate, eDate);
            //console.log("(purchase + filter).length=");
            //console.log(res.length);
        });
});