using System.Web.Mvc;

namespace SMSNofication.Areas.AuditTrail
{
    public class AuditTrailAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AuditTrail";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AuditTrail_default",
                "AuditTrail/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}