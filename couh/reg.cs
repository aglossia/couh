using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace couh
{
    class reg
    {

        [DllImport("advapi32.dll")]
        public static extern int RegOpenKeyEx(
            UIntPtr hKey,
            string subKey,
            int ulOptions,
            int samDesired,
            out UIntPtr hkResult
        );

        [DllImport("advapi32.dll")]
        public static extern int RegQueryInfoKey(
            UIntPtr hkey,
            out StringBuilder lpClass,
            ref uint lpcbClass,
            IntPtr lpReserved,
            out uint lpcSubKeys,
            IntPtr lpcbMaxSubKeyLen,
            IntPtr lpcbMaxClassLen,
            IntPtr lpcValues,
            IntPtr lpcbMaxValueNameLen,
            IntPtr lpcbMaxValueLen,
            IntPtr lpcbSecurityDescriptor,
            IntPtr lpftLastWriteTime
        );

        [DllImport("advapi32.dll")]
        extern public static int RegEnumKeyEx(
            UIntPtr hkey,
            uint index,
            StringBuilder lpName,
            ref uint lpcbName,
            IntPtr reserved,
            IntPtr lpClass,
            IntPtr lpcbClass,
            out long lpftLastWriteTime
        );

        public enum RegSAM
        {
            QueryValue = 0x0001,
            SetValue = 0x0002,
            CreateSubKey = 0x0004,
            EnumerateSubKeys = 0x0008,
            Notify = 0x0010,
            CreateLink = 0x0020,
            WOW64_32Key = 0x0200,
            WOW64_64Key = 0x0100,
            WOW64_Res = 0x0300,
            Read = 0x00020019,
            Write = 0x00020006,
            Execute = 0x00020019,
            AllAccess = 0x000f003f
        }

        public static class RegHive
        {
            public static UIntPtr HKEY_LOCAL_MACHINE = new UIntPtr(0x80000002u);
            public static UIntPtr HKEY_CURRENT_USER = new UIntPtr(0x80000001u);
        }



        public void GetSubKeyLastUpdate(ref Dictionary<string, long>listLastUpdate, string keyName)
        {
            UIntPtr key;
            int status = RegOpenKeyEx(
                RegHive.HKEY_LOCAL_MACHINE, //UIntPtr hKey,
                keyName, //string subKey,
                0, //int ulOptions,
                //(int)RegSAM.QueryValue, //int samDesired,
                (int)RegSAM.AllAccess,
                out key //out UIntPtr hkResult
            );
            if (0 != status)
            {
                Console.WriteLine("RegOpenKeyEx failed");
                return;
            }

            var className = new StringBuilder(1024);
            var classNameLength = (uint)className.Capacity;
            uint ctSubKeys;

            status = RegQueryInfoKey(
                key, //UIntPtr hkey,
                out className, //out StringBuilder lpClass,
                ref classNameLength, //ref uint lpcbClass,
                IntPtr.Zero, //IntPtr lpReserved,
                out ctSubKeys, //out uint lpcSubKeys,
                IntPtr.Zero, //IntPtr lpcbMaxSubKeyLen,
                IntPtr.Zero, //IntPtr lpcbMaxClassLen,
                IntPtr.Zero, //IntPtr lpcValues,
                IntPtr.Zero, //IntPtr lpcbMaxValueNameLen,
                IntPtr.Zero, //IntPtr lpcbMaxValueLen,
                IntPtr.Zero, //IntPtr lpcbSecurityDescriptor,
                IntPtr.Zero //IntPtr lpftLastWriteTime
            );
            if (0 != status)
            {
                Console.WriteLine("RegQueryInfoKey failed");
                return;
            }

            long lpftLastWriteTime;

            for (uint xSubKey = 0; xSubKey < ctSubKeys; ++xSubKey)
            {
                var subKeyName = new StringBuilder(1024);
                var subKeyNameLength = (uint)subKeyName.Capacity;

                int status2 = RegEnumKeyEx(
                    key, //UIntPtr hkey,
                    xSubKey, //uint index,
                    subKeyName, //StringBuilder lpName,
                    ref subKeyNameLength, //ref uint lpcbName,
                    IntPtr.Zero, //IntPtr reserved,
                    IntPtr.Zero, //IntPtr lpClass,
                    IntPtr.Zero, //IntPtr lpcbClass,
                    // IntPtr.Zero //out long lpftLastWriteTime
                    out lpftLastWriteTime
                );

                if (0 != status2)
                {
                    string errorMessage = new Win32Exception(status2).Message;
                    Console.WriteLine("RegEnumKeyEx failed:" + errorMessage);
                    break;
                }
                if (!listLastUpdate.ContainsKey(subKeyName.ToString()))
                {
                    listLastUpdate.Add(subKeyName.ToString(), (long)lpftLastWriteTime);
                }
                
            }
        }

    }
}
