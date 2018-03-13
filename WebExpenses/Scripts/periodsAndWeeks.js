//Установить выбранный период(mm-YYYY) в контроллер
//в качестве текущего
//и обновить select с неделями
function setCurrentPeriod() {
    //var bDate = $("#week").val();
    var period = new Date($("#period").val());

    console.log(period);

    $.ajax({
        async: false,
        url: "/Purchase/SetCurrentPeriod",
        data: { period_: period.toLocaleDateString() },
        //data: { bDate_: bDate },
        success: replaceWeekSelect,
        error: function () { console.log('/Purchase/SetCurrentPeriod ошибка'); }
    });
}
//Установить выбранную неделю в контроллер
//в качестве текущей
//и обновить select с днями недели
function setCurrentWeek() {
    var value = $("#week").val();
    var bDate = -1;
    console.log('(#week").val=' + value);
    if (value != -1)
        bDate = new Date(value).toLocaleDateString();
    
    $.ajax({
        async: false,
        url: "/Purchase/SetCurrentWeekByBDate",
        data: { bDate_: bDate },
        //data: { bDate_: bDate },
        success: replaceDaysOfWeekSelect,
        error: function () { console.log('/Purchase/SetCurrentWeekByBDate ошибка'); }
    });
}
//Установить выбранный день недели в контроллер
//в качестве текущего
function setCurrentDayOfWeek() {

    var dayofweek = $("#daysOfWeek").val();
    var dayofweekInt = -1;
    if (dayofweek != -1)
        dayofweekInt = new Date(dayofweek).getDay();

    console.log(dayofweekInt);
    $.ajax({
        async: false,
        url: "/Purchase/SetCurrentDay",
        data: { dayOfWeek_: dayofweekInt },
        success: function () {
            replacePurchaseTable();
            console.log("/Purchase/SetCurrentDay успех");
        },
        error: function () {
            console.log("/Purchase/SetCurrentDay ошибка");
        }
    });
}

//обновить недели в периоде(mm-YYYY)
function replaceWeekSelect()
{
    
    $.ajax({
        async: false,
        url: "/Purchase/WeekOptions",
        success: function (result) {
            var weekSelect = $('#week')[0];
            weekSelect.options.length = 0;
            weekSelect.innerHTML = result;

            console.log('/Purchase/WeekOptions успех');
            replaceDaysOfWeekSelect();
        },
        error: function() {
            console.log('/Purchase/WeekOptions ошибка')
        }
    });

    setCurrentWeek();
}
//обновить дни недели
function replaceDaysOfWeekSelect()
{
    $.ajax({
        async: false,
        url: "/Purchase/DaysOfWeekSelect",
        success: function (result) {
            var weekSelect = $('#daysOfWeek')[0];
            weekSelect.options.length = 0;
            weekSelect.innerHTML = result;

            setPurchaseDetailClickHandler();
            console.log('/Purchase/DaysOfWeekSelect успех');
        },
        error: function () {
            console.log('/Purchase/DaysOfWeekSelect ошибка');
        }
    })

    setCurrentDayOfWeek();

}
//Обновить таблицу расходов
function replacePurchaseTable()
{
    $.ajax({
        async: false,
        url: "/Purchase/PurchaseSumByGroupTotal",
        success: function (result) {
            var purchTable = $('#purchTBody')[0];
            purchTable.innerHTML = result;

            setPurchaseDetailClickHandler();
            console.log('/Purchase/PurchaseSumByGroupTotal успех');
        },
        error: function () {
            console.log('/Purchase/PurchaseSumByGroupTotal ошибка');
        }
    })
}
