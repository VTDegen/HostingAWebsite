using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace SMSNotification.Controllers
{
    public class HomeController : Controller
    {
        Security ph = new Security();
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult SaveUser(mUser User)
        {
            string endMsg = "";
            int SetLastInsertID = 0;
            ModelState.Remove("ID");
            if (ModelState.IsValid)
            {
                try
                {
                    int ReadAndWrite = 0;
                    int CanDelete = 0;
                    if (User.Role == "HeadAdministrator")
                    {
                        ReadAndWrite = 1;
                        CanDelete = 1;
                    }
                    else
                    {
                        ReadAndWrite = 0;
                        CanDelete = 0;
                    }
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ToString()))
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            User.Password = ph.base64Encode(User.Password).ToString();

                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_InsertUpdate";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@ID", User.ID);
                            cmdMySql.Parameters.AddWithValue("@Username", User.Username);
                            cmdMySql.Parameters.AddWithValue("@Password", User.Password);
                            cmdMySql.Parameters.AddWithValue("@FirstName", User.FirstName);
                            cmdMySql.Parameters.AddWithValue("@LastName", User.LastName);
                            cmdMySql.Parameters.AddWithValue("@MiddleName", User.MiddleName);
                            cmdMySql.Parameters.AddWithValue("@Gender", User.Gender);
                            cmdMySql.Parameters.AddWithValue("@Email", User.Email);
                            cmdMySql.Parameters.AddWithValue("@ContactNumber", User.ContactNumber);
                            cmdMySql.Parameters.AddWithValue("@CivilStatus", User.CivilStatus);
                            cmdMySql.Parameters.AddWithValue("@Role", User.Role);
                            cmdMySql.Parameters.AddWithValue("@ReadAndWrite", ReadAndWrite);
                            cmdMySql.Parameters.AddWithValue("@CreateID", Session["Username"]);
                            SqlParameter EndMsg = cmdMySql.Parameters.Add("@EndMsg", SqlDbType.VarChar, 200);
                            SqlParameter ErrorMessage = cmdMySql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 200);
                            SqlParameter Error = cmdMySql.Parameters.Add("@IsError", SqlDbType.Bit);
                            SqlParameter LastID = cmdMySql.Parameters.Add("@LastInsertID", SqlDbType.Int);

                            EndMsg.Direction = ParameterDirection.Output;
                            Error.Direction = ParameterDirection.Output;
                            ErrorMessage.Direction = ParameterDirection.Output;
                            LastID.Direction = ParameterDirection.Output;

                            cmdMySql.ExecuteNonQuery();

                            error = Convert.ToBoolean(Error.Value);
                            if (error)
                                modelErrors.Add(ErrorMessage.Value.ToString());

                            endMsg = EndMsg.Value.ToString();
                            SetLastInsertID = Convert.ToInt32(LastID.Value);
                        }
                        conn.Close();
                        if (User.Role == "Student" || User.Role == "Faculty" || User.Role == "NonFaculty")
                        {
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                string SpData = "";
                                if (User.Role == "Student")
                                {
                                    SpData = "spUser_InsertUpdateStudentInformation";
                                }
                                else if (User.Role == "Faculty")
                                {
                                    SpData = "spUser_InsertUpdateFacultyInformation";
                                }
                                else if (User.Role == "NonFaculty")
                                {
                                    SpData = "spUser_InsertUpdateNonFacultyInformation";
                                }
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = SpData;
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@UserID", SetLastInsertID);

                                if (User.Role == "Student")
                                {
                                    cmdMySql.Parameters.AddWithValue("@Department", User.StuDepartment);
                                    cmdMySql.Parameters.AddWithValue("@Course", User.StuCourse);
                                    cmdMySql.Parameters.AddWithValue("@Section", User.StuSection);
                                    cmdMySql.Parameters.AddWithValue("@Year", User.StuYearLevel);
                                    cmdMySql.Parameters.AddWithValue("@Organization", User.Organization);
                                }
                                else if (User.Role == "Faculty")
                                {
                                    cmdMySql.Parameters.AddWithValue("@Department", User.FacDepartment);
                                    cmdMySql.Parameters.AddWithValue("@Position", User.FacPosition);
                                }
                                else if (User.Role == "NonFaculty")
                                {
                                    cmdMySql.Parameters.AddWithValue("@Position", User.NonFacPosition);
                                }
                                cmdMySql.Parameters.AddWithValue("@CreateID", Session["Username"]);
                                cmdMySql.ExecuteNonQuery();
                            }
                            conn.Close();
                            if (User.Role == "Student")
                            {
                                conn.Open();
                                using (SqlCommand cmdMySql = conn.CreateCommand())
                                {
                                    cmdMySql.CommandType = CommandType.StoredProcedure;
                                    cmdMySql.CommandText = "spUser_InsertUpdateStudentScholarShipInformation";
                                    cmdMySql.Parameters.Clear();
                                    cmdMySql.Parameters.AddWithValue("@UserID", SetLastInsertID);
                                    cmdMySql.Parameters.AddWithValue("@TES", User.TES == null ? 0 : User.TES == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@TDP", User.TDP == null ? 0 : User.TDP == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@CreateID", Session["Username"]);
                                    cmdMySql.ExecuteNonQuery();
                                }
                                conn.Close();
                                conn.Open();
                                using (SqlCommand cmdMySql = conn.CreateCommand())
                                {
                                    cmdMySql.CommandType = CommandType.StoredProcedure;
                                    cmdMySql.CommandText = "spUser_InsertUpdateStudentSocialStatusInformation";
                                    cmdMySql.Parameters.Clear();
                                    cmdMySql.Parameters.AddWithValue("@UserID", SetLastInsertID);
                                    cmdMySql.Parameters.AddWithValue("@PersonDisability", User.PersonDisability == null ? 0 : User.PersonDisability == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@WorkingStudent", User.WorkingStudent == null ? 0 : User.WorkingStudent == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@NotApplicable", User.NotApplicable == null ? 0 : User.NotApplicable == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@CreateID", Session["Username"]);
                                    cmdMySql.ExecuteNonQuery();
                                }
                                conn.Close();

                                if (User.Organization == "1")
                                {
                                    conn.Open();
                                    using (SqlCommand cmdMySql = conn.CreateCommand())
                                    {
                                        cmdMySql.CommandType = CommandType.StoredProcedure;
                                        cmdMySql.CommandText = "spUser_InsertUpdateOrganizationInformation";
                                        cmdMySql.Parameters.Clear();
                                        cmdMySql.Parameters.AddWithValue("@UserID", SetLastInsertID);
                                        cmdMySql.Parameters.AddWithValue("@ALLS", User.ALLS == null ? 0 : User.ALLS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@APL", User.APL == null ? 0 : User.APL == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ASGRD", User.ASGRD == null ? 0 : User.ASGRD == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CAITO", User.CAITO == null ? 0 : User.CAITO == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CAPS", User.CAPS == null ? 0 : User.CAPS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CSS", User.CSS == null ? 0 : User.CSS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@DCP", User.DCP == null ? 0 : User.DCP == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ENC", User.ENC == null ? 0 : User.ENC == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@EXCEL", User.EXCEL == null ? 0 : User.EXCEL == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@FELTA", User.FELTA == null ? 0 : User.FELTA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@IFIGHT", User.IFIGHT == null ? 0 : User.IFIGHT == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ITS", User.ITS == null ? 0 : User.ITS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@JPIA", User.JPIA == null ? 0 : User.JPIA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@LAGABATA", User.LAGABATA == null ? 0 : User.LAGABATA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@LEAD", User.LEAD == null ? 0 : User.LEAD == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@MDAS", User.MDAS == null ? 0 : User.MDAS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@MSS", User.MSS == null ? 0 : User.MSS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ROTARACT", User.ROTARACT == null ? 0 : User.ROTARACT == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SAVE", User.SAVE == null ? 0 : User.SAVE == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SFJ", User.SFJ == null ? 0 : User.SFJ == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SOCIOS", User.SOCIOS == null ? 0 : User.SOCIOS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@PROTEGE", User.PROTEGE == null ? 0 : User.PROTEGE == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@UM3P", User.UM3P == null ? 0 : User.UM3P == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CreateID", Session["Username"]);
                                        cmdMySql.ExecuteNonQuery();
                                    }
                                    conn.Close();
                                }
                            }

                        }

                    }
                }
                catch (Exception err)
                {
                    string errmsg;
                    if (err.InnerException != null)
                        errmsg = "Error: " + err.InnerException.ToString();
                    else
                        errmsg = "Error: " + err.Message.ToString();

                    return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                foreach (var modelStateKey in ViewData.ModelState.Keys)
                {
                    var modelStateVal = ViewData.ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        var key = modelStateKey;
                        var errMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        modelErrors.Add(errMessage);
                    }
                }
            }
            if (modelErrors.Count != 0 || error)
                return Json(new { success = false, errors = modelErrors });
            else
            {
                return Json(new { success = true, msg = "User was successfully " + endMsg });
            }
        }
        public ActionResult GetUserDetails(string Username)
        {
            List<mUser> userDetails = new List<mUser>();
            List<mUser> userStudentDetails = new List<mUser>();
            List<mUser> userStudentSocialStatusDetails = new List<mUser>();
            List<mUser> userStudentScholarShipDetails = new List<mUser>();
            List<mUser> userStudentOrganizeDetails = new List<mUser>();
            List<mUser> userFacultyDetails = new List<mUser>();
            List<mUser> userNonFacultyDetails = new List<mUser>();

            string UserRoles = "";
            int CurrentSelectedID = 0;
            int isOrganizedData = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.StoredProcedure;
                        cmdMySql.CommandText = "spUser_GetAllListbyUsername";
                        cmdMySql.Parameters.Clear();
                        cmdMySql.Parameters.AddWithValue("@username", Username);
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                CurrentSelectedID = Convert.ToInt32(sdr["ID"]);
                                UserRoles = sdr["UserRole"].ToString();
                                userDetails.Add(new mUser
                                {
                                    ID = Convert.ToInt32(sdr["ID"]),
                                    FirstName = sdr["FirstName"].ToString(),
                                    MiddleName = sdr["MiddleName"].ToString(),
                                    LastName = sdr["LastName"].ToString(),
                                    Username = sdr["Username"].ToString(),
                                    Password = ph.base64Decode(sdr["Password"].ToString()),
                                    Email = sdr["Email"].ToString(),
                                    ContactNumber = sdr["ContactNumber"].ToString(),
                                    Gender = Convert.ToInt32(sdr["Gender"]),
                                    GenderData = sdr["GenderData"].ToString(),
                                    Role = sdr["UserRole"].ToString(),
                                    CivilStatus = Convert.ToInt32(sdr["CivilStatus"].ToString()),
                                    CivilStatusData = sdr["CivilStatusData"].ToString(),
                                    IsActive = sdr["IsActive"].ToString() == "0" ? "No Active" : "Active"
                                });
                            }
                        }
                    }
                    conn.Close();
                    if (UserRoles == "Student" || UserRoles == "Faculty" || UserRoles == "NonFaculty")
                    {
                        if (UserRoles == "Student")
                        {
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = "spUser_GetAllListbyUsername_StudentInformation";
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {
                                        isOrganizedData = Convert.ToInt32(sdr["IsOrganization"]);
                                        userStudentDetails.Add(new mUser
                                        {
                                            StuCourse = Convert.ToInt32(sdr["Course"]),
                                            StuYearLevel = Convert.ToInt32(sdr["Year"]),
                                            StuSection = Convert.ToInt32(sdr["Section"]),
                                            StuDepartment = Convert.ToInt32(sdr["Department"]),
                                            StuCourseData = sdr["CourseData"].ToString(),
                                            StuYearLevelData = sdr["YearData"].ToString(),
                                            StuSectionData = sdr["SectionData"].ToString(),
                                            StuDepartmentData = sdr["DepartmentData"].ToString(),
                                            Organization = sdr["IsOrganization"].ToString()
                                        });
                                    }
                                }
                            }
                            conn.Close();
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = "spUser_GetAllListbyUsername_StudentScholarShipInformation";
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {

                                        userStudentScholarShipDetails.Add(new mUser
                                        {
                                            TES = sdr["TES"].ToString(),
                                            TDP = sdr["TDP"].ToString()
                                        });
                                    }
                                }
                            }
                            conn.Close();
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = "spUser_GetAllListbyUsername_StudentSocialStatusInformation";
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {

                                        userStudentSocialStatusDetails.Add(new mUser
                                        {
                                            PersonDisability = sdr["PersonDisability"].ToString(),
                                            WorkingStudent = sdr["WorkingStudent"].ToString(),
                                            NotApplicable = sdr["NotApplicable"].ToString()
                                        });
                                    }
                                }
                            }
                            conn.Close();

                            if (isOrganizedData == 1)
                            {
                                conn.Open();
                                using (SqlCommand cmdMySql = conn.CreateCommand())
                                {
                                    cmdMySql.CommandType = CommandType.StoredProcedure;
                                    cmdMySql.CommandText = "spUser_GetAllListbyUsername_StudentOrganizationInformation";
                                    cmdMySql.Parameters.Clear();
                                    cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                    using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                    {
                                        while (sdr.Read())
                                        {
                                            userStudentOrganizeDetails.Add(new mUser
                                            {
                                                ALLS = sdr["ALLS"].ToString(),
                                                APL = sdr["APL"].ToString(),
                                                ASGRD = sdr["ASGRD"].ToString(),
                                                CAITO = sdr["CAITO"].ToString(),
                                                CAPS = sdr["CAPS"].ToString(),
                                                CSS = sdr["CSS"].ToString(),
                                                DCP = sdr["DCP"].ToString(),
                                                ENC = sdr["ENC"].ToString(),
                                                EXCEL = sdr["EXCEL"].ToString(),
                                                FELTA = sdr["FELTA"].ToString(),
                                                IFIGHT = sdr["IFIGHT"].ToString(),
                                                ITS = sdr["ITS"].ToString(),
                                                JPIA = sdr["JPIA"].ToString(),
                                                LAGABATA = sdr["LAGABATA"].ToString(),
                                                LEAD = sdr["LEAD"].ToString(),
                                                MDAS = sdr["MDAS"].ToString(),
                                                MSS = sdr["MSS"].ToString(),
                                                ROTARACT = sdr["ROTARACT"].ToString(),
                                                SAVE = sdr["SAVE"].ToString(),
                                                SFJ = sdr["SFJ"].ToString(),
                                                SOCIOS = sdr["SOCIOS"].ToString(),
                                                PROTEGE = sdr["PROTEGE"].ToString(),
                                                UM3P = sdr["UM3P"].ToString()
                                            });
                                        }
                                    }
                                }
                                conn.Close();
                            }
                        }
                        else if (UserRoles == "Faculty")
                        {
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = "spUser_GetAllListbyUsername_FacultyInformation";
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {
                                        userFacultyDetails.Add(new mUser
                                        {
                                            FacDepartment = Convert.ToInt32(sdr["Department"]),
                                            FacPosition = Convert.ToInt32(sdr["Position"]),
                                            FacDepartmentData = sdr["DepartmentData"].ToString(),
                                            FacPositionData = sdr["PositionData"].ToString()
                                        });
                                    }
                                }
                            }
                            conn.Close();
                        }
                        else if (UserRoles == "NonFaculty")
                        {
                            conn.Open();
                            using (SqlCommand cmdMySql = conn.CreateCommand())
                            {
                                cmdMySql.CommandType = CommandType.StoredProcedure;
                                cmdMySql.CommandText = "spUser_GetAllListbyUsername_NonFacultyInformation";
                                cmdMySql.Parameters.Clear();
                                cmdMySql.Parameters.AddWithValue("@userID", CurrentSelectedID);
                                using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                                {
                                    while (sdr.Read())
                                    {
                                        userNonFacultyDetails.Add(new mUser
                                        {
                                            NonFacPosition = Convert.ToInt32(sdr["Position"]),
                                            NonFacPositionData = sdr["PositionData"].ToString(),

                                        });
                                    }
                                }
                            }
                            conn.Close();
                        }
                    }
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                success = true,
                data = new
                {
                    userData = userDetails,
                    userStudentDetails = userStudentDetails,
                    userStudentSocialStatusDetails = userStudentSocialStatusDetails,
                    userStudentScholarShipDetails = userStudentScholarShipDetails,
                    userStudentOrganizeDetails = userStudentOrganizeDetails,
                    userFacultyDetails = userFacultyDetails,
                    userNonFacultyDetails = userNonFacultyDetails,
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
