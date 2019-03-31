$.ajax({
    type: 'GET',
    url: "http://localhost/nbl/api/departments",
    dataType: 'jsonp',
    success: function (data) {
        $('#table_Department_list').dataTable({
            data: data,
            columns: [
                { 'data': 'DepartmentCode' },
                { 'data': 'DepartmentName' },
                {
                    data: null,
                    className: "text-center",
                    render: function (data, type, row) {
                        return "<a href='/editor/department/Edit/"
                            + data.DepartmentId + "'><i class='fa fa-edit'></i> Edit</a>";
                    }
                }
            ]
        });
    }
});