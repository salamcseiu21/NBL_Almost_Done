$(document).ready(function () {
    loadAllReceivableProducts();
    loadScannedProducts();
});

function SaveScannedBarcodeToTextFile(btnClicked) {
    // var $form = $(btnClicked).parents('form');
    var code = $("#ProductCode").val();
    var salesReturnId = $("#SalesReturnId").val();
    $.ajax({
        type: "POST",
        url: RootUrl + 'qc/Product/SaveScannedBarcodeToTextFile',
        data: { barcode: code, salesReturnId: salesReturnId },
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
    var salesReturnId = $("#SalesReturnId").val();
    var json = { salesReturnId: salesReturnId };

    $.ajax({
        type: 'POST',
        url: RootUrl + "qc/Product/LoadReceiveableProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#receivable_list").html(data);
        }
    });
}

function ReceiveProductToBranch(btnClick) {
    var salesReturnId = $("#SalesReturnId").val();
    if (confirm("Are you sure to Save?")) {
        //$("#scanProductBarcodeForm").submit();
        $.ajax({
            type: "Post",
            url: RootUrl + 'qc/Product/ReceiveProduct',
            data: { salesReturnId: salesReturnId },
            error: function (xhr, status, error) {
                alert(error);
            },
            success: function (response) {
                loadAllReceivableProducts();
                loadScannedProducts();
                window.location.href = RootUrl + "qc/product/viewallreturns";
            }
        });

    } else {
        return false;// if it's a link to prevent post
    }
}

function loadScannedProducts() {
    var salesReturnId = $("#SalesReturnId").val();
    var json = { salesReturnId: salesReturnId };
    $.ajax({
        type: 'POST',
        url: RootUrl + "qc/product/LoadScannecdProduct",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(json),
        success: function (data) {
            $("#scanned_Products").html(data);
        }
    });
}