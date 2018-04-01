//Установить текущую группу покупок
//и скрыть/развернуть детали
function setCurrentPurchaseGId() {

    let groupPurchTr = $(this)[0].parentElement.getElementsByClassName('wPurchGroupId')[0];

    let gId = groupPurchTr.textContent;

    $.ajax({
        url: "/Purchase/SetCurrentPurchaseGId",
        data: { gId_: gId },
        async: false,
        success: function () { console.log('func setCurrentPurchaseGId успех'); },
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

//Установить обработчик Click на кнопку "Подробнее/Скрыть"
function setPurchaseDetailClickHandler() {
    $(document).ready(function () {
        console.log('PurchGroupDetail.b');
        $('tbody#purchTBody tr th.wPurchGroupDetail').click(setCurrentPurchaseGId);
        console.log('PurchGroupDetail.e');
    });
}

function setCurrentPurchaseId() {

    let pTh = $(this)[0].parentElement.getElementsByClassName('purchId')[0];
    console.log('pTh=' + pTh);
    let purchaseId = pTh.textContent;
    console.log('purchaseId=' + purchaseId);
    $.ajax({
        url: "/Purchase/SetCurrentPurchaseId",
        data: { purchaseId_: purchaseId },
        async: false,
        success: function () { console.log('func SetCurrentPurchaseId успех'); },
        error: function () { console.log('func SetCurrentPurchaseId ошибка'); }
    })  
}

function setPurchaseEditDeleteClickHandler() {
    $(document).ready(function () {
        console.log('PurchGroupEditDelete.b');
        $('tbody#purchTBody tr th.purchEdit').click(setCurrentPurchaseId);
        $('tbody#purchTBody tr th.purchDelete').click(setCurrentPurchaseId);
        console.log('PurchGroupEditDelete.e');
    });
}
//Обновить "Итоги" в заголовке таблицы "Расходы"
function updateTotalSumIntoPurchaseHeader()
{
    $.ajax({
        url: "/Purchase/PurchaseTotalSum",
        async: true,
        success: function (data)
        {
            //alert(data);
            var th = $('table#purchTable thead tr th#total')[0];
            th.textContent = data;
            console.log(th);
            console.log('func /Purchase/PurchaseTotalSum успех');
        },
        error: function () { console.log('func /Purchase/PurchaseTotalSum ошибка'); }
    })

}

    //weekSelect.innerHTML = result;




