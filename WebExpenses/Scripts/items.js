function setEditDeleteItemClickEventHandler() {
    console.log("Items click установлен:b");
    $('div#items tbody tr th.itemEdit a').click(setCurrentIId);
    $('div#items tbody tr th.itemDelete a').click(setCurrentIId);
    console.log("Items click установлен:e");
}
/*
function ajaxItems() {
    $.ajax({
        url: "/Item/ItemsTable",
        success: function (result) {
            console.log('into ajaxItems success');
            //асинхронно меняем список товаров
            var table = $('#itemsTable')[0];
            //table.innerHTML = result;

            console.log('table' = table);
            console.log('result' = result);
            //alert(result);
            
            
            
            //Привязываем к кнопкам "редактировать", "удалить"
            //скрипты, чтобы устанавливались текущие товары перед кликом по линку
            setEditDeleteItemClickEventHandler();
        },
        error: function () {
            alert("Ошибка ajaxItems!");
        }
    });
}
*/
//синхронно устанавливаем текущий товар
function setCurrentIId() {
    let iid = $(this)[0].parentElement.parentElement.getElementsByClassName('itemId')[0].textContent;
    console.log('currentItemId='+iid);
    $.ajax({
        url: "/Item/SetCurrentIId",
        data: { iid_: iid },
        async: false
    })
}