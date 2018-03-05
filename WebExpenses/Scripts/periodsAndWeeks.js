function setCurrentWeek() {
    //var bDate = $("#week").val();
    var bDate = new Date($("#week").val());

    console.log(bDate);

    $.ajax({
        url: "/Purchase/SetCurrentWeekByBDate",
        data: { bDate_: bDate.toLocaleDateString() },
        //data: { bDate_: bDate },
        success: function () { console.log('/Purchase/SetCurrentWeekByBDate успех'); },
        error: function () { console.log('/Purchase/SetCurrentWeekByBDate ошибка'); }
    });
}

function setCurrentPeriod() {
    //var bDate = $("#week").val();
    var period = new Date($("#period").val());

    console.log(period);

    $.ajax({
        url: "/Purchase/SetCurrentPeriod",
        data: { period_: period.toLocaleDateString() },
        //data: { bDate_: bDate },
        success: replaceWeekSelect,
        error: function () { consol.log('/Purchase/SetCurrentPeriod ошибка'); }
    });
}

function replaceWeekSelect()
{
    console.log('мы в replaceWeekSelect');
    $.ajax({
        url: "/Purchase/WeekOptions",
        success: function (result) {

            var weekSelect = $('#week')[0];
            weekSelect.options.length = 0;
            weekSelect.innerHTML = result;

            console.log('before /Purchase/DaysOfWeekSelect');

            $.ajax({
                url: "/Purchase/DaysOfWeekSelect",
                success: function (result) {
                    console.log('/Purchase/DaysOfWeekSelect успех');
                    var weekSelect = $('#daysOfWeek')[0];
                    weekSelect.options.length = 0;
                    weekSelect.innerHTML = result;
                },
                error: function ()
                {
                    console.log('/Purchase/DaysOfWeekSelect ошибка');
                }
            })
        }
    });
}

function dayOfWeekSelect() {
    var dayofweek = $("#daysOfWeek").val();
    console.log(dayofweek);
    $.ajax({
        url: "/Purchase/SetCurrentDay",
        data: { dayOfWeek_ : dayofweek },
        success: function () {
            console.log("/Purchase/SetCurrentDay успех");
        },
        error: function () {
            console.log("/Purchase/SetCurrentDay ошибка");
        }
    });
}
