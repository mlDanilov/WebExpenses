'use strict';
purchApp.factory('DaysOfWeek', function () {

    let _dayOfWeeks = [
       { Id: -1, Name: "-" },
       { Id: 0, Name: "Воскресение", ShortName: "ВС" },
       { Id: 1, Name: "Понедельник", ShortName: "ПН" },
       { Id: 2, Name: "Вторник", ShortName: "ВТ" },
       { Id: 3, Name: "Среда", ShortName: "СР" },
       { Id: 4, Name: "Четверг", ShortName: "ЧТ" },
       { Id: 5, Name: "Пятница", ShortName: "ПТ" },
       { Id: 6, Name: "Суббота", ShortName: "СБ" }
    ]

    let _monthes = [
     { Id: -1, Name: "-" },
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
     { Id: 11, Name: "Декабрь" }
    ]

    let getPreviousMonday = function (date) {
        let monday = 1;
        let dw = date.getDay();
        if (dw == monday)
            return date;

        let delta = Math.abs(dw - monday) % 7;
        let res_millisec = date.getTime() - delta * 24 * 60 * 60 * 1000;
        let result = new Date(res_millisec);
        //console.log(result);
        return result;

    }

    let GetMonthById =  function (id) {
        return _monthes.find(m => m.Id == id);

            
    }

    var weekToString = function (bDate, eDate) {
        //Если совпадают месяцы(а значит и год)
        if (bDate.getMonth() == eDate.getMonth()) {
            return "(" + bDate.getDate() + "-" + eDate.getDate() + ")." + GetMonthById(bDate.getMonth()).Name + "." + bDate.getFullYear();
        }
        else {
            //Если годы совпадают
            if (bDate.getFullYear() == eDate.getFullYear()) {
                return "(" + bDate.getDate() + "." + GetMonthById(bDate.getMonth()).Name + "-" + eDate.getDate() + "." + GetMonthById(eDate.getMonth()).Name + ")." + bDate.getFullYear();
            }
            else {
                return bDate.toISOString() + "-" + eDate.toISOString();
            }
        }

    }

    return {

        GetDayOfWeekById: function (id) {
            return _dayOfWeeks.find(d => d.Id == id);
        },

        GetWeeksByMonth: function (date) {
            let weeks = [{ Id: -1, Display : "-"}];

            let y = date.getFullYear();
            let m = date.getMonth();

            let firstDayOfMonth = new Date(y, m, 1);
            let firstDay = getPreviousMonday(firstDayOfMonth);


            //Первый день недели(ПН)
            let firstDayOfWeek = new Date(firstDay);
            //Последний день недели(ВС)    
            let lastDayOfWeek = new Date(
              firstDayOfWeek.getFullYear(),
              firstDayOfWeek.getMonth(),
              firstDayOfWeek.getDate() + 6,
                23, 59, 59, 999);
            
            //Пока мы находимся в текущем месяце
            while ((firstDayOfWeek.getMonth() == m) || (lastDayOfWeek.getMonth() == m)) 
            {
                console.log("first=" + firstDayOfWeek.toDateString());
                console.log("last=" + lastDayOfWeek.toDateString());

                let week = {
                    BDate: firstDayOfWeek,
                    EDate: lastDayOfWeek,
                    Display: weekToString(firstDayOfWeek, lastDayOfWeek)
                };
                weeks.push(week);
                console.log(week.Display);
                //Первый день следующей недели (ПН)
                firstDayOfWeek = new Date(
                lastDayOfWeek.getFullYear(),
                lastDayOfWeek.getMonth(),
                lastDayOfWeek.getDate() + 1);
                //Последний день недели(ВС)    
                lastDayOfWeek = new Date(
                firstDayOfWeek.getFullYear(),
                firstDayOfWeek.getMonth(),
                firstDayOfWeek.getDate() + 6,
                    23, 59, 59, 999);
                
            }


            return weeks;
        }
    }
});