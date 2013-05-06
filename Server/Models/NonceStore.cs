using DotNetOpenAuth.Messaging.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class NonceStore : INonceStore
    {
        private List<Nonces> nonces;
        public NonceStore()
        {
            nonces = new List<Nonces>();
        }

        public bool StoreNonce(string context, string nonce, DateTime timestampUtc)
        {
            var newNonce = new Nonces { Context = context, Code = nonce, Timestamp = timestampUtc };
            var exist = nonces.Any(n => n.Context == context && n.Code == nonce && n.Timestamp == timestampUtc);

            if (exist == false)
            {
                nonces.Add(newNonce);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class Nonces
    {
        public string Context { get; set; }
        public string Code { get; set; }
        public DateTime Timestamp { get; set; }
    }
}