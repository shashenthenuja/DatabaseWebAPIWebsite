using System.Web.Http;
using System.Web.Mvc;

namespace BusinessLayer.Areas.HelpPage
{
    public class HelpPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "HelpPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "HelpPage",
                "Help/{action}/{apiId}",
                new { controller = "Help", action = "Index", apiId = UrlParameter.Optional },
                namespaces: new[] { "BankWebAPI.BusinessLayer.WebMvc.Controllers" });

            HelpPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}