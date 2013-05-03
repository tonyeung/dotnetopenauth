using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.Messaging;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        private readonly AuthorizationServer authorizationServer = new AuthorizationServer(new OAuth2AuthorizationServer());

        public ActionResult Token()
        {
            return this.authorizationServer.HandleTokenRequest(this.Request).AsActionResult();
        }

        public ActionResult Authorize()
        {
            var pendingRequest = this.authorizationServer.ReadAuthorizationRequest();
            if (pendingRequest == null)
            {
                throw new HttpException((int)HttpStatusCode.BadRequest, "Missing authorization request.");
            }

            IDirectedProtocolMessage response;
            //use the requesting client to validate something here to auto approve
            if (((OAuth2AuthorizationServer)this.authorizationServer.AuthorizationServerServices).CanBeAutoApproved(pendingRequest))
            {
                response = this.authorizationServer.PrepareApproveAuthorizationRequest(pendingRequest, "SampleUser");
            }
            else
            {

                response = this.authorizationServer.PrepareRejectAuthorizationRequest(pendingRequest);
            }
            //return RedirectToAction("AuthorizeResponse", new { isApproved = true, client_id = "SampleConsumer", redirect_uri = "http://localhost:54644/AccessControl/About", state = "", scope = "http://localhost:38828/api/values", response_type = "code" });
            return this.authorizationServer.Channel.PrepareResponse(response).AsActionResult();
        }
    }
}
