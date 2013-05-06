using DotNetOpenAuth.Messaging.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models
{
    public class CryptoKeyStore : ICryptoKeyStore
    {
        private List<SymmetricCryptoKeys> keys;

        /// <summary>
        /// Instantiates the key store, should have dependency injection insert a repo instead of having defined here.
        /// </summary>
        public CryptoKeyStore()
        {
            keys = new List<SymmetricCryptoKeys>();
        }


        public CryptoKey GetKey(string bucket, string handle)
        {
            // It is critical that this lookup be case-sensitive, which can only be configured at the database.
            var matches = from key in keys
                          where key.Bucket == bucket && key.Handle == handle
                          select key;

            var match = matches.FirstOrDefault();
            var cryptoKey = new CryptoKey(match.Secret, match.ExpiresUtc.ToUniversalTime());

            return cryptoKey;
        }

        public IEnumerable<KeyValuePair<string, CryptoKey>> GetKeys(string bucket)
        {

            var matches = from key in keys
                          where key.Bucket == bucket
                          orderby key.ExpiresUtc descending
                          select key;

            List<KeyValuePair<string, CryptoKey>> en = new List<KeyValuePair<string, CryptoKey>>();

            foreach (var key in matches)
                en.Add(new KeyValuePair<string, CryptoKey>(key.Handle, new CryptoKey(key.Secret, key.ExpiresUtc.ToUniversalTime())));

            return en.AsEnumerable<KeyValuePair<string, CryptoKey>>();
        }

        public void RemoveKey(string bucket, string handle)
        {
            var match = keys.FirstOrDefault(k => k.Bucket == bucket && k.Handle == handle);
            if (match != null)
            {
                keys.Remove(match);
            }
        }

        public void StoreKey(string bucket, string handle, CryptoKey key)
        {
            var keyRow = new SymmetricCryptoKeys()
            {
                Bucket = bucket,
                Handle = handle,
                Secret = key.Key,
                ExpiresUtc = key.ExpiresUtc,
            };

            keys.Add(keyRow);
        }
    }

    /// <summary>
    /// Data object, should be a POCO with a repo backing it
    /// </summary>
    public class SymmetricCryptoKeys
    {
        public string Bucket { get; set; }
        public string Handle { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public byte[] Secret { get; set; }
    }
}