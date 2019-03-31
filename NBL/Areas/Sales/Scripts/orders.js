



var productIdlist = [];
function AddItemToList(btnClicked) {
    var id = $("#ProductId").val();
    var a = $.inArray(id, productIdlist);
    if (a < 0) {
        productIdlist.push(id);
        var $form = $(btnClicked).parents('form');
        $.ajax({
            type: "POST",
            url: RootUrl + 'Sales/Order/Order',
            data: $form.serialize(),
            error: function (xhr, status, error) {
                //do something about the error
            },
            success: function (response) {
                //alert("Saved Successfully");
                ViewTempOrders();
            }
        });
    }
    else {
        alert("This Product already exits in the list");
    }

}




$(function () {
    $("#Quantity").change(function () {
        var unitPrice = $("#UnitPrice").val();
        var price = parseFloat(unitPrice).toFixed(2);
        $("#TotalAmount").val($(this).val() * price);
    });
});

function minmax(value, min, max) {
    if (parseInt(value) < min || isNaN(parseInt(value)))
        return 0;
    else if (parseInt(value) > max)
        return 0;
    else return value;
}


function myFunction(value) {
    //alert(value.id);
    var qty = value.id;
    var stock = $("#StockQty").val();
    var q = stock - qty;

    if (q < 0) {
        alert("Quantiy out of Stock!");
        return;
    }

}