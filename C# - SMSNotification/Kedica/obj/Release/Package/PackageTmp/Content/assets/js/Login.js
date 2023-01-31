/*****************************************
A. Name: Login Script
B. Date Created: Aug 09, 2020
C. Author: Jay-R A. Magdaluyo
D. Modification History:
E. Synopsis: For User login
***********************************************/
'use strict';
(function () {
    var Login = $D();
    var Username = "";
    var populateMode = false;
    var costCenterSelectElementCounter = 1;
    var CurrentCodeData = "";
    var isValidRegister = false;
    $(document).ready(function () {
        $("#UserName").focus();
        $("#frmLogin").submit(function (e) {
            e.preventDefault();
            LoginMeIn();
        });
        $("#btnRegisterUser").click(function () {
            populateMode = false;
            $("#mdlUser").modal("show");
        });
        $("#Username").change(function () {
            validateUsername(this);
        });
        $("#Email").change(function () {
            valiteEmail(this);
        });
        $("#frmUser").submit(function (e) {
            e.preventDefault();
            var email = $("#Email").val();
            if (email.substr(email.length - 11) == "@ccc.edu.ph") {
                $("#Email").addClass("parsley-success");
                $("#Email").removeClass("parsley-error");
                $("#mdlUser").modal("hide");
                $("#mdlTermsCondition").modal("show")
            }
            else {
                $("#Email").removeClass("parsley-success");
                $("#Email").addClass("parsley-error");
                Login.showError("Your Email ending must be: <b> @ccc.edu.ph </b>");
            }
           
        });
        $("#btnGeneratePassword").click(function () {
            $("#RegPassword").val(generatePassword());
            Login.parsleyValidate("frmUser");
        });
        $("#btnRealTime").click(function () {
            window.open("/Reports/RealTimeReportViewing", '_blank');
        });
        $("#ContactNumber").on('change', function () {
            var self = this;
            var getValue = $(self).val().trim();
            var getValidation = getValue.substring(0, 3);
            if (getValue.length === 13) {
                if (getValidation === "+63") {

                } else {
                    Login.showError("Invalid Format, Please try again");
                    $(self).val("");
                    $(self).focus();
                }
            } else {
                Login.showError("Invalid Input, Please try again");
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
        $("#Organization").on('change', function () {
            var DataValue = parseInt($(this).val())
            if (DataValue === 1) {
                $("#StudentOrganization").prop("hidden", false);
            } else {
                $("#StudentOrganization").prop("hidden", true);
                $(".orgClass").prop('checked', false);
            }
        });
        $('#btnCancelUser,#btnCloseUpperUser').click(function () {
            cancelUserForm();
            CUI.setDatatableMaxHeight();
        });
        $("#btnTermConditions").click(function () {
            saveUser();
        })
        $("#btnGetCodeFromEmail").click(function () {
            var checkEmail = $("#Email").val().trim();
            var checkFirstName = $("#FirstName").val().trim();
            
            if (checkEmail === "") {
                Login.showInfo("Fill Email field to get the code");
            } else {
                if (checkEmail.substr(checkEmail.length - 11) == "@ccc.edu.ph") {
                    $("#Email").addClass("parsley-success");
                    $("#Email").removeClass("parsley-error");
                    $("#FirstName").addClass("parsley-success");
                    $("#FirstName").removeClass("parsley-error");
                    if (checkFirstName === "") {
                        $("#FirstName").removeClass("parsley-success");
                        $("#FirstName").addClass("parsley-error");
                        Login.showError("Please Fill Up First Name");
                    } else {
                        verifiedUser(checkEmail, checkFirstName);
                    }
                    
                }
                else {
                    $("#Email").removeClass("parsley-success");
                    $("#Email").addClass("parsley-error"); 
                    Login.showError("Your Email ending must be: <b> @ccc.edu.ph </b>");
                }

            }
        });
        $("#btnVerifyCode").click(function () {

            var checkEmail = $("#Email").val().trim();
            if (checkEmail === "") {
                Login.showInfo("Fill Email field to get the code");
            } else {
                if (checkEmail.substr(checkEmail.length - 11) == "@ccc.edu.ph") {
                    $("#Email").addClass("parsley-success");
                    $("#Email").removeClass("parsley-error");
                    var getCurrentEmail = $("#EmailVerification").val();
                    if (getCurrentEmail === CurrentCodeData) {
                        $("#EmailVerification").prop("disabled", true);
                        $("#btnVerifyCode").prop("disabled", true);
                        $("#btnGetCodeFromEmail").prop("disabled", true);
                        isValidRegister = true;
                        Login.showSuccess("Successfully verified email");
                    } else {
                        Login.showInfo("Invalid code,Kindly check to your email");
                        isValidRegister = false;

                    }
                }
                else {
                    $("#Email").removeClass("parsley-success");
                    $("#Email").addClass("parsley-error");
                    Login.showError("Your Email ending must be: <b> @ccc.edu.ph </b>");
                }

            }
            

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

            clearVerifiedEmail();
        });
        //All Function --------------------------------------------------------------------------------
        function LoginMeIn() {
            Login.formData = $('#frmLogin').serializeArray();
            Login.setJsonData();
            Login.formAction = '/Login/LoginEntry';
            Login.sendData().then(function () {
                var login = Login.responseData;
                if (login.error) {
                    Login.showError(login.errmsg);
                    $("#UserName").addClass("input-error");
                    $("#Password").addClass("input-error");
                    $("#UserName").addClass("parsley-success");
                    $("#Password").addClass("parsley-success");
                    $("#Password").val("");
                } else {
                    $("#UserName").removeClass("input-error");
                    $("#Password").removeClass("input-error");
                    $("#frmLogin > div.login-buttons > button").attr("disabled", true);
                    window.location = "/";
                }
            });
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
        function valiteEmail(self) {
            var emailData = $(self).val().trim();
            if (emailData) {
                Login.formAction = '/Login/ValidateEmailAddress';
                Login.jsonData = { EmailAddress: emailData }
                Login.sendData().then(function () {
                    var validUsername = Login.responseData.isValid;
                    if (!validUsername) {
                        Login.showError("Email Address already exists. Please try another Email");
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
                Login.formAction = '/Login/ValidateUsername';
                Login.jsonData = { Username: username }
                Login.sendData().then(function () {
                    var validUsername = Login.responseData.isValid;
                    if (!validUsername) {
                        Login.showError("Username already exists. Please try another Username");
                        $(self).val("");
                        $(self).focus();
                    } else {
                        $("#RegPassword").focus();
                    }
                });
            }
        }
        function saveUser() {
            if (isValidRegister) {
                Login.formData = $('#frmUser').serializeArray();
                Login.formAction = '/Login/SaveUser';
                Login.setJsonData();
                Login.sendData().then(function () {
                    cancelUserForm();
                    clearVerifiedEmail();
                });
            } else {
                Login.showInfo("Verified Email first");
            }
            
        }
        function cancelUserForm() {
            populateMode = false;
            Login.clearFromData("frmUser");
            $('#Username').prop('readonly', false);
            $("#mdlUserTitle").text(" Create User");
            $("#btnSaveUser .btnLabel").text(" Save");
            $("#Role option[value='Custom']").remove();
            $("#mdlUser").modal("hide");
            $("#mdlTermsCondition").modal("hide");
            costCenterSelectElementCounter = 1;
            $("#StudentOtherInformation").prop("hidden", true);
            $("#FacultyOtherInformation").prop("hidden", true);
            $("#NonFacultyOtherInformation").prop("hidden", true);
            $("#StudentOrganization").prop("hidden", true);
            $(".chckStudentData").prop('checked', false)
        }
        function verifiedUser(email,firstname) {
            Login.formAction = '/Login/SendEmailCode';
            Login.jsonData = { EmaiLData: email, FirstName: firstname }
            Login.sendData().then(function () {
                var codeData = Login.responseData.CurrentCode;
                $("#EmailVerification").prop("disabled", false);
                $("#btnVerifyCode").prop("disabled", false);
                CurrentCodeData = codeData;
            });
        }
        function clearVerifiedEmail() {
            $("#EmailVerification").prop("disabled", true);
            $("#btnVerifyCode").prop("disabled", true);
            $("#btnGetCodeFromEmail").prop("disabled", false);
            $("#EmailVerification").val("");
            isValidRegister = false;
        }
    });
})();
