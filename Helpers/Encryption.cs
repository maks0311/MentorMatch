using System;
using System.Reflection;
using System.Text;

namespace Mentor
{
    public static class Encryption
    {
        private static NLog.ILogger AppLogger = NLog.LogManager.GetCurrentClassLogger();

        public static string DecryptString(string queryString)
        {
            try
            {
                byte[] b;
                string decrypted;
                try
                {
                    b = Convert.FromBase64String(queryString);
                    decrypted = ASCIIEncoding.ASCII.GetString(b);
                }
                catch (FormatException)
                {
                    decrypted = string.Empty;
                }
                return decrypted;
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return string.Empty;
            }
        }

        public static string EnryptString(string decryptedString)
        {
            try
            {
                byte[] b = ASCIIEncoding.ASCII.GetBytes(decryptedString);
                string encrypted = Convert.ToBase64String(b);
                return encrypted;
            }
            catch (Exception ex)
            {
                AppLogger.Error("{0} {1}", MethodBase.GetCurrentMethod().Name, ex.Message);
                return string.Empty;
            }
        }
    }
}
