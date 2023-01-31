"use strict";
(function () {
    const UserLog = function () {
        return new UserLog.init();
    }
    UserLog.init = function () {
        $D.init.call(this);
        this.$tblUserlogList = "";
    }
    UserLog.prototype = {
        drawDataTable: function () {
            var self = this;
            if (!$.fn.DataTable.isDataTable('#tblUserLogList')) {
                self.$tblUserlogList = $('#tblUserLogList').DataTable({
                    processing: true,
                    serverSide: true,
                    "order": [[0, "desc"]],
                    "pageLength": 25,
                    "ajax": {
                        "url": "/AuditTrail/UserLogs/GetUserList",
                        "type": "POST",
                        "datatype": "json",
                    },
                    dataSrc: "data",
                    scrollY: '100%', scrollX: '100%',
                    select: true,
                    columns: [
                        { title: "ID", data: "ID", visible: false },
                        {
                            title: "Full Name", data: "FirstName", render: function (data, type, row, meta) {
                                return row.FirstName + " " + row.MiddleName + " " + row.LastName
                            }
                        },
                        { title: "Update By", data: "UpdateID" },
                        { title: "Email Name", data: "Email" },
                        { title: "Activity", data: "InTime" },
                        { title: "Date", data: "InDate" },
                    ],
                    "createdRow": function (row, data, dataIndex) {
                        $(row).attr('data-id', data.ID);
                    },

                })
            }
        },
        viewDetails: function () {
            var self = this;
            var ID = self.$tblUserlogList.rows({ selected: true }).data()[0].ID;
            self.formAction = '/AuditTrail/UserLogs/GetUserData';
            self.jsonData = { ID: ID };
            self.sendData().then(function () {
                var data = self.responseData.data;
                $("#FirstName").val(data.FirstName);
                $("#LastName").val(data.LastName);
                $("#MiddleName").val(data.MiddleName);
                $("#InDate").val(data.InDate);
                $("#InTime").val(data.InTime);
                $("#OutDate").val(data.OutDate);
                $("#OutTime").val(data.OutTime);
                $("#mdlViewDetails").modal("show");
            });

        }

    }
    UserLog.init.prototype = $.extend(UserLog.prototype, $D.init.prototype);
    UserLog.init.prototype = UserLog.prototype;
    var AuditTrail = UserLog();
    var CUI2 = $UI();
    $(document).ready(function () {

        AuditTrail.drawDataTable();

        $("#btnViewDetails").click(function () {
            AuditTrail.viewDetails();
        });
        $('#mdlViewDetails').on('hidden.bs.modal', function () {
            $(".input").val("");
        })
        AuditTrail.$tblUserlogList.on('draw.dt', function () {
            CUI2.dataTableID = "#tblUserLogList";
            CUI2.setDatatableMaxHeight();

        });

    });

})();



