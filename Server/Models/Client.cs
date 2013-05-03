using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class Client : IClientDescription
    {
        public int ClientId { get; set; }
        public string ClientIdentifier { get; set; }
        public string ClientSecret { get; set; }

        private ClientType clientType;
        public ClientType ClientType
        {
            get
            {
                return clientType;
            }
        }

        private Uri defaultCallback;
        public Uri DefaultCallback
        {
            get
            {
                return defaultCallback;
            }

            set
            {
                defaultCallback = value;
            }
        }

        private bool hasNonEmptySecret;
        public bool HasNonEmptySecret
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ClientSecret);
            }
        }

        public Client(string clientIdentifier, int clientType)
        {
            this.ClientIdentifier = clientIdentifier;
            this.clientType = (ClientType)clientType;
        }

        public bool IsCallbackAllowed(Uri callback)
        {
            if (defaultCallback == null)
            {
                // No callback rules have been set up for this client.
                return true;
            }

            // In this sample, it's enough of a callback URL match if the scheme and host match.
            // In a production app, it is advisable to require a match on the path as well.
            if (string.Equals(defaultCallback.GetLeftPart(UriPartial.Authority), callback.GetLeftPart(UriPartial.Authority), StringComparison.Ordinal))
            {
                return true;
            }

            return false;
        }

        public bool IsValidClientSecret(string secret)
        {
            return MessagingUtilities.EqualsConstantTime(secret, this.ClientSecret);
        }
    }
}