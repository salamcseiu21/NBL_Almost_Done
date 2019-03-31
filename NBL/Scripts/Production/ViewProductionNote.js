
$.ajax({
    url:RootUrl+ 'factory/product/GetAllProductionNotes',
    type: "GET",
    datatype: "JSON",
    success: function (data) {
        $('#table_production_note_list').dataTable({
            data: data,
            "order": [[ 0, "desc" ]],
            columns: [
                { 'data': 'ProductionNoteNo' },
                { 'data': 'ProductionNoteRef' },
                { 'data': 'Product.ProductName' },
                {
                    'data': 'Quantity',
                    className:"text-right"
                },
                {
                    'data': 'ProductionNoteDate',
                    className: "text-center",
                    'render': function (jsonDate) {
                        var date = new Date(parseInt(jsonDate.substr(6)));
                        var month = date.getMonth() + 1;
                        return date.getDate() + "-" + month + "-" + date.getFullYear();
                    }
                }
            ]
        });
    }
});

