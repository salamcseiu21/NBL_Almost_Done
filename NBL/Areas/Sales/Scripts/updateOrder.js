function Update(btnClicked) {
    $("#productIdToRemove").val(0);
    var $form = $(btnClicked).parents('form');
    var pId = btnClicked.id;

    var json = { productId: pId };

    $.ajax({
        type: "POST",
        url: RootUrl + 'Common/GetProductQuantityInStockById',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(json),
        success: function (data) {

            var qty = data.StockQty;
            $("#StockQty").val(qty);
            var q = qty - btnClicked.value;
            if (q >= 0) {
                //alert("OK");
                $.ajax({
                    type: "POST",
                    url: RootUrl + 'Sales/Order/UpdateOrder',
                    data: $form.serialize(),
                    error: function (xhr, status, error) {
                        //do something about the error
                    },
                    success: function (response) {
                        ViewTempOrders();

                    }
                });

                return false; // if it's a link to prevent post
            } else {
                alert("Quantity out of stock");
                ViewTempOrders();
                return $form;
            }
        }
    });

}