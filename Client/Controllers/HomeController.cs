using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public string About()
        {
            var authServerDescription = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri("http://localhost:2396/OAuth/Token"),
                AuthorizationEndpoint = new Uri("http://localhost:2396/OAuth/Authorize")
            };
            var Client = new WebServerClient(authServerDescription, "sampleconsumer", "samplesecret");

            var Authorization = Client.ProcessUserAuthorization();
            /*****************************************************************************************************/
            /*my code*/

            if (!string.IsNullOrEmpty(Request.QueryString["code"]))
            {
                try
                {
                    var valueString = string.Empty;
                    if (!string.IsNullOrEmpty(Authorization.AccessToken))
                    {
                        //this means that the authorization is now working
                        //theoretically, I can redirect to the appropriate page after storing the access token.
                        var a = "a";
                    }
                    ViewBag.Values = valueString;
                }
                catch (Exception)
                {
                }
            }
            else
            {
                Client.RequestUserAuthorization();
            }

            /*end code*/
            /*****************************************************************************************************/

            return "";
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
