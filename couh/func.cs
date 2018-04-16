using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Security;
using System.Windows.Forms;

using System.Runtime.InteropServices;

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
        public const int columnName = 0;
        public const int columnDate = 1;
        public static int g_Index = 0;

        public static byte[] sortColumnBit = {0x1, 0x2};
        public const byte sortColumnName = 0x1;
        public const byte sortColumnDate = 0x2;
        public const byte sortDescending = 0x80; // ascending:8bit目off,descending:8bit目on
        public const byte sortAscending = 0x0;
        public const byte sortMask = sortDescending | sortColumnName | sortColumnDate;

        public static List<string> searchValueList = new List<string>(){ "DisplayName", "ReleaseType", "SystemComponent", "InstallDate"};
        public static List<string> ignoreList = new List<string>(){ "Hotfix", "Update", "ServicePack", "Security Update" };
        
        public static Dictionary<string, long> listLastUpdate = new Dictionary<string,long>();

        public enum operation
        {
            KEEP,
            HIDE,
            REDISPLAY
        }
    }

    public class regElement
    {
        public string keyName { get; set; }
        public string displayName { get; set; }
        public int bit { get; set; }
        public string update { get; set; }

        public regElement(string keyname, string displayname, int bit, string update)
        {
            this.keyName = keyname;
            this.displayName = displayname;
            this.bit = bit;
            this.update = update;
        }

    }

    class func //:Form
    {
        // FILETIME構造体は 1601 年 1 月 1 日から 100 ナノ秒間隔の数を表す 64 ビット値。
        public static DateTime FILETIME_SECOND = new DateTime(1601, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public Dictionary<int, regElement> GetUninstallList(int bit)
        {
            List<string> regPath = new List<string>();
            var regList = new Dictionary<int, regElement>();
            string displayname;
            string releasetype;
            int? syscom;
            string installDate;

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
                installDate = (string)regValueName.GetValue(Constants.searchValueList[3]);

                if ( ( displayname != null ) &&
                     ( syscom != 1 ) &&
                     !Constants.ignoreList.Contains( releasetype ) )
                {

                    if (installDate == null || installDate == "")
                    {
                        installDate = FILETIME_SECOND.
                        AddSeconds(Constants.listLastUpdate[aryKeyName[i]]/10000000).
                            ToLocalTime().ToString("d");
                    }
                    else
                    {
                        installDate = DateTime.ParseExact(installDate,"yyyyMMdd",null).ToString("d");
                    }

                    regList.Add(Constants.g_Index, new regElement( aryKeyName[i], displayname, bit, installDate ) );
                    Constants.g_Index++;
                }
            }
            return regList;
        }

        public void RefreshDicIndex(ref Dictionary<int, regElement> refDic, byte sortObjNum)
        {

            Dictionary<int,regElement> refDic_Sorted = new Dictionary<int,regElement>();

            Console.WriteLine(Convert.ToString(sortObjNum & Constants.sortMask,2).PadLeft(8,'0'));
            Console.WriteLine(Convert.ToString(Constants.sortAscending | Constants.columnName,2).PadLeft(8,'0'));
            Console.WriteLine(Convert.ToString(Constants.sortAscending | Constants.columnDate,2).PadLeft(8,'0'));
            Console.WriteLine(Convert.ToString(Constants.sortDescending | Constants.columnName,2).PadLeft(8,'0'));
            Console.WriteLine(Convert.ToString(Constants.sortDescending | Constants.columnDate,2).PadLeft(8,'0'));



            switch (sortObjNum & Constants.sortMask)
            {
                case Constants.sortAscending | Constants.sortColumnName :
                    refDic_Sorted = refDic.OrderBy(x => x.Value.displayName).ToDictionary(s => s.Key, s => s.Value);
                    break;

                case Constants.sortAscending | Constants.sortColumnDate :
                    refDic_Sorted = refDic.OrderBy(x => x.Value.update).ToDictionary(s => s.Key, s => s.Value);
                    break;

                case Constants.sortDescending | Constants.sortColumnName :
                    refDic_Sorted = refDic.OrderByDescending(x => x.Value.displayName).ToDictionary(s => s.Key, s => s.Value);
                    break;

                case Constants.sortDescending | Constants.sortColumnDate :
                    refDic_Sorted = refDic.OrderByDescending(x => x.Value.update).ToDictionary(s => s.Key, s => s.Value);
                    break;

                default:
                    break;
            }
            

            var tmp = new Dictionary<int, regElement>();
            int index = 0;

            foreach(KeyValuePair<int, regElement> element in refDic_Sorted)
            {
                tmp.Add(index, new regElement(element.Value.keyName, 
                    element.Value.displayName, 
                    element.Value.bit, 
                    element.Value.update));
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