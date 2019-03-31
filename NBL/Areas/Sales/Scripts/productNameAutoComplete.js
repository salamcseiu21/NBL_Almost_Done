
$(function () {
    $("#ProductName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl + 'common/ProductNameAutoComplete/',
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
            $("#ProductId").val(i.item.val);
            var json = { productId: i.item.val };
            $.ajax({
                type: "POST",
                url: RootUrl + 'Common/GetProductById/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {

                    var unitPrice = data.UnitPrice;
                    var dealerPrice = data.DealerPrice;
                    var vat = data.Vat;
                    var dealerComision = data.DealerComision;
                    $("#UnitPrice").val(unitPrice);
                    $("#DealerPrice").val(dealerPrice);
                    $("#Vat").val(vat);
                    $("#DealerComision").val(dealerComision);
                }
            });

            $.ajax({
                type: "POST",
                url: RootUrl + 'Common/GetProductQuantityInStockById/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {
                    var qty = data.StockQty;
                    $("#StockQty").val(qty);
                    //$("#Quantity").attr("max", qty);
                }
            });
        },
        minLength: 1
    });
});