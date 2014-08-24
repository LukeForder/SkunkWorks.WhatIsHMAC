using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.ModelBinding;
using SkunkWorks.WhatIsHMAC.Client;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Text;

namespace SkunkWorks.WhatIsHMAC.Server
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Post["/message"] = 
                (args) =>
                {
                    var message = this.Bind<Message>();

                    var hmac = Request.Headers.FirstOrDefault(x => string.Compare(x.Key, "X-SkunkWorks-HMAC", true) == 0);

                    if (hmac.Value == null || hmac.Value.Count() == 0)
                        return HttpStatusCode.BadRequest;

                    byte[] secret = GetSharedSecret();

                    var messageData = message.ToHMAC(secret);

                   if (messageData != hmac.Value.First())
                   {
                       // Log to security 
                       return HttpStatusCode.BadRequest;
                   }

                   // Do work here

                   return HttpStatusCode.OK;

                };
        }

        private byte[] GetSharedSecret()
        {
            var sharedSecret = ConfigurationManager.AppSettings["shared-secret"];

            byte[] sharedSecretBytes = Convert.FromBase64String(sharedSecret);

            return sharedSecretBytes;
        }
    }
}