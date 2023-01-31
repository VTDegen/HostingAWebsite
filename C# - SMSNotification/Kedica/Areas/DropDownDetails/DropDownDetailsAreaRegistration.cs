using System.Web.Mvc;

namespace SMSNofication.Areas.DropDownDetails
{
    public class DropDownDetailsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DropDownDetails";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DropDownDetails_default",
                "DropDownDetails/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}