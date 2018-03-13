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
        static char[] separator = {':'};
        string sep_str = new String(separator);
        Index_Dic hideDic = new Index_Dic();
        Index_Dic showList_withIndex = new Index_Dic();
        Index_Dic ShowDic = new Index_Dic();
        Dictionary<string, int> preHideKey = new Dictionary<string,int>();

        List<int> hideIndices = new List<int>();

        func fc = new func();

        public couh()
        {
            InitializeComponent();

            string[] splitted;
            List<string> showList_OutPut = new List<string>();

            Index_Dic uninstList_x64 =
                fc.GetUninstallList(Constants.x64);

            Index_Dic uninstList_x86 =
                fc.GetUninstallList(Constants.x86);

            ShowDic =
                uninstList_x64.Union(uninstList_x86)
                .OrderBy(x => x.Value.Item2)
                .ToDictionary(s => s.Key, s => s.Value);
            
            fc.RefreshDicIndex(ref ShowDic);

            foreach (var subkey in ShowDic)
            {
                showList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
            }

            if (!File.Exists(show_ini))
            {
                //couh_show.iniが存在しなかった場合作成
                StreamWriter sw = new StreamWriter( show_ini, false, Encoding.GetEncoding(Constants.SJIS));

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
                StreamReader sr = new StreamReader( hide_ini, Encoding.GetEncoding(Constants.SJIS));
                string readLine;
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
            }
        }

        private void btnToHide_Click(object sender, EventArgs e)
        {
            int selectNum = lstShow.SelectedIndices.Count;
            int hCount = hideDic.Count;

            for (int i = 0; i < selectNum; i++)
            {
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

        /// <summary>
        /// Applyボタンクリック
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            // applyするキー情報を保持する辞書
            // {Key,(operation, bit)}
            var applyDic = new Dictionary<string, Tuple<int, int>>();

            foreach (var hide in hideDic.Values)
            {
                if (preHideKey.ContainsKey(hide.Item1))
                {
                    applyDic.Add(hide.Item1, new Tuple<int, int>((int)Constants.operation.KEEP, hide.Item3));
                    preHideKey.Remove(hide.Item1);
                }
                else
                {
                    applyDic.Add(hide.Item1, new Tuple<int, int>((int)Constants.operation.HIDE, hide.Item3));
                }
            }

            foreach (var reminder in preHideKey)
            {
                applyDic.Add(reminder.Key, new Tuple<int, int>((int)Constants.operation.REDISPLAY, reminder.Value));
            }



            foreach (var ap in applyDic)
            {
                string regPath = (ap.Value.Item2 == Constants.x64) ? 
                    Constants.baseKeyName_x64 + "\\" + ap.Key :
                    Constants.baseKeyName_x86 + "\\" + ap.Key;

                switch (ap.Value.Item1)
                {
                    case (int)Constants.operation.KEEP:

                        Console.WriteLine("KEEP:{0}",ap.Key);

                        break;
                    case (int)Constants.operation.HIDE:

                        Console.WriteLine("HIDE:{0}",ap.Key);

                        if (!fc.UninstRegOperation(regPath, (int)Constants.operation.HIDE))
                        {
                            applyDic.Clear();
                            return;
                        }

                        break;
                    case (int)Constants.operation.REDISPLAY:

                        Console.WriteLine("REDISPLAY:{0}",ap.Key);

                        if (!fc.UninstRegOperation(regPath, (int)Constants.operation.REDISPLAY))
                        {
                            applyDic.Clear();
                            return;
                        }

                        break;
                    default:
                        break;
                }  
            }
            Console.WriteLine("==============");

            //正常にレジストリ書き換え完了後、初期化を行い、各帳票を出力する

            preHideKey.Clear();

            foreach (var pre in hideDic)
            {
                preHideKey.Add(pre.Value.Item1, pre.Value.Item3);
            }

            applyDic.Clear();

            List<string> showList_OutPut = new List<string>();
            List<string> hideList_OutPut = new List<string>();

            // 非表示プログラム帳票出力用リスト作成
            foreach (var subkey in hideDic)
            {
                hideList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
            }

            // 表示プログラム帳票出力用リスト作成
            foreach (var subkey in ShowDic)
            {
                showList_OutPut.Add(subkey.Value.Item1 + sep_str + subkey.Value.Item2 + sep_str + subkey.Value.Item3);
            }

            // 非表示帳票出力
            StreamWriter hf = new StreamWriter( hide_ini, false, Encoding.GetEncoding(Constants.SJIS));

            foreach (string line in hideList_OutPut)
            {
                hf.WriteLine(line);
            }
            hf.Close();

            // 表示帳票出力
            StreamWriter sf = new StreamWriter(show_ini, false, Encoding.GetEncoding(Constants.SJIS));

            foreach (string line in showList_OutPut)
            {
                sf.WriteLine(line);
            }
            sf.Close();

            MessageBox.Show("設定完了");

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
                    MessageBox.Show("Key: " + ShowDic[lstShow.SelectedIndex].Item1);
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
                    MessageBox.Show("Key: " + hideDic[lstHide.SelectedIndex].Item1);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
