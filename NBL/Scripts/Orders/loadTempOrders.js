function ViewTempOrders() {
    $("#orders").html("");
    $.ajax({
        type: "GET",
        url: RootUrl+'sales/ordernsm/GetTempOrders',
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var total = 0;
            var tQty = 0;
            var discount = $("#Discount").val();
            for (var i = 0; i < data.length; i++) {
                total = total + data[i].SubTotal;
                tQty = tQty + data[i].Quantity;
            }
            $.each(data, function (key, value) {

                //total =+ value.SubTotal;
                //alert(key);
                //$("#orders").append('<option value=' + value.ClientId + '>' + value.ProductId + '</option>');
                var row = $("<tr><td style='border: 1px solid black; padding: 5px 10px;'>" + value.ProductName + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.UnitPrice + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.Vat + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.DiscountAmount + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'>" + value.SalePrice + "</td><td style='border: 1px solid black; padding: 5px 10px;'>  <input type='number' min='1' value='" + value.Quantity + "' class='form-control text-right' id='" + value.ProductId + "' name='NewQuantity_" + value.ProductId + "'  onchange='Update(this)'/>" + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-right'><input type='hidden' name='product_Id_" + value.ProductId + "' value='" + value.ProductId + "'> " + value.SubTotal + "</td><td style='border: 1px solid black; padding: 5px 10px;' class='text-center'><button id='" + value.ProductId + "' type='button' onclick='RemoveProduct(this)' class='btn btn-default btn-sm'><i class='fa fa-times' style='color:red;'></i></button>" + "</td></tr>");
                $("#orders").append(row);
            });
            $("#Amount").val(total);
            $("#TotalQuantity").val(tQty);
            $("#payment").attr("max", total);
            var payment = $("#payment").val();
            var net = total - discount;
            $("#Net").val(net);
        }
    });
}