
$.ajax({
    type: 'GET',
    url: "http://localhost/nbl/api/employees",
    dataType: 'jsonp',
    success: function (data) {
        $('#table_employee_list').dataTable({
            data: data,
            responsive: true,
            "order": [[ 0, "desc" ]],
            columns: [
                { 'data': 'EmployeeNo' },
                { 'data': 'EmployeeName' },
                { 'data': 'Department.DepartmentName' },
                { 'data': 'Designation.DesignationName' },
                {
                    'data': 'PresentAddress'
                    
                },
                {
                    'data': 'Phone'

                },
                {
                    'data': 'Email'

                },
                {
                    'data': 'JoiningDate',
                    'render': function (jsonDate) {
                        var date = jsonDate.substr(8,2);
                        var month = jsonDate.substr(5, 2);
                        var year = jsonDate.substr(0, 4);
                        return date + "-" + month + "-" + year;
                    },
                    className: "text-center"
                }
            ]
        });
    }
});

