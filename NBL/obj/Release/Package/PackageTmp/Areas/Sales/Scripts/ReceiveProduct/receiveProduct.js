$(document).ready(function () {
    loadAllReceivableProducts();
    loadScannedProducts();
});

function SaveScannedBarcodeToTextFile(btnClicked) {
    // var $form = $(btnClicked).parents('form');
    var code = $("#ProductCode").val();
    var tripId = $("#TripId").val();
    $.ajax({
        type: "POST",
        url: RootUrl + 'Sales/Product/SaveScannedBarcodeToTextFile',
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

function loadAllReceivableProducts() {
    var tripId = $("#TripId").val();
    var json = { tripId: tripId };

    $.ajax({
        type: 'POST',
        url: RootUrl + "Sales/Product/LoadReceiveableProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#receivable_list").html(data);
        }
    });
}

function ReceiveProductToBranch(btnClick) {
    var tripId = $("#TripId").val();
    if (confirm("Are you sure to Save?")) {
        //$("#scanProductBarcodeForm").submit();
        $.ajax({
            type: "Post",
            url: RootUrl + 'Sales/Product/ReceiveProduct',
            data: { tripId: tripId },
            error: function (xhr, status, error) {
                alert(error);
            },
            success: function (response) {
                loadAllReceivableProducts();
                loadScannedProducts();
                window.location.href = RootUrl + "sales/product/receive";
            }
        });

    } else {
        return false;// if it's a link to prevent post
    }
}

function loadScannedProducts() {
    var tripId = $("#TripId").val();
    var json = { tripId: tripId };
    $.ajax({
        type: 'POST',
        url: RootUrl + "sales/product/LoadScannecdProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#scanned_Products").html(data);
        }
    });
}