function ajaxItems() {
    $.ajax({
        url: "/Expenses/ItemsTable",
        success: function (result) {
            //асинхронно меняем список товаров
            var table = $('#itemsTable')[0];
            table.innerHTML = result;
            
            //Привязываем к кнопкам "редактировать", "удалить"
            //скрипты, чтобы устанавливались текущие товары перед кликом по линку
            $('div#items tbody tr th.itemEdit a').click(setCurrentIId);
            $('div#items tbody tr th.itemDelete a').click(setCurrentIId);
            console.log('items events');
        },
        error: function () {
            alert("Ошибка!");
        }
    });
}
//синхронно устанавливаем текущий товар
function setCurrentIId() {
    let iid = $(this)[0].parentElement.parentElement.getElementsByClassName('itemId')[0].textContent;
    console.log('currentItemId='+iid);
    $.ajax({
        url: "/Expenses/SetCurrentIId",
        data: { iid_: iid },
        async: false
    })
}