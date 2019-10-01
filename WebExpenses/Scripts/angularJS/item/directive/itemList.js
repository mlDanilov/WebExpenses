'use strict'
purchApp.directive("itemList", function () {
    return {
        scope: false, //true - scope директивы(локальный), false - контроллера, где присутствует директива
        restrict: "E",
        link: function (scope, element, attrs) {
            //Берем группы и текущую группу из значечния атрибута директивы source
            scope.$watch('Items', function () {
                //Берем коллекцию, если она изменилась
                let data = scope[attrs["source"]];
                scope.Items = data;

                ////Берем обработчик событий
                //let funcName = attrs["currentchanged"];
                //if (funcName != undefined) {
                //    let func = scope[funcName];
                //    if (typeof (func) == 'function') {
                //        data.currentChanged = func;
                //    }
                //}

                //Устанавливаем выбранную группу по id, если она указана
                let selectedId = attrs["selectedid"];
                if (selectedId == undefined)
                    return;

                

                let selectedItem = scope.data.Items.find(it => it.Id == selectedId);
                data.SelectedItem = selectedItem;
            })
        },

        templateUrl: "/Scripts/angularJS/item/directive/itemListTemplate.html"
    }
});