$("#tbl_Temp_return_Products").DataTable();
function DeleteProduct(btnClick) {

    var isComfirm = confirm("Are you sure to remove this product ?");
    if (isComfirm) {

        DeleteProductFromTripXmlFile(btnClick);
    }
    return false;
}

function DeleteProductFromTripXmlFile(btnClick) {
  
    var json = { returnId: btnClick.value };
    $.ajax({
        type: "POST",
        url: RootUrl + 'ResearchAndDevelopment/product/DeleteProductFromTempReturn',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(json),
        error: function (xhr, status, error) {
            alert(error);
        },
        success: function (response) {
            var result = response.Message;
            //alert("Saved Successfully");
            $('#message').html(response).fadeIn('slow');
            $('#message').html(result).fadeIn('slow'); //also show a success message
            $('#message').delay(700).fadeOut('slow');
            loadTempReturnProducts();
            $('#Deliverydetails').html('');
        }
    });
}