using SMSNotification.Areas.MasterMaintenance.Models;
using SMSNotification.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMSNofication.Areas.AuditTrail.Controllers
{
    public class UserLogsController : Controller
    {
        // GET: AuditTrail/UserLogs
        public ActionResult Index()
        {
            return View("UserLogs");
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
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spUser_GetUserLog";
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
                                    Email = sdr["Email"].ToString(),
                                    InDate = sdr["DateActivity"].ToString(),
                                    InTime = sdr["Activities"].ToString(),
                                    UpdateID = sdr["UpdateUser"].ToString(),
                                    
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
                                    x.FirstName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.LastName.ToLower().Contains(searchValue.ToLower()) ||
                                    x.Email.ToLower().Contains(searchValue.ToLower()) ||
                                    x.ContactNumber.ToLower().Contains(searchValue.ToLower()) ||
                                    x.InDate.ToLower().Contains(searchValue.ToLower()) ||
                                    x.OutTime.ToLower().Contains(searchValue.ToLower()) ||
                                    x.UpdateID.ToLower().Contains(searchValue.ToLower())
                                 ).ToList<mUser>();

            int totalrowsafterfiltering = data.Count;
            if (sortDirection == "asc")
                data = data.OrderBy(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();

            if (sortDirection == "desc")
                data = data.OrderByDescending(x => TypeHelper.GetPropertyValue(x, sortColumnName)).ToList();
            data = data.Skip(start).Take(length).ToList<mUser>();

            return Json(new { data = data, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUserData(string ID)
        {
            mUser data = new mUser();

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SMSConfig"].ConnectionString.ToString()))

                {
                    conn.Open();
                    using (SqlCommand cmdSql = conn.CreateCommand())
                    {
                        cmdSql.CommandType = CommandType.StoredProcedure;
                        cmdSql.CommandText = "spUser_GetUserLog";
                        cmdSql.Parameters.Clear();
                        cmdSql.Parameters.AddWithValue("@ID", Convert.ToInt32(ID));
                        using (SqlDataReader sdr = cmdSql.ExecuteReader())
                        {
                            if (sdr.Read())
                            {

                                data.ID = Convert.ToInt32(sdr["ID"]);
                                data.FirstName = sdr["FirstName"].ToString();
                                data.MiddleName = sdr["MiddleName"].ToString();
                                data.LastName = sdr["LastName"].ToString();
                                data.Email = sdr["Email"].ToString();
                                data.ContactNumber = sdr["ContactNumber"].ToString();
                                data.InDate = sdr["InDate"].ToString();
                                data.InTime = sdr["InTime"].ToString();
                                data.OutDate = sdr["OutDate"].ToString();
                                data.OutTime = sdr["OutTime"].ToString();
                           
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

            return Json(new { success = true, data = new { data = data } });
        }
    }
}