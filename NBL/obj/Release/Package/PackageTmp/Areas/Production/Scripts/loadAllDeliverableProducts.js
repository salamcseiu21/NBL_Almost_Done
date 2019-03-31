function loadAllDeliverableProducts() {
    var tripId = $("#TripId").val();
    $.ajax({
        type: 'GET',
        url: RootUrl + "production/Delivery/LoadDeliverableProduct",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { tripId: tripId },
        success: function (data) {
            $("#required_Trip_products").html(data);
            $("#tbl_required_Trip_products").DataTable();
        }
    });
}