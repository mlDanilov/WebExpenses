'use strict';
purchApp.factory('Groups', function ($http) {
    var _selectedGroupId = undefined;

    return {
        //Получить список групп
        getAll: function () {
            return $http({
                    method: 'GET',
                    url: "/Group/GetGroupList"
                });
        },
        //Установить текущую выбранную группу
        setSelectedGroupId : function(groupId)
        {   
            _selectedGroupId = groupId;
        },
        //Получить текущую выбранную группу
        getSelectedGroupId: function () {
            return _selectedGroupId;
        },
        //Удалить группу товаров по уникальному коду
        DeleteGroup: function (groupId_) {
            return $http({
                method: 'POST',
                url: "/Group/DeleteGroup",
                params: { groupId_: groupId_ }
            });
        },
        //Редактировать группу товаров
        EditGroup: function (groupId_){
    
        }

    }
});
