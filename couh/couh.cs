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
//using static couh.Constants; C# 5では使えない

namespace couh
{
    // <index,Tuple<key, displayname, bit, update>>
    using Index_Dic = Dictionary<int, regElement>;

    public partial class couh : Form
    {
        const string show_ini = @"couh_show.ini";
        const string hide_ini = @"couh_hide.ini";
        Dictionary<SortOrder, byte> dicDirectionBit = 
            new Dictionary<SortOrder,byte>{{SortOrder.Ascending, Constants.sortAscending},
            {SortOrder.Descending, Constants.sortDescending}};
        static char[] separator = {'㍻'};
        string sep_str = new String(separator);
        Index_Dic hideDic = new Index_Dic();
        Index_Dic showList_withIndex = new Index_Dic();
        Index_Dic ShowDic = new Index_Dic();
        Dictionary<string, int> preHideKey = new Dictionary<string,int>();

        byte showSortDirection = Constants.sortAscending | Constants.sortColumnName;
        byte hideSortDirection = Constants.sortAscending | Constants.sortColumnName;


        ListSortDirection sortDirection = ListSortDirection.Ascending;

        List<int> hideIndices = new List<int>();

        func fc = new func();
        reg regcon = new reg();

        private void couh_Shown(object sender, EventArgs e)
        {
            dgvShow.CurrentCell = null;
            dgvHide.CurrentCell = null;
            System.Reflection.Assembly     assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName asmName  = assembly.GetName();
            System.Version                 version  = asmName.Version;
 
            labelVer.Text = "Ver " + version.Major + "." + version.MinorRevision;

            // dataGridView1 の すべてのカラムで ソート を 無効化
            //foreach (DataGridViewColumn column in this.dgvShow.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}
        }

        public couh()
        {
            InitializeComponent();

            foreach (DataGridViewColumn c in dgvShow.Columns) c.SortMode = DataGridViewColumnSortMode.Automatic;

            // iniファイルから読み込んだ各要素（key,value,bit,update）
            string[] splitted;
            // show.ini出力用
            List<string> showList_OutPut = new List<string>();
            
            regcon.GetSubKeyLastUpdate(ref Constants.listLastUpdate, Constants.baseKeyName_x64);
            regcon.GetSubKeyLastUpdate(ref Constants.listLastUpdate, Constants.baseKeyName_x86);

            // x64アンインストール情報取得
            Index_Dic uninstList_x64 = fc.GetUninstallList(Constants.x64);
            // x86アンインストール情報取得
            Index_Dic uninstList_x86 = fc.GetUninstallList(Constants.x86);

            // アンインストール情報をマージしてソート
            ShowDic =
                uninstList_x64.Union(uninstList_x86)
                .OrderBy(x => x.Value.displayName)
                .ToDictionary(s => s.Key, s => s.Value);
            
            // インデックス振り直し
            fc.RefreshDicIndex(ref ShowDic, showSortDirection);

            //couh_show.iniが存在しなかった場合作成
            if (!File.Exists(show_ini))
            {
                // show.ini出力用に整形
                foreach (var subkey in ShowDic)
                {
                    showList_OutPut.Add(subkey.Value.keyName + sep_str + subkey.Value.displayName + sep_str + subkey.Value.bit);
                }

                StreamWriter sw = new StreamWriter( show_ini, false, Encoding.GetEncoding(Constants.SJIS));

                foreach (string line in showList_OutPut)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
            }

            // 表示情報をリストに表示
            foreach (var line in ShowDic)
            {
                dgvShow.Rows.Add(new string[]{line.Value.displayName, line.Value.update});
            }

            //dataGridView1.DataSource = Test;
            dgvShow.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

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
                    hideDic.Add( index, new regElement(splitted[0], splitted[1], int.Parse(splitted[2]), splitted[3]) );
                    preHideKey.Add(splitted[0], int.Parse(splitted[2]));
                    index++;
                }

                sr.Close();

                // 非表示リストを表示
                foreach(regElement name in hideDic.Values)
                {
                    dgvHide.Rows.Add(new string[]{name.displayName, name.update});
                }
            }
            dgvShow.CurrentCell = null;
            dgvHide.CurrentCell = null;
        }

        /// <summary>
        /// 非表示リストに追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToHide_Click(object sender, EventArgs e)
        {
            if(dgvShow.CurrentCell == null) return;

            fc.RefreshDicIndex(ref ShowDic, showSortDirection);
            fc.RefreshDicIndex(ref hideDic, hideSortDirection);

            // 非表示リストに追加する項目数
            //int selectNum = lstShow.SelectedIndices.Count;
            int selectNum = dgvShow.SelectedRows.Count;
            // 既に非表示リストに入っている項目数
            int hCount = hideDic.Count;

            foreach (DataGridViewRow selectCell in dgvShow.SelectedRows)
            {
                hideDic.Add(hCount++,
                    new regElement (ShowDic[selectCell.Index].keyName,
                                                    ShowDic[selectCell.Index].displayName, 
                                                    ShowDic[selectCell.Index].bit,
                                                    ShowDic[selectCell.Index].update));
                ShowDic.Remove(selectCell.Index);

                dgvHide.Rows.Add(dgvShow.Rows[selectCell.Index].Cells[0].Value.ToString(),
                    dgvShow.Rows[selectCell.Index].Cells[1].Value.ToString());
                dgvShow.Rows.Remove(selectCell);
            }
            fc.RefreshDicIndex(ref ShowDic, showSortDirection);
            fc.RefreshDicIndex(ref hideDic, hideSortDirection);

            DataGridViewColumn sortColumn = dgvHide.CurrentCell.OwningColumn;

            //並び替えの方向（昇順か降順か）を決める
            if (dgvHide.SortedColumn != null &&
                dgvHide.SortedColumn.Equals(sortColumn))
            {
                sortDirection =
                    dgvHide.SortOrder == SortOrder.Ascending ?
                    ListSortDirection.Ascending : ListSortDirection.Descending;
            }

            //並び替えを行う
            dgvHide.Sort(sortColumn, sortDirection);

            dgvShow.CurrentCell = null;
            dgvHide.CurrentCell = null;
        }
        /// <summary>
        /// 表示リストに追加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToShow_Click(object sender, EventArgs e)
        {
            if(dgvHide.CurrentCell == null) return;

            fc.RefreshDicIndex(ref ShowDic, showSortDirection);
            fc.RefreshDicIndex(ref hideDic, hideSortDirection);

            // 非表示リストに追加する項目数
            int selectNum = dgvShow.SelectedRows.Count;
            // 既に非表示リストに入っている項目数
            int sCount = ShowDic.Count;

            foreach (DataGridViewRow selectCell in dgvHide.SelectedRows)
            {
                // （非）表示リストの更新
                ShowDic.Add(sCount++,
                    new regElement (hideDic[selectCell.Index].keyName,
                                                    hideDic[selectCell.Index].displayName, 
                                                    hideDic[selectCell.Index].bit,
                                                    hideDic[selectCell.Index].update));
                hideDic.Remove(selectCell.Index);

                dgvShow.Rows.Add(dgvHide.Rows[selectCell.Index].Cells[0].Value.ToString(),
                    dgvHide.Rows[selectCell.Index].Cells[1].Value.ToString());
                dgvHide.Rows.Remove(selectCell);
            }
            fc.RefreshDicIndex(ref ShowDic, showSortDirection);
            fc.RefreshDicIndex(ref hideDic, hideSortDirection);
            
            DataGridViewColumn sortColumn = dgvShow.CurrentCell.OwningColumn;

            //並び替えの方向（昇順か降順か）を決める
            if (dgvShow.SortedColumn != null &&
                dgvShow.SortedColumn.Equals(sortColumn))
            {
                sortDirection =
                    dgvShow.SortOrder == SortOrder.Ascending ?
                    ListSortDirection.Ascending : ListSortDirection.Descending;
            }

            //並び替えを行う
            dgvShow.Sort(sortColumn, sortDirection);
            dgvShow.CurrentCell = null;
            dgvHide.CurrentCell = null;
        }

        /// <summary>
        /// Applyボタンクリック
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "非表示にしますか？",
                "確認",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Exclamation,
                MessageBoxDefaultButton.Button1);

            if (result == DialogResult.Cancel) return;

            // applyするキー情報を保持する辞書
            // {Key,(operation, bit)}
            var applyDic = new Dictionary<string, Tuple<int, int>>();

            // ハイド情報のそれぞれに操作情報をつけてapplyに設定
            foreach (var hide in hideDic.Values)
            {
                if (preHideKey.ContainsKey(hide.keyName))
                {
                    // 起動時 or apply後のハイド情報に今のハイド情報があれば操作しない 
                    applyDic.Add(hide.keyName, new Tuple<int, int>((int)Constants.operation.KEEP, hide.bit));
                    preHideKey.Remove(hide.keyName);
                }
                else
                {
                    // なければ新参なので非表示する
                    applyDic.Add(hide.keyName, new Tuple<int, int>((int)Constants.operation.HIDE, hide.bit));
                }
            }

            foreach (var reminder in preHideKey)
            {
                // KEEPでもHIDEでもないプレハイドに残ったものは再表示する
                applyDic.Add(reminder.Key, new Tuple<int, int>((int)Constants.operation.REDISPLAY, reminder.Value));
            }

            foreach (var ap in applyDic)
            {
                // レジストリパスを設定
                string regPath = (ap.Value.Item2 == Constants.x64) ? 
                    Constants.baseKeyName_x64 + "\\" + ap.Key :
                    Constants.baseKeyName_x86 + "\\" + ap.Key;

                // applyの持つ操作情報に依って実行
                switch (ap.Value.Item1)
                {
                    case (int)Constants.operation.KEEP:
                        // KEEPは何もしない
                        Console.WriteLine("KEEP:{0}",ap.Key);

                        break;
                    case (int)Constants.operation.HIDE:
                        // HIDEは非表示にする
                        Console.WriteLine("HIDE:{0}",ap.Key);

                        if (!fc.UninstRegOperation(regPath, (int)Constants.operation.HIDE))
                        {   
                            // レジストリ操作でエラーがあった場合はapplyを削除する
                            applyDic.Clear();
                            return;
                        }

                        break;
                    case (int)Constants.operation.REDISPLAY:
                        // 再表示する
                        Console.WriteLine("REDISPLAY:{0}",ap.Key);

                        if (!fc.UninstRegOperation(regPath, (int)Constants.operation.REDISPLAY))
                        {
                            // レジストリ操作でエラーがあった場合はapplyを削除する
                            applyDic.Clear();
                            return;
                        }

                        break;
                    default:
                        break;
                }  
            }
            Console.WriteLine("==============");

            // 正常にレジストリ書き換え完了後、初期化を行い、各帳票を出力する

            preHideKey.Clear();

            foreach (var pre in hideDic)
            {
                preHideKey.Add(pre.Value.keyName, pre.Value.bit);
            }

            applyDic.Clear();

            List<string> showList_OutPut = new List<string>();
            List<string> hideList_OutPut = new List<string>();

            // 非表示プログラム帳票出力用リスト作成
            foreach (var subkey in hideDic)
            {
                hideList_OutPut.Add(subkey.Value.keyName + sep_str + subkey.Value.displayName + sep_str + subkey.Value.bit);
            }

            // 表示プログラム帳票出力用リスト作成
            foreach (var subkey in ShowDic)
            {
                showList_OutPut.Add(subkey.Value.keyName + sep_str + subkey.Value.displayName + sep_str + subkey.Value.bit);
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

        private void dgvShow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvShow.CurrentRow.Index != -1 && e.RowIndex >= 0)
                {
                    MessageBox.Show("Key: " + ShowDic[dgvShow.CurrentRow.Index].keyName);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvHide_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvHide.CurrentRow.Index != -1 && e.RowIndex >= 0)
                {
                    MessageBox.Show("Key: " + hideDic[dgvHide.CurrentRow.Index].keyName);
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvShow_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //MessageBox.Show(dgvShow.SortOrder.ToString());
            //MessageBox.Show(e.ColumnIndex.ToString());

            showSortDirection = 0x0;
            showSortDirection |= (byte)(dicDirectionBit[dgvShow.SortOrder] | Constants.sortColumnBit[e.ColumnIndex]);
            fc.RefreshDicIndex(ref ShowDic, showSortDirection);
            Console.WriteLine("{0} {1}",Convert.ToString(showSortDirection,2).PadLeft(8,'0'),dgvShow.SortOrder.ToString());
                //// show.ini出力用に整形
                //        List<string> showList_OutPut = new List<string>();

                //foreach (var subkey in ShowDic)
                //{
                //    showList_OutPut.Add(subkey.Value.displayName+","+subkey.Value.update);
                //}

                //StreamWriter sw = new StreamWriter( "test.csv", false, Encoding.GetEncoding(Constants.SJIS));

                //foreach (string line in showList_OutPut)
                //{
                //    sw.WriteLine(line);
                //}
                //sw.Close();
        }

        private void dgvHide_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            hideSortDirection = 0x0;
            hideSortDirection |= (byte)(dicDirectionBit[dgvHide.SortOrder] | Constants.sortColumnBit[e.ColumnIndex]);
            fc.RefreshDicIndex(ref hideDic, hideSortDirection);
            Console.WriteLine("{0} {1}",Convert.ToString(hideSortDirection,2).PadLeft(8,'0'),dgvHide.SortOrder.ToString());

        }

        private void dgvShow_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            //指定されたセルの値を文字列として取得する
            //string str1 = (e.RowIndex1 == null ? "" : e.RowIndex1.ToString());
            //string str2 = (e.RowIndex2 == null ? "" : e.RowIndex2.ToString());

            string str1 = dgvShow.Rows[e.RowIndex1].Cells[1].Value.ToString();
            string str2 = dgvShow.Rows[e.RowIndex2].Cells[1].Value.ToString();


            //結果を代入
            e.SortResult = String.Compare(str1,str2,true);
            //処理したことを知らせる
            e.Handled = true;
        }


    }
}
