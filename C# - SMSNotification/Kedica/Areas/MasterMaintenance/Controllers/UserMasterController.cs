using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMSNofication.Areas.RestoreInformation.Models;

namespace SMSNotification.Areas.MasterMaintenance.Controllers
{
    public class UserMasterController : Controller
    {
        Security ph = new Security();
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";

        public ActionResult Index()
        {
            return View("UserMaster");
        }
        public ActionResult GetUserList()
        {
            List<mUser> data = new List<mUser>();
            DataTableHelper TypeHelper = new DataTableHelper();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
            string sortDirection = Request["order[0][dir]"];

            //string UserName = Request["UserNameData"].ToString();
            //string Role = Request["RoleData"].ToString();
            //string Gender = Request["GenderData"].ToString();
            //string CivilStatus = Request["CivilStatusData"].ToString();
            //string Status = Request["StatusData"].ToString();

            string SrchUsername = Request["SrchUsername"].ToString() == "" ? "" : "AND u.Username = '" + Request["SrchUsername"].ToString() +"' ";
            string SrchFirstName = Request["SrchFirstName"].ToString() == "" ? "" : "AND u.FirstName = '" + Request["SrchFirstName"].ToString() +"' ";
            string SrchLastName = Request["SrchLastName"].ToString() == "" ? "" : "AND u.LastName = '" + Request["SrchLastName"].ToString() +"' ";
            string SrchMiddleName = Request["SrchMiddleName"].ToString() == "" ? "" : "AND u.MiddleName = '" + Request["SrchMiddleName"].ToString() +"' ";
            string SrchContactNumber = Request["SrchContactNumber"].ToString() == "" ? "" : "AND u.ContactNumber = '" + Request["SrchContactNumber"].ToString() +"' ";
            string SrchEmail = Request["SrchEmail"].ToString() == "" ? "" : "AND u.Email = '" + Request["SrchEmail"].ToString() +"' ";
            string SrchGender = Request["SrchGender"].ToString() == "" ? "" : "AND u.Gender = '" + Request["SrchGender"].ToString() +"' ";
            string SrchCivilStatus = Request["SrchCivilStatus"].ToString() == "" ? "" : "AND u.CivilStatus = '" + Request["SrchCivilStatus"].ToString() +"' ";
            string SrchRole = Request["SrchRole"].ToString() == "" ? "" : "AND u.UserRole = '" + Request["SrchRole"].ToString() +"' ";
            string SrchStuDepartment = Request["SrchStuDepartment"].ToString() == "" ? "" : "AND si.Department = '" + Request["SrchStuDepartment"].ToString() +"' ";
            string SrchStuCourse = Request["SrchStuCourse"].ToString() == "" ? "" : "AND si.Course = '" + Request["SrchStuCourse"].ToString() +"' ";
            string SrchStuSection = Request["SrchStuSection"].ToString() == "" ? "" : "AND si.Section = '" + Request["SrchStuSection"].ToString() +"' ";
            string SrchStuYearLevel = Request["SrchStuYearLevel"].ToString() == "" ? "" : "AND si.Year = '" + Request["SrchStuYearLevel"].ToString() +"' ";
            string SrchTES = Request["SrchTES"].ToString() == "0" ? "" : "AND ss.TES = '" + Request["SrchTES"].ToString() +"' ";
            string SrchTDP = Request["SrchTDP"].ToString() == "0" ? "" : "AND ss.TDP = '" + Request["SrchTDP"].ToString() +"' ";
            string SrchPersonDisability = Request["SrchPersonDisability"].ToString() == "0" ? "" : "AND st.PersonDisability = '" + Request["SrchPersonDisability"].ToString() +"' ";
            string SrchWorkingStudent = Request["SrchWorkingStudent"].ToString() == "0" ? "" : "AND st.WorkingStudent = '" + Request["SrchWorkingStudent"].ToString() +"' ";
            //string SrchNotApplicable = Request["SrchNotApplicable"].ToString() == "0" ? "" : "AND st.NotApplicable = '" + Request["SrchNotApplicable"].ToString() +"' ";
            string SrchOrganization = Request["SrchOrganization"].ToString() == "" ? "" : "AND si.IsOrganization = '" + Request["SrchOrganization"].ToString() +"' ";
            string SrchALLS = Request["SrchALLS"].ToString() == "0" ? "" : "AND so.ALLS = '" + Request["SrchALLS"].ToString() +"' ";
            string SrchAPL = Request["SrchAPL"].ToString() == "0" ? "" : "AND so.APL = '" + Request["SrchAPL"].ToString() +"' ";
            string SrchASGRD = Request["SrchASGRD"].ToString() == "0" ? "" : "AND so.ASGRD = '" + Request["SrchASGRD"].ToString() +"' ";
            string SrchCAITO = Request["SrchCAITO"].ToString() == "0" ? "" : "AND so.CAITO = '" + Request["SrchCAITO"].ToString() +"' ";
            string SrchCAPS = Request["SrchCAPS"].ToString() == "0" ? "" : "AND so.CAPS = '" + Request["SrchCAPS"].ToString() +"' ";
            string SrchCSS = Request["SrchCSS"].ToString() == "0" ? "" : "AND so.CSS = '" + Request["SrchCSS"].ToString() +"' ";
            string SrchDCP = Request["SrchDCP"].ToString() == "0" ? "" : "AND so.DCP = '" + Request["SrchDCP"].ToString() +"' ";
            string SrchENC = Request["SrchENC"].ToString() == "0" ? "" : "AND so.ENC = '" + Request["SrchENC"].ToString() +"' ";
            string SrchEXCEL = Request["SrchEXCEL"].ToString() == "0" ? "" : "AND so.EXCEL = '" + Request["SrchEXCEL"].ToString() +"' ";
            string SrchFELTA = Request["SrchFELTA"].ToString() == "0" ? "" : "AND so.FELTA = '" + Request["SrchFELTA"].ToString() +"' ";
            string SrchIFIGHT = Request["SrchIFIGHT"].ToString() == "0" ? "" : "AND so.IFIGHT = '" + Request["SrchIFIGHT"].ToString() +"' ";
            string SrchITS = Request["SrchITS"].ToString() == "0" ? "" : "AND so.ITS = '" + Request["SrchITS"].ToString() +"' ";
            string SrchJPIA = Request["SrchJPIA"].ToString() == "0" ? "" : "AND so.JPIA = '" + Request["SrchJPIA"].ToString() +"' ";
            string SrchLAGABATA = Request["SrchLAGABATA"].ToString() == "0" ? "" : "AND so.LAGABATA = '" + Request["SrchLAGABATA"].ToString() +"' ";
            string SrchLEAD = Request["SrchLEAD"].ToString() == "0" ? "" : "AND so.LEAD = '" + Request["SrchLEAD"].ToString() +"' ";
            string SrchMDAS = Request["SrchMDAS"].ToString() == "0" ? "" : "AND so.MDAS = '" + Request["SrchMDAS"].ToString() +"' ";
            string SrchMSS = Request["SrchMSS"].ToString() == "0" ? "" : "AND so.MSS = '" + Request["SrchMSS"].ToString() +"' ";
            string SrchROTARACT = Request["SrchROTARACT"].ToString() == "0" ? "" : "AND so.ROTARACT = '" + Request["SrchROTARACT"].ToString() +"' ";
            string SrchSAVE = Request["SrchSAVE"].ToString() == "0" ? "" : "AND so.[SAVE] = '" + Request["SrchSAVE"].ToString() +"' ";
            string SrchSFJ = Request["SrchSFJ"].ToString() == "0" ? "" : "AND so.SFJ = '" + Request["SrchSFJ"].ToString() +"' ";
            string SrchSOCIOS = Request["SrchSOCIOS"].ToString() == "0" ? "" : "AND so.SOCIOS = '" + Request["SrchSOCIOS"].ToString() +"' ";
            string SrchPROTEGE = Request["SrchPROTEGE"].ToString() == "0" ? "" : "AND so.PROTEGE = '" + Request["SrchPROTEGE"].ToString() +"' ";
            string SrchUM3P = Request["SrchUM3P"].ToString() == "0" ? "" : "AND so.UM3P = '" + Request["SrchUM3P"].ToString() +"' ";
            string SrchFacDepartment = Request["SrchFacDepartment"].ToString() == "" ? "" : "AND fi.Department = '" + Request["SrchFacDepartment"].ToString() +"' ";
            string SrchFacPosition = Request["SrchFacPosition"].ToString() == "" ? "" : "AND fi.Position = '" + Request["SrchFacPosition"].ToString() +"' ";
            string searcQuery = SrchUsername + SrchFirstName + SrchLastName + SrchMiddleName + SrchContactNumber + SrchEmail + SrchGender +
                SrchCivilStatus + SrchRole + SrchStuDepartment + SrchStuCourse + SrchStuSection + SrchStuYearLevel + SrchTES + SrchTDP +
                SrchPersonDisability + SrchWorkingStudent + /*SrchNotApplicable +*/ SrchOrganization + SrchALLS + SrchAPL + SrchASGRD + SrchCAITO +
                SrchCAPS + SrchCSS + SrchDCP + SrchENC + SrchEXCEL + SrchFELTA + SrchIFIGHT + SrchITS + SrchJPIA + SrchLAGABATA + SrchLEAD +
                SrchMDAS + SrchMSS + SrchROTARACT + SrchSAVE + SrchSFJ + SrchSOCIOS + SrchPROTEGE + SrchUM3P + SrchFacDepartment + SrchFacPosition;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spUser_GetAllList";
                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@SearchItems", searcQuery);

                        cmdSql.ExecuteNonQuery();
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                data.Add(new mUser
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
                                    IsActive = sdr["IsActive"].ToString() 
                                });
                            }

                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            int totalrows = data.Count;
            if (!string.IsNullOrEmpty(searchValue))//filter
                data = data.Where(x =>
                                    x.Username.ToLower().Contains(searchValue.ToLower()) ||
                                    x.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.LastName.ToLower().Contains(searchValue.ToLower())
                                 ).ToList<mUser>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
            data = data.Skip(start).Take(length).ToList<mUser>();

            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSelect2Data()
        {
            ArrayList results = new ArrayList();
            string val = Request.QueryString["q"];
            string id = Request.QueryString["id"];
            string text = Request.QueryString["text"];
            string table = Request.QueryString["table"];
            string db = Request.QueryString["db"];
            string condition = Request.QueryString["condition"] == null ? "" : Request.QueryString["condition"];
            string isDistict = Request.QueryString["isDistict"] == null ? "" : Request.QueryString["isDistict"];
            string display = Request.QueryString["display"];
            string addOptionVal = Request.QueryString["addOptionVal"];
            string addOptionText = Request.QueryString["addOptionText"];

            if (addOptionVal != null && display == "id&text")
                results.Add(new { id = addOptionVal, text = addOptionText });

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[db].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.Text;
                        if (isDistict != "")
                        {
                            cmdMySql.CommandText = "SELECT DISTINCT(" + id + ")," + text + " FROM [vUsers]";
                            using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    var desc = sdr[text].ToString();
                                    var isDateTime = desc.Contains("12:00:00 AM");
                                    if (isDateTime)
                                    {
                                        DateTime aDate = DateTime.Parse(desc);
                                        desc = aDate.ToString("MM/dd/yyyy");
                                    }

                                    if (display == "id&text")
                                        results.Add(new { id = sdr[id].ToString(), text = desc });

                                    if (display == "id&id-text")
                                        results.Add(new { id = sdr[id].ToString(), text = sdr[id].ToString() + "-" + desc });
                                }

                            }
                        }
                        else
                        {
                            cmdMySql.CommandText = "SELECT " + id + "," + text + " FROM [" + table + "] WHERE IsDeleted=0 " + condition + " AND ( " + id + " like '%" + val + "%' OR " + text + " like '%" + val + "%')";
                            using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                            {
                                while (sdr.Read())
                                {
                                    if (display == "id&text")
                                        results.Add(new { id = sdr[id].ToString(), text = sdr[text].ToString() });
                                    if (display == "id&id-text")
                                        results.Add(new { id = sdr[id].ToString(), text = sdr[id].ToString() + "-" + sdr[text].ToString() });
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, msg = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { results }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ValidateEmailAddress(string EmailAddress)
        {
            bool isValid = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.Text;
                        cmdMySql.CommandText = "SELECT UserName FROM mUser WHERE Email= '" + EmailAddress + "'  ";
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            if (sdr.HasRows)
                                isValid = false;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString(); ;
                error = true;
            }
            if (error)
                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new { success = true, data = new { isValid = isValid } });
            }
        }
        public ActionResult ValidateUsername(string Username)
        {
            bool isValid = true;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.Text;
                        cmdMySql.CommandText = "SELECT UserName FROM mUser WHERE IsDeleted=0 AND UserName='" + Username + "'";
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            if (sdr.HasRows)
                                isValid = false;
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString(); ;
                error = true;
            }
            if (error)
                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            else
            {
                return Json(new { success = true, data = new { isValid = isValid } });
            }
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
                    else {
                        ReadAndWrite = 0;
                        CanDelete = 0;
                    }
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ToString()))
                    {

                        conn.Open();
                        using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                        {
                            cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                            cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                            cmdSqlAudit.Parameters.Clear();
                            cmdSqlAudit.Parameters.AddWithValue("@UserID", User.ID);
                            cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                            if (User.ID == 0)
                            {
                                cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Insert User: " + User.Email);
                            }
                            else {
                                cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Update User: " + User.Email);
                            }
                            
                            cmdSqlAudit.ExecuteNonQuery();
                        }
                        conn.Close();

                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            if (User.Role == "Student" || User.Role == "Faculty" || User.Role == "NonFaculty")
                            {
                                User.Password = User.Password == null ? "": User.Password.ToString();
                            }
                            else {
                                User.Password = ph.base64Encode(User.Password).ToString();
                            }

                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_InsertUpdate";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@ID", User.ID);
                            //cmdMySql.Parameters.AddWithValue("@Username", User.Username);
                            cmdMySql.Parameters.AddWithValue("@Password", User.Password);
                            cmdMySql.Parameters.AddWithValue("@FirstName", User.FirstName);
                            cmdMySql.Parameters.AddWithValue("@LastName", User.LastName);
                            cmdMySql.Parameters.AddWithValue("@MiddleName", User.MiddleName == null ? "" : User.MiddleName);
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
                                    cmdMySql.Parameters.AddWithValue("@TES",   User.TES == null ? 0 : User.TES == "on" ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@TDP",  User.TDP == null ? 0 : User.TDP == "on" ? 1 : 0);
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
                                    cmdMySql.Parameters.AddWithValue("@NotApplicable",  User.NotApplicable == null ? 0 : User.NotApplicable == "on" ? 1 : 0);
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
                                        cmdMySql.Parameters.AddWithValue("@ASGRD",   User.ASGRD == null ? 0 : User.ASGRD == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CAITO",   User.CAITO == null ? 0 : User.CAITO == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CAPS",    User.CAPS == null ? 0 : User.CAPS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@CSS",    User.CSS == null ? 0 : User.CSS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@DCP",  User.DCP == null ? 0 : User.DCP == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ENC",  User.ENC == null ? 0 : User.ENC == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@EXCEL",   User.EXCEL == null ? 0 : User.EXCEL == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@FELTA",   User.FELTA == null ? 0 : User.FELTA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@IFIGHT", User.IFIGHT == null ? 0 : User.IFIGHT == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ITS",  User.ITS == null ? 0 : User.ITS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@JPIA",   User.JPIA == null ? 0 : User.JPIA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@LAGABATA",    User.LAGABATA == null ? 0 : User.LAGABATA == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@LEAD",    User.LEAD == null ? 0 : User.LEAD == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@MDAS",   User.MDAS == null ? 0 : User.MDAS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@MSS",    User.MSS == null ? 0 : User.MSS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@ROTARACT",    User.ROTARACT == null ? 0 : User.ROTARACT == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SAVE",    User.SAVE == null ? 0 : User.SAVE == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SFJ",    User.SFJ == null ? 0 : User.SFJ == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@SOCIOS",    User.SOCIOS == null ? 0 : User.SOCIOS == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@PROTEGE",    User.PROTEGE == null ? 0 : User.PROTEGE == "on" ? 1 : 0);
                                        cmdMySql.Parameters.AddWithValue("@UM3P",     User.UM3P == null ? 0 : User.UM3P == "on" ? 1 : 0);
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
                                    CivilStatus = Convert.ToInt32( sdr["CivilStatus"].ToString()),
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
            return Json(new { success = true,
                    data = new {
                            userData = userDetails ,
                            userStudentDetails = userStudentDetails,
                            userStudentSocialStatusDetails = userStudentSocialStatusDetails,
                            userStudentScholarShipDetails = userStudentScholarShipDetails,
                            userStudentOrganizeDetails = userStudentOrganizeDetails,
                            userFacultyDetails = userFacultyDetails,
                            userNonFacultyDetails = userNonFacultyDetails,
                    } }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUser(List<restoreUser> UserListID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {

                    conn.Open();
                    using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                    {
                        cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                        cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                        cmdSqlAudit.Parameters.Clear();
                        cmdSqlAudit.Parameters.AddWithValue("@UserID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Delete " + UserListID.Count + " users");

                        cmdSqlAudit.ExecuteNonQuery();
                    }
                    conn.Close();

                    for (int i = 0; i < UserListID.Count; i++)
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_Delete";

                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@UserID", UserListID[i].UserID);
                            cmdMySql.Parameters.AddWithValue("@UpdateID", Session["Username"]);
                            cmdMySql.ExecuteNonQuery();

                        }
                        conn.Close();
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
            return Json(new { success = true, msg = "User was successfully deleted." });

        }
        public ActionResult GetUserAccess(int ID)
        {

            ArrayList userMenuList = new ArrayList();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))

                {


                   
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.StoredProcedure;
                        cmdMySql.CommandText = "spUserPageAccess_GetAccessList";
                        cmdMySql.Parameters.Clear();
                        cmdMySql.Parameters.AddWithValue("@ID", ID);
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                //var userid = Session["UserID"].ToString();
                                userMenuList.Add(new
                                {
                                    ID = Convert.ToInt32(sdr["ID"]),
                                    GroupLabel = sdr["GroupLabel"].ToString(),
                                    PageName = sdr["PageName"].ToString(),
                                    PageLabel = sdr["PageLabel"].ToString(),
                                    URL = sdr["URL"].ToString(),
                                    HasSub = Convert.ToInt32(sdr["HasSub"]),
                                    ParentMenu = sdr["ParentMenu"].ToString(),
                                    ParentOrder = Convert.ToInt32(sdr["ParentOrder"]),
                                    Order = Convert.ToInt32(sdr["Order"]),
                                    Icon = sdr["Icon"].ToString(),
                                    Status = sdr["Status"].ToString() == "" ? 0 : Convert.ToInt32(sdr["Status"]),
                                    ReadAndWrite = sdr["ReadAndWrite"].ToString() == "" ? 0 : Convert.ToInt32(sdr["ReadAndWrite"]),
                                    Delete = sdr["Delete"].ToString() == "" ? 0 : Convert.ToInt32(sdr["Delete"])
                                });
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "An error occured: " + err.InnerException.ToString();
                else
                    errmsg = "An error occured: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = userMenuList }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SaveUserAccess(List<mUserPageAccess> userAccessList, string Role, int UserID)
        {
            string endMsg = " saved.";
            ModelState.Remove("ID");
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ToString()))
                    {

                        conn.Open();
                        using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                        {
                            cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                            cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                            cmdSqlAudit.Parameters.Clear();
                            cmdSqlAudit.Parameters.AddWithValue("@UserID", Session["ID"]);
                            cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                            cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Set web access");

                            cmdSqlAudit.ExecuteNonQuery();
                        }
                        conn.Close();

                        conn.Open();
                        SqlTransaction transaction;
                        transaction = conn.BeginTransaction();
                        try
                        {
                            foreach (mUserPageAccess userAccess in userAccessList)
                            {
                                using (SqlCommand cmdMySql = conn.CreateCommand())
                                {
                                    cmdMySql.Connection = conn;
                                    cmdMySql.Transaction = transaction;
                                    cmdMySql.CommandType = CommandType.StoredProcedure;
                                    cmdMySql.CommandText = "spUserPageAccess_INSERT_UPDATE";

                                    cmdMySql.Parameters.Clear();
                                    cmdMySql.Parameters.AddWithValue("@UserID", Convert.ToInt32(userAccess.UserID));
                                    cmdMySql.Parameters.AddWithValue("@PageID", userAccess.PageID);
                                    cmdMySql.Parameters.AddWithValue("@Status", userAccess.Status == true ? 1 : 0);
                                    cmdMySql.Parameters.AddWithValue("@ReadAndWrite", userAccess.ReadAndWrite);
                                    cmdMySql.Parameters.AddWithValue("@Delete", userAccess.Delete);
                                    cmdMySql.Parameters.AddWithValue("@CreateID", Session["ID"].ToString());
                                    SqlParameter ErrorMessage = cmdMySql.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 200);
                                    SqlParameter Error = cmdMySql.Parameters.Add("@IsError", SqlDbType.Bit);

                                    Error.Direction = ParameterDirection.Output;
                                    ErrorMessage.Direction = ParameterDirection.Output;

                                    cmdMySql.ExecuteNonQuery();

                                    error = Convert.ToBoolean(Error.Value);
                                    if (error)
                                    {
                                        modelErrors.Add(ErrorMessage.Value.ToString());
                                        //throw new System.InvalidOperationException(ErrorMessage.Value.ToString());
                                    }
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception err)
                        {
                            if (err.InnerException != null)
                                modelErrors.Add("An error occured: " + err.InnerException.ToString());
                            else
                                modelErrors.Add("An error occured: " + err.Message.ToString());
                            error = true;
                        }

                        conn.Close();
                    }
                }
                catch (Exception err)
                {
                    if (err.InnerException != null)
                        modelErrors.Add("An error occured: " + err.InnerException.ToString());
                    else
                        modelErrors.Add("An error occured: " + err.Message.ToString());
                    error = true;
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
                return Json(new { success = true, msg = "User Page Access was successfully " + endMsg });
        }
        public ActionResult GetOtherUserInformation(int SelectedID, string UserRole)
        {
            List<mUser> userStudentDetails = new List<mUser>();
            List<mUser> userStudentSocialStatusDetails = new List<mUser>();
            List<mUser> userStudentScholarShipDetails = new List<mUser>();
            List<mUser> userStudentOrganizeDetails = new List<mUser>();
            List<mUser> userFacultyDetails = new List<mUser>();
            List<mUser> userNonFacultyDetails = new List<mUser>();
            string UserRoles = "";
            int isOrganizedData = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    if (UserRole == "student")
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_GetAllListbyUsername_StudentInformation";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
                            cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
                            cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
                                cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
                    else if (UserRole == "faculty")
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_GetAllListbyUsername_FacultyInformation";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
                    else if (UserRole == "nonfaculty")
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_GetAllListbyUsername_NonFacultyInformation";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@userID", SelectedID);
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
            catch (Exception err)
            {
                string errmsg;
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();

                return Json(new { success = false, errors = errmsg }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true, data = new {
                userStudentDetails = userStudentDetails,
                userStudentSocialStatusDetails = userStudentSocialStatusDetails,
                userStudentScholarShipDetails = userStudentScholarShipDetails,
                userStudentOrganizeDetails = userStudentOrganizeDetails,
                userFacultyDetails = userFacultyDetails,
                userNonFacultyDetails = userNonFacultyDetails,
            } }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateStatus(string Username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                    {
                        cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                        cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                        cmdSqlAudit.Parameters.Clear();
                        cmdSqlAudit.Parameters.AddWithValue("@UserID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Activate Users");

                        cmdSqlAudit.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.StoredProcedure;
                        cmdMySql.CommandText = "spUser_UpdateStatus";

                        cmdMySql.Parameters.Clear();
                        cmdMySql.Parameters.AddWithValue("@Username", Username);
                        cmdMySql.Parameters.AddWithValue("@UpdateID", Session["Username"]);
                        cmdMySql.ExecuteNonQuery();

                    }
                    conn.Close();
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
            return Json(new { success = true, msg = "User was successfully activate." });

        }
        public ActionResult ActivateAllUsers()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                    {
                        cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                        cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                        cmdSqlAudit.Parameters.Clear();
                        cmdSqlAudit.Parameters.AddWithValue("@UserID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                        cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Activate Users");

                        cmdSqlAudit.ExecuteNonQuery();
                    }
                    conn.Close();
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.StoredProcedure;
                        cmdMySql.CommandText = "spUser_ActivateAllUsers";
                        cmdMySql.Parameters.Clear();
                        cmdMySql.Parameters.AddWithValue("@UpdateID", Session["Username"]);
                        cmdMySql.ExecuteNonQuery();

                    }
                    conn.Close();
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
            return Json(new { success = true, msg = "All User was successfully activate." });

        }
    }
}
