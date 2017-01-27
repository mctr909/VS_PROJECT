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

namespace RPGツクールネタ帳
{
    public partial class Form1 : Form
    {
        private int CurSeqNo;
        private string SelectedSeq;

        /// <summary>
        /// キャラクターリストの項目ID
        /// </summary>
        private struct CharaListColID
        {
            /// <summary>キー</summary>
            public const string KeySeq = "KeySeq";
            /// <summary>キャラクター名</summary>
            public const string CharacterName = "キャラクター名";
            /// <summary>出身</summary>
            public const string Shusshin = "出身";
            /// <summary>職業</summary>
            public const string Shokugyou = "職業";
            /// <summary>年齢</summary>
            public const string Nenrei = "年齢";
            /// <summary>顔グラ</summary>
            public const string FaceGraphic = "顔グラ";
            /// <summary>自由項目</summary>
            public const string FreeWord = "自由項目";
            /// <summary>「詳細を開く」ボタン</summary>
            public const string BtnDetail = "詳細を開く";
            /// <summary>「削除」ボタン</summary>
            public const string BtnDelete = "削除";
        }

        public Form1()
        {
            InitializeComponent();
            SelectedSeq = "";
        }

        #region メニューバー
        /// <summary>
        /// メニューバー新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新規作成ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// メニューバー開く
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// メニューバー上書き保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// メニューバー名前を付けて保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "RPGツクールネタ帳(*.rpc)|*.rpc";
            saveFileDialog1.ShowDialog();
            string fileName = saveFileDialog1.FileName;

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);
            fs.Close();
        }
        #endregion

        #region グリッドビュー
        /// <summary>
        /// キャラクターの追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCharacter_Click(object sender, EventArgs e)
        {
            int iRow = 0;

            if (dataGridView1.Rows.Count == 0)
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Name = CharaListColID.KeySeq;
                col.Visible = false;
                dataGridView1.Columns.Add(col);

                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageCol.Name = CharaListColID.FaceGraphic;
                imageCol.HeaderText = CharaListColID.FaceGraphic;
                imageCol.DefaultCellStyle.NullValue = null;
                dataGridView1.Columns.Add(imageCol);

                DataGridViewTextBoxColumn colChar = new DataGridViewTextBoxColumn();
                colChar.Name = CharaListColID.CharacterName;
                colChar.HeaderText = CharaListColID.CharacterName;
                colChar.Width = 320;
                dataGridView1.Columns.Add(colChar);

                DataGridViewTextBoxColumn colShusshin = new DataGridViewTextBoxColumn();
                colShusshin.Name = CharaListColID.Shusshin;
                colShusshin.Visible = false;
                dataGridView1.Columns.Add(colShusshin);

                DataGridViewTextBoxColumn colShokugyou = new DataGridViewTextBoxColumn();
                colShokugyou.Name = CharaListColID.Shokugyou;
                colShokugyou.Visible = false;
                dataGridView1.Columns.Add(colShokugyou);

                DataGridViewTextBoxColumn colNenrei = new DataGridViewTextBoxColumn();
                colNenrei.Name = CharaListColID.Nenrei;
                colNenrei.Visible = false;
                dataGridView1.Columns.Add(colNenrei);

                DataGridViewTextBoxColumn colFreeWord = new DataGridViewTextBoxColumn();
                colFreeWord.Name = CharaListColID.FreeWord;
                colFreeWord.Visible = false;
                dataGridView1.Columns.Add(colFreeWord);

                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                btnCol.Name = CharaListColID.BtnDetail;
                btnCol.Text = CharaListColID.BtnDetail;
                btnCol.HeaderText = "";
                btnCol.Width = 80;
                btnCol.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnCol);

                btnCol = new DataGridViewButtonColumn();
                btnCol.Name = CharaListColID.BtnDelete;
                btnCol.Text = CharaListColID.BtnDelete;
                btnCol.HeaderText = "";
                btnCol.Width = 50;
                btnCol.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnCol);
            }
            else
            {
                iRow = dataGridView1.Rows.Add();
            }

            dataGridView1.Rows[iRow].Height = 64;
            dataGridView1.Rows[iRow].Cells[CharaListColID.KeySeq].Value = CurSeqNo.ToString("00000000");
            dataGridView1.Rows[iRow].Cells[CharaListColID.FaceGraphic].Value = null;

            DataGridViewCellStyle styleChara = new DataGridViewCellStyle();
            styleChara.Font = new Font("Meiryo UI", 16.0f, FontStyle.Regular);
            dataGridView1.Rows[iRow].Cells[CharaListColID.CharacterName].ReadOnly = true;
            dataGridView1.Rows[iRow].Cells[CharaListColID.CharacterName].Value = "";
            dataGridView1.Rows[iRow].Cells[CharaListColID.CharacterName].Style = styleChara;

            dataGridView1.Rows[iRow].Cells[CharaListColID.Shusshin].Value = "";
            dataGridView1.Rows[iRow].Cells[CharaListColID.Shokugyou].Value = "";
            dataGridView1.Rows[iRow].Cells[CharaListColID.Nenrei].Value = "";
            dataGridView1.Rows[iRow].Cells[CharaListColID.FreeWord].Value = "";
            dataGridView1.Rows[iRow].Cells[CharaListColID.BtnDetail].Value = CharaListColID.BtnDetail;
            dataGridView1.Rows[iRow].Cells[CharaListColID.BtnDelete].Value = CharaListColID.BtnDelete;

            CurSeqNo++;
        }

        /// <summary>
        /// リストのコントロールをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // 詳細を開くボタン
            if (CharaListColID.BtnDetail == dgv.Columns[e.ColumnIndex].Name)
            {
                DataGridViewRow curRow = dataGridView1.Rows[e.RowIndex];
                SelectedSeq = curRow.Cells[CharaListColID.KeySeq].Value as string;
                txtCharacter.Text = curRow.Cells[CharaListColID.CharacterName].Value as string;
                txtNenrei.Text = curRow.Cells[CharaListColID.Nenrei].Value as string;
                txtJiyuuKoumoku.Text = curRow.Cells[CharaListColID.FreeWord].Value as string;
            }
            
            // 削除ボタン
            if(CharaListColID.BtnDelete == dgv.Columns[e.ColumnIndex].Name)
            {
                if(SelectedSeq == dgv.Rows[e.RowIndex].Cells[CharaListColID.KeySeq].Value as string)
                {
                    SelectedSeq = "";
                }

                if (dgv.Rows[e.RowIndex].IsNewRow)
                {
                    dgv.Rows[e.RowIndex].Dispose();
                }
                else
                {
                    dgv.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
        #endregion

        #region 詳細欄
        /// <summary>
        /// 反映ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow curRow = dataGridView1.Rows[i];
                if (SelectedSeq == curRow.Cells[CharaListColID.KeySeq].Value as string)
                {
                    curRow.Cells[CharaListColID.CharacterName].Value = txtCharacter.Text;
                    curRow.Cells[CharaListColID.Nenrei].Value = txtNenrei.Text;
                    curRow.Cells[CharaListColID.FreeWord].Value = txtJiyuuKoumoku.Text;
                    break;
                }
            }
        }

        /// <summary>
        /// 地名追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPlace_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 職業追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddJob_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
