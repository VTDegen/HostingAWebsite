(function () {
    var User = $D();
    var Username = "";
    var populateMode = false;
    var costCenterSelectElementCounter = 1;
    $(document).ready(function () {
        SetupDisplay();
       //
        $("#btnChangePassword").click(function () {
            editUser();
        });
        $("#frmUser").submit(function (e) {
            e.preventDefault();
            saveUser();
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
                width: "58%",
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
                width: "70%",
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
    });
    function SetupDisplay() {
        var SelectedRole = $("#CurrentRole").text();

        $("#AdministratorDetails").prop("hidden", false);
        $("#AdminNotification").prop("hidden", false);
        $("#UserNotification").prop("hidden", true);
    }
    function saveUser() {
        User.formData = $('#frmUser').serializeArray();
        User.formAction = '/Home/SaveUser';
        User.setJsonData();
        User.sendData().then(function () {
            cancelUserForm();
        });
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
    function editUser() {
        populateMode = true;
        var Username = $("#UserNameData").val();
        User.formAction = '/Home/GetUserDetails';
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
    function populateUserData(user, student, socialstatus, scholarship, organize, faculty, nonfaculty) {
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
            SetCheckBox(socialstatus.NotApplicable, "NotApplicable");
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
})();