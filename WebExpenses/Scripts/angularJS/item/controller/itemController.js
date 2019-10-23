'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("itemController", function ($scope, $http, Groups) {

    $scope.init = function (model) {
        $scope.Id = model.Id;
        $scope.Name = model.Name;
        $scope.GroupId = model.GId;
    }

    $scope.Header = "Карточка товара";
    //Текущая выбранная группа
    $scope.GroupId;
    //Код товара
    $scope.Id;
    //Название
    $scope.Name;

    //Создать новую карточку товара
    $scope.CreateItem = function () {

        console.log('CreateItem');

        $http(
            {
                method: 'POST',
                url: '/Item/CreateItem',
                params: {
                    name_ : $scope.Name, 
                    gId_: $scope.Groups.SelectedItem.Id
                }
            }
            ).then(
            function success(response) {
                console.log('/Item/CreateItem успех');
                console.log(response);
                window.location.href = "/Groups";
            },
            function error(response) {
                console.log('/Item/CreateItem ошибка');
                console.log(response);
            }
            )
    }

    //Создать новую карточку товара
    $scope.EditItem = function () {
        console.log('EditItem');

            $http(
                   {
                       method: 'POST',
                       url: '/Item/EditItem',
                       params: {
                           id_ : $scope.Id,
                           name_: $scope.Name,
                           gId_: $scope.Groups.SelectedItem.Id
                       }
                   }
                   ).then(
                   function success(response) {
                       console.log('/Item/EditItem успех');
                       console.log(response);
                       window.location.href = "/Groups";
                   },
                   function error(response) {
                       console.log('/Item/EditItem ошибка');
                       console.log(response);
                   }
                   )

        }
    

    //Получить объект с группами товаров 
    //и текущей группой
    $scope.getGroups = function(currentGroupId)
    {
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
            console.log('$scope.getGroups => Groups.getAll спешно' + response.data);
            console.log(response);
        });
        
        
    }

    $scope.getGroups(2);

    //Список групп и текущая выбранная
    $scope.Groups  = {
        Items: [],
        SelectedItem: undefined
    }
    //$scope.Groups.SelectedItem = $scope.Groups.Items[1];

  
    $scope.deleteGroup = function () {
        delete $scope.Groups.Items[0];
    }

   
});