'use strict';
purchApp.factory('Periods', function ($http) {

    
    return {
        //Взять список годов, за которые есть покупки
        getAllYears: function () {
            return $http({
                method: 'GET',
                url: 'api/Period/GetYears',
                isArray: false,
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
            });
        },

        Void: { Id: -1, Name: "-" }
    }
});
