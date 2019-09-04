'use strict';
purchApp.factory('Groups', function ($http) {
    return {
        //Получить список групп
        getAll : function() { 
            return $http({
                    method: 'GET',
                    url: "/Group/GetGroupList"
                });
        }
    }
});
