'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("groupController", function ($scope, $http, Groups) {

    $scope.init = function (model) {
        $scope.Id = model.Id;
        $scope.Name = model.Name;
        $scope.ParentGroupId = model.GId;
    }

    $scope.Header = "Карточка товара";
    //Текущая выбранная группа
    $scope.ParentGroupId;
    //Код товара
    $scope.Id;
    //Название
    $scope.Name;


    //Получить объект с группами товаров 
    //и текущей группой
    $scope.getGroups = function (currentGroupId) {
        Groups.getAll().then(function success(response) {
            console.log('$scope.getGroups => Groups.getAll успешно');
            //Список групп
            let groupList = response.data.GroupList;
            let groups = {
                Items: groupList,
                SelectedItem: undefined
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

    $scope.getGroups();


   
    //Список групп и текущая выбранная
    $scope.Groups = {
        Items: [],
        //Items: [
        //        { Id: 1, Name: 'Тестовая группа1' },
        //        { Id: 2, Name: 'Тестовая группа2' },
        //        { Id: 3, Name: 'Тестовая группа3' },
        //    ],
        SelectedItem: undefined,
      
    }
    //$scope.Groups.SelectedItem = $scope.Groups.Items[1];

    //Создать новую карточку товара
    $scope.CreateGroup = function () {

        console.log('CreateGroup');

        $http(
            {
                method: 'POST',
                url: '/Group/CreateNewGroup',
                params: {
                    name_: $scope.Name,
                    idParent_: $scope.Groups.SelectedItem.Id
                }
            }
            ).then(
            function success(response) {
                console.log('/Group/CreateNewGroup успех');
                console.log(response);
                window.location.href = "/Groups";
            },
            function error(response) {
                console.log('/Group/CreateNewGroup ошибка');
                console.log(response);
            }
            )
    }

    //Создать новую карточку товара
    $scope.EditGroup = function () {

        console.log('EditGroup');

        $http(
               {
                   method: 'POST',
                   url: '/Group/EditGroupCard',
                   params: {
                       id_: $scope.Id,
                       name_: $scope.Name,
                       idParent_: $scope.Groups.SelectedItem.Id
                   }
               }
               ).then(
               function success(response) {
                   console.log('/Group/EditGroupCard успех');
                   console.log(response);
                   window.location.href = "/Groups";
               },
               function error(response) {
                   console.log('/Group/EditGroupCard ошибка');
                   console.log(response);
               }
               )
    }
});