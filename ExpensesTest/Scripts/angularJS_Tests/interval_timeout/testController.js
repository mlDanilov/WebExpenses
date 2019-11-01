angular.module("exampleApp", [])

    .controller("defaultCtrl", function ($scope, $interval, $timeout) {

    $scope.intervalCounter = 0;
    $scope.timerCounter = 0;

        $interval(function () {
        $scope.intervalCounter++;
    },5000, 10);

    $timeout(function () {
        $scope.timerCounter++;
    }, 5000);

});