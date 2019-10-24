'use strict'
purchApp.config(
    function ($routeProvider) {
        $routeProvider.when('/groups', {
            templateUrl: '/Scripts/angularJS/group/views/List.html'
        });
        $routeProvider.when('/shops', {
            templateUrl: '/Scripts/angularJS/shop/views/List.html'
        });
        $routeProvider.when('/purchases', {
            templateUrl: '/Scripts/angularJS/menu/views/List.html'
        });
        
    });