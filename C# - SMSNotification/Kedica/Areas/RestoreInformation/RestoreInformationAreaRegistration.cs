using System.Web.Mvc;

namespace SMSNofication.Areas.RestoreInformation
{
    public class RestoreInformationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RestoreInformation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RestoreInformation_default",
                "RestoreInformation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}