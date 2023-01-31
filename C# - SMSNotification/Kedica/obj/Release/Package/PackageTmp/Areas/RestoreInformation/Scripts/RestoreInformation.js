"use strict";
    (function () {
        var RestoreInfo = $D();
        var tblRestore = "";
        $(document).ready(function () {
            drawDatatables();

            $("#btnRestoreInformation").click(function () {
                RestoreInfo.msg = "Are you sure you want to restore this User?";
                RestoreInfo.confirmAction().then(function (approve) {
                    if (approve)
                        RestoreInformation();
                });
            });

            tblRestore.on('select', function (e, dt, type, indexes) {
                $(".tbl-tr-btns").prop("disabled", false);
            });
            tblRestore.on('deselect', function (e, dt, type, indexes) {
                $(".tbl-tr-btns").prop("disabled", true);
            });
        });
        function drawDatatables() {
            if (!$.fn.DataTable.isDataTable('#tblRestoreInformation')) {
                tblRestore = $('#tblRestoreInformation').DataTable({
                    processing: true,
                    serverSide: true,
                    "order": [[0, "asc"]],
                    "pageLength": 25,
                    "ajax": {
                        "url": "/RestoreInformation/RestoreUserInformation/GetUserList",
                        "type": "POST",
                        "datatype": "json",
                    },
                    dataSrc: "data",
                    select: {
                        style: 'multi'
                    },
                    columns: [
                        {
                            title: "Name", data: 'Email', render: function (data, type, row, meta) {
                                return row.FirstName + " " + row.MiddleName + " " + row.LastName
                            }
                        },
                        { title: "Email Address", data: "Email" },
                        { title: "Contact Number", data: "ContactNumber" },
                        { title: "Contact Number", data: "ContactNumber" },
                        {
                            title: "Role", data: "Role", render: function (data) {
                                if (data === "Administrator") {
                                    return "Administrator";
                                } else if (data === "HeadAdministrator") {
                                    return "Head Administrator";
                                }
                                else if (data === "Faculty") {
                                    return "Faculty Staff";
                                }
                                else if (data === "NonFaculty") {
                                    return "Non Faculty Staff";
                                }
                                else if (data === "Student") {
                                    return "Student";
                                }
                            }
                        },
                        {
                            title: "Status", data: "IsActive", render: function (data) {
                                if (data === "True") {
                                    return "<span class='badge bg-success'>Active</span>"
                                } else {
                                    return "<span class='badge bg-danger'>Pending</span>"
                                }
                            }
                        },
                    ],
                    "createdRow": function (row, data, dataIndex) {
                        $(row).attr('data-id', data.ID);
                        $(row).attr('data-username', data.Username);
                    },

                })
            }
        }
        function RestoreInformation() {
            var checkall = tblRestore.rows({ selected: true }).data();
            var tempArray = [];
            for (var i = 0; i < checkall.length; i++) {
                tempArray.push({ "UserID": checkall[i].ID });
            }
            RestoreInfo.formAction = '/RestoreInformation/RestoreUserInformation/RestoreUser';
            RestoreInfo.jsonData = { UserIDList: tempArray };
            RestoreInfo.sendData().then(function () {
                tblRestore.ajax.reload(null, false);
                cancelRestoreInfoTbl();
            });
        }
        function cancelRestoreInfoTbl() {
            $('#btnRestoreInformation').attr("disabled", "disabled");
        }
    })();