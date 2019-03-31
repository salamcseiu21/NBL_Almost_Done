
$.ajax({
    type:"GET",
    url: RootUrl + "sales/product/LoadAllReceiveableList",
    dataType: "json",
    contentType: 'application/json; charset=utf-8',
    success:function(data) {
        $("#table_receivable_list").dataTable({
            data: data,
            columns:
              [
                  { 'data': 'FromBranch.BranchName' },
                  { 'data': 'DeliveryRef' },
                  { 'data': 'Quantity'},
                  {
                      data: null,
                      className: "text-center",
                      render: function (data, type, row) {
                          return "<a href='/sales/product/ReceiveableDetails/"
                              + data.DeliveryId + "' class='btn btn-info' >Details</a>";
                      }
                  }

              ]
        });
    }
});


