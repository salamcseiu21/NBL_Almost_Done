$(document).ready(function () {
    loadTempTripProducts();
});
$("#tbl_requisition_list").DataTable();


function AddRequisition(btnClicked) {
    //alert(str);
    var $form = $(btnClicked).parents('form');
    $.ajax({
        type: "POST",
        url: '@Url.Action("AddRequistionToTripXmlFile")',
        data: $form.serialize(),
        error: function (xhr, status, error) {
            //do something about the error
        },
        success: function (response) {
            //alert("Saved Successfully");
            $("#RequisitionRef").val("");
            $('#RequisitionId').val(0);
            $('#RequisitioRequisitionQtynId').val(0);
        }
    });
    return false; // if it's a link to prevent post

}
function loadTempTripProducts() {
    $.ajax({
        url: RootUrl + "production/transfer/GetTempTrip",
        type: "Get",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            // var qty = data.Quantity;
            // $("#RequisitionQty").val(qty);
            $("#RequisitionRef").val("");
            $('#TempTripProducts').html(data);
            var tQty = parseInt($("#total_requisition_qty").val());
            if (tQty > 0) {
                $("#confirmButton").removeAttr('disabled');
            } else {
                $("#confirmButton").prop("disabled", true);
            }
        }
    });
}

function ConfirmTrip(btnClick) {
    var isConfirm = confirm("Are you Confirm to Crate Trip ?");
    if (isConfirm) {
        window.location.href = RootUrl + "production/transfer/ConfirmTrip";
    }
    return false;
}

$(function () {
    $("#RequisitionRef").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl + 'production/transfer/GetRequisitionRefeAutoComplete/',
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
            $("#RequisitionId").val(i.item.val);
            var requisitionId = i.item.val;
            var json = { requisitionId: requisitionId };

            $.ajax({
                type: "POST",
                url: RootUrl + 'production/transfer/GetRequisitionById',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    // var qty = data.Quantity;
                    // $("#RequisitionQty").val(qty);

                    $('#Requisitiondetails').html(data);

                }
            });
        },
        minLength: 1
    });
});

function RemoveAll() {
    var result = confirm('Are you sure to remove all items?');
    if (result) {

        $.ajax({
            type: 'POST',
            url: RootUrl + 'production/transfer/RemoveAll',
            success: function (data) {
                loadTempTripProducts();
            }
        });
    }
}


function ClearFields() {
    $("#Requisitiondetails").html("");
    $("#RequisitionRef").val('');
}