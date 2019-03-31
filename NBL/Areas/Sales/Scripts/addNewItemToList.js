function AddItemToList(btnClicked) {
    var id = $("#ProductId").val();
    //alert(btnClicked.id);

    var a = $.inArray(id, productIdlist);

    if (a < 0) {
        productIdlist.push(id);
        var stock = $("#StockQty").val();
        var qty = $("#Quantity").val();
        if (stock - qty >= 0) {
            var $form = $(btnClicked).parents('form');
            $.ajax({
                type: "POST",
                url: RootUrl + 'sales/order/AddNewItemToExistingOrder',
                data: $form.serialize(),
                error: function (xhr, status, error) {
                    //do something about the error
                },
                success: function (response) {
                    //alert("Saved Successfully");
                    ViewTempOrders();
                }
            });

            return false; // if it's a link to prevent post
        } else {
            alert("Quantity Out of Stock!");
        }
    }
    else {
        alert("This Product already exits in the list");
    }



}