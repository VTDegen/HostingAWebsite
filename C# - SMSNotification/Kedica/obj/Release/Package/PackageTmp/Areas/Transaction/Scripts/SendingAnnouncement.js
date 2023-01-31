"use strict";
(function () {
    const SendAnnounce = function () {
        return new SendAnnounce.init();
    }
    SendAnnounce.init = function () {
        $D.init.call(this);
        this.$tblAnnouncementList = "";
    }
    SendAnnounce.prototype = {
        sendAnnouncement: function () {
            var self = this;
            self.formAction = '/Transaction/RequestNotification/GetAnnouncementListCount';
            self.sendData().then(function () {
                self.setupAnnouncement(self.responseData.announceCount);
            });
           
        },
        clearFields: function () {
            var self = this;
            self.clearFromData("frmSendTransaction");
            self.$tblAnnouncementList.ajax.reload(null, false);
            $(".fold").attr("hidden", true);
        },
        drawDataTable: function () {
            var self = this;
            if (!$.fn.DataTable.isDataTable('#tblAnnouncementList')) {
                self.$tblAnnouncementList = $('#tblAnnouncementList').DataTable({
                    processing: true,
                    serverSide: true,
                    "order": [[0, "desc"]],
                    "pageLength": 10,
                    "ajax": {
                        "url": "/Transaction/RequestNotification/GetAnnouncementList",
                        "type": "POST",
                        "datatype": "json",
                    },
                    dataSrc: "data",
                    scrollY: '100%', scrollX: '100%',
                    select: true,
                    columns: [
                        { title: "ID", data: "ID", visible: false },
                        { title: "Full Name", data: "FullName" },
                        { title: "Category", data: "UserRole" },
                        { title: "Announcement", data: "Announcement" },
                        { title: "Attachement Name", data: "attachmentName" },
                        { title: "Date Posted", data: "DatePosted" },
                        { title: "Post Body", data: "PostBody" },
                        { title: "Posted By", data: "PostedBy" },
                    ],
                    "createdRow": function (row, data, dataIndex) {
                        $(row).attr('data-id', data.ID);
                        $(row).attr('data-username', data.Username);
                    },

                })
            }
        },
        initSelect2ForFilters: function () {

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
        },
        setupAnnouncement: function (CurrentPost) {
            var self = this;
            var formList = [];
            var SelectedFile = $("#PostAttachment")[0].files;
            var ArrayListFile = "";
            var FileName = "";
            if (SelectedFile.length === 0) {
                ArrayListFile = "";
                FileName = "";
            } else {
                ArrayListFile = SelectedFile[0].name.split('.');
                FileName = ArrayListFile[ArrayListFile.length - 2] + CurrentPost + "." + ArrayListFile[ArrayListFile.length - 1];
            }
            formList = {
                UserName: $("#UsernameData").val(),
                UserRole: $("#RoleData").val(),
                Gender: $("#GenderData").val() == null ? "0" : $("#GenderData").val(),
                CivilStatus: $("#CivilStatusData").val() == null ? "0" : $("#CivilStatusData").val(),
                PostBody: $("#PostBody").val(),
                PostAttachment: $("#PostAttachment")[0].files.length === 0 ? "" : FileName,
                PostContains: $("#PostContains").val(),
                SrchUsername: $("#SrchUsername").val() || "",
                SrchFirstName: $("#SrchFirstName").val() || "",
                SrchLastName: $("#SrchLastName").val() || "",
                SrchMiddleName: $("#SrchMiddleName").val() || "",
                SrchContactNumber: $("#SrchContactNumber").val() || "",
                SrchEmail: $("#SrchEmail").val() || "",
                SrchGender: $("#SrchGender").val() || "",
                SrchCivilStatus: $("#SrchCivilStatus").val() || "",
                SrchRole: $("#SrchRole").val() || "",
                SrchStuDepartment: $("#SrchStuDepartment").val() || "",
                SrchStuCourse: $("#SrchStuCourse").val() || "",
                SrchStuSection: $("#SrchStuSection").val() || "",
                SrchStuYearLevel: $("#SrchStuYearLevel").val() || "",
                SrchTES: $("#SrchTES").is(":checked") ? "1" : "0" || "0",
                SrchTDP: $("#SrchTDP").is(":checked") ? "1" : "0" || "0",
                SrchPersonDisability: $("#SrchPersonDisability").is(":checked") ? "1" : "0" || "0",
                SrchWorkingStudent: $("#SrchWorkingStudent").is(":checked") ? "1" : "0" || "0",
                SrchNotApplicable: $("#SrchNotApplicable").is(":checked") ? "1" : "0" || "0",
                SrchOrganization: $("#SrchOrganization").val() || "",
                SrchALLS: $("#SrchALLS").is(":checked") ? "1" : "0" || "0",
                SrchAPL: $("#SrchAPL").is(":checked") ? "1" : "0" || "0",
                SrchASGRD: $("#SrchASGRD").is(":checked") ? "1" : "0" || "0",
                SrchCAITO: $("#SrchCAITO").is(":checked") ? "1" : "0" || "0",
                SrchCAPS: $("#SrchCAPS").is(":checked") ? "1" : "0" || "0",
                SrchCSS: $("#SrchCSS").is(":checked") ? "1" : "0" || "0",
                SrchDCP: $("#SrchDCP").is(":checked") ? "1" : "0" || "0",
                SrchENC: $("#SrchENC").is(":checked") ? "1" : "0" || "0",
                SrchEXCEL: $("#SrchEXCEL").is(":checked") ? "1" : "0" || "0",
                SrchFELTA: $("#SrchFELTA").is(":checked") ? "1" : "0" || "0",
                SrchIFIGHT: $("#SrchIFIGHT").is(":checked") ? "1" : "0" || "0",
                SrchITS: $("#SrchITS").is(":checked") ? "1" : "0" || "0",
                SrchJPIA: $("#SrchJPIA").is(":checked") ? "1" : "0" || "0",
                SrchLAGABATA: $("#SrchLAGABATA").is(":checked") ? "1" : "0" || "0",
                SrchLEAD: $("#SrchLEAD").is(":checked") ? "1" : "0" || "0",
                SrchMDAS: $("#SrchMDAS").is(":checked") ? "1" : "0" || "0",
                SrchMSS: $("#SrchMSS").is(":checked") ? "1" : "0" || "0",
                SrchROTARACT: $("#SrchROTARACT").is(":checked") ? "1" : "0" || "0",
                SrchSAVE: $("#SrchSAVE").is(":checked") ? "1" : "0" || "0",
                SrchSFJ: $("#SrchSFJ").is(":checked") ? "1" : "0" || "0",
                SrchSOCIOS: $("#SrchSOCIOS").is(":checked") ? "1" : "0" || "0",
                SrchPROTEGE: $("#SrchPROTEGE").is(":checked") ? "1" : "0" || "0",
                SrchUM3P: $("#SrchUM3P").is(":checked") ? "1" : "0" || "0",
                SrchFacDepartment: $("#SrchFacDepartment").val() || "",
                SrchFacPosition: $("#SrchFacPosition").val() || "",
            }
            if ($("#PostAttachment")[0].files.length !== 0) {
                var formData = new FormData();
                var _Myfile = $("#PostAttachment")[0].files;
                var extension = _Myfile[0].name.split('.');
                var FileName = extension[extension.length - 2] + CurrentPost;
                formData.append('files[0]', _Myfile[0], FileName + "." + extension[extension.length - 1]);

                self.jsonData = formData;
                self.formAction = '/Transaction/RequestNotification/SaveFileUploadIMG';
                self.sendFile().then(function () {
                    self.formAction = '/Transaction/RequestNotification/SendAnnouncement';
                    self.jsonData = { formListData: formList };
                    self.sendData().then(function () {
                        self.clearFields();
                        window.location = '/Transaction/RequestNotification';
                    });
                });
            } else {
                self.formAction = '/Transaction/RequestNotification/SendAnnouncement';
                self.jsonData = { formListData: formList };
                self.sendData().then(function () {
                    self.clearFields();
                    window.location = '/Transaction/RequestNotification';
                });
            }
        }

    }
    SendAnnounce.init.prototype = $.extend(SendAnnounce.prototype, $D.init.prototype);
    SendAnnounce.init.prototype = SendAnnounce.prototype;
    var SendingAnnouncement = SendAnnounce();
    var CUI2 = $UI();
    $(document).ready(function () {
        var Genderquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1) AND IsDeleted = 0 ";
        var CivilStatusquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 2) AND IsDeleted = 0 ";
        SendingAnnouncement.drawDataTable();
        SendingAnnouncement.initSelect2ForFilters();

        $("#frmSendTransaction").submit(function (e) {
            e.preventDefault();
            SendingAnnouncement.sendAnnouncement();
        });
        $("#btnPostAnnouncement").click(function () {
            $("#frmSendTransaction").submit()
        });
        $("#btnCancelSearch").click(function () {
            SendingAnnouncement.clearFields();
        });

        SendingAnnouncement.$tblAnnouncementList.on('draw.dt', function () {
            CUI2.dataTableID = "#tblAnnouncementList";
            CUI2.setDatatableMaxHeight();
          
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
    });

})();



