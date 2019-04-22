$(document).ready(function () {
    loadAllDeliverableProducts();
    loadScannedProducts();
});

function SaveScannedBarcodeToTextFile(event) {
    // var $form = $(btnClicked).parents('form');
    var code = $("#ProductCode").val();
    var tripId = $("#TripId").val();
    $.ajax({
        type: "POST",
        url: RootUrl + 'production/delivery/SaveScannedBarcodeToTextFile',
        data: { barcode: code, tripId: tripId },
        error: function (xhr, status, error) {
            alert(error);
        },
        success: function (response) {
            var result = response.Message;
            //alert("Saved Successfully");
            $('#message').html(response).fadeIn('slow');
            $('#message').html(result).fadeIn('slow'); //also show a success message
            $('#message').delay(700).fadeOut('slow');
            loadScannedProducts();
            $('#ProductCode').val("");
        }
    });
}

function SaveDispatchInfo(id) {

    if (confirm("Are you confirm to Dispatch?")) {
        var tripId = $("#TripId").val();
        $.ajax({
            type: "Post",
            url: RootUrl + 'production/delivery/SaveDispatchInformation',
            data: { tripId: tripId },
            error: function (xhr, status, error) {
                alert(error);
            },
            success: function (response) {
                loadAllDeliverableProducts();
                loadScannedProducts();
                window.location.href = RootUrl + "production/delivery/triplist";
            }
        });
    } else {
        return false;// if it's a link to prevent post
    }

    return false;// if it's a link to prevent post


}




function loadAllDeliverableProducts() {
    var tripId = $("#TripId").val();
    var json = { tripId: tripId };

    $.ajax({
        type: 'POST',
        url: RootUrl + "production/Delivery/LoadDeliverableProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#required_Trip_products").html(data);

        }
    });
}

function loadScannedProducts() {
    var tripId = $("#TripId").val();
    var json = { tripId: tripId };
    $.ajax({
        type: 'POST',
        url: RootUrl + "production/Delivery/LoadScannecdProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#scanned_Trip_Products").html(data);
            var requisitionQty = parseInt($("#total_requisition_qty").val());
            var scannedQty = parseInt($("#total_Scanned_qty").val());
            $("#lbl_requisition_qty").text(requisitionQty);
            $("#lbl_scanned_qty").text(scannedQty);
            if (requisitionQty !== 0) {
                $("#btnSaveDispatchInfo").prop('disabled', true);
                $("#div_status").html("<p style='color:red'>Not Complete</p>");
            } else {
                $("#btnSaveDispatchInfo").removeAttr('disabled');
                $("#div_status").html("<p style='color:green'>Complete</p>");
            }
        }
    });
}