﻿using Chalesh.Core.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chalesh.Core.Utils
{
    public class CodeFactory
    {
        // Global variable 
        

        public static Guid GenerateGuidFromMacAddress()
        {
            var mac = NetworkInterface.GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault();

            if (mac == null)
            {
                throw new Exception("Could not retrieve MAC address");
            }

            var guidBytes = Encoding.UTF8.GetBytes(mac);
            var hash = MD5.Create().ComputeHash(guidBytes);

            // Set the version number to 3 for MD5 hashing algorithm
            hash[6] = (byte)((hash[6] & 0x0f) | 0x30);
            // Set the variant number to 2 as per RFC4122
            hash[8] = (byte)((hash[8] & 0x3f) | 0x80);

            return new Guid(hash);
        }
        public static string GetSpecificStingWithRegex(string msg, string pattern)
        {
            Regex regex = new Regex(pattern);
            string st = regex.Match(msg).Groups[0].Value;
            return st;
        }
    }
}
