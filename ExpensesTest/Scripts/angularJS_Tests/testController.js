//'use strict'
describe("Example Controller Test", function () {

    var mockScope = {};
    var controller;

    //beforeEach(angular.mock.module("purchApp"));
    beforeEach(angular.mock.module("exampleApp"));

    beforeEach(angular.mock.inject(
        function ($controller, $rootScope) {
            mockScope = $rootScope.$new();

            controller = $controller("defaultCtrl", {
                $scope : mockScope

            });
        }
    ));

    it("Создание свойства counter", function () {
        expect(mockScope.counter).toEqual(0);
    });

    it("Инкермент свойства", function () {
        mockScope.incrementCounter();
        mockScope.incrementCounter();
        expect(mockScope.counter).toEqual(2);
    });

});