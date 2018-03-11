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

function setCurrentPurchaseGId() {

    let groupPurchTr = $(this)[0].parentElement.getElementsByClassName('wPurchGroupId')[0];
    /*
    let thDetail = $(this)[0].parentElement.getElementsByClassName('wPurchGroupDetail')[0];
    if (thDetail.textContent == "Подробнее")
        thDetail.textContent = "Свернуть";
    else
        thDetail.textContent = "Подробнее";
    */
    let gId = groupPurchTr.textContent;

    $.ajax({
        url: "/Purchase/SetCurrentPurchaseGId",
        data: { gId_: gId },
        async: false,
        success: function () { console.log('func setCurrentPurchaseGId успех');},
        error: function () { console.log('func setCurrentPurchaseGId ошибка'); }
    })
    let selector = 'tr#purchDetail' + gId.toString();
    console.log('selector=' + selector);
    //let trArray = groupPurchTr.querySelectorAll(selector);
    let trArray = $(selector);
    console.log(trArray.length);
    for (let i = 0; i < trArray.length; i++) {
        var tr = trArray[i];
        var c = tr.getAttribute('class');
        console.log(c);
        tr.removeAttribute('class');
        if (c != 'detail_none')
            tr.setAttribute('class', 'detail_none');
        else {
            if (i != 0)
                tr.setAttribute('class', 'detail_visible' + ((i % 2) + 1).toString());
            else
                tr.setAttribute('class', 'detailHeader_visible');
        }
        console.log('set display none success')
    }
    
    //weekSelect.innerHTML = result;
}

function setPurchaseDetailClickHandler()
{
    $(document).ready(function () {
        console.log('PurchGroupDetail.b');
        $('tbody#purchTBody tr th.wPurchGroupDetail').click(setCurrentPurchaseGId);
        console.log('PurchGroupDetail.e');
    });
}

