//Перебирает в таблице div c id=shops в теле(tbody) все tr, 
//и устанавливает атрибут current тому, на который кликнул, 
//а с остальных tr убирает
//затем, асинхронный запрос к контроллеру, SetCurrentId
//устанавливает текущую группу
//В случае успеха запроса выполняет обновление списка товаров(скрипт ajaxItems)
function setCurrentShopByClick() {

    //let gId = $(this)[0].getElementsByClassName('groupId')[0].textContent;
    let shId = $(this)[0].parentElement.getElementsByClassName('shopId')[0].textContent;
    console.log('shId=' + shId);
    let trArray = $('div#shops tbody tr');
    for (let i = 0; i < trArray.length; i++) {
        let shId_item = trArray[i].getElementsByClassName('shopId')[0].textContent;
        trArray[i].removeAttribute('class')
        //console.log('gId_item='+gId_item);
        if (shId_item == shId) {
            trArray[i].setAttribute('class', 'current')
        }
        else {

        }
    }

    $.ajax({
        url: "/Shop/SetCurrentShopId",
        data: { shId_: shId },
        success: function () {
            console.log('/Shop/SetCurrentShopId успех');
        },
        error: function () {
            console.log('/Shop/SetCurrentShopId ошибка');
        }
    });
}

function setCurrentShopId() {
    let shId = $(this)[0].parentElement.parentElement.getElementsByClassName('shopId')[0].textContent;
    $.ajax({
        url: "/Shop/SetCurrentShopId",
        data: { shId_: shId },
        async: false
    })
}