using SMSNofication.Api;
using SMSNofication.Areas.Transaction.Models;
using SMSNofication.Models;
using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SMSNofication.Areas.Transaction.Controllers
{
    public class RequestNotificationController : Controller
    {
        private IClient m_currentApi;
        private IResponse m_lastResponse;
        Security ph = new Security();
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        string EmailUser = "sms2022.noreply@gmail.com";
        string EmailPass = "SMSAnnouncementSystem2022";
        // GET: Transaction/RequestNotification
        public ActionResult Index()
        {
            return View("RequestNotification");
        }
        public ActionResult SendAnnouncement(RequestNotificationData formListData)
        {
            List<mUser> userListdata = new List<mUser>();
            List<SendAnnouncement> sendAnnouncementList = new List<SendAnnouncement>();
            string SrchUsername = formListData.SrchUsername == null || formListData.SrchUsername == "" ? "" : "AND u.Username = '" + formListData.SrchUsername.ToString() + "' ";
            string SrchFirstName = formListData.SrchFirstName == null || formListData.SrchFirstName == "" ? "" : "AND u.FirstName = '" + formListData.SrchFirstName.ToString() + "' ";
            string SrchLastName = formListData.SrchLastName == null || formListData.SrchLastName == "" ? "" : "AND u.LastName = '" + formListData.SrchLastName.ToString() + "' ";
            string SrchMiddleName = formListData.SrchMiddleName == null || formListData.SrchMiddleName == "" ? "" : "AND u.MiddleName = '" + formListData.SrchMiddleName.ToString() + "' ";
            string SrchContactNumber = formListData.SrchContactNumber == null || formListData.SrchContactNumber == "" ? "" : "AND u.ContactNumber = '" + formListData.SrchContactNumber.ToString() + "' ";
            string SrchEmail = formListData.SrchEmail == null || formListData.SrchEmail == "" ? "" : "AND u.Email = '" + formListData.SrchEmail.ToString() + "' ";
            string SrchGender = formListData.SrchGender == null || formListData.SrchGender == "" ? "" : "AND u.Gender = '" + formListData.SrchGender.ToString() + "' ";
            string SrchCivilStatus = formListData.SrchCivilStatus == null || formListData.SrchCivilStatus == "" ? "" : "AND u.CivilStatus = '" + formListData.SrchCivilStatus.ToString() + "' ";
            string SrchRole = formListData.SrchRole == null || formListData.SrchRole == "" ? "" : "AND u.UserRole = '" + formListData.SrchRole.ToString() + "' ";
            string SrchStuDepartment = formListData.SrchStuDepartment == null || formListData.SrchStuDepartment == "" ? "" : "AND si.Department = '" + formListData.SrchStuDepartment.ToString() + "' ";
            string SrchStuCourse = formListData.SrchStuCourse == null || formListData.SrchStuCourse == "" ? "" : "AND si.Course = '" + formListData.SrchStuCourse.ToString() + "' ";
            string SrchStuSection = formListData.SrchStuSection == null || formListData.SrchStuSection == "" ? "" : "AND si.Section = '" + formListData.SrchStuSection.ToString() + "' ";
            string SrchStuYearLevel = formListData.SrchStuYearLevel == null || formListData.SrchStuYearLevel == "" ? "" : "AND si.Year = '" + formListData.SrchStuYearLevel.ToString() + "' ";
            string SrchTES = formListData.SrchTES == null || formListData.SrchTES == "0" ? "" : "AND ss.TES = '" + formListData.SrchTES.ToString() + "' ";
            string SrchTDP = formListData.SrchTDP == null || formListData.SrchTDP == "0" ? "" : "AND ss.TDP = '" + formListData.SrchTDP.ToString() + "' ";
            string SrchPersonDisability = formListData.SrchPersonDisability == null || formListData.SrchPersonDisability == "0" ? "" : "AND st.PersonDisability = '" + formListData.SrchPersonDisability.ToString() + "' ";
            string SrchWorkingStudent = formListData.SrchWorkingStudent == null || formListData.SrchWorkingStudent == "0" ? "" : "AND st.WorkingStudent = '" + formListData.SrchWorkingStudent.ToString() + "' ";
            string SrchNotApplicable = formListData.SrchNotApplicable == null || formListData.SrchNotApplicable == "0" ? "" : "AND st.NotApplicable = '" + formListData.SrchNotApplicable.ToString() + "' ";
            string SrchOrganization = formListData.SrchOrganization == null || formListData.SrchOrganization == "" ? "" : "AND si.IsOrganization = '" + formListData.SrchOrganization.ToString() + "' ";
            string SrchALLS = formListData.SrchALLS == null || formListData.SrchALLS == "0" ? "" : "AND so.ALLS = '" + formListData.SrchALLS.ToString() + "' ";
            string SrchAPL = formListData.SrchAPL == null || formListData.SrchAPL == "0" ? "" : "AND so.APL = '" + formListData.SrchAPL.ToString() + "' ";
            string SrchASGRD = formListData.SrchASGRD == null || formListData.SrchASGRD == "0" ? "" : "AND so.ASGRD = '" + formListData.SrchASGRD.ToString() + "' ";
            string SrchCAITO = formListData.SrchCAITO == null || formListData.SrchCAITO == "0" ? "" : "AND so.CAITO = '" + formListData.SrchCAITO.ToString() + "' ";
            string SrchCAPS = formListData.SrchCAPS == null || formListData.SrchCAPS == "0" ? "" : "AND so.CAPS = '" + formListData.SrchCAPS.ToString() + "' ";
            string SrchCSS = formListData.SrchCSS == null || formListData.SrchCSS == "0" ? "" : "AND so.CSS = '" + formListData.SrchCSS.ToString() + "' ";
            string SrchDCP = formListData.SrchDCP == null || formListData.SrchDCP == "0" ? "" : "AND so.DCP = '" + formListData.SrchDCP.ToString() + "' ";
            string SrchENC = formListData.SrchENC == null || formListData.SrchENC == "0" ? "" : "AND so.ENC = '" + formListData.SrchENC.ToString() + "' ";
            string SrchEXCEL = formListData.SrchEXCEL == null || formListData.SrchEXCEL == "0" ? "" : "AND so.EXCEL = '" + formListData.SrchEXCEL.ToString() + "' ";
            string SrchFELTA = formListData.SrchFELTA == null || formListData.SrchFELTA == "0" ? "" : "AND so.FELTA = '" + formListData.SrchFELTA.ToString() + "' ";
            string SrchIFIGHT = formListData.SrchIFIGHT == null || formListData.SrchIFIGHT == "0" ? "" : "AND so.IFIGHT = '" + formListData.SrchIFIGHT.ToString() + "' ";
            string SrchITS = formListData.SrchITS == null || formListData.SrchITS == "0" ? "" : "AND so.ITS = '" + formListData.SrchITS.ToString() + "' ";
            string SrchJPIA = formListData.SrchJPIA == null || formListData.SrchJPIA == "0" ? "" : "AND so.JPIA = '" + formListData.SrchJPIA.ToString() + "' ";
            string SrchLAGABATA = formListData.SrchLAGABATA == null || formListData.SrchLAGABATA == "0" ? "" : "AND so.LAGABATA = '" + formListData.SrchLAGABATA.ToString() + "' ";
            string SrchLEAD = formListData.SrchLEAD == null || formListData.SrchLEAD == "0" ? "" : "AND so.LEAD = '" + formListData.SrchLEAD.ToString() + "' ";
            string SrchMDAS = formListData.SrchMDAS == null || formListData.SrchMDAS == "0" ? "" : "AND so.MDAS = '" + formListData.SrchMDAS.ToString() + "' ";
            string SrchMSS = formListData.SrchMSS == null || formListData.SrchMSS == "0" ? "" : "AND so.MSS = '" + formListData.SrchMSS.ToString() + "' ";
            string SrchROTARACT = formListData.SrchROTARACT == null || formListData.SrchROTARACT == "0" ? "" : "AND so.ROTARACT = '" + formListData.SrchROTARACT.ToString() + "' ";
            string SrchSAVE = formListData.SrchSAVE == null || formListData.SrchSAVE == "0" ? "" : "AND so.[SAVE] = '" + formListData.SrchSAVE.ToString() + "' ";
            string SrchSFJ = formListData.SrchSFJ == null || formListData.SrchSFJ == "0" ? "" : "AND so.SFJ = '" + formListData.SrchSFJ.ToString() + "' ";
            string SrchSOCIOS = formListData.SrchSOCIOS == null || formListData.SrchSOCIOS == "0" ? "" : "AND so.SOCIOS = '" + formListData.SrchSOCIOS.ToString() + "' ";
            string SrchPROTEGE = formListData.SrchPROTEGE == null || formListData.SrchPROTEGE == "0" ? "" : "AND so.PROTEGE = '" + formListData.SrchPROTEGE.ToString() + "' ";
            string SrchUM3P = formListData.SrchUM3P == null || formListData.SrchUM3P == "0" ? "" : "AND so.UM3P = '" + formListData.SrchUM3P.ToString() + "' ";
            string SrchFacDepartment = formListData.SrchFacDepartment == null || formListData.SrchFacDepartment == "" ? "" : "AND fi.Department = '" + formListData.SrchFacDepartment.ToString() + "' ";
            string SrchFacPosition = formListData.SrchFacPosition == null || formListData.SrchFacPosition == "" ? "" : "AND fi.Position = '" + formListData.SrchFacPosition.ToString() + "' ";

            string searcQuery = SrchUsername + SrchFirstName + SrchLastName + SrchMiddleName + SrchContactNumber + SrchEmail + SrchGender +
                SrchCivilStatus + SrchRole + SrchStuDepartment + SrchStuCourse + SrchStuSection + SrchStuYearLevel + SrchTES + SrchTDP +
                SrchPersonDisability + SrchWorkingStudent + SrchNotApplicable + SrchOrganization + SrchALLS + SrchAPL + SrchASGRD + SrchCAITO +
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
                        //cmdSql.Parameters.AddWithValue("@UserNameData", formListData.UserName == null ? "" : formListData.UserName);
                        //cmdSql.Parameters.AddWithValue("@RoleData", formListData.UserRole == null ? "" : formListData.UserRole);
                        //cmdSql.Parameters.AddWithValue("@GenderData", formListData.Gender == null ? "" : formListData.Gender);
                        //cmdSql.Parameters.AddWithValue("@CivilStatusData", formListData.CivilStatus == null ? "" : formListData.CivilStatus);
                        //cmdSql.Parameters.AddWithValue("@StatusData", "");
                        cmdSql.ExecuteNonQuery();
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                userListdata.Add(new mUser
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
                    if (userListdata.Count != 0)
                    {


                        conn.Open();
                        using (SqlCommand cmdSqlAudit = conn.CreateCommand())
                        {
                            cmdSqlAudit.CommandType = CommandType.StoredProcedure;
                            cmdSqlAudit.CommandText = "spUser_UserLog_InserUpdate";

                            cmdSqlAudit.Parameters.Clear();
                            cmdSqlAudit.Parameters.AddWithValue("@UserID", Session["ID"]);
                            cmdSqlAudit.Parameters.AddWithValue("@UpdateID", Session["ID"]);
                            cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Send Notification for " + userListdata.Count + " users");

                            cmdSqlAudit.ExecuteNonQuery();
                        }
                        conn.Close();

                        if (formListData.PostAttachment == null)
                        {
                            #region Email Sending
                            SmtpClient SMTPSendEmail = new SmtpClient();
                            SMTPSendEmail.Host = "smtp.office365.com";
                            SMTPSendEmail.Port = 587;
                            SMTPSendEmail.EnableSsl = true;
                            SMTPSendEmail.UseDefaultCredentials = true;
                            SMTPSendEmail.Credentials = new NetworkCredential(EmailUser, EmailPass);
                            for (int i = 0; i < userListdata.Count; i++)
                            {
                                MailMessage mail = new MailMessage();
                                mail.From = new MailAddress(EmailUser);
                                mail.To.Add(userListdata[i].Email);
                                mail.CC.Add("dummyjomar69@gmail.com");
                                mail.Subject = formListData.PostBody;
                                mail.Body = formListData.PostContains;
                                mail.IsBodyHtml = true;
                                SMTPSendEmail.Send(mail);
                            }
                            #endregion
                        }
                        else {
                            string filepart = Server.MapPath("~/Areas/Transaction/Uploads/" + formListData.PostAttachment);
                            Attachment att = new Attachment(filepart);
                            att.ContentDisposition.Inline = true;
                            //Email Sending
                            #region Email Sending
                            SmtpClient SMTPSendEmail = new SmtpClient();
                            SMTPSendEmail.Host = "smtp.office365.com";
                            SMTPSendEmail.Port = 587;
                            SMTPSendEmail.EnableSsl = true;
                            SMTPSendEmail.UseDefaultCredentials = true;
                            SMTPSendEmail.Credentials = new NetworkCredential(EmailUser, EmailPass);
                            for (int i = 0; i < userListdata.Count; i++)
                            {
                                MailMessage mail = new MailMessage();
                                mail.From = new MailAddress(EmailUser);
                                mail.To.Add(userListdata[i].Email);
                                mail.CC.Add("dummyjomar69@gmail.com");
                                mail.Subject = formListData.PostBody;
                                mail.Body = String.Format(formListData.PostContains +
                                                     @"<img src=""cid:{0}"" />", att.ContentId);
                                mail.IsBodyHtml = true;
                                mail.Attachments.Add(att);
                                SMTPSendEmail.Send(mail);
                            }
                            #endregion
                        }
                        //Save Database
                        #region Save Database
                        for (int i = 0; i < userListdata.Count; i++)
                        {
                            sendAnnouncementList.Add(new SendAnnouncement
                            {
                                UserID = Convert.ToInt32(userListdata[i].ID),
                                Announcement = formListData.PostContains,
                                attachmentName = formListData.PostAttachment,
                                DatePosted = DateTime.Now.ToString("yyyy-MM-dd"),
                                PostBody = formListData.PostBody,
                                PostedBy = Session["Username"].ToString(),
                                IsDeleted = false

                            });
                        }
                        conn.Open();
                        SqlTransaction transaction;
                        transaction = conn.BeginTransaction();

                        DataTable table = sendAnnouncementList.AsDataTable();
                        table = dataHelper.UpdateCreateCols(table, Session["Username"].ToString(), Session["Username"].ToString());
                        table = dataHelper.UpdateDates(table);

                        using (SqlBulkCopy sqlbc = new SqlBulkCopy(conn, SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints, transaction))
                        {
                            sqlbc.DestinationTableName = "tSendingNotification";

                            sqlbc.ColumnMappings.Add("ID", "ID");
                            sqlbc.ColumnMappings.Add("UserID", "UserID");
                            sqlbc.ColumnMappings.Add("Announcement", "Announcement");
                            sqlbc.ColumnMappings.Add("attachmentName", "attachmentName");
                            sqlbc.ColumnMappings.Add("DatePosted", "DatePosted");
                            sqlbc.ColumnMappings.Add("PostBody", "PostBody");
                            sqlbc.ColumnMappings.Add("PostedBy", "PostedBy");
                            sqlbc.ColumnMappings.Add("IsDeleted", "IsDeleted");
                            sqlbc.ColumnMappings.Add("CreateID", "CreateID");
                            sqlbc.ColumnMappings.Add("CreateDate", "CreateDate");
                            sqlbc.ColumnMappings.Add("UpdateID", "UpdateID");
                            sqlbc.ColumnMappings.Add("UpdateDate", "UpdateDate");
                            sqlbc.WriteToServer(table);
                        }

                        transaction.Commit();
                        conn.Close();
                        #endregion
                        //Send SMS
                        #region Send SMS
                        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        TwilioClient.Init("ACba5f9a4b4238043b8bd4025cddc13fa1", "915cff28270e21120089032b4d2cecd1");
                        string vhtml = "";
                        string convertRole = "";

                        if (formListData.SrchRole == "Administrator")
                        {
                            convertRole = "Administrator";
                        }
                        else if (formListData.SrchRole == "HeadAdministrator")
                        {
                            convertRole = "Head Administrator";
                        }
                        else if (formListData.SrchRole == "Faculty")
                        {
                            convertRole = "Faculty";
                        }
                        else if (formListData.SrchRole == "NonFaculty")
                        {
                            convertRole = "Non - Faculty";
                        }
                        else if (formListData.SrchRole == "Student")
                        {
                            convertRole = "Student";
                        }

                        // vhtml += "Announcement to all " + convertRole + "  \n\n";

                        vhtml += formListData.PostContains;

                        for (int i = 0; i < userListdata.Count; i++)
                        {
                            MessageResource.Create(
                                                            body: vhtml,
                                                            messagingServiceSid: "MG5218c9b6077aafa9bedd9bc3e6e14dca",
                                                            from: new Twilio.Types.PhoneNumber("+13862603744"),
                                                            to: new Twilio.Types.PhoneNumber(userListdata[i].ContactNumber)
                                                        );
                        }


                        #endregion
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
            return Json(new { success = true, msg = "Announcement was successfully posted and send to respective numbers." });

        }
        public ActionResult GetAnnouncementList()
        {
            List<AnnouncementList> data = new List<AnnouncementList>();
            DataTableHelper TypeHelper = new DataTableHelper();

            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
            string sortDirection = Request["order[0][dir]"];

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spSending_GetAnnouncementList";
                        cmdSql.ExecuteNonQuery();
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                data.Add(new AnnouncementList
                                {
                                    ID = Convert.ToInt32(sdr["ID"]),
                                    FullName = sdr["FullName"].ToString(),
                                    UserRole = sdr["UserRole"].ToString(),
                                    Announcement = sdr["Announcement"].ToString(),
                                    attachmentName = sdr["attachmentName"].ToString(),
                                    DatePosted = sdr["DatePosted"].ToString(),
                                    PostBody = sdr["PostBody"].ToString(),
                                    PostedBy = sdr["PostedBy"].ToString()
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
                                    x.FullName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.UserRole.ToLower().Contains(searchValue.ToLower()) ||
                                    x.attachmentName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.DatePosted.ToLower().Contains(searchValue.ToLower()) ||
                                    x.PostBody.ToLower().Contains(searchValue.ToLower()) ||
                                    x.PostedBy.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Announcement.ToLower().Contains(searchValue.ToLower())
                                 ).ToList<AnnouncementList>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
            data = data.Skip(start).Take(length).ToList<AnnouncementList>();
            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAnnouncementListCount()
        {

            int CountAnnouncement = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.Text;
                        cmdMySql.CommandText = "Select count(*) as NotifCount from tSendingNotification where  IsDeleted = 0";
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                CountAnnouncement = Convert.ToInt32(sdr["NotifCount"]);
                            }
                        }
                    }
                    conn.Close();
                }
                // TestingSms();
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
                    announceCount = CountAnnouncement,
                }
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult SaveFileUploadIMG()
        {
            var postedfile = Request.Files[0] as HttpPostedFileBase;
            string[] filename = postedfile.FileName.Split('\\');
            string filepart = Server.MapPath("~/Areas/Transaction/Uploads/");
            if (filename.Length == 1)
            {
                //Chrome
                postedfile.SaveAs(filepart + "/" + postedfile.FileName);
            }
            else
            {
                //IE
                postedfile.SaveAs(filepart + "/" + filename[filename.Length - 1]);
            }


            return Json(new { success = true, msg = "Image was successfully Upload." }, JsonRequestBehavior.AllowGet);
        }
    }
}