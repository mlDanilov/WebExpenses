'use strict'
purchApp.config(
    function ($routeProvider) {
        $routeProvider.when('/groups', {
            templateUrl: 'Views/Group/List.cshtml'
        });
        $routeProvider.when('/shops', {
            templateUrl: 'Views/Shop/List.cshtml'
        });
        $routeProvider.when('/purchases', { 
            //templateUrl: '/Views/Purchases/List.cshtml'
            templateUrl: '/Views/Menu/preved.html'
        })

    });