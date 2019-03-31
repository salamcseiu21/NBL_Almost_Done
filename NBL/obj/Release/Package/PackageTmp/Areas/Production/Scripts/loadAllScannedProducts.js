function loadAllScannedProducts() {
    $.ajax({
        type: 'GET',
        url: RootUrl + "production/product/LoadScannedProducts",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            $('#production_list').dataTable({
                destroy: true,
                data: data,
                columns: [
                    { 'data': 'ProductCode' },
                    { 'data': 'ProductName' },
                    { 'data': 'CategoryName' }
               
                ]
            });
        }
    });
}