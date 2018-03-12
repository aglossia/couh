using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
//using static couh.Constants;

namespace couh
{
    using Index_Dic = Dictionary<int, Tuple<string, string, int>>;

    public partial class couh : Form
    {
        const string show_ini = @"couh_show.ini";
        const string hide_ini = @"couh_hide.ini";
        
        const int KEEP = 0;
        const int HIDE = 1;
        const int REDISPLAY = 2;
        
        static char[] separator = {':'};
        string sep_str = new String(separator);

        Index_Dic hideDic = new Index_Dic();
        Index_Dic showList_withIndex = new Index_Dic();

        Index_Dic ShowDic = new Index_Dic();

        //Tuple<string, string, int> ShowDic = new Tuple<string,string,int>();

        //IOrderedEnumerable<KeyValuePair<int,Tuple<string,string,int>>> ShowDic ;

        Dictionary<string, int> preHideKey = new Dictionary<string,int>();

        //Dictionary<string, string> hideDic_selected = new Dictionary<string, string>();
        List<int> hideIndices = new List<int>();
        //List<string> preHideKey = new List<string>();



        func fc = new func();

        public couh()
        {
            InitializeComponent();

            string[] splitted;
            List<string> showList_OutPut = new List<string>();
            //List<string> hideList = new List<string>();
            string readLine;

            /*
            var uninstList_x64 = 
                fc.GetUninstallList(baseKeyName_x64).
                ToDictionary(s => s.Item1, s=> s.Item2);

            var uninstList_x86 = 
                fc.GetUninstallList(baseKeyName_x86).
                ToDictionary(s => s.Item1, s=> s.Item2);
            */

            Index_Dic uninstList_x64 =
                fc.GetUninstallList(Constants.x64);

            Index_Dic uninstList_x86 =
                fc.GetUninstallList(Constants.x86);

            ShowDic =
                uninstList_x64.Union(uninstList_x86)
                .OrderBy(x => x.Value.Item2)
                .ToDictionary(s => s.Key, s => s.Value);
            


//            var unionShowDic_Sorted = unionShowDic.OrderBy(x => x.Value);
            
            //int i = 0;

            foreach (var subkey in ShowDic)
            {
                showList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
                //showList_withIndex.Add(i, new Tuple<string, string, int>(subkey.Key.Item1,subkey.Value,subkey.Key.Item2));
                //i++;
            }

            if (!File.Exists(show_ini))
            {
                //couh_show.iniが存在しなかった場合作成
                StreamWriter sw = new StreamWriter(
                show_ini,
                false,
                System.Text.Encoding.GetEncoding("shift_jis"));

                foreach (string line in showList_OutPut)
                {
                    sw.WriteLine(line);
                }

                sw.Close();

            }

            foreach (var line in ShowDic)
            {
                lstShow.Items.Add(line.Value.Item2);
            }

            // 非表示プログラム一覧ファイルが存在するか
            if (File.Exists(hide_ini))
            {
                int index = 0;
                // 存在した場合１行ずつ読み込む
                StreamReader sr = new StreamReader(
                    hide_ini,
                    Encoding.GetEncoding("Shift_JIS"));
                while( ( readLine = sr.ReadLine() ) != null )
                {
                    // セパレータでサブキーと値の名前に分割し、非表示辞書に設定
                    splitted = readLine.Split(separator);
                    hideDic.Add( index, new Tuple<string, string, int>(splitted[0], splitted[1], int.Parse(splitted[2]) ) );
                    preHideKey.Add(splitted[0], int.Parse(splitted[2]));
                    index++;
                }

                sr.Close();

                foreach(Tuple<string, string, int> name in hideDic.Values)
                {
                    lstHide.Items.Add(name.Item2);
                }

                //lstHide.Sorted = true;
            }
        }

        private void btnToHide_Click(object sender, EventArgs e)
        {
            int selectNum = lstShow.SelectedIndices.Count;
            int hCount = hideDic.Count;

            for (int i = 0; i < selectNum; i++)
            {
                //hideIndices.Add(lstShow.SelectedIndices[i]);

                //preHideKey.Add(showList_withIndex[lstShow.SelectedIndices[i]].Item1);

                hideDic.Add(hCount + i, new Tuple<string, string, int> (ShowDic[lstShow.SelectedIndices[i]].Item1,
                    ShowDic[lstShow.SelectedIndices[i]].Item2, ShowDic[lstShow.SelectedIndices[i]].Item3));

                ShowDic.Remove(lstShow.SelectedIndices[i]);
            }

            fc.RefreshDicIndex(ref ShowDic);
            fc.RefreshDicIndex(ref hideDic);

            for (int i = 0; i < selectNum; i++)
            {
                lstHide.Items.Add(lstShow.SelectedItems[0]);
                lstShow.Items.RemoveAt(lstShow.SelectedIndices[0]);

                lstHide.Sorted = true;
                lstShow.Sorted = true;
                
            }
        }

        private void btnToShow_Click(object sender, EventArgs e)
        {
            int selectNum = lstHide.SelectedIndices.Count;
            int sCount = ShowDic.Count;

            for (int i = 0; i < selectNum; i++)
            {

                ShowDic.Add(sCount + i, new Tuple<string, string, int>(hideDic[lstHide.SelectedIndices[i]].Item1,
                    hideDic[lstHide.SelectedIndices[i]].Item2, hideDic[lstHide.SelectedIndices[i]].Item3));

                hideDic.Remove(lstHide.SelectedIndices[i]);
            }

            fc.RefreshDicIndex(ref ShowDic);
            fc.RefreshDicIndex(ref hideDic);
            
            for (int i = 0; i < selectNum; i++)
            {
                lstShow.Items.Add(lstHide.SelectedItems[0]);
                lstHide.Items.RemoveAt(lstHide.SelectedIndices[0]);

                lstShow.Sorted = true;
                lstHide.Sorted = true;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            var applyDic = new Dictionary<string, Tuple<int, int>>(); // {Key,(operation, bit)}

            List<string> showList_OutPut = new List<string>();
            List<string> hideList_OutPut = new List<string>();

            foreach (var subkey in hideDic)
            {
                hideList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
            }

            foreach (var subkey in ShowDic)
            {
                showList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
            }

            StreamWriter sw = new StreamWriter(
            hide_ini,
            false,
            System.Text.Encoding.GetEncoding("shift_jis"));

            foreach (string line in hideList_OutPut)
            {
                sw.WriteLine(line);
            }

            sw.Close();

            StreamWriter sw2 = new StreamWriter(
            show_ini,
            false,
            System.Text.Encoding.GetEncoding("shift_jis"));

            foreach (string line in showList_OutPut)
            {
                sw2.WriteLine(line);
            }
            
            sw2.Close();

            foreach (var hide in hideDic.Values)
            {
                if (preHideKey.ContainsKey(hide.Item1))
                {
                    applyDic.Add(hide.Item1, new Tuple<int, int>(KEEP, hide.Item3));
                    preHideKey.Remove(hide.Item1);
                }
                else
                {
                    applyDic.Add(hide.Item1, new Tuple<int, int>(HIDE, hide.Item3));
                }
            }

            foreach (var reminder in preHideKey)
            {
                applyDic.Add(reminder.Key, new Tuple<int, int>(REDISPLAY, reminder.Value));
            }

            preHideKey.Clear();

            foreach (var pre in hideDic)
            {
                preHideKey.Add(pre.Value.Item1, pre.Value.Item3);
            }

            foreach (var s in applyDic)
            {
                /***************TEST
                */
                string directorypath;

                if (s.Value.Item2 == Constants.x64)
                {
                    directorypath = "reg_test_64//" + s.Key;
                }else
                {
                    directorypath = "reg_test_86//" + s.Key;
                }

                /********TEST******/

                /********本番*****/

                //レジストリパスを設定する

                /********本番******/

                Dictionary<string, string> refList = new Dictionary<string, string>(){
                    {"key_64_00.txt","64_00test"},
                    {"key_64_01.txt","64_01test"},
                    {"key_64_02.txt","64_02test"},
                    {"key_64_03.txt","64_03test"},
                    {"key_64_04.txt","64_04test"},
                    {"key_64_05.txt","64_05test"},
                    {"key_64_06.txt","64_06test"},
                    {"key_64_07.txt","64_07test"},
                    {"key_64_08.txt","64_08test"},
                    {"key_64_09.txt","64_09test"},
                    {"key_64_10.txt","64_10test"},
                    {"key_86_00.txt","86_00test"},
                    {"key_86_01.txt","86_01test"},
                    {"key_86_02.txt","86_02test"},
                    {"key_86_03.txt","86_03test"},
                    {"key_86_04.txt","86_04test"},
                    {"key_86_05.txt","86_05test"},
                    {"key_86_06.txt","86_06test"},
                    {"key_86_07.txt","86_07test"},
                    {"key_86_08.txt","86_08test"},
                    {"key_86_09.txt","86_09test"},
                    {"key_86_10.txt","86_10test"}
                };

                /********TEST******/                
                /******
                ******/
                switch (s.Value.Item1)
                {
                    case KEEP:
                        Console.WriteLine("KEEP:{0}",s.Key);

                        break;
                    case HIDE:
                        Console.WriteLine("HIDE:{0}",s.Key);
                        StreamWriter f2 = new StreamWriter(
                                    directorypath,
                                    false,
                                    System.Text.Encoding.GetEncoding("shift_jis"));

                        //f2.WriteLine(0x1a);

                        f2.Close();
                        break;
                    case REDISPLAY:
                        Console.WriteLine("REDISPLAY:{0}",s.Key);
                        StreamWriter f1 = new StreamWriter(
                                    directorypath,
                                    false,
                                    System.Text.Encoding.GetEncoding("shift_jis"));

                        f1.WriteLine(refList[s.Key]);

                        f1.Close();
                        break;
                    default:
                        break;
                }
                
            }
            Console.WriteLine("==============");
            applyDic.Clear();

            /********本番*******/

            //systemcompornentの設定を行う

            /********本番*******/


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lstShow_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstShow.SelectedIndex != -1)
                {
                    MessageBox.Show(showList_withIndex[lstShow.SelectedIndex].Item1);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void lstHide_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (lstHide.SelectedIndex != -1)
                {
                    MessageBox.Show(hideDic[lstHide.SelectedIndex].Item1);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
