function ajaxItems() {
    $.ajax({
        url: "/Expenses/Items",
        success: function (result) {
            $('#itemsTable')[0].innerHTML = result;
        },
        error: function () {
            alert("Ошибка!");
        }
    });
}
