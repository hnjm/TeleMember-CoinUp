#region Using

using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Win32;

#endregion

namespace TeleMember_CoinUp
{
    public static class Activation
    {
        #region Hash Function

        public static string MyCustomHash(string ID, string Hash)
        {
            if (Hash != Math.Pow(ID.ToCharArray().Length, 2) + 20.ToString())
            {
                return "error:Hash Is Invalid";
            }
            var x =
                new MD5CryptoServiceProvider();
            var bs = Encoding.UTF32.GetBytes(ID);
            bs = x.ComputeHash(bs);
            var s = new StringBuilder();
            var select = 1;
            foreach (var b in bs)
            {
                switch (select)
                {
                    case 1:
                        s.Append(b.ToString("x4").ToLower());
                        break;
                    case 2:
                        s.Append(b.ToString("x3").ToLower());
                        break;
                    case 3:
                        s.Append(b.ToString("x2").ToLower());
                        break;
                    case 4:
                        s.Append(b.ToString("x1").ToLower());
                        break;
                    default:
                        break;
                }
                select++;
                if (select > 4) select = 1;
            }
            var password = s.ToString();
            return password.Replace("0","O").Replace("1","I");
        }

        #endregion

        #region  Redegit Key

        public static void MakeRegeditActivationCode()
        {
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TeleMember");
        }

        public static string ReadRegeditKey()
        {
            try
            {
                var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TeleMember", true);
                return myKey.GetValue("ActivationCode").ToString();
            }
            catch
            {
                SetRegeditKeyValue("");
                return null;
            }
        }

        public static void SetRegeditKeyValue(string value)
        {
            try
            {
                var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TeleMember", true);
                myKey.SetValue("ActivationCode", value, RegistryValueKind.String);
            }
            catch (Exception)
            {
                MakeRegeditActivationCode();
                var myKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\TeleMember", true);
                myKey.SetValue("ActivationCode", value, RegistryValueKind.String);
            }
        }

        #endregion

        #region Get Hardware Information

        public static string GetID()
        {
            var ID = "";

            foreach (var nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                ID = nic.GetPhysicalAddress().ToString();
                break;
            }
            if (ID == "")
            {
                ID = Environment.UserName;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(ID));
        }

        public static string GetHash()
        {
            return Math.Pow(GetID().ToCharArray().Length, 2) + 20.ToString();
        }

        #endregion
    }
}