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
    }

    class func
    {
        public List<Tuple<string, string, int>> GetUninstallList(string path)
        {
            List<string> list64 = new List<string>();
            //List<string> displaynameList64 = new List<string>();
            //Dictionary<string,string> displaynameList64 = new Dictionary<string,string>();
            var displaynameList64 = new List<Tuple<string, string, int>>();
            string displayname;
            string releasetype;
            int? syscom;

            int xbit;

            if (path == Constants.baseKeyName_x64)
            {
                xbit = Constants.x64;
            }
            else
            {
                xbit = Constants.x86;
            }

            RegistryKey regSubkey = Registry.LocalMachine.OpenSubKey(path, false);

            if (regSubkey == null) return null;

            string[] aryKeyName = regSubkey.GetSubKeyNames();

            foreach(string key in aryKeyName)
            {
                list64.Add(path + "\\" + key);
            }

            //foreach(string name in list64)
            for (int i = 0; i < list64.Count; i++)
            {
                RegistryKey regValueName = Registry.LocalMachine.OpenSubKey(list64[i], false);
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
                    displaynameList64.Add( new Tuple<string, string, int>( aryKeyName[i], displayname, xbit ) );
                }
            }
            return displaynameList64;
        }

        public void RefreshDicIndex(ref Dictionary<int, Tuple<string, string>> refDic)
        {
            var aaa = refDic.OrderBy(x => x.Value.Item2);
            //refDic.Clear();

            var tmp = new Dictionary<int, Tuple<string, string>>();
            int index = 0;

            foreach(KeyValuePair<int, Tuple<string, string>> bbb in aaa)
            {
                //refDic.Add(bbb.Key, new Tuple<string, string>(bbb.Value.Item1,bbb.Value.Item2));
                tmp.Add(index, new Tuple<string, string>(bbb.Value.Item1, bbb.Value.Item2));
                index++;
            }
            refDic = tmp;
        }

        public List<Tuple<string, string, int>> GetUninstallList_TEST(string path)
        {
            /*
            const string testcsv = "test.csv";
            string readLine;
            char[] separator = {'='};
            string[] splitted;

            var displaynameList64 = new List<Tuple<string,string,int>>();

            // 非表示プログラム一覧ファイルが存在するか
            if (File.Exists(testcsv))
            {
                // 存在した場合１行ずつ読み込む
                StreamReader sr = new StreamReader(
                    testcsv,
                    Encoding.GetEncoding("Shift_JIS"));
                while( ( readLine = sr.ReadLine() ) != null )
                {
                    // セパレータでサブキーと値の名前に分割し、非表示辞書に設定
                    splitted = readLine.Split(separator);
                    //hideDic.Add(Int32.Parse(splitted[2]), new Tuple<string, string> (splitted[1], splitted[0] ));
                    //hideDic_selected.Add(splitted[1], splitted[0]);
                    displaynameList64.Add( new Tuple<string,string,int>( splitted[0], splitted[1], 0 ) );
                }

                sr.Close();
            }

            return displaynameList64;
            */
            int xbit;
            string folder;

            var regList = new List<Tuple<string, string, int>>();

            if (path == Constants.baseKeyName_x64)
            {
                xbit = Constants.x64;
                folder = "reg_test_64";
            }
            else
            {
                xbit = Constants.x86;
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

                regList.Add(new Tuple<string, string, int>(f.Substring(12), readLine, xbit));

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
