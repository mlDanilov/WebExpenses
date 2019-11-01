'use strict';
describe("Real Purchase Controller Test", function () {

    let bDate = new Date(2017, 10, 3);
    let eDate = new Date(2017, 10, 18);

    let fPurchByPeriod;
    let fPurchGroupsByPeriod;

    beforeEach(angular.mock.module("purchApp"));

    beforeEach(angular.mock.inject(function ($filter) {
        //fPurchGroupsByPeriod = $filter("purchGroupsByPeriod");
        fPurchByPeriod = $filter("purchasesByPeriod");
    }));


    it("Фильтрация покупок по периоду", function () {

            
            let res = fPurchByPeriod(purchases, bDate, eDate);
            expect(res.length).toEqual(13);
            console.log("(purchase + filter).length=" + res.length);
            //console.log(res.length);
        });
});