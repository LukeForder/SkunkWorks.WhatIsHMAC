using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SkunkWorks.WhatIsHMAC.Client
{
    public static class MessageExtension
    {
        public static string ToHMAC(this Message message, byte[] sharedSecret)
        {
            var messageData = string.Format("sender:{0}:content:{1}", message.Sender, message.Content);

            var messageDataBytes = Encoding.UTF8.GetBytes(messageData);

            using (var hashProvider = new HMACSHA256(sharedSecret))
            {
                var messageDataHash = hashProvider.ComputeHash(messageDataBytes);

                return BitConverter.ToString(messageDataHash).Replace("-", "");
            }
        }
    }
}
