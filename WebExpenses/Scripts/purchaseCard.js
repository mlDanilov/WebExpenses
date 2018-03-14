function purchaseCardGroupChanged() {

    let pItG = $("#purchItemGroup")[0];
    console.log('purchItemGroup=' + pItG.localName);
    /*
    let purchaseId = pTh.textContent;
    console.log('purchaseId=' + purchaseId);
    $.ajax({
        url: "/Purchase/SetCurrentPurchaseId",
        data: { purchaseId_: purchaseId },
        async: false,
        success: function () { console.log('func SetCurrentPurchaseId успех'); },
        error: function () { console.log('func SetCurrentPurchaseId ошибка'); }
    })
    */
}

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


