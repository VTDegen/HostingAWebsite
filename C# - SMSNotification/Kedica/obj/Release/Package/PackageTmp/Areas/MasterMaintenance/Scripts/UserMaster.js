"use strict";
(function () {
    var User = $D();
    var tblUser = "";
    var Username = "";
    var populateMode = false;
    var costCenterSelectElementCounter = 1;
    $(document).ready(function () {
        drawDatatables();
        initSelect2ForFilters();
        $("#btnAddUser").click(function () {
            populateMode = false;
            $("#mdlUser").modal("show");
        });
        $("#Username").change(function () {
            validateUsername(this);
        });
        $("#Email").change(function () {
            valiteEmail(this);
        });
        $('#btnCancelUser,#btnCloseUpperUser').click(function () {
            cancelUserForm();
            CUI.setDatatableMaxHeight();
        });
        $("#btnGeneratePassword").click(function () {
            $("#Password").val(generatePassword());
            User.parsleyValidate("frmUser");
        });
        $("#frmUser").submit(function (e) {
            e.preventDefault();
            var email = $("#Email").val(); 
            if (email.substr(email.length - 11) == "@ccc.edu.ph") {
                $("#Email").addClass("parsley-success");
                $("#Email").removeClass("parsley-error");
                saveUser();
            }
            else {
                $("#Email").removeClass("parsley-success");
                $("#Email").addClass("parsley-error");
                User.showError("Yuor Email ending must be: <b> @ccc.edu.ph </b>");
            }
    
        });

        //$('#tblUser tbody').on('click', 'tr', function () {
        //    dis_se_lectUserRow($(this));
        //});
        tblUser.on('select', function (e, dt, type, indexes) {
            $(".tbl-tr-btns").prop("disabled", false);
        });
        tblUser.on('deselect', function (e, dt, type, indexes) {
            $(".tbl-tr-btns").prop("disabled", true);
        });
        $('#btnEditUser').click(function () {
            editUser();
        });
        $('#btnDeleteUser').click(function () {
            User.msg = "Are you sure you want to delete this User?";
            User.confirmAction().then(function (approve) {
                if (approve)
                    deleteUser();
            });
        });
        $('#btnUserAccess').click(function () {
            getUserAccess();
        });
        $("#mdlBodyUserAccess").on("click", ".parentMenu", function () {
            var childClass = $(this).data("childclass");
            $("." + childClass).prop("checked", $(this).is(":checked"));
        });
        $("#mdlBodyUserAccess").on("click", ".subMenu", function () {
            var parentID = $(this).data("parent");
            var myClass = $(this).data("myclass");
            $("#" + parentID).prop("checked", $("." + myClass + ":checked").length);
        });
        $("#btnSaveUserAccess").click(function () {
            saveUserAccess();
        });
        $("#tblUser").on("change", '.columnSearch', function () {
            tblUser.ajax.reload(null, false);
        });
        $('#btnPrint').on('click', function (e) {
            tblUser.ajax.reload(null, false);
            window.location = "/MasterMaintenance/UserMaster/DownloadUser";
        });
        $("#btnSearchList").click(function () {
            tblUser.ajax.reload(null, false);
        });
        $("#btnCancelSearch").click(function () {
            $(".isFilter").val("");
            var GenderOption = new Option("", "", true, true);
            var CivilStatusOption = new Option("", "", true, true);
            $('#GenderData').append(GenderOption);
            $('#CivilStatusData').append(CivilStatusOption);
            tblUser.ajax.reload(null, false);
            cancelUserTbl();
            User.clearFromData("frmSearchFilters");
            $(".fold").attr("hidden", true);
            setInterval(tblUser.ajax.reload(null, false), 1000);
        });
        $("#ContactNumber").on('change', function () {
            var self = this;
            var getValue = $(self).val().trim();
            var getValidation = getValue.substring(0, 3);
            if (getValue.length === 13) {
                if (getValidation === "+63") {

                } else {
                    User.showError("Invalid Format, Please try again");
                    $(self).val("");
                    $(self).focus();
                }
            } else {
                User.showError("Invalid Input, Please try again");
                $(self).val("");
                $(self).focus();
            }
        });
        $("#Role").on('change', function () {
            var datavalue = $(this).val();
            if (datavalue === "Student") {
                $("#StudentOtherInformation").prop("hidden", false);
                $("#FacultyOtherInformation").prop("hidden", true);
                $("#NonFacultyOtherInformation").prop("hidden", true);
            } else if (datavalue === "Faculty") {
                $("#FacultyOtherInformation").prop("hidden", false);
                $("#StudentOtherInformation").prop("hidden", true);
                $("#NonFacultyOtherInformation").prop("hidden", true);
            } else if (datavalue === "NonFaculty") {
                $("#NonFacultyOtherInformation").prop("hidden", false);
                $("#FacultyOtherInformation").prop("hidden", true);
                $("#StudentOtherInformation").prop("hidden", true);
            } else {
                $("#StudentOtherInformation").prop("hidden", true);
                $("#FacultyOtherInformation").prop("hidden", true);
                $("#NonFacultyOtherInformation").prop("hidden", true);
            }
        });
        $("#SrchRole").on('change', function () {
            var datavalue = $(this).val();
            if (datavalue === "Student") {
                $("#SrchStudentOtherInformation").prop("hidden", false);
                $("#SrchFacultyOtherInformation").prop("hidden", true);
                $("#SrchNonFacultyOtherInformation").prop("hidden", true);
            } else if (datavalue === "Faculty") {
                $("#SrchFacultyOtherInformation").prop("hidden", false);
                $("#SrchStudentOtherInformation").prop("hidden", true);
                $("#SrchNonFacultyOtherInformation").prop("hidden", true);
            } else if (datavalue === "NonFaculty") {
                $("#SrchNonFacultyOtherInformation").prop("hidden", false);
                $("#SrchFacultyOtherInformation").prop("hidden", true);
                $("#SrchStudentOtherInformation").prop("hidden", true);
            } else {
                $("#SrchStudentOtherInformation").prop("hidden", true);
                $("#SrchFacultyOtherInformation").prop("hidden", true);
                $("#SrchNonFacultyOtherInformation").prop("hidden", true);
            }
        });
        $("#tblUser").on('click', '.btnShowOtherInfoDetails', function () {
            var SelectedID = $(this).data('id');
            var FirstName = $(this).data('firstname');
            var LastName = $(this).data('lastname');
            var Type = $(this).data('type');
            getOtherInformationDetails(SelectedID, FirstName, LastName, Type);
        });
        $("#btnCancelShowOtherInfo,#btnCloseUpperShowOtherInfo").click(function () {
            cancelUserForm();
        });
        $("#btnUserActivate").click(function () {
            UpdateStatus();
        });
        $("#btnUserActivateAll").click(function () {
            ActivateAll();
        });
        $("#Organization").on('change', function () {
            var DataValue = parseInt($(this).val())
            if (DataValue === 1) {
                $("#StudentOrganization").prop("hidden", false);
            } else {
                $("#StudentOrganization").prop("hidden", true);
                $(".orgClass").prop('checked', false);
            }
        });
        $("#SrchOrganization").on('change', function () {
            var DataValue = parseInt($(this).val())
            if (DataValue === 1) {
                $("#SrchStudentOrganization").prop("hidden", false);
            } else {
                $("#SrchStudentOrganization").prop("hidden", true);
                $(".SrchorgClass").prop('checked', false);
            }
        });
        var CivilStatusquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 2) AND IsDeleted = 0 ";
        var Genderquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1) AND IsDeleted = 0 ";
        $('#CivilStatusData').select2({
            ajax: {
                url: "/General/GetSelect2Data",
                data: function (params) {
                    var d = {
                        q: params.term,
                        id: 'ID',
                        text: 'Value',
                        table: 'mGeneral',
                        db: 'SMSConfig',
                        display: 'id&text',
                        query: CivilStatusquery
                    }
                    return d;
                },
            },
            placeholder: '--Please Select--',
            width: "87%",
            allowClear: true,
        });
        $('#GenderData').select2({
            ajax: {
                url: "/General/GetSelect2Data",
                data: function (params) {
                    var d = {
                        q: params.term,
                        id: 'ID',
                        text: 'Value',
                        table: 'mGeneral',
                        db: 'SMSConfig',
                        display: 'id&text',
                        query: Genderquery
                    }
                    return d;
                },
            },
            placeholder: '--Please Select--',
            width: "87%",
            allowClear: true,
        });
        //#Special Events
        tblUser.on('draw.dt', function () {
            CUI.dataTableID = "#tblUser";
            CUI.setDatatableMaxHeight();
        });
        $.listen('parsley:field:error', function (fieldInstance) {//Parsley Validation Error Event Listener
            setTimeout(function () { CUI.setDatatableMaxHeight(); }, 500);
        });
        $('#mdlUser').on('shown.bs.modal', function (e) {
            populateMode = false;
            var Genderquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1) AND IsDeleted = 0 ";
            var CivilStatusquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 2) AND IsDeleted = 0 ";
            var StudentDepartquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 3) AND IsDeleted = 0 ";
            var StudentCoursequery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 4) AND IsDeleted = 0 ";
            var StudentSectionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1002) AND IsDeleted = 0 ";
            var StudentYearLevelquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 5) AND IsDeleted = 0 ";
            var StudentScholarquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 6) AND IsDeleted = 0 ";
            var StudentSocialStatquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1003) AND IsDeleted = 0 ";
            var FacultyDepartmentquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 8) AND IsDeleted = 0 ";
            var FacultyPositionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 7) AND IsDeleted = 0 ";
            var NonFacultyPositionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 9) AND IsDeleted = 0 ";
            $('#Gender').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: Genderquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "51%",
                allowClear: true,
            });
            $('#CivilStatus').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: CivilStatusquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#StuDepartment').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentDepartquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#StuCourse').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentCoursequery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            }); 
            $('#StuSection').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentSectionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#StuYearLevel').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentYearLevelquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#ScholarShip').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentScholarquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SocialStatus').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentSocialStatquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#FacDepartment').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: FacultyDepartmentquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#FacPosition').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: FacultyPositionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#NonFacPosition').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: NonFacultyPositionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "83%",
                allowClear: true,
            });
        });
        //#Functions Here-------------------------------------------------------------------------------------------------------------------
        function drawDatatables() {
            if (!$.fn.DataTable.isDataTable('#tblUser')) {
                tblUser = $('#tblUser').DataTable({
                    processing: true,
                    serverSide: true,
                    "order": [[0, "asc"]],
                    "pageLength": 25,
                    "ajax": {
                        "url": "/MasterMaintenance/UserMaster/GetUserList",
                        "type": "POST",
                        "data": function (d) {
                            d.SrchUsername = $("#SrchUsername").val() || "";
                            d.SrchFirstName = $("#SrchFirstName").val() || "";
                            d.SrchLastName = $("#SrchLastName").val() || "";
                            d.SrchMiddleName = $("#SrchMiddleName").val() || "";
                            d.SrchContactNumber = $("#SrchContactNumber").val() || "";
                            d.SrchEmail = $("#SrchEmail").val() || "";
                            d.SrchGender = $("#SrchGender").val() || "";
                            d.SrchCivilStatus = $("#SrchCivilStatus").val() || "";
                            d.SrchRole = $("#SrchRole").val() || "";
                            d.SrchStuDepartment = $("#SrchStuDepartment").val() || "";
                            d.SrchStuCourse = $("#SrchStuCourse").val() || "";
                            d.SrchStuSection = $("#SrchStuSection").val() || "";
                            d.SrchStuYearLevel = $("#SrchStuYearLevel").val() || "";
                            d.SrchTES = $("#SrchTES").is(":checked") ? "1" : "0"|| "0";
                            d.SrchTDP = $("#SrchTDP").is(":checked") ? "1" : "0"|| "0";
                            d.SrchPersonDisability = $("#SrchPersonDisability").is(":checked") ? "1" : "0"|| "0";
                            d.SrchWorkingStudent = $("#SrchWorkingStudent").is(":checked") ? "1" : "0"|| "0";
                           // d.SrchNotApplicable = $("#SrchNotApplicable").is(":checked") ? "1" : "0"|| "0";
                            d.SrchOrganization = $("#SrchOrganization").val() || "";
                            d.SrchALLS = $("#SrchALLS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchAPL = $("#SrchAPL").is(":checked") ? "1" : "0"|| "0";
                            d.SrchASGRD = $("#SrchASGRD").is(":checked") ? "1" : "0"|| "0";
                            d.SrchCAITO = $("#SrchCAITO").is(":checked") ? "1" : "0"|| "0";
                            d.SrchCAPS = $("#SrchCAPS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchCSS = $("#SrchCSS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchDCP = $("#SrchDCP").is(":checked") ? "1" : "0"|| "0";
                            d.SrchENC = $("#SrchENC").is(":checked") ? "1" : "0"|| "0";
                            d.SrchEXCEL = $("#SrchEXCEL").is(":checked") ? "1" : "0"|| "0";
                            d.SrchFELTA = $("#SrchFELTA").is(":checked") ? "1" : "0"|| "0";
                            d.SrchIFIGHT = $("#SrchIFIGHT").is(":checked") ? "1" : "0"|| "0";
                            d.SrchITS = $("#SrchITS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchJPIA = $("#SrchJPIA").is(":checked") ? "1" : "0"|| "0";
                            d.SrchLAGABATA = $("#SrchLAGABATA").is(":checked") ? "1" : "0"|| "0";
                            d.SrchLEAD = $("#SrchLEAD").is(":checked") ? "1" : "0"|| "0";
                            d.SrchMDAS = $("#SrchMDAS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchMSS = $("#SrchMSS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchROTARACT = $("#SrchROTARACT").is(":checked") ? "1" : "0"|| "0";
                            d.SrchSAVE = $("#SrchSAVE").is(":checked") ? "1" : "0"|| "0";
                            d.SrchSFJ = $("#SrchSFJ").is(":checked") ? "1" : "0"|| "0";
                            d.SrchSOCIOS = $("#SrchSOCIOS").is(":checked") ? "1" : "0"|| "0";
                            d.SrchPROTEGE = $("#SrchPROTEGE").is(":checked") ? "1" : "0"|| "0";
                            d.SrchUM3P = $("#SrchUM3P").is(":checked") ? "1" : "0"|| "0";
                            d.SrchFacDepartment = $("#SrchFacDepartment").val() || "";
                            d.SrchFacPosition = $("#SrchFacPosition").val() || "";
                            //d.UserNameData = $("#UsernameData").val().trim();
                            //d.RoleData = $("#RoleData").val().trim();
                            //d.GenderData =  $("#GenderData").val();
                            //d.CivilStatusData =$("#CivilStatusData").val();
                            //d.StatusData =  $("#StatusData").val();
                        },
                        "datatype": "json",
                    },
                    dataSrc: "data",
                    select: {
                        style: 'multi'
                    },
                    columns: [
                        //{ title: "Username", data: "Username" },
                        {
                            title: "Name", data: 'Email', render: function (data, type, row, meta) {
                                return row.FirstName + " " + row.MiddleName + " " + row.LastName
                            }
                        },
                        { title: "Email Address", data: "Email" },
                        { title: "Gender", data: "GenderData" },
                        { title: "Contact Number", data: "ContactNumber" },
                        { title: "Civil Status", data: "CivilStatusData" },
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
                            title: "Options", data: "ID", render: function (data, type, row, meta) {
                                if (row.Role == "Student") {
                                    return '<button type="button" data-type="student" data-id="'+row.ID+'" data-firstname="' + row.FirstName + '" data-lastname="' + row.LastName +'" id="btnStudentInformation' + row.ID +'" class="btn btn-sm btn-success  btnShowOtherInfoDetails"><span class="fa fa-list"></span><span class="btnLabel"> Student Information</span></button>'
                                }
                                else if (row.Role == "Faculty") {
                                    return '<button type="button"  data-type="faculty" data-id="' + row.ID + '" data-firstname="' + row.FirstName + '" data-lastname="' + row.LastName + '" id="btnFacultyInformation' + row.ID +'" class="btn btn-sm btn-success  btnShowOtherInfoDetails"><span class="fa fa-list"></span><span class="btnLabel"> Faculty Information</span></button>'
                                }
                                else if (row.Role == "NonFaculty") {
                                    return '<button type="button"  data-type="nonfaculty" data-id="' + row.ID + '" data-firstname="' + row.FirstName + '" data-lastname="' + row.LastName + '" id="btnNonFacultyInformation' + row.ID + '" class="btn btn-sm btn-success  btnShowOtherInfoDetails"><span class="fa fa-list"></span><span class="btnLabel"> Non-Faculty Information</span></button>'
                                }
                                else {
                                    return '';
                                }
                                
                            }
                        },
                        {
                            title: "Status", data: "IsActive" , render: function (data) {
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
        function initSelect2ForFilters() {

            var Genderquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1) AND IsDeleted = 0 ";
            var CivilStatusquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 2) AND IsDeleted = 0 ";
            var StudentDepartquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 3) AND IsDeleted = 0 ";
            var StudentCoursequery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 4) AND IsDeleted = 0 ";
            var StudentSectionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1002) AND IsDeleted = 0 ";
            var StudentYearLevelquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 5) AND IsDeleted = 0 ";
            var StudentScholarquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 6) AND IsDeleted = 0 ";
            var StudentSocialStatquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1003) AND IsDeleted = 0 ";
            var FacultyDepartmentquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 8) AND IsDeleted = 0 ";
            var FacultyPositionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 7) AND IsDeleted = 0 ";
            var NonFacultyPositionquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 9) AND IsDeleted = 0 ";
            $('#SrchGender').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: Genderquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "58%",
                allowClear: true,
            });
            $('#SrchCivilStatus').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: CivilStatusquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "70%",
                allowClear: true,
            });
            $('#SrchStuDepartment').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentDepartquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchStuCourse').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentCoursequery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchStuSection').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentSectionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchStuYearLevel').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentYearLevelquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchScholarShip').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentScholarquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchSocialStatus').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: StudentSocialStatquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchFacDepartment').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: FacultyDepartmentquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchFacPosition').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: FacultyPositionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "65%",
                allowClear: true,
            });
            $('#SrchNonFacPosition').select2({
                ajax: {
                    url: "/General/GetSelect2Data",
                    data: function (params) {
                        var d = {
                            q: params.term,
                            id: 'ID',
                            text: 'Value',
                            table: 'mGeneral',
                            db: 'SMSConfig',
                            display: 'id&text',
                            query: NonFacultyPositionquery
                        }
                        return d;
                    },
                },
                placeholder: '--Please Select--',
                width: "83%",
                allowClear: true,
            });
        }
        function valiteEmail(self) {
            var emailData = $(self).val().trim();
            if (emailData) {
                User.formAction = '/MasterMaintenance/UserMaster/ValidateEmailAddress';
                User.jsonData = { EmailAddress: emailData }
                User.sendData().then(function () {
                    var validUsername = User.responseData.isValid;
                    if (!validUsername) {
                        User.showError("Email Address already exists. Please try another Email");
                        $(self).val("");
                        $(self).focus();
                    } else {
                        $("#Email").focus();
                    }
                });
            }
        }
        function validateUsername(self) {
            var username = $(self).val().trim();
            if (username) {
                User.formAction = '/MasterMaintenance/UserMaster/ValidateUsername';
                User.jsonData = { Username: username }
                User.sendData().then(function () {
                    var validUsername = User.responseData.isValid;
                    if (!validUsername) {
                        User.showError("Username already exists. Please try another Username");
                        $(self).val("");
                        $(self).focus();
                    } else {
                        $("#Password").focus();
                    }
                });
            }
        }
        function generatePassword() {
            var length = 8,
                charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                retVal = "";
            for (var i = 0, n = charset.length; i < length; ++i) {
                retVal += charset.charAt(Math.floor(Math.random() * n));
            }
            return retVal;
        }
        function saveUser() {
            User.formData = $('#frmUser').serializeArray();
            User.formAction = '/MasterMaintenance/UserMaster/SaveUser';
            User.setJsonData();
            User.sendData().then(function () {
                tblUser.ajax.reload(null, false);
                cancelUserTbl();
                cancelUserForm();
            });
        }
        function ActivateAll() {
            User.formAction = '/MasterMaintenance/UserMaster/ActivateAllUsers';
            User.sendData().then(function () {
                tblUser.ajax.reload(null, false);
                cancelUserTbl();
                cancelUserForm();
            });
        }
        function UpdateStatus() {
            var Username = tblUser.rows({ selected: true }).data()[0].Email;
            User.formAction = '/MasterMaintenance/UserMaster/UpdateStatus';
            if (User.formAction) {
                User.jsonData = { Username: Username };
                User.sendData().then(function () {
                    tblUser.ajax.reload(null, false);
                    cancelUserTbl();
                    cancelUserForm();
                });
            } else {
                User.showError("Please try again.");
            }
        }
        function cancelUserForm() {
            populateMode = false;
            User.clearFromData("frmUser");
            $('#Username').prop('readonly', false);
            $("#mdlUserTitle").text(" Create User");
            $("#btnSaveUser .btnLabel").text(" Save");
            $("#Role option[value='Custom']").remove();
            $("#mdlUser").modal("hide");
            costCenterSelectElementCounter = 1;
            $("#StudentOtherInformation").prop("hidden", true);
            $("#FacultyOtherInformation").prop("hidden", true);
            $("#NonFacultyOtherInformation").prop("hidden", true);
            $("#StudentOrganization").prop("hidden", true);
            $(".chckStudentData").prop('checked', false)
        }
        function cancelUserTbl() {
            $('#btnEditUser').attr("disabled", "disabled");
            $('#btnDeleteUser').attr("disabled", "disabled");
            $('#btnUserAccess').attr("disabled", "disabled");
            $('#btnUserActivate').attr("disabled", "disabled");
        }
        function dis_se_lectUserRow(UserRow) {
            if (UserRow.data("username")) {
                if (UserRow.hasClass('selected')) {
                    UserRow.removeClass('selected');
                    Username = "";
                    ID = 0;
                    $('#btnEditUser').attr("disabled", "disabled");
                    $('#btnDeleteUser').attr("disabled", "disabled");
                    $('#btnUserAccess').attr("disabled", "disabled");
                }
                else {
                    tblUser.$('tr.selected').removeClass('selected');
                    UserRow.addClass('selected');
                    Username = UserRow.data("username");
                    ID = UserRow.data("id");
                    $('#btnEditUser').removeAttr("disabled");
                    $('#btnDeleteUser').removeAttr("disabled");
                    $('#btnUserAccess').removeAttr("disabled");
                }
            }
        }
        function editUser() {
            populateMode = true;
            var Username = tblUser.rows({ selected: true }).data()[0].Email;
            User.formAction = '/MasterMaintenance/UserMaster/GetUserDetails';
            if (User.formAction) {
                User.jsonData = { Username: Username };
                User.sendData().then(function () {
                    populateUserData(
                        User.responseData.userData[0],
                        User.responseData.userStudentDetails[0],
                        User.responseData.userStudentSocialStatusDetails[0],
                        User.responseData.userStudentScholarShipDetails[0],
                        User.responseData.userStudentOrganizeDetails[0],
                        User.responseData.userFacultyDetails[0],
                        User.responseData.userNonFacultyDetails[0],
                    );
                });
            } else {
                User.showError("Please try again.");
            }
        }
        function populateUserData(user,student,socialstatus,scholarship,organize,faculty,nonfaculty) {
            $('#Username').prop('readonly', true);
            $("#frmUser").parsley().reset();
            $("#mdlUserTitle").text(" Update User");
            $("#btnSaveUser .btnLabel").text(" Update");
            User.populateToFormInputs(user, "#frmUser");
            var GenderOption = new Option(user.GenderData, user.Gender, true, true);
            var CivilStatusOption = new Option(user.CivilStatusData, user.CivilStatus, true, true);
            $('#Gender').append(GenderOption);
            $('#CivilStatus').append(CivilStatusOption);
            if (user.Role === "Student") {
                var DepartmentOption = new Option(student.StuDepartmentData, student.StuDepartment, true, true);
                var CourseOption = new Option(student.StuCourseData, student.StuCourse, true, true);
                var SectionOption = new Option(student.StuSectionData, student.StuSection, true, true);
                var YearLevelOption = new Option(student.StuYearLevelData, student.StuYearLevel, true, true);
                $('#StuDepartment').append(DepartmentOption);
                $('#StuCourse').append(CourseOption);
                $('#StuSection').append(SectionOption);
                $('#StuYearLevel').append(YearLevelOption);
                SetCheckBox(socialstatus.PersonDisability, "PersonDisability");
               // SetCheckBox(socialstatus.NotApplicable, "NotApplicable");
                SetCheckBox(socialstatus.WorkingStudent, "WorkingStudent");
                SetCheckBox(scholarship.TES, "TES");
                SetCheckBox(scholarship.TDP, "TDP");
                if (student.Organization === "True") {
                    $("#Organization").val(1);
                    $("#StudentOrganization").prop("hidden", false);
                } else {
                    $("#Organization").val(0);
                    $("#StudentOrganization").prop("hidden", true);
                }
                SetOrganizationList(organize);
                ShowDetails("Student");
            } else if (user.Role === "Faculty") {
                var DepartmentOption = new Option(faculty.FacDepartmentData, faculty.FacDepartment, true, true);
                var PositionOption = new Option(faculty.FacPositionData, faculty.FacPosition, true, true);
                $('#FacDepartment').append(DepartmentOption);
                $('#FacPosition').append(PositionOption);
                ShowDetails("Faculty");
            } else if (user.Role === "NonFaculty") {
                var PositionOption = new Option(nonfaculty.NonFacPositionData, nonfaculty.NonFacPosition, true, true);
                $('#NonFacPosition').append(PositionOption);
                ShowDetails("NonFaculty");
            }
            $("#mdlUser").modal("show");
        }
        function deleteUser() {
            var checkIDList = tblUser.rows({ selected: true }).data();
            var tempArray = [];
            for (var i = 0; i < checkIDList.length; i++) {
                tempArray.push({ "UserID": checkIDList[i].ID })
            }
            var Username = tblUser.rows({ selected: true }).data()[0].Username;
            User.formAction = '/MasterMaintenance/UserMaster/DeleteUser';
            User.jsonData = { UserListID: tempArray };
            User.sendData().then(function () {
                tblUser.ajax.reload(null, false);
                cancelUserTbl();
                cancelUserForm();
            });
        }
        function getUserAccess() {
            var ID = tblUser.rows({ selected: true }).data()[0].ID;
            User.formAction = '/MasterMaintenance/UserMaster/GetUserAccess';
            User.jsonData = { ID: ID };
            User.sendData().then(function () {
                drawUserMenu(User.responseData);
            });
        }
        function drawUserMenu(menu) {
            const tmpForParentMenu = menu;
            const tmpForSubMenu = menu;
            //Grouping menus into GroupLabel
            var arrGroupedData = _.mapValues(_.groupBy(menu, 'GroupLabel'), (function (clist) {
                return clist.map(function (byGroupLabel) {
                    return _.omit(byGroupLabel, 'GroupLabel');
                });
            }));
            //End Grouping menus into GroupLabel 
            //Creating GroupLabel Tab Link
            var menuTab = "<ul class='nav nav-tabs'>";
            $.each(arrGroupedData, function (i) {
                var GroupLabelID = i == "" ? "Common" : i;
                var active = i == "" ? "active" : "";
                menuTab += "<li class='nav-items'>\
                                    <a href='#tab" + GroupLabelID + "' data-toggle='tab' class='nav-link " + active + "'>\
                                        <span class='d-sm-none'>" + GroupLabelID + "</span>\
                                        <span class='d-sm-block d-none'> " + GroupLabelID + "</span>\
                                    </a>\
                                 </li>";
            });
            menuTab += "</ul>";
            //End Creating GroupLabel Tab Link

            //Creating GroupLabel Tab Content
            menuTab += "<div class='tab-content'>";
            $.each(arrGroupedData, function (i, groupedMainMenu) {
                var GroupLabelID = i == "" ? "Common" : i;
                var active = i == "" ? "active" : "";
                //Creating GroupLabel Individual Tab Content
                menuTab += "<div class='tab-pane show " + active + "' id='tab" + GroupLabelID + "'>";
                //Creating Parent Menu Tab Link
                menuTab += "    <ul class='nav nav-tabs'>";
                var parentMenu = _.filter(groupedMainMenu, function (obj) { return obj.Icon != 0; });
                var sortedParentMenu = _.sortBy(parentMenu, function (o) { return new Number(o.ParentOrder); }, ['asc']);
                $.each(sortedParentMenu, function (ii) {
                    var active1 = (ii == 0 && GroupLabelID == "Common") || (ii == 0 && GroupLabelID != "Common") ? 'active' : '';
                    if (this.URL != "/") {
                        menuTab += "<li class='nav-items'>\
                                    <a href='#tab" + (this.PageName).replace(/\s/g, "") + "' data-toggle='tab' class='nav-link " + active1 + "'>\
                                        <span class='d-sm-none'><span class='" + this.Icon + "'></span> " + this.PageLabel + "</span>\
                                        <span class='d-sm-block d-none'><span class='" + this.Icon + "'></span> " + this.PageLabel + "</span>\
                                    </a>\
                                 </li>";
                    }
                });
                menuTab += "    </ul>";
                //End Creating Parent Menu Tab Link
                //Creating Parent Menu Tab Content
                menuTab += "<div class='tab-content'>";
                $.each(sortedParentMenu, function (iii, m) {
                    if (this.URL != "/") {
                        var PageName = this.PageName;
                        var PageLabel = this.PageLabel;
                        var submenus = _.filter(groupedMainMenu, function (obj) {
                            return obj.ParentMenu == PageName && obj.Icon == 0;
                        });
                        var sortedSubmenu = _.sortBy(submenus, function (o) { return new Number(o.Order); }, ['asc']);

                        //var show = i == 1 ? 'fade active' : '';
                        var show = (iii == 0 && GroupLabelID == "Common") || (iii == 0 && GroupLabelID != "Common") ? 'fade active' : '';
                        var checked = this.Status != '' ? 'checked' : '';

                        var readOnly = this.ReadAndWrite ? '' : 'checked';
                        var readAndWrite = this.ReadAndWrite ? 'checked' : '';
                        var delChecked = this.Delete ? 'checked' : '';
                        //Creating indivdial menu list content
                        menuTab += "<div class='tab-pane show " + show + "' id='tab" + (PageName).replace(/\s/g, '') + "'>";
                        menuTab += "    <ul>";
                        menuTab += "        <li>";
                        menuTab += "            <div class='checkbox checkbox-css checkbox-inline'>\
                                                    <input type='checkbox'  id='chk" + PageName.replace(/\s/g, '') + "' value='" + this.ID + "' " + checked + " data-pagename='" + PageName.replace(/\s/g, '') + "'  data-hassub='" + this.HasSub + "'  data-readandwrite='" + this.ReadAndWrite + "' data-delete='" + this.Delete + "'   data-childclass='" + PageName.replace(/\s/g, '') + "' class='parentMenu chkUserAccess clickable'>\
                                                    <label class='clickable' for='chk" + PageName.replace(/\s/g, '') + "'>" + PageLabel + "</label>\
                                                </div>";
                        if (!this.HasSub) {
                            menuTab += "            <div class='radio radio-css radio-inline m-l-20'>\
                                                        <input class='form-check-input clickable' type='radio' name='rdoReadAndWrite" + PageName.replace(/\s/g, '') + "' id='rdoRead" + PageName.replace(/\s/g, '') + "' value='0' " + readOnly + ">\
                                                        <label class='form-check-label clickable' for='rdoRead" + PageName.replace(/\s/g, '') + "'>Read Only</label>\
                                                    </div>\
                                                    <div class='radio radio-css radio-inline '>\
                                                        <input class='form-check-input clickable' type='radio' name='rdoReadAndWrite" + PageName.replace(/\s/g, '') + "' id='rdoReadWrite" + PageName.replace(/\s/g, '') + "' value='1' " + readAndWrite + ">\
                                                        <label class='form-check-label clickable' for='rdoReadWrite" + PageName.replace(/\s/g, '') + "'>Read & Write</label>\
                                                    </div>";
                            menuTab += "            <div class='checkbox checkbox-css checkbox-inline m-l-20'>\
                                                        <input type='checkbox'  id='chkDel" + PageName.replace(/\s/g, "") + "' value='" + this.ID + "' " + delChecked + " data-pagename='" + PageName.replace(/\s/g, "") + "' class='clickable'>\
                                                        <label class='clickable' for='chkDel" + PageName.replace(/\s/g, "") + "'>Delete</label>\
                                                    </div>";
                        }
                        menuTab += "        </li>";

                        if (sortedSubmenu.length) {
                            if (PageName == sortedSubmenu[0].ParentMenu) {
                                menuTab += "        <li>";
                                menuTab += "            <ul>";
                                $.each(sortedSubmenu, function (index) {
                                    var subChecked = this.Status != '' ? 'checked' : '';
                                    var readOnlySub = this.ReadAndWrite ? '' : 'checked';
                                    var delChecked = this.Delete ? 'checked' : '';
                                    var readAndWriteSub = this.ReadAndWrite ? 'checked' : '';
                                    menuTab += "            <li>";
                                    menuTab += "                <div class='row'>";
                                    menuTab += "                    <div class='col-sm-2'>";
                                    menuTab += "                        <div class='checkbox checkbox-css checkbox-inline'>\
                                                                            <input type='checkbox'  id='chkSub" + this.PageName.replace(/\s/g, "") + "' value='" + this.ID + "' " + subChecked + " data-pagename='" + this.PageName.replace(/\s/g, "") + "' data-parent='chk" + this.ParentMenu.replace(/\s/g, "") + "' data-myclass='" + this.ParentMenu.replace(/\s/g, "") + "' class='subMenu chkUserAccess " + this.ParentMenu.replace(/\s/g, "") + " clickable'>\
                                                                            <label class='clickable' for='chkSub" + this.PageName.replace(/\s/g, "") + "'>" + this.PageLabel + "</label>\
                                                                        </div>";
                                    menuTab += "                    </div>";
                                    menuTab += "                    <div class='col-sm-2'>";
                                    menuTab += "                        <div class='radio radio-css radio-inline'>\
                                                                            <input class='form-check-input clickable' type='radio' name='rdoReadAndWrite" + this.PageName.replace(/\s/g, '') + "' id='rdoRead" + this.PageName.replace(/\s/g, '') + "' value='0' " + readOnlySub + ">\
                                                                            <label class='form-check-label  clickable' for='rdoRead" + this.PageName.replace(/\s/g, '') + "'>Read Only</label>\
                                                                        </div>\
                                                                    </div>";
                                    menuTab += "                    <div class='col-sm-2'>\
                                                                        <div class='radio radio-css radio-inline'>\
                                                                            <input class='form-check-input clickable' type='radio' name='rdoReadAndWrite" + this.PageName.replace(/\s/g, '') + "' id='rdoReadWrite" + this.PageName.replace(/\s/g, '') + "' value='1' " + readAndWriteSub + ">\
                                                                            <label class='form-check-label  clickable' for='rdoReadWrite" + this.PageName.replace(/\s/g, '') + "'>Read & Write</label>\
                                                                        </div>\
                                                                    </div>";
                                    menuTab += "                    <div class='col-sm-2'>";
                                    menuTab += "                        <div class='checkbox checkbox-css checkbox-inline'>\
                                                                            <input type='checkbox'  id='chkDel" + this.PageName.replace(/\s/g, "") + "' value='" + this.ID + "' " + delChecked + " class='clickable'/>\
                                                                            <label class='clickable' for='chkDel" + this.PageName.replace(/\s/g, "") + "'>Delete</label>\
                                                                        </div>";
                                    menuTab += "                    </div>";
                                    menuTab += "                </div>";
                                    menuTab += "            </li>";
                                });
                                menuTab += "            </ul>";
                                menuTab += "        </li>";
                            }
                        }

                        menuTab += "    </ul>";
                        menuTab += "</div>";
                        //End Creating indivdial menu list content
                    }
                });

                menuTab += "</div>";
                //Creating Parent Menu Tab Content
                menuTab += "</div>";
                //End Creating GroupLabel Individual Tab Content
            });
            menuTab += "</div>";
            //End Creating GroupLabel Tab Content
            $("#mdlBodyUserAccess").html(menuTab);
            $("#mdlUserAccess").modal("show");
            return;
            //==============================================================================================================================================================
        }
        function saveUserAccess() {
            var ID = tblUser.rows({ selected: true }).data()[0].ID;
            var arrAccessList = [];
            $(".chkUserAccess").each(function (i, val) {
                var userAccessList = {};
                var status = "";
                var pagename = $(this).data("pagename");
                var readAndWrite = "0";
                var deleteFunction = "0";
                if ($(this).is(":checked")) {
                    status = true;
                } else {
                    status = false;
                }
                if ($("input[name=rdoReadAndWrite" + pagename + "]").length) {
                    readAndWrite = $("input[name=rdoReadAndWrite" + pagename + "]:checked").val();
                } else {
                    if ($(this).data('hassub') == '1')
                        readAndWrite = $(this).data('readandwrite');
                }
                if ($("#chkDel" + pagename).is(":checked")) {
                    deleteFunction = "1"
                } else {
                    if ($(this).data('hassub') == '1')
                        deleteFunction = $(this).data('delete');
                }

                userAccessList = {
                    UserID: ID,
                    PageID: $(this).val(),
                    Status: status,
                    ReadAndWrite: readAndWrite,
                    Delete: deleteFunction
                }
                arrAccessList.push(userAccessList);
            });

            var AdminLength = _.filter(arrAccessList, function (o) { if (o.Delete == '1' && o.ReadAndWrite == '1') return o }).length
            var EncoderLength = _.filter(arrAccessList, function (o) { if (o.Delete == '0' && o.ReadAndWrite == '1') return o }).length
            var ViewerLength = _.filter(arrAccessList, function (o) { if (o.Delete == '0' && o.ReadAndWrite == '0') return o }).length
            var Role = "";
            if (arrAccessList.length == AdminLength)
                Role = "Administrator";
            else if (arrAccessList.length == EncoderLength)
                Role = "Encoder";
            else if (arrAccessList.length == ViewerLength)
                Role = "Viewer";
            else if (arrAccessList.length != AdminLength && arrAccessList.length != ViewerLength && arrAccessList.length != ViewerLength)
                Role = "Custom";

            User.formAction = '/MasterMaintenance/UserMaster/SaveUserAccess';
            User.jsonData = { userAccessList: arrAccessList, Role: Role, UserID: ID };
            User.sendData().then(function (response) {
                tblUser.ajax.reload(null, false);
                cancelUserTbl();
                $("#mdlUserAccess").modal("hide");
            });
        }
        function getOtherInformationDetails(SelectedID, FirstName, LastName, Type) {
            var ModalTitle = "";
            var ListData = "";
            if (Type === "student") {
                ModalTitle = "Student Information";
                $("#ShowStudentOtherInformation").prop("hidden", false);
                $("#ShowFacultyOtherInformation").prop("hidden", true);
                $("#ShowNonFacultyOtherInformation").prop("hidden", true);
            } else if (Type === "faculty") {
                ModalTitle = "Faculty Information";
                $("#ShowFacultyOtherInformation").prop("hidden", false);
                $("#ShowStudentOtherInformation").prop("hidden", true);
                $("#ShowNonFacultyOtherInformation").prop("hidden", true);
            }
            else if (Type === "nonfaculty") {
                ModalTitle = "Non-Faculty Information";
                $("#ShowNonFacultyOtherInformation").prop("hidden", false);
                $("#ShowFacultyOtherInformation").prop("hidden", true);
                $("#ShowStudentOtherInformation").prop("hidden", true);
            }

            User.formAction = '/MasterMaintenance/UserMaster/GetOtherUserInformation';
            if (User.formAction) {
                User.jsonData = { SelectedID: SelectedID, UserRole: Type };
                User.sendData().then(function () {
                    populateOtherInformation(
                        User.responseData.userStudentDetails[0]
                        ,User.responseData.userStudentSocialStatusDetails[0]
                        ,User.responseData.userStudentScholarShipDetails[0]
                        ,User.responseData.userStudentOrganizeDetails[0]
                        ,User.responseData.userFacultyDetails[0]
                        ,User.responseData.userNonFacultyDetails[0]
                        , ModalTitle
                        , FirstName
                        , LastName
                        , Type);
                });
            } else {
                User.showError("Please try again.");
            }

            
        }
        function populateOtherInformation(studentInfo,studentSocialStatus,studentScholarShip,StudentOrganize,facultyInfo,NonfacultyInfo, ModalTitle, FirstName, LastName,userType) {
            $("#mdlShowOtherInfoTitle").text(ModalTitle);
            $("#ShowFirstName").val(FirstName);
            $("#ShowLastName").val(LastName);
            $(".infodetails").prop("readonly", true);

            if (userType === "student") {
                $("#ShowStuDepartment").val(studentInfo.StuDepartmentData);
                $("#ShowStuCourse").val(studentInfo.StuCourseData);
                $("#ShowStuSection").val(studentInfo.StuSectionData);
                $("#ShowStuYearLevel").val(studentInfo.StuYearLevelData);

                SetCheckBox(studentSocialStatus.PersonDisability, "ShowPersonDisability");
                //SetCheckBox(studentSocialStatus.NotApplicable, "ShowNotApplicable");
                SetCheckBox(studentSocialStatus.WorkingStudent, "ShowWorkingStudent");
                SetCheckBox(studentScholarShip.TES, "ShowTES");
                SetCheckBox(studentScholarShip.TDP, "ShowTDP");
                if (studentInfo.Organization === "True") {
                    $("#ShowOrganization").val(1);
                    $("#ShowStudentOrganization").prop("hidden", false);
                } else {
                    $("#ShowOrganization").val(0);
                    $("#ShowStudentOrganization").prop("hidden", true);
                }
                SetShowOrganizationList(StudentOrganize);
            }
            else if (userType === "faculty") {
                $("#ShowFacDepartment").val(facultyInfo.FacDepartmentData);
                $("#ShowFacPosition").val(facultyInfo.FacPositionData);
            }
            else if (userType === "nonfaculty") {
                $("#ShowNonFacPosition").val(NonfacultyInfo.NonFacPositionData)
            }
            $("#mdlShowOtherInfo").modal('show');
        }
        function SetCheckBox(type,InputID) {
            if (type === "True") {
                $("#" + InputID).prop('checked', true);
            } else {
                $("#" + InputID).prop('checked', false);
            }
        }
        function SetOrganizationList(OrgList) {
            if (OrgList !== undefined) {
                SetCheckBox(OrgList.ALLS, "ALLS");
                SetCheckBox(OrgList.APL, "APL");
                SetCheckBox(OrgList.ASGRD, "ASGRD");
                SetCheckBox(OrgList.CAITO, "CAITO");
                SetCheckBox(OrgList.CAPS, "CAPS");
                SetCheckBox(OrgList.CSS, "CSS");
                SetCheckBox(OrgList.DCP, "DCP");
                SetCheckBox(OrgList.ENC, "ENC");
                SetCheckBox(OrgList.EXCEL, "EXCEL");
                SetCheckBox(OrgList.FELTA, "FELTA");
                SetCheckBox(OrgList.IFIGHT, "IFIGHT");
                SetCheckBox(OrgList.ITS, "ITS");
                SetCheckBox(OrgList.JPIA, "JPIA");
                SetCheckBox(OrgList.LAGABATA, "LAGABATA");
                SetCheckBox(OrgList.LEAD, "LEAD");
                SetCheckBox(OrgList.MDAS, "MDAS");
                SetCheckBox(OrgList.MSS, "MSS");
                SetCheckBox(OrgList.ROTARACT, "ROTARACT");
                SetCheckBox(OrgList.SAVE, "SAVE");
                SetCheckBox(OrgList.SFJ, "SFJ");
                SetCheckBox(OrgList.SOCIOS, "SOCIOS");
                SetCheckBox(OrgList.PROTEGE, "PROTEGE");
                SetCheckBox(OrgList.UM3P, "UM3P");
            }
           
        }
        function SetShowOrganizationList(OrgList) {
            if (OrgList !== undefined) {
                SetCheckBox(OrgList.ALLS, "ShowALLS");
                SetCheckBox(OrgList.APL, "ShowAPL");
                SetCheckBox(OrgList.ASGRD, "ShowASGRD");
                SetCheckBox(OrgList.CAITO, "ShowCAITO");
                SetCheckBox(OrgList.CAPS, "ShowCAPS");
                SetCheckBox(OrgList.CSS, "ShowCSS");
                SetCheckBox(OrgList.DCP, "ShowDCP");
                SetCheckBox(OrgList.ENC, "ShowENC");
                SetCheckBox(OrgList.EXCEL, "ShowEXCEL");
                SetCheckBox(OrgList.FELTA, "ShowFELTA");
                SetCheckBox(OrgList.IFIGHT, "ShowIFIGHT");
                SetCheckBox(OrgList.ITS, "ShowITS");
                SetCheckBox(OrgList.JPIA, "ShowJPIA");
                SetCheckBox(OrgList.LAGABATA, "ShowLAGABATA");
                SetCheckBox(OrgList.LEAD, "ShowLEAD");
                SetCheckBox(OrgList.MDAS, "ShowMDAS");
                SetCheckBox(OrgList.MSS, "ShowMSS");
                SetCheckBox(OrgList.ROTARACT, "ShowROTARACT");
                SetCheckBox(OrgList.SAVE, "ShowSAVE");
                SetCheckBox(OrgList.SFJ, "ShowSFJ");
                SetCheckBox(OrgList.SOCIOS, "ShowSOCIOS");
                SetCheckBox(OrgList.PROTEGE, "ShowPROTEGE");
                SetCheckBox(OrgList.UM3P, "ShowUM3P");
            }
           
        }
        function ShowDetails(userType) {
            if (userType === "Student") {
                $("#StudentOtherInformation").prop("hidden", false);
                $("#FacultyOtherInformation").prop("hidden", true);
                $("#NonFacultyOtherInformation").prop("hidden", true);
            } else if (userType === "Faculty") {
                $("#FacultyOtherInformation").prop("hidden", false);
                $("#StudentOrganization").prop("hidden", true);
                $("#StudentOtherInformation").prop("hidden", true);
                $("#NonFacultyOtherInformation").prop("hidden", true);
            }else if (userType === "NonFaculty") {
                $("#NonFacultyOtherInformation").prop("hidden", false);
                $("#StudentOrganization").prop("hidden", true);
                $("#FacultyOtherInformation").prop("hidden", true);
                $("#StudentOtherInformation").prop("hidden", true);
            }
        }
    });
})();
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         
         