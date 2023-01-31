using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SMSNofication.Models;
using OfficeOpenXml;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.ModelBinding;
using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;

using Jitbit.Utils;
using SMSNofication.Areas.Transaction.Models;
using System.Net.PeerToPeer;
using System.Web.Helpers;

namespace SMSNofication.Areas.Report.Controllers
{
    public class ReportGenerationController : Controller
    {
        // GET: Report/ReportGeneration
        Security ph = new Security();
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        public ActionResult Index()
        {
            return View("ReportGeneration");
        }
        public ActionResult DownloadReport(string searcQuery)
        {
            List<mUser> data = new List<mUser>();


            List<mUser> userListdata = new List<mUser>();

            string val = searcQuery.Replace("'0'", "0");
            var DCA_CSV = new CsvExport();
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
                        cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Generate Report");

                        cmdSqlAudit.ExecuteNonQuery();
                    }
                    conn.Close();


                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spUser_GetDownloadReport";
                        cmdSql.Parameters.AddWithValue("@SearchItems", searcQuery.Replace("'0'", "0"));

                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            while (sdr.Read())
                            {
                                DCA_CSV.AddRow();
                                DCA_CSV["FirstName"] = sdr["FirstName"].ToString();
                                DCA_CSV["MiddleName"] = sdr["MiddleName"].ToString();
                                DCA_CSV["LastName"] = sdr["LastName"].ToString();
                                DCA_CSV["Announcement"] = sdr["Announcement"].ToString();
                                DCA_CSV["Post Body"] = sdr["PostBody"].ToString();
                                DCA_CSV["Email Address"] = sdr["Email"].ToString();
                                DCA_CSV["Contact Number"] = sdr["ContactNumber"].ToString();
                                if (sdr["UserRole"].ToString() == "HeadAdministrator")
                                {
                                    DCA_CSV["Role"] = "Head Administrator";
                                }
                                else if (sdr["UserRole"].ToString() == "NonFaculty")
                                {
                                    DCA_CSV["Role"] = "Non Faculty";
                                }
                                else {
                                    DCA_CSV["Role"] = sdr["UserRole"].ToString();
                                }
                                DCA_CSV["Gender"] = sdr["GenderData"].ToString();
                                DCA_CSV["Civil Status"] = sdr["CivilStatusData"].ToString();
                                DCA_CSV["Date Posted"] = sdr["DatePosted"].ToString();
                                DCA_CSV["Posted By"] = sdr["PostedBy"].ToString();   
                            }
                            if (!sdr.HasRows)
                            {
                                DCA_CSV.AddRow();
                                DCA_CSV["No records were found. Please try again."] = "";
                            }

                        }
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

            if (modelErrors.Count != 0 || error)
                return Json(new { success = false, errors = modelErrors }, JsonRequestBehavior.AllowGet);
            else
                return File(DCA_CSV.ExportToBytes(), "text/csv", "Report as of_"+ DateTime.Now.ToString() +".csv");
        }
    }
}