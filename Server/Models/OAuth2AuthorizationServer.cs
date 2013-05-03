using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class OAuth2AuthorizationServer : IAuthorizationServerHost
    {
        public ICryptoKeyStore CryptoKeyStore
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public INonceStore NonceStore
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IClientDescription GetClient(string clientIdentifier)
        {
            return new Client("SampleConsumer", 1) { ClientSecret = "SampleSecret" };
        }

        public AutomatedAuthorizationCheckResponse CheckAuthorizeClientCredentialsGrant(IAccessTokenRequest accessRequest)
        {
            throw new NotImplementedException();
        }

        public AutomatedUserAuthorizationCheckResponse CheckAuthorizeResourceOwnerCredentialGrant(string userName, string password, IAccessTokenRequest accessRequest)
        {
            throw new NotImplementedException();
        }

        public AccessTokenResult CreateAccessToken(IAccessTokenRequest accessTokenRequestMessage)
        {
            throw new NotImplementedException();
        }

        public bool IsAuthorizationValid(IAuthorizationDescription authorization)
        {
            throw new NotImplementedException();
        }

        public bool CanBeAutoApproved(EndUserAuthorizationRequest authorizationRequest)
        {
            if (authorizationRequest == null)
            {
                throw new ArgumentNullException("authorizationRequest");
            }
            return true;
        }

    }
}