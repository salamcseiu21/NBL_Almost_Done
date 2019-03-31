function RemoveProduct(btnClicked) {

    if (confirm("Are you sure to remove this product ??")) {

        $("#productIdToRemove").val(btnClicked.id);
        alert(btnClicked.id);
        var $form = $(btnClicked).parents('form');
        $.ajax({
            type: "POST",
            url: RootUrl+'sales/order/UpdateOrder',
            data: $form.serialize(),
            error: function (xhr, status, error) {
                //do something about the error
            },
            success: function (response) {
                for (var i = productIdlist.length - 1; i >= 0; i--) {
                    if (productIdlist[i] === btnClicked.id) {
                        productIdlist.splice(i, 1);
                    }
                }
                ViewTempOrders();

            }
        });
    } else {
        return false;// if it's a link to prevent post
    }

    return false;// if it's a link to prevent post

}