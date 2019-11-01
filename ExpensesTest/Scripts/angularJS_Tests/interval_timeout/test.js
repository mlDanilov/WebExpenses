describe("Interval and Timeout test", function () {

    let mockScope = {};
    let mockInterval = null;
    let mockTimeout = null;
    let controller = null;

    beforeEach(angular.mock.module("exampleApp"));

    beforeEach(angular.mock.inject(function ($controller, $rootScope, $interval, $timeout) {
        //сервисы $interval и $timeout предоставляют возможность для тестирования callback функций
        mockScope = $rootScope.$new();
        mockInterval = $interval;
        mockTimeout = $timeout;

        controller = $controller("defaultCtrl", {
            $scope: mockScope,
            $interval: mockInterval,
            $timeout: mockTimeout
        });
    }));


    it("Ограничиваем интервал до 10 обновлений", function () {

        for (let i = 0; i < 11; i++) {
            mockInterval.flush(5000);
        }
        expect(mockScope.intervalCounter).toEqual(10);
        //console.log("Ограничиваем интервал до 10 обновлений1");
    });


    it("Изменение таймера", function () {
        mockTimeout.flush(5000);
        expect(mockScope.timerCounter).toEqual(1);
        //console.log("Изменение таймера1");
    });


});