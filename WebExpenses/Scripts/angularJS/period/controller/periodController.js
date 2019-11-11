'use strict'
var purchApp = angular.module('purchApp');
purchApp.controller("periodController", function ($scope,
    Purchases, Periods, Years, Monthes, DaysOfWeek) {
    //$scope.periodTest = "periodController";

    $scope.currentYear = Periods.Void;
    $scope.currentMonth = Periods.Void;
    $scope.currentWeek = Periods.Void;
    $scope.currentDay = Periods.Void;

    $scope.weekList = Periods.Void;
    $scope.dayList = undefined;

    //Изменился текущий год
    $scope.selectedYearChanged = function () {
        Years.currentYear = $scope.currentYear;
        $scope.selectedMonth = Periods.Void;
        $scope.selectedWeek = "-";
        $scope.selectedDay = "-";

        let bDate = new Date(Years.currentYear, 0, 1);
        let eDate = new Date(Years.currentYear, 11, 31, 23, 59, 59);

        $scope.reloadPurchases(bDate, eDate);
        $scope.Monthes = $scope.getMonthes();
    },

    //Изменился текущий месяц
    $scope.selectedMonthChanged = function () {
        //console.log($scope.currentMonth);
        $scope.selectedWeek = Periods.Void;
        $scope.selectedDay = Periods.Void;
        let y = $scope.currentYear;
        var firstDay = undefined;
        var lastDay = undefined;
        if ($scope.currentMonth.Id != -1) {
            let m = $scope.currentMonth.Id;
            firstDay = new Date(y, m, 1);
            lastDay = new Date(y, m + 1, 0);
           
        }
        else {
            firstDay = new Date(y, 0, 1);
            lastDay = new Date(y, 11, 31, 23, 59, 59);
        }
        $scope.beginAndEndDatesChanged(firstDay, lastDay);
        $scope.getWeeks();
    },
    //Изменилась текущая неделя
    $scope.selectedWeekChanged = function () {
        let y = $scope.currentYear;
        let m = $scope.currentMonth.Id;
        let week = $scope.currentWeek;

        $scope.selectedDay = Periods.Void;
        $scope.dateList = Periods.Void;
        var firstDay = undefined;
        var lastDay = undefined;

        if (week.Id != -1) {
            firstDay = week.BDate;
            lastDay = week.EDate;
        }
        else {
            firstDay = new Date(y, m, 1);
            let firstDayMilliseconds = firstDay.getTime();
            if (m != 11) 
                lastDay = new Date(y, m + 1, 1);
            else
                lastDay = new Date(y + 1, 0, 1);
            lastDay = new Date(lastDay.getTime() - 1);
        }
        console.log('firstDay=' + firstDay);
        console.log('lastDayDay=' + lastDay);
        $scope.beginAndEndDatesChanged(firstDay, lastDay);
        $scope.getDays();
    },

    //Изменился текущий день
    $scope.selectedDayChanged = function () {
        let y = $scope.currentYear;
        let m = $scope.currentMonth.Id;
        let week = $scope.currentWeek;
        let day = $scope.currentDay;

        var firstDay = undefined;
        var lastDay = undefined;

        if (day.Id != -1) {
            firstDay = day.BDate;
            lastDay = day.EDate;
        }
        else {
            firstDay = week.BDate;
            lastDay = week.EDate;
        }
        console.log('firstDay=' + firstDay);
        console.log('lastDayDay=' + lastDay);
        $scope.beginAndEndDatesChanged(firstDay, lastDay);
    }
    
    $scope.reloadPurchases = function (bDate_, eDate_) {
        //Вызвать в родительском контроллере событие 
        $scope.$emit('yearsChanges', { 'bDate': bDate_, 'eDate': eDate_ });
    }
    $scope.beginAndEndDatesChanged = function (bDate_, eDate_) {
        //Вызвать в родительском контроллере событие 
        $scope.$emit('beginAndEndDatesChanged', { 'bDate': bDate_, 'eDate': eDate_ });
    }
  
    //Взять года
    $scope.getYears = function () {
        Periods.getAllYears().then(
            function successCallback(response) {
                let years = response.data;
                years.sort();
                //years.unshift('-');
                //Years.currentYear = '-';
                Years.setYears(years);
                console.log('getAllYears успех');
            },
            function errorCallback(response) {
                console.log('getAllYears ошибка:' + response.data.Message);
                //window.document = response.data;
                window.document.body.innerHTML = response.data;

            });
    }

    $scope.getYears();

    $scope.getAllYears = function () {
        let years = Years.getYears();
        return years;
    }

    //Взять месяцы из сгруппированных покупок
    $scope.getMonthes = function ()
    {
        let monthes = [Periods.Void];
        //let monthes = ["-" ];
        let gPurch = Purchases.GroupedPurchases;
        if (!gPurch)
            return monthes;

        //Немного костыльно. 
        //В Purchases правильнее все-таки было бы хранить список покупок
        //А потом процедурой дергать сгруппированные покупи
        gPurch.forEach(
            gp => {
                gp.Purchases.forEach(p => {
                    let monthId = p.Date.getMonth();
                    if (!monthes.find(m => m.Id == monthId))
                    {
                        let month = Monthes.GetMonthById(monthId);
                        monthes.push(month);
                    }
                });
            }
            );
        monthes.sort((m1, m2) => m1.Id > m2.Id);
        
        return monthes;
    }

    //Взять недели
    $scope.getWeeks = function () {

        let y = $scope.currentYear;
        let m = $scope.currentMonth.Id;
        if ((y == undefined) || (y == -1) || (m == undefined) || (m == -1))
            return;

        let firstDay = new Date(y, m, 1);
        //Кол-во милисекунд после 1970.01.01
        let lastDayMilliseconds = new Date(y, m + 1, 1).getTime();
        let lastDay = new Date(lastDayMilliseconds - 1);

        console.log(firstDay);
        console.log(lastDay);

        let weeks = DaysOfWeek.GetWeeksByMonth(firstDay);
        $scope.weekList = weeks;
    }

    //Взять дни за неделю
    $scope.getDays = function () {

        let y = $scope.currentYear;
        let m = $scope.currentMonth.Id;
        let w = $scope.currentWeek;

        if ((y == undefined) || (y == -1) ||
            (m == undefined) || (m == -1) ||
            (w == undefined) || (w.Id == -1))
            return;

        let d = w.BDate;
        
        let days = [
            {
                Id : -1,
                Display : "-"
            },
            {
                Id: w.BDate.getDay(),
                BDate: w.BDate,
                EDate: new Date(d.getFullYear(), d.getMonth(), d.getDate(), 23, 59, 59, 999),
                Display: DaysOfWeek.GetDayOfWeekById(w.BDate.getDay()).ShortName+ " - " + w.BDate.getDate()
            }
        ];
        let day = w.BDate;
        for (let i = 0; i < 6; i++)
        {
            //Добавляем один день в милисекундах
            d = new Date(d.getTime() + 24 * 60 * 60 * 1000);
            let day = {
                Id : d.getDay(),
                BDate: d,
                EDate: new Date(d.getFullYear(), d.getMonth(), d.getDate(), 23, 59, 59, 999),
                Display: DaysOfWeek.GetDayOfWeekById(d.getDay()).ShortName+ " - " +d.getDate()
            };
            days.push(day);
            console.log('day.BDate=' + day.BDate + ' ' +day.Display);
            console.log('day.EDate=' + day.EDate+ ' ' +day.Display);
        }
        $scope.dayList = days;
    }


});