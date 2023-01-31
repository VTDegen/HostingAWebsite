using SMSNofication.Api;
using SMSNofication.Areas.Transaction.Models;
using SMSNotification.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SMSNofication.Areas.Transaction.Controllers
{
    public class AnnouncementController : Controller
    {

        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        // GET: Transaction/Announcement
       
        public ActionResult Index()
        {

            return View("Announcement");
           
        }
        public  ActionResult GetAnnouncementListDetails() {

            List<SendAnnouncement> announcementDetails = new List<SendAnnouncement>();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))
                {
                    conn.Open();
                    using (SqlCommand cmdMySql = conn.CreateCommand())
                    {
                        cmdMySql.CommandType = CommandType.StoredProcedure;
                        cmdMySql.CommandText = "spAnnouncement_GetList";
                        cmdMySql.Parameters.Clear();
                        cmdMySql.Parameters.AddWithValue("@UserName", Session["Username"]);
                        using (SqlDataReader sdr = cmdMySql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {

                                announcementDetails.Add(new SendAnnouncement
                                {
                                    CreateDate = sdr["CreateDate"].ToString(),
                                    Announcement = sdr["Announcement"].ToString(),
                                    PostBody = sdr["PostBody"].ToString(),
                                    attachmentName = sdr["attachmentName"].ToString(),
                                    PostedBy = sdr["PostedBy"].ToString()
                                });
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
                    announcementList = announcementDetails,
                }
            }, JsonRequestBehavior.AllowGet);

        }
        

        
    }
}