'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("groupListController", function ($scope, $http, Groups, Items) {


    //Получить объект с группами товаров 
    $scope.getGroups = function (currentGroupId) {
        Groups.getAll().then(function success(response) {
            console.log('$scope.getGroups => Groups.getAll успешно');
            //Список групп
            let groupList = response.data.GroupList;
            let groups = {
                Items: groupList,
                SelectedItem: undefined,
                ItemOver : undefined
            };
            //В качестве текущей первую из массива
            if (groupList.length > 0)
                groups.SelectedItem = groupList.find(g => g.Id == currentGroupId);

            //Установить в контроллер
            $scope.Groups = groups;
        }, function error(response) {
            console.log('$scope.getGroups => Groups.getAll ошибка' + response.data);
            console.log(response);
        });


    }

    //Установить текующую группу
    $scope.setSelected = function(group)
    {
        //console.log('setSelected');
        $scope.Groups.SelectedItem = group;

        Items.ReloadItemsByGId(group.Id);
     
        Groups.setSelectedGroupId(group.Id);

        //console.log('preved!!');
    }

    //Редактировать группу
    $scope.EditGroup = function () {
        console.log('EditGroup');
        let groupId = Groups.getSelectedGroupId();
        window.location.href = "/Group/EditGroup?gId_=" + groupId;
    }

    //Удалить группу
    $scope.DeleteGroup = function () {
        console.log('DeleteGroup');
        alert('DeleteGroup');
    }
    //Создать новую группу товаров
    $scope.CreateNewGroup = function () {
        console.log('CreateNewGroup');
        window.location.href = "/Group/CreateGroupCard?idParent_=0";
    }

    //Создать новую группу товаров
    $scope.DeleteGroup = function (group) {
        //console.log('DeleteGroup ' + group.Id);
        Groups.DeleteGroup(group.Id).then(function success(response) {
            console.log("DeleteGroup " + group.Id + " успех " + response.data);
            $scope.getGroups(0); //Перезагрузить список групп и установить текущую группу
            Items.ReloadItemsByGId(0);  //Перезагрузить список товаров текущей группы
        }, function error(response) {
            console.log("DeleteGroup ошибка: " + response.data);
        });
        
        //window.location.href = "/Group/CreateGroupCard?idParent_=0";
    }

    //Установить группу, над которой в данный момент находится курсор мыши
    $scope.setOver = function (group) {
        $scope.Groups.ItemOver = group;
        //console.log(group);
//        console.log("group.Id=" + group.Id);
//        console.log("Groups.ItemOver.Id=" + $scope.Groups.ItemOver.Id);
    }

    $scope.getGroups();

    //Список групп и текущая выбранная
    $scope.Groups = {
        Items: [],
        ItemOver: undefined,
        SelectedItem: undefined
        //ItemOver: undefined
        
    }
});