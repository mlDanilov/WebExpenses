'use strict';
purchApp.factory('Monthes', function () {

    let _monthes = [
       { Id: 0, Name: "Январь" },
       { Id: 1, Name: "Февраль" },
       { Id: 2, Name: "Март" },
       { Id: 3, Name: "Апрель" },
       { Id: 4, Name: "Май" },
       { Id: 5, Name: "Июнь" },
       { Id: 6, Name: "Июль" },
       { Id: 7, Name: "Август" },
       { Id: 8, Name: "Сентябрь" },
       { Id: 9, Name: "Октябрь" },
       { Id: 10, Name: "Ноябрь" },
       { Id: 11, Name: "Декабрь" }]

    return {

        GetMonthById: function (id) {
            return _monthes.find(m => m.Id == id);

            
        }

    }
});