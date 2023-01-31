using SMSNotification.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using SMSNotification.Areas.MasterMaintenance.Models;
using System.Net.Mail;
using System.Net;

namespace SMSNotification.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        
        Security ph = new Security();
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        string EmailUser = "sms2022.noreply@gmail.com";
        string EmailPass = "SMSAnnouncementSystem2022";
        // GET: Login
        public ActionResult Index()
        {
            if (Session["ID"] != null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                return View("Login");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginEntry(Login data)
        {
            try
            {
                Security ph = new Security();
                int ID = 0;
                string Username = "";
                string Name = "";
                string username = data.Username;
                string Email = data.Email;
                string password = ph.base64Encode(data.Password.ToString());
                string UserRole = "";

                DataHelper dataHelper = new DataHelper();

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spUser_Login";

                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@_Email", username);
                        //cmdSql.Parameters.AddWithValue("@_Username", username);
                        cmdSql.Parameters.AddWithValue("@_Password", password);

                        using (SqlDataReader rdSql = cmdSql.ExecuteReader())
                        {
                            if (rdSql.Read())
                            {
                                ID = Convert.ToInt32(rdSql["ID"]);
                                Username = rdSql["Email"].ToString();
                                Name = rdSql["FirstName"].ToString() + " " + rdSql["LastName"].ToString();
                                UserRole = rdSql["UserRole"].ToString();

                                Session["ID"] = ID;
                                Session["Username"] = Username;
                                Session["Name"] = Name;

                                string UseRoleSelected = "";

                                if (UserRole == "Administrator")
                                {
                                    UseRoleSelected = "Administrator";
                                }
                                else if (UserRole == "HeadAdministrator")
                                {
                                    UseRoleSelected = "Head Administrator";
                                }
                                else if (UserRole == "Faculty")
                                {
                                    UseRoleSelected = "Faculty Staff";
                                }
                                else if (UserRole == "NonFaculty")
                                {
                                    UseRoleSelected = "Non-Faculty Staff";
                                }
                                else if (UserRole == "Student")
                                {
                                    UseRoleSelected = "Student";
                                }
                                Session["UserRole"] = UseRoleSelected;

                                rdSql.Close();
                                using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                                {
                                    cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                                    cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                                    cmdSqlAudit.Parameters.Clear();
                                    cmdSqlAudit.Parameters.AddWithValue("@UserID", ID);
                                    cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                                    cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Login");
                                    cmdSqlAudit.ExecuteNonQuery();
                                }
                                
                            }
                            else
                            {
                                errmsg = "Invalid Email Address or Not Active";
                            }
                        }
                    }
                    conn.Close();
                    
                }
            }
            catch (Exception err)
            {
                if (err.InnerException != null)
                    errmsg = "Error: " + err.InnerException.ToString();
                else
                    errmsg = "Error: " + err.Message.ToString();
            }
            if ( errmsg != "")
                return Json(new { success = true, data = new { error = true, errmsg = errmsg } });
            else
            {
                return Json(new { success = true, data = new { error = false } });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout(int ID)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
            {
                conn.Open();
                using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                {
                    cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                    cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                    cmdSqlAudit.Parameters.Clear();
                    cmdSqlAudit.Parameters.AddWithValue("@UserID", ID);
                    cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                    cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Logout");
                    cmdSqlAudit.ExecuteNonQuery();
                }
                conn.Close();
            }
      
            Session.Abandon();
            return RedirectToAction("Index", "Login", new { area = "" });
        }

        public ActionResult SessionError()
        {
            return Json(new { success = false, type = "Login", errors = "Session has expired. Please login again. Thank you." }, JsonRequestBehavior.AllowGet);
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
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_InsertUpdate";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@ID", User.ID);
                            //cmdMySql.Parameters.AddWithValue("@Username", User.Username);
                            cmdMySql.Parameters.AddWithValue("@Password", "");
                            cmdMySql.Parameters.AddWithValue("@FirstName", User.FirstName);
                            cmdMySql.Parameters.AddWithValue("@LastName", User.LastName);
                            cmdMySql.Parameters.AddWithValue("@MiddleName", User.MiddleName == null ? "" : User.MiddleName);
                            cmdMySql.Parameters.AddWithValue("@Gender", User.Gender);
                            cmdMySql.Parameters.AddWithValue("@Email", User.Email);
                            cmdMySql.Parameters.AddWithValue("@ContactNumber", User.ContactNumber);
                            cmdMySql.Parameters.AddWithValue("@CivilStatus", User.CivilStatus);
                            cmdMySql.Parameters.AddWithValue("@Role", User.Role);
                            cmdMySql.Parameters.AddWithValue("@ReadAndWrite", ReadAndWrite);
                            cmdMySql.Parameters.AddWithValue("@CreateID", "Administration");
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
                                cmdMySql.Parameters.AddWithValue("@CreateID", "Administration");
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
                                    cmdMySql.Parameters.AddWithValue("@CreateID", "Administration");
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
                                    cmdMySql.Parameters.AddWithValue("@CreateID", "Administration");
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
                                        cmdMySql.Parameters.AddWithValue("@CreateID", "Administration");
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
                return Json(new { success = true, msg = "User was successfully registered" });
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
        public ActionResult SendEmailCode(string EmaiLData, string FirstName)
        {
            string GetNewCode = GetRandomData().ToString();
            try
            {
                
                SmtpClient SMTPSendEmail = new SmtpClient();
                SMTPSendEmail.Host = "smtp.office365.com";
                SMTPSendEmail.Port = 587;
                SMTPSendEmail.EnableSsl = true;
                SMTPSendEmail.UseDefaultCredentials = true;
                SMTPSendEmail.Credentials = new NetworkCredential(EmailUser, EmailPass);



                string emailContent = String.Format(
               @"
                    <p>Hello {1},</p>
                    <p>To complete your registration, please verify your school email account by entering the verification code.</p>
                    {0}
                    <br /> <br />
                    Sincerely,<br />
                    MIS Department
                ", GetNewCode, FirstName);


                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(EmailUser);
                mail.To.Add(EmaiLData);
                mail.CC.Add("dummyjomar69@gmail.com");
                mail.Subject = "Email Verification Code";
                mail.Body = emailContent;
                mail.IsBodyHtml = true;
                SMTPSendEmail.Send(mail);
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
                return Json(new { success = true, data = new { CurrentCode = GetNewCode } });
            }
        }
        private int GetRandomData()
        {
            Random rnd = new Random();
            int num = rnd.Next(000001, 999999);
            return num;
        }
    }
}
