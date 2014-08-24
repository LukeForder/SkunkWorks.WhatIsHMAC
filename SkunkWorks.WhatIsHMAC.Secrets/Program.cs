using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SkunkWorks.WhatIsHMAC.Secrets
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Generating shared secret");

            using (RNGCryptoServiceProvider keyGenerator = new RNGCryptoServiceProvider())
            {
                byte[] keyData = new byte[32];

                keyGenerator.GetBytes(keyData);

                string keyString = Convert.ToBase64String(keyData);

                File.WriteAllText("shared-secret.xxx", keyString);
            }
            
            Console.WriteLine("Generated shared secret");

        }
    }
}
