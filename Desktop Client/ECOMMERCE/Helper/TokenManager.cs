using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ECOMMERCE.Helper
{
    public static class TokenManager
    {
        public static SecureString AccessToken { get; private set; }

        public static void SetAccessToken(string token)
        {
            if (AccessToken != null)
            {
                AccessToken.Dispose(); 
            }

            AccessToken = new SecureString();
            foreach (char c in token)
            {
                AccessToken.AppendChar(c);
            }
        }

        public static string GetAccessToken()
        {
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToBSTR(AccessToken);
                return Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(ptr);
                }
            }
        }
    }
}
