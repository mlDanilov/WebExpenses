//В карточке покупки при смене группы товаров
//поменять в select'е товаров, список товаров
function purchaseCardGroupChanged() {

    let selectPurchItG = $("#purchItemGroup")[0];
    console.log('purchItemGroup=' + selectPurchItG.localName);
    let value = $("#purchItemGroup").val();
    console.log('purchItemGroup.value=' + value);
    console.log('selectPurchItG.innerHTML=' + selectPurchItG.innerHTML);
    
    $.ajax({
        url: "/Item/ItemOptions",
        data: { groupId_: value },
        async: false,
        success: function (result) {
            let itemsSelect = $("#Item_Id")[0];
            itemsSelect.innerHTML = result;
            console.log('func purchCardItemsRefresh успех');
        },
        error: function () { console.log('func purchCardItemsRefresh ошибка'); }
    })
}
//Установить обработчик для смены текущего option'а 
//в select'е с группами товаров в карточке покупки
function setPurchaseCardGroupSelectChange() {
    $(document).ready(function () {
        console.log('PurchCardGroup.SetHandler.b');
        var r = $("#purchItemGroup")[0];
        console.log(r);
        //r.onchange(purchaseCardGroupChanged);
        $("#purchItemGroup").change(purchaseCardGroupChanged);
        
        console.log('PurchCardGroup.SetHandler.e');
    });
}


