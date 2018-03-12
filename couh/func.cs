using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;

namespace couh
{
    static class Constants
    {
        public const string baseKeyName_x64 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
        public const string baseKeyName_x86 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public const int x64 = 0;
        public const int x86 = 1;
        public static int g_Index = 0;
    }

    class func
    {
        public Dictionary<int, Tuple<string, string, int>> GetUninstallList(int bit)
        {

            string keyName = "";
            List<string> regPath = new List<string>();
            var regList = new Dictionary<int, Tuple<string, string, int>>();
            string displayname;
            string releasetype;
            int? syscom;

            if (bit == Constants.x64)
            {
                keyName = Constants.baseKeyName_x64;
            }
            else
            {
                keyName = Constants.baseKeyName_x86;
            }

            RegistryKey regSubkey = Registry.LocalMachine.OpenSubKey(keyName, false);

            if (regSubkey == null) return null;

            string[] aryKeyName = regSubkey.GetSubKeyNames();

            foreach(string key in aryKeyName)
            {
                regPath.Add(keyName + "\\" + key);
            }

            //foreach(string name in list64)
            for (int i = 0; i < regPath.Count; i++)
            {
                RegistryKey regValueName = Registry.LocalMachine.OpenSubKey(regPath[i], false);
                if(regValueName == null) return null;

                displayname = (string)regValueName.GetValue("DisplayName");
                releasetype = (string)regValueName.GetValue("ReleaseType");
                syscom = (int?)regValueName.GetValue("SystemComponent");

                if( ( displayname != null ) &&
                    ( syscom != 1 ) && 
                    ( releasetype != "Hotfix" ) && 
                    ( releasetype != "Update" ) &&
                    ( releasetype != "ServicePack" ) &&
                    ( releasetype != "Security Update" ) )
                {
                    regList.Add(Constants.g_Index, new Tuple<string, string, int>( aryKeyName[i], displayname, bit ) );
                    Constants.g_Index++;
                }
            }
            return regList;
        }

        public void RefreshDicIndex(ref Dictionary<int, Tuple<string, string, int>> refDic)
        {
            var aaa = refDic.OrderBy(x => x.Value.Item2);
            //refDic.Clear();

            var tmp = new Dictionary<int, Tuple<string, string, int>>();
            int index = 0;

            foreach(KeyValuePair<int, Tuple<string, string, int>> bbb in aaa)
            {
                //refDic.Add(bbb.Key, new Tuple<string, string>(bbb.Value.Item1,bbb.Value.Item2));
                tmp.Add(index, new Tuple<string, string, int>(bbb.Value.Item1, bbb.Value.Item2, bbb.Value.Item3));
                index++;
            }
            refDic = tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">64bit、32bitのパス</param>
        /// <returns>(string,string,int) = (Key,Value,64bit or 32bit)</returns>
        //public List<Tuple<string, string, int>> GetUninstallList_TEST(string path)
        public Dictionary<int, Tuple<string, string, int>> GetUninstallList_TEST(int bit)
        {

            string folder = "";

            var regList = new Dictionary<int, Tuple<string, string, int>>();

            if (bit == Constants.x64)
            {
                folder = "reg_test_64";
            }
            else
            {
                folder = "reg_test_86";
            }

            string readLine = "";

            string[] files = System.IO.Directory.GetFiles(
                folder, "*", System.IO.SearchOption.AllDirectories);

            foreach (var f in files)
            {
                if (File.Exists(f))
                {
                    // 存在した場合１行ずつ読み込む
                    StreamReader sr = new StreamReader(
                        f,
                        Encoding.GetEncoding("Shift_JIS"));

                    readLine = sr.ReadLine();

                    sr.Close();
                }
                if (readLine == null)
                {
                    continue;
                }

                regList.Add(Constants.g_Index, new Tuple<string, string, int>(Path.GetFileName(f), readLine, bit));

                //Constants.g_Index++;
                Constants.g_Index++;
            }
            return regList;

        }

        public void UninstallHiding(string hidekey)
        {

        }

        public void UninstallHiding_TEST(string hidekey)
        {
            string[] files = System.IO.Directory.GetFiles(
            @"reg_test_64", "*", System.IO.SearchOption.AllDirectories);
            foreach (var i in files)
            {
                Console.WriteLine(i);
            }
        }
    }
}