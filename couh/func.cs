using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace couh
{
    static class Constants
    {
        public const string baseKeyName_x64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string baseKeyName_x86 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string systemComponent = @"SystemComponent";
        public const string SJIS = "Shift_JIS";
        public const int x64 = 0;
        public const int x86 = 1;
        public static int g_Index = 0;

        public static List<string> searchValueList = new List<string>(){ "DisplayName", "ReleaseType", "SystemComponent" };
        public static List<string> ignoreList = new List<string>(){ "Hotfix", "Update", "ServicePack", "Security Update" };

        public enum operation
        {
            KEEP,
            HIDE,
            REDISPLAY
        }
    }

    class func :Form
    {
        public Dictionary<int, Tuple<string, string, int>> GetUninstallList(int bit)
        {
            List<string> regPath = new List<string>();
            var regList = new Dictionary<int, Tuple<string, string, int>>();
            string displayname;
            string releasetype;
            int? syscom;

            string keyName = (bit == Constants.x64) ? Constants.baseKeyName_x64 : Constants.baseKeyName_x86;

            RegistryKey regSubkey = Registry.LocalMachine.OpenSubKey(keyName, false);

            if (regSubkey == null) return null;

            string[] aryKeyName = regSubkey.GetSubKeyNames();

            foreach(string key in aryKeyName)
            {
                regPath.Add(keyName + "\\" + key);
            }

            for (int i = 0; i < regPath.Count; i++)
            {
                RegistryKey regValueName = Registry.LocalMachine.OpenSubKey(regPath[i], false);
                if(regValueName == null) return null;

                displayname = (string)regValueName.GetValue(Constants.searchValueList[0]);
                releasetype = (string)regValueName.GetValue(Constants.searchValueList[1]);
                syscom = (int?)regValueName.GetValue(Constants.searchValueList[2]);

                if ( ( displayname != null ) &&
                     ( syscom != 1 ) &&
                     !Constants.ignoreList.Contains( releasetype ) )
                {
                    regList.Add(Constants.g_Index, new Tuple<string, string, int>( aryKeyName[i], displayname, bit ) );
                    Constants.g_Index++;
                }
            }
            return regList;
        }

        public void RefreshDicIndex(ref Dictionary<int, Tuple<string, string, int>> refDic)
        {
            var refDic_Sorted = refDic.OrderBy(x => x.Value.Item2);

            var tmp = new Dictionary<int, Tuple<string, string, int>>();
            int index = 0;

            foreach(KeyValuePair<int, Tuple<string, string, int>> element in refDic_Sorted)
            {
                tmp.Add(index, new Tuple<string, string, int>(element.Value.Item1, element.Value.Item2, element.Value.Item3));
                index++;
            }
            refDic = tmp;
        }

        public bool UninstRegOperation(string regPath, int ope)
        {
            try
            {
                RegistryKey regkey = Registry.LocalMachine.OpenSubKey(regPath, true);

                switch (ope)
                {
                    case (int)Constants.operation.HIDE:
                        regkey.SetValue(Constants.systemComponent, 1, RegistryValueKind.DWord);
                        break;
                    case (int)Constants.operation.REDISPLAY:
                        regkey.DeleteValue(Constants.systemComponent);
                        break;
                    default:
                        // 非表示と表示の操作しかないため通常は入らない
                        return false;
                }
                return true;
            }
            catch (SecurityException ex)
            {
                MessageBox.Show(ex.Message + "\n管理者権限でプログラムを実行してください。");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }
    }
}