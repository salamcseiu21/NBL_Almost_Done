function loadAllDeliverableOrderedProducts() {
    var invoiceId = $("#InvoiceId").val();
    $.ajax({
        type: 'GET',
        url: RootUrl + "sales/Delivery/LoadDeliverableProduct",
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: { invoiceId: invoiceId },
        success: function (data) {
            $("#divDeliverableProducts").html(data);
        }
    });
}