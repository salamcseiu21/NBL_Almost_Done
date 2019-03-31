$(function () {
    $("#ClientName").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: RootUrl+'Common/ClientNameAutoComplete/',
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
            $("#ClientId").val(i.item.val);
            $("#orders").html("");
            productIdlist = [];
            $("#Total").val('');
            $("#Net").val('');
            $("#SpecialDiscount").val('');
            //alert(i.item.val);

            $("#Discount").val('');
            $("#Address").val('');
            $("#ClientPhone").val('');
            $("#ClientEmail").val('');
            $("#CreditLimit").val('');
            var cId = i.item.val;
            var json = { clientId: cId };

            $.ajax({
                type: "POST",
                url: RootUrl + 'sales/Order/GetClientById/',
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(json),
                success: function (data) {

                    var address = data.Address;
                    var phone = data.Phone;
                    var email = data.Email;
                    var discount = data.Discount;
                    var code = data.SubSubSubAccountCode;
                    var ctype = data.ClientTypeName;
                    var cl = data.CreditLimit;
                    var ct = data.ClientTypeId;
                    $("#Address").val(address);
                    $("#ClientPhone").val(phone);
                    $("#ClientEmail").val(email);
                    $("#Discount").val(discount);
                    $("#SubSubSubAccountCode").val(code);
                    $("#ClientTypeName").val(ctype);
                    $("#CreditLimit").val(cl);
                    $("#ClientTypeId").val(ct);

                }
            });
        },
        minLength: 1
    });
});