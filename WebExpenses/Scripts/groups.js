//Перебирает в таблице div c id= groups в теле(tbody) все tr, 
//и устанавливает атрибут current тому, на который кликнул, 
//а с остальных tr убирает
//затем, асинхронный запрос к контроллеру, SetCurrentId
//устанавливает текущую группу
//В случае успеха запроса выполняет обновление списка товаров(скрипт ajaxItems)
function setCurrentGroupByClick() {

    //let gId = $(this)[0].getElementsByClassName('groupId')[0].textContent;
    let gId = $(this)[0].parentElement.getElementsByClassName('groupId')[0].textContent;
    //console.log('gId=' + gId);
    let trArray = $('div#groups tbody tr');
    for (let i = 0; i < trArray.length; i++)
    {
        let gId_item = trArray[i].getElementsByClassName('groupId')[0].textContent;
        trArray[i].removeAttribute('class')
        //console.log('gId_item='+gId_item);
        if (gId_item == gId) {
            trArray[i].setAttribute('class', 'current')
        }
        else {

        }
    }
    
    $.ajax({
        url: "/Group/SetCurrentGId",                      
        data: { gId_: gId },
        success: ajaxItems,
        //success: ajaxItemsJSON,
        error: function () {
            alert('/Group/SetCurrentGId ошибка');
        }});
}

function setCurrentGId()
{
    let gId = $(this)[0].parentElement.parentElement.getElementsByClassName('groupId')[0].textContent;
    $.ajax({
        url: "/Group/SetCurrentGId",
        data: { gId_: gId },
        async: false
    })
}
