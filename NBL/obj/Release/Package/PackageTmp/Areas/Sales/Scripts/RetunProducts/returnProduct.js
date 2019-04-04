$(document).ready(function () {
    loadTempReturnProducts();
});
$("#tbl_Temp_return_Products").DataTable();

function loadTempReturnProducts() {
    $.ajax({
        url: RootUrl + "sales/return/GetTempReturnProducts",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            // var qty = data.Quantity;
            // $("#RequisitionQty").val(qty);
            $('#TempReturnProducts').html(data);
            var tQty = parseInt($("#total_returns_qty").val());
            if (tQty > 0) {
                $("#confirmButton").removeAttr('disabled');
            } else {
                $("#confirmButton").prop("disabled", true);
            }
        }
    });
}

function ConfirmToReturn(btnClick) {
    var isConfirm = confirm("Are you Confirm?");
    if (isConfirm) {
        window.location.href = RootUrl + "sales/return/ConfirmReturnEntry";
    }
    return false;
}

function RemoveAll() {
    var result = confirm('Are you sure to remove all items?');
    if (result) {

        $.ajax({
            type: 'POST',
            url: RootUrl + 'sales/return/RemoveAll',
            success: function (data) {
                loadTempReturnProducts();
            }
        });
    }
}


function ClearFields() {
    $("#Deliverydetails").html("");
    $("#ClientName").val('');
    $("#DeliveryId").empty();
    $("#DeliveryId").append('<option value="">--Select--</option>');
}




$(function () {
    $("#ClientName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl + 'Common/ClientNameAutoComplete/',
                data: "{ 'prefix': '" + request.term + "'}",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            return item;
                        }));
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            });
        },
        select: function (e, i) {
            RemoveAllProductsWhenClientChange();
            $("#DeliveryId").empty();
            $("#DeliveryId").append('<option value="">--Select--</option>');
            $("#ClientId").val(i.item.val);
            var json = { clientId: i.item.val };

            $.ajax({

                type: "POST",
                url: RootUrl + 'sales/Order/GetDeliveredOrderByClientId/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    $.each(data, function (key, value) {
                        //alert(key);
                        $("#DeliveryId").append('<option value=' + value.DeliveryId + '>' + value.DeliveryRef + '</option>');

                    });
                }
            });
        },
        minLength: 1
    });
});
function RemoveAllProductsWhenClientChange()
{
    $.ajax({
        type: 'POST',
        url: RootUrl + 'sales/return/RemoveAll',
        success: function (data) {
            loadTempReturnProducts();
        }
    });
}
function ShowDeliveryDetails(event) {
    $('#Deliverydetails').html('');
    var deliveryId = event.value;
    var json = { deliveryId: deliveryId };

    $.ajax({
        type: "POST",
        url: RootUrl + 'sales/return/DeliveryDetailsByDeliveryId',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(json),
        success: function (data) {
            // var qty = data.Quantity;
            // $("#RequisitionQty").val(qty);

            $('#Deliverydetails').html(data);

        }
    });
}