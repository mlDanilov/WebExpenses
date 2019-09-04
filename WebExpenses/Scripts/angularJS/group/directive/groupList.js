'use strict'
purchApp.directive("groupList", function () {
    return {
        scope: false, //true - scope директивы(локальный), false - контроллера, где присутствует директива
        restrict: "E",
        link: function (scope, element, attrs) {
            //Берем группы и текущую группу из значечния атрибута директивы source
            scope.$watch('Groups', function () {
                //Берем коллекцию, если она изменилась
                let data = scope[attrs["source"]];
                scope.data = data;

                //Устанавливаем выбранную группу по id
                let selectedId = attrs["selectedid"];
                if (selectedId == undefined)
                    return;

                let selectedItem = scope.data.Items.find(g => g.Id == selectedId);
                data.SelectedItem = selectedItem;
            })          
        },

        templateUrl: "/Scripts/angularJS/group/directive/groupListTemplate.html"
    }
});