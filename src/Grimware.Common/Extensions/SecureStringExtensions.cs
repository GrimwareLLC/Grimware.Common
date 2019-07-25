using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Grimware.Extensions
{
    public static class SecureStringExtensions
    {
        public static string ToUnsecuredString(this SecureString source)
        {
            if (source == null)
                return null;

            var unmanaged = IntPtr.Zero;

            try
            {
                unmanaged = Marshal.SecureStringToGlobalAllocUnicode(source);
                return Marshal.PtrToStringUni(unmanaged);
            }
            finally
            {
                if (unmanaged != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(unmanaged);
            }
        }
    }
}