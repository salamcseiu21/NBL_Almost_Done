function loadAllReceivableProducts() {
    var tripId = $("#TripId").val();

    $.ajax({
        type: 'GET',
        url: RootUrl + "Sales/Product/LoadReceiveableProduct",
        dataType: 'json',
        data: { tripId: tripId },
        success: function (data) {
            $('#tbl_receivable_list').dataTable({
                destroy: true,
                data: data,
                columns: [
                    { 'data': 'ProductName' },
                    { 'data': 'Quantity' }
                    
                ]
            });
        }
    });
}

