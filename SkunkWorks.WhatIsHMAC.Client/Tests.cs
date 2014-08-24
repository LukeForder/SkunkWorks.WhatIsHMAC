using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SkunkWorks.WhatIsHMAC.Client
{
    public class Tests
    {
        [Fact]
        public async Task CanSendMessageWithValidHMACAsync()
        {
            byte[] sharedSecret = Convert.FromBase64String("k+x9YK5CcjFUwmhxk+ehxiL2xOVPY87KenAt0ebmSsA=");

            Message message =
                new Message
                {
                    Content = "TestMessage",
                    Sender = "Sender"
                };


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:53393/message")
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(message),
                    Encoding.UTF8,
                    "application/json")
            };

            request.Headers.Add("X-SkunkWorks-HMAC", message.ToHMAC(sharedSecret));

            var response = await new HttpClient().SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task CanNotSendMessageWithInvalidHMACAsync()
        {
            byte[] sharedSecret = Convert.FromBase64String("k+x9YK5CcjFUwmhxk+ehxiL2xOVPY87KenAt0ebmSsA=");

            Message message =
                new Message
                {
                    Content = "TestMessage",
                    Sender = "Sender"
                };


            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:53393/message");

            request.Headers.Add("X-SkunkWorks-HMAC", message.ToHMAC(sharedSecret));

            message.Sender = "AlteredSender";

            request.Content = new StringContent(
                    JsonConvert.SerializeObject(message),
                    Encoding.UTF8,
                    "application/json");

            var response = await new HttpClient().SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
