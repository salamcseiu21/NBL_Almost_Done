function Cancel(id) {

    if (confirm("Are you sure to cancel  this Order ?")) {
        var pId = btnClicked.id;
        var json = { productId: id.id };
        var $form = $(btnClicked).parents('form');
        $.ajax({
            type: "POST",
            url: RootUrl + 'Sales/Order/Cancel',
            data: $form.serialize(),
            error: function (xhr, status, error) {
                //do something about the error
            },
            success: function (response) {
                //alert("Saved Successfully");
                ViewTempOrders();
            }
        });
    } else {
        return false;// if it's a link to prevent post
    }

    return false;// if it's a link to prevent post


}