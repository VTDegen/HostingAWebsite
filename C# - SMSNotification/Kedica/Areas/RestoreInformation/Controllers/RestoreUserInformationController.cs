using SMSNofication.Areas.RestoreInformation.Models;
using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSNofication.Areas.RestoreInformation.Controllers
{
    public class RestoreUserInformationController : Controller
    {
     
        DataHelper dataHelper = new DataHelper();
        List<string> modelErrors = new List<string>();
        bool error = false;
        string errmsg = "";
        // GET: RestoreInformation/RestoreUserInformation
        public ActionResult Index()
        {
            return View("RestoreUserInformation");
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

          

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))

                {

                   

                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.Text;
                        cmdSql.CommandText = "select * from mUser where IsDeleted = 1";
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
                                    Email = sdr["Email"].ToString(),
                                    Role = sdr["UserRole"].ToString(),
                                    ContactNumber = sdr["ContactNumber"].ToString(),
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
        public ActionResult RestoreUser(List<restoreUser> UserIDList)
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
                        cmdSqlAudit.Parameters.AddWithValue("@Transaction", "Restore " + UserIDList.Count + "User");

                        cmdSqlAudit.ExecuteNonQuery();
                    }
                    conn.Close();

                    for (int i = 0; i < UserIDList.Count; i++)
                    {
                        conn.Open();
                        using (SqlCommand cmdMySql = conn.CreateCommand())
                        {
                            cmdMySql.CommandType = CommandType.StoredProcedure;
                            cmdMySql.CommandText = "spUser_Restore";
                            cmdMySql.Parameters.Clear();
                            cmdMySql.Parameters.AddWithValue("@UserID", UserIDList[i].UserID);
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
            return Json(new { success = true, msg = "User was successfully restored." });

        }
    }
}