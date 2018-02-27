function ajaxItems() {
    $.ajax({
        url: "/Expenses/ItemsTable",
        success: function (result) {
            var table = $('#itemsTable')[0];
            table.innerHTML = result;
        },
        error: function () {
            alert("Ошибка!");
        }
    });
}
/*
function ajaxItemsJSON() {
    $.ajax({
        url: "/Expenses/ItemsTableJSON",
        success: function (data) {
            //alert(JSON.stringify(data));
            //alert(data.GroupId);
            
            alert(JSON.stringify(data.ItemList));
            var tBody = $('#itemsTable tbody');
            tBody.empty();
            alert(JSON.stringify(data.ItemList.length));
            for (let i = 0; i < data.ItemList.length; i++)
            {
                let item = data.ItemList[i];
                tBody.append(
                    "<tr><th>" + i + "</th><th>" + item.Id + "</th><th>" + item.Name + "</th><th>" + "</th><th></tr>"
                    );
            }
        },
        error: function () {
            alert("Ошибка!");
        }
    });
}
*/