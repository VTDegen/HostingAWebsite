"use strict";
(function () {
    const GenReport = function () {
        return new GenReport.init();
    }
    GenReport.init = function () {
        $D.init.call(this);
        this.$tblAnnouncementList = "";
    }
    GenReport.prototype = {
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
        /*    self.$tblAnnouncementList.ajax.reload(null, false);*/
            $(".fold").attr("hidden", true);
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
                width: "82%",
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
                width: "87%",
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
        downloadReport: function () {
            var self = this;
            var formList = [];


            var SrchUsername = $("#SrchUsername").val() == "" || $("#SrchUsername").val() == null ? "" : "AND u.Username = '" + $("#SrchUsername").val() + "' ";
            var SrchFirstName = $("#SrchFirstName").val() == "" || $("#SrchFirstName").val() == null ? "" : "AND u.FirstName = '" + $("#SrchFirstName").val() + "' ";
            var SrchLastName = $("#SrchLastName").val() == "" || $("#SrchLastName").val() == null ? "" : "AND u.LastName = '" + $("#SrchLastName").val() + "' ";
            var SrchMiddleName = $("#SrchMiddleName").val() == "" || $("#SrchMiddleName").val() == null ? "" : "AND u.MiddleName = '" + $("#SrchMiddleName").val() + "' ";
            var SrchContactNumber = $("#SrchContactNumber").val() == "" || $("#SrchContactNumber").val() == null ? "" : "AND u.ContactNumber = '" + $("#SrchContactNumber").val() + "' ";
            var SrchEmail = $("#SrchEmail").val() == "" || $("#SrchEmail").val() == null ? "" : "AND u.Email = '" + $("#SrchEmail").val() + "' ";
            var SrchGender = $("#SrchGender").val() == "" || $("#SrchGender").val() == null ? "" : "AND u.Gender = '" + $("#SrchGender").val() + "' ";
            var SrchCivilStatus = $("#SrchCivilStatus").val() == "" || $("#SrchCivilStatus").val() == null ? "" : "AND u.CivilStatus = '" + $("#SrchCivilStatus").val() + "' ";
            var SrchRole = $("#SrchRole").val() == "" || $("#SrchRole").val() == null ? "" : "AND u.UserRole = '" + $("#SrchRole").val() + "' ";
            var SrchStuDepartment = $("#SrchStuDepartment").val() == "" || $("#SrchStuDepartment").val() == null ? "" : "AND si.Department = '" + $("#SrchStuDepartment").val() + "' ";
            var SrchStuCourse = $("#SrchStuCourse").val() == "" || $("#SrchStuCourse").val() == null ? "" : "AND si.Course = '" + $("#SrchStuCourse").val() + "' ";
            var SrchStuSection = $("#SrchStuSection").val() == "" || $("#SrchStuSection").val() == null ? "" : "AND si.Section = '" + $("#SrchStuSection").val() + "' ";
            var SrchStuYearLevel = $("#SrchStuYearLevel").val() == "" || $("#SrchStuYearLevel").val() == null ? "" : "AND si.Year = '" + $("#SrchStuYearLevel").val() + "' ";
            var SrchTES = $("#SrchTES").is(":checked") == false || $("#SrchTES").val() == null ? "" : "AND ss.TES = 1 ";
            var SrchTDP = $("#SrchTDP").is(":checked") == false || $("#SrchTDP").val() == null ? "" : "AND ss.TDP = 1 ";
            var SrchPersonDisability = $("#SrchPersonDisability").is(":checked") == false || $("#SrchPersonDisability").val() == null ? "" : "AND st.PersonDisability = 1 ";
            var SrchWorkingStudent = $("#SrchWorkingStudent").is(":checked") == false || $("#SrchWorkingStudent").val() == null ? "" : "AND st.WorkingStudent = 1 ";
            var SrchNotApplicable = $("#SrchNotApplicable").is(":checked") == false || $("#SrchNotApplicable").val() == null ? "" : "AND st.NotApplicable = 1 ";
            var SrchOrganization = $("#SrchOrganization").val() == "" ? "" : "AND si.IsOrganization = 1 ";
            var SrchALLS = $("#SrchALLS").is(":checked") == false ? "" : "AND so.ALLS = 1 ";
            var SrchAPL = $("#SrchAPL").is(":checked") == false ? "" : "AND so.APL = 1 ";
            var SrchASGRD = $("#SrchASGRD").is(":checked") == false ? "" : "AND so.ASGRD = 1 ";
            var SrchCAITO = $("#SrchCAITO").is(":checked") == false ? "" : "AND so.CAITO = 1 ";
            var SrchCAPS = $("#SrchCAPS").is(":checked") == false ? "" : "AND so.CAPS = 1 ";
            var SrchCSS = $("#SrchCSS").is(":checked") == false ? "" : "AND so.CSS = 1 ";
            var SrchDCP = $("#SrchDCP").is(":checked") == false ? "" : "AND so.DCP = 1 ";
            var SrchENC = $("#SrchENC").is(":checked") == false ? "" : "AND so.ENC = 1 ";
            var SrchEXCEL = $("#SrchEXCEL").is(":checked") == false ? "" : "AND so.EXCEL = 1 ";
            var SrchFELTA = $("#SrchFELTA").is(":checked") == false ? "" : "AND so.FELTA = 1 ";
            var SrchIFIGHT = $("#SrchIFIGHT").is(":checked") == false ? "" : "AND so.IFIGHT = 1 ";
            var SrchITS = $("#SrchITS").is(":checked") == false ? "" : "AND so.ITS = 1 ";
            var SrchJPIA = $("#SrchJPIA").is(":checked") == false ? "" : "AND so.JPIA = 1 ";
            var SrchLAGABATA = $("#SrchLAGABATA").is(":checked") == false ? "" : "AND so.LAGABATA = 1 ";
            var SrchLEAD = $("#SrchLEAD").is(":checked") == false ? "" : "AND so.LEAD = 1 ";
            var SrchMDAS = $("#SrchMDAS").is(":checked") == false ? "" : "AND so.MDAS = 1 ";
            var SrchMSS = $("#SrchMSS").is(":checked") == false ? "" : "AND so.MSS = 1 ";
            var SrchROTARACT = $("#SrchROTARACT").is(":checked") == false ? "" : "AND so.ROTARACT = 1 ";
            var SrchSAVE = $("#SrchSAVE").is(":checked") == false ? "" : "AND so.[SAVE] = 1 ";
            var SrchSFJ = $("#SrchSFJ").is(":checked") == false ? "" : "AND so.SFJ = 1 ";
            var SrchSOCIOS = $("#SrchSOCIOS").is(":checked") == false ? "" : "AND so.SOCIOS = 1 ";
            var SrchPROTEGE = $("#SrchPROTEGE").is(":checked") == false ? "" : "AND so.PROTEGE = 1 ";
            var SrchUM3P = $("#SrchUM3P").is(":checked") == false ? "" : "AND so.UM3P = 1 ";
            var SrchFacDepartment = $("#SrchFacDepartment").val() == null ? "" : "AND fi.Department = '" + $("#SrchFacDepartment").val() + "' ";
            var SrchFacPosition = $("#SrchFacPosition").val() == null ? "" : "AND fi.Position = '" + $("#SrchFacPosition").val() + "' ";
            var SrchDateRange = "AND (ts.DatePosted between '" + $("#SrchDateFrom").val() + "' and '" + $("#SrchDateTo").val() + "')"

            
            var searcQuery = SrchUsername + SrchFirstName + SrchLastName + SrchMiddleName + SrchContactNumber + SrchEmail + SrchGender +
                SrchCivilStatus + SrchRole + SrchStuDepartment + SrchStuCourse + SrchStuSection + SrchStuYearLevel + SrchTES + SrchTDP +
                SrchPersonDisability + SrchWorkingStudent + SrchNotApplicable + SrchOrganization + SrchALLS + SrchAPL + SrchASGRD + SrchCAITO +
                SrchCAPS + SrchCSS + SrchDCP + SrchENC + SrchEXCEL + SrchFELTA + SrchIFIGHT + SrchITS + SrchJPIA + SrchLAGABATA + SrchLEAD +
                SrchMDAS + SrchMSS + SrchROTARACT + SrchSAVE + SrchSFJ + SrchSOCIOS + SrchPROTEGE + SrchUM3P + SrchFacDepartment + SrchFacPosition + SrchDateRange;

            formList = {
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
            //self.formAction = '/Transaction/RequestNotification/SendAnnouncement';
            //self.jsonData = { formListData: formList };
            //self.sendData().then(function () {

            //});
            //self.formAction = '/Report/ReportGeneration/DownloadReport';
            //self.jsonData = { searcQuery: searcQuery };
            //self.sendData().then(function () {
            //    window.location = '/Report/ReportGeneration/DownloadReport?file=' + data;
            //});
            //self.formAction = '/Report/ReportGeneration/DownloadCSV';
            //self.jsonData = { formListData: formList };
            //self.sendData().then(function (data) {
            
            //});
                        //$.ajax({
            //    type: 'POST',
            //    url: '/Report/ReportGeneration/DownloadCSV',
            //    data: { formListData: formList },
            //    contentType: 'application/json; charset=utf-8',
            //    dataType: 'json',
            //    success: function (returnValue) {
            //        window.location = '/Report/ReportGeneration/DownloadCSV?file=' + returnValue;
            //    }
            //});
            location.assign("/Report/ReportGeneration/DownloadReport?searcQuery=" + searcQuery);

        }

    }
    GenReport.init.prototype = $.extend(GenReport.prototype, $D.init.prototype);
    GenReport.init.prototype = GenReport.prototype;
    var GenerateReport = GenReport();
    var CUI2 = $UI();
    $(document).ready(function () {
        var Genderquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 1) AND IsDeleted = 0 ";
        var CivilStatusquery = "SELECT * FROM mGeneral WHERE [TypeID] = (SELECT ID FROM mTypes WHERE ID = 2) AND IsDeleted = 0 ";
        GenerateReport.initSelect2ForFilters();

        $("#SrchDateFrom").datepicker({
            todayHighlight: true,
            autoclose: true,
            format: 'yyyy-mm-dd',
        }).on('changeDate', function (selected) {
            var minDate = new Date(selected.date.valueOf());
            $('#SrchDateTo').datepicker('setStartDate', minDate);
        });

        $("#SrchDateTo").datepicker({
            todayHighlight: true,
            autoclose: true,
            format: 'yyyy-mm-dd',
        }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#SrchDateFrom').datepicker('setEndDate', maxDate);
           
        });
       

        $("#btnDownload").click(function () {
            var datefrom = $("#SrchDateFrom").val();
            var dateTo = $("#SrchDateTo").val();
            if (datefrom === "" || dateTo === "") {
                GenerateReport.showInfo("Fill Date From or Date To");
            } else {
                GenerateReport.downloadReport();
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



