﻿using DotNetOpenAuth.Messaging.Bindings;
using DotNetOpenAuth.OAuth2;
using DotNetOpenAuth.OAuth2.ChannelElements;
using DotNetOpenAuth.OAuth2.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Server.Models
{
    public class OAuth2AuthorizationServer : IAuthorizationServerHost
    {		
        private static readonly RSAParameters ResourceServerEncryptionPublicKey = new RSAParameters
        {
            Exponent = new byte[] { 1, 0, 1 },
            Modulus = new byte[] { 166, 175, 117, 169, 211, 251, 45, 215, 55, 53, 202, 65, 153, 155, 92, 219, 235, 243, 61, 170, 101, 250, 221, 214, 239, 175, 238, 175, 239, 20, 144, 72, 227, 221, 4, 219, 32, 225, 101, 96, 18, 33, 117, 176, 110, 123, 109, 23, 29, 85, 93, 50, 129, 163, 113, 57, 122, 212, 141, 145, 17, 31, 67, 165, 181, 91, 117, 23, 138, 251, 198, 132, 188, 213, 10, 157, 116, 229, 48, 168, 8, 127, 28, 156, 239, 124, 117, 36, 232, 100, 222, 23, 52, 186, 239, 5, 63, 207, 185, 16, 137, 73, 137, 147, 252, 71, 9, 239, 113, 27, 88, 255, 91, 56, 192, 142, 210, 21, 34, 81, 204, 239, 57, 60, 140, 249, 15, 101 },
        };

        public ICryptoKeyStore CryptoKeyStore
        {
            get
            {
                if (WebApiApplication.KeyStore == null){
                    WebApiApplication.KeyStore = new CryptoKeyStore();
                }
                return WebApiApplication.KeyStore;
            }
        }
        public INonceStore NonceStore
        {
            get
            {
                if (WebApiApplication.NonceStore == null)
                {
                    WebApiApplication.NonceStore = new NonceStore();
                }
                return WebApiApplication.NonceStore;
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
            var accessToken = new AuthorizationServerAccessToken();
            accessToken.Lifetime = TimeSpan.FromMinutes(2);

            accessToken.ResourceServerEncryptionKey = new RSACryptoServiceProvider();
            accessToken.ResourceServerEncryptionKey.ImportParameters(ResourceServerEncryptionPublicKey);

            accessToken.AccessTokenSigningKey = CreateRSA();

            var result = new AccessTokenResult(accessToken);
            return result;
        }

        /// <summary>
        /// DEBUG ONLY
        /// this gives EVERYONE access to EVERYTHING!
        /// </summary>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public bool IsAuthorizationValid(IAuthorizationDescription authorization)
        {
            return true;
        }

        public bool CanBeAutoApproved(EndUserAuthorizationRequest authorizationRequest)
        {
            if (authorizationRequest == null)
            {
                throw new ArgumentNullException("authorizationRequest");
            }
            return true;
        }

        private static RSACryptoServiceProvider CreateRSA()
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(CreateAuthorizationServerSigningKey());
            return rsa;
        }

        private static RSAParameters CreateAuthorizationServerSigningKey()
        {
            return new RSAParameters
            {
                Exponent = new byte[] { 1, 0, 1 },
                Modulus = new byte[] { 210, 95, 53, 12, 203, 114, 150, 23, 23, 88, 4, 200, 47, 219, 73, 54, 146, 253, 126, 121, 105, 91, 118, 217, 182, 167, 140, 6, 67, 112, 97, 183, 66, 112, 245, 103, 136, 222, 205, 28, 196, 45, 6, 223, 192, 76, 56, 180, 90, 120, 144, 19, 31, 193, 37, 129, 186, 214, 36, 53, 204, 53, 108, 133, 112, 17, 133, 244, 3, 12, 230, 29, 243, 51, 79, 253, 10, 111, 185, 23, 74, 230, 99, 94, 78, 49, 209, 39, 95, 213, 248, 212, 22, 4, 222, 145, 77, 190, 136, 230, 134, 70, 228, 241, 194, 216, 163, 234, 52, 1, 64, 181, 139, 128, 90, 255, 214, 60, 168, 233, 254, 110, 31, 102, 58, 67, 201, 33 },
                P = new byte[] { 237, 238, 79, 75, 29, 57, 145, 201, 57, 177, 215, 108, 40, 77, 232, 237, 113, 38, 157, 195, 174, 134, 188, 175, 121, 28, 11, 236, 80, 146, 12, 38, 8, 12, 104, 46, 6, 247, 14, 149, 196, 23, 130, 116, 141, 137, 225, 74, 84, 111, 44, 163, 55, 10, 246, 154, 195, 158, 186, 241, 162, 11, 217, 77 },
                Q = new byte[] { 226, 89, 29, 67, 178, 205, 30, 152, 184, 165, 15, 152, 131, 245, 141, 80, 150, 3, 224, 136, 188, 248, 149, 36, 200, 250, 207, 156, 224, 79, 150, 191, 84, 214, 233, 173, 95, 192, 55, 123, 124, 255, 53, 85, 11, 233, 156, 66, 14, 27, 27, 163, 108, 199, 90, 37, 118, 38, 78, 171, 80, 26, 101, 37 },
                DP = new byte[] { 108, 176, 122, 132, 131, 187, 50, 191, 203, 157, 84, 29, 82, 100, 20, 205, 178, 236, 195, 17, 10, 254, 253, 222, 226, 226, 79, 8, 10, 222, 76, 178, 106, 230, 208, 8, 134, 162, 1, 133, 164, 232, 96, 109, 193, 226, 132, 138, 33, 252, 15, 86, 23, 228, 232, 54, 86, 186, 130, 7, 179, 208, 217, 217 },
                DQ = new byte[] { 175, 63, 252, 46, 140, 99, 208, 138, 194, 123, 218, 101, 101, 214, 91, 65, 199, 196, 220, 182, 66, 73, 221, 128, 11, 180, 85, 198, 202, 206, 20, 147, 179, 102, 106, 170, 247, 245, 229, 127, 81, 58, 111, 218, 151, 76, 154, 213, 114, 2, 127, 21, 187, 133, 102, 64, 151, 7, 245, 229, 34, 50, 45, 153 },
                InverseQ = new byte[] { 137, 156, 11, 248, 118, 201, 135, 145, 134, 121, 14, 162, 149, 14, 98, 84, 108, 160, 27, 91, 230, 116, 216, 181, 200, 49, 34, 254, 119, 153, 179, 52, 231, 234, 36, 148, 71, 161, 182, 171, 35, 182, 46, 164, 179, 100, 226, 71, 119, 23, 0, 16, 240, 4, 30, 57, 76, 109, 89, 131, 56, 219, 71, 206 },
                D = new byte[] { 108, 15, 123, 176, 150, 208, 197, 72, 23, 53, 159, 63, 53, 85, 238, 197, 153, 187, 156, 187, 192, 226, 186, 170, 26, 168, 245, 196, 65, 223, 248, 81, 170, 79, 91, 191, 83, 15, 31, 77, 39, 119, 249, 143, 245, 183, 49, 105, 115, 15, 122, 242, 87, 221, 94, 230, 196, 146, 59, 7, 103, 94, 9, 223, 146, 180, 189, 86, 190, 94, 242, 59, 32, 54, 23, 181, 124, 170, 63, 172, 90, 158, 169, 140, 6, 102, 170, 0, 135, 199, 35, 196, 212, 238, 196, 56, 14, 0, 140, 197, 169, 240, 156, 43, 182, 123, 102, 79, 89, 20, 120, 171, 43, 223, 58, 190, 230, 166, 185, 162, 186, 226, 31, 206, 196, 188, 104, 1 },
            };
        }
    }
}