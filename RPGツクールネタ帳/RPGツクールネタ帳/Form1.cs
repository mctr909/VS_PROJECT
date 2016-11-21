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
        private long CurSeqNo;
        private long SelectSeqNo;

        public Form1()
        {
            InitializeComponent();
            pnlCharacter.Enabled = false;
        }

        /// <summary>
        /// メニューバー新規作成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 新規作成ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "RPGツクールネタ帳(*.rpc)|*.rpc";
            saveFileDialog1.ShowDialog();
            string fileName = saveFileDialog1.FileName;

            if(string.IsNullOrEmpty(fileName))
            {
                return;
            }

            FileStream fs = new FileStream(fileName, FileMode.Create);
            fs.Close();
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
        /// リストのコントロールをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // 詳細を開くボタン
            if(dgv.Columns[e.ColumnIndex].Name == "BtnDtl")
            {

            }
            
            // 削除ボタン
            if(dgv.Columns[e.ColumnIndex].Name == "BtnDel")
            {

            }
        }

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
                col.Name = "KeySeq";
                col.Visible = false;
                dataGridView1.Columns.Add(col);

                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imageCol.Name = "FaceGraphic";
                imageCol.HeaderText = "顔グラ";
                imageCol.DefaultCellStyle.NullValue = null;
                dataGridView1.Columns.Add(imageCol);

                dataGridView1.Columns.Add("CharacterName", "キャラクター名");

                DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                btnCol.Name = "BtnDtl";
                btnCol.Text = "詳細を開く";
                btnCol.HeaderText = "";
                btnCol.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnCol);

                btnCol = new DataGridViewButtonColumn();
                btnCol.Name = "BtnDel";
                btnCol.Text = "削除";
                btnCol.HeaderText = "";
                btnCol.UseColumnTextForButtonValue = true;
                dataGridView1.Columns.Add(btnCol);
            }
            else
            {
                iRow = dataGridView1.Rows.Add();
            }

            dataGridView1.Rows[iRow].Height = 64;
            dataGridView1.Rows[iRow].Cells["KeySeq"].Value = CurSeqNo;
            dataGridView1.Rows[iRow].Cells["FaceGraphic"].Value = null;
            dataGridView1.Rows[iRow].Cells["CharacterName"].ReadOnly = true;
            dataGridView1.Rows[iRow].Cells["BtnDtl"].Value = "詳細を開く";
            dataGridView1.Rows[iRow].Cells["BtnDel"].Value = "削除";

            CurSeqNo++;
        }

        /// <summary>
        /// 保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
