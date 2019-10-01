'use strict'
purchApp.directive("shopList", function () {
    return {
        scope: false, //true - scope директивы(локальный), false - контроллера, где присутствует директива
        restrict: "E",
        link: function (scope, element, attrs) {
            //Берем группы и текущую группу из значечния атрибута директивы source
            scope.$watch('Shops', function () {
                //Берем коллекцию, если она изменилась
                let data = scope[attrs["source"]];
                scope.Shops = data;

                //Устанавливаем выбранную группу по id, если она указана
                let selectedId = attrs["selectedid"];
                if (selectedId == undefined)
                    return;

                let selectedItem = scope.Shops.Items.find(g => g.Id == selectedId);
                data.SelectedItem = selectedItem;
            })          
        },

        templateUrl: "/Scripts/angularJS/shop/directive/shopListTemplate.html"
    }
});