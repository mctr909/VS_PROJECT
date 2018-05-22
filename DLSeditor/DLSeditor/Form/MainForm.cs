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

namespace DLSeditor
{
	public partial class MainForm : Form
	{
		private DLS.INS_ mClipboardInst;
		private DLS.LINS mInstPool;
		private DLS.WVPL mWavePool;
		private DLS.INFO mInfo;

		public MainForm()
		{
			InitializeComponent();
			SetTabSize();
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			SetTabSize();
		}

		#region メニューバー[ファイル]
		private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 開くOToolStripMenuItem_Click(object sender, EventArgs e)
		{
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
			openFileDialog1.ShowDialog();
			var filePath = openFileDialog1.FileName;
			if (!File.Exists(filePath)) return;

			var dls = new DLS.File(filePath);
			mInstPool = dls.InstPool;
			mWavePool = dls.WavePool;
			mInfo = dls.Info;
			DispInstList();
		}

		private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var f = new DLS.File("C:\\Users\\owner\\Desktop\\test.dls", mInstPool, mWavePool, mInfo);
		}

		private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			saveFileDialog1.FileName = "";
			saveFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
			saveFileDialog1.ShowDialog();
		}
		#endregion

		#region メニューバー[編集]
		private void 追加AToolStripMenuItem_Click(object sender, EventArgs e)
		{
			AddInst();
		}

		private void 削除DToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DeleteInst();
		}

		private void コピーCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyInst();
		}

		private void 貼り付けPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			PasteInst();
		}
		#endregion

		#region メニューバー[表示]
		private void 詳細表示IToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DispInstInfo();
		}

		private void pCM一覧表示PToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DispPcmList();
		}
		#endregion

		#region ツールストリップ
		private void tsbAddInst_Click(object sender, EventArgs e)
		{
			AddInst();
		}

		private void tsbDeleteInst_Click(object sender, EventArgs e)
		{
			DeleteInst();
		}

		private void tsbCopyInst_Click(object sender, EventArgs e)
		{
			CopyInst();
		}

		private void tsbPasteInst_Click(object sender, EventArgs e)
		{
			PasteInst();
		}
		#endregion

		#region サイズ調整
		private void SetTabSize()
		{
			var offsetX = 40;
			var offsetY = 80;
			var width = Width - offsetX;
			var height = Height - offsetY;

			if (width < 100)
			{
				return;
			}

			if (height < 100)
			{
				return;
			}

			tabControl.Width = width;
			tabControl.Height = height;

			SetInstListSize();
			SetInstAttributeSize();
		}

		private void SetInstListSize()
		{
			var offsetX = 16;
			var offsetY = 62;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			lstInst.Width = width;
			lstInst.Height = height;
		}

		private void SetInstAttributeSize()
		{
			var offsetX = 16;
			var offsetY = 36;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			pnlInstAttribute.Width = width;
			pnlInstAttribute.Height = height;
		}
		#endregion

		#region 音色一覧
		private void lstInst_DoubleClick(object sender, EventArgs e)
		{
			DispInstInfo();
		}

		private void DispInstList()
		{
			lstInst.Items.Clear();

			if (null == mInstPool) return;

			foreach (var inst in mInstPool.List)
			{
				lstInst.Items.Add(string.Format(
					"{0} PRG:{1} MSB:{2} LSB:{3} {4}",
					inst.InstHeader.IsDrum ? "DRUM" : "NOTE",
					inst.InstHeader.ProgramNo.ToString("000"),
					inst.InstHeader.BankMSB.ToString("000"),
					inst.InstHeader.BankLSB.ToString("000"),
					inst.Info.Name
				));
			}
		}

		private void DispInstInfo()
		{
			var idx = lstInst.SelectedIndex;
			if (idx < 0) return;

			//InstInfoForm fm = new InstInfoForm();
			//fm.mInst = mInstPool[idx];
			//fm.mWave = mWavePool;
			//fm.ShowDialog();

			tbpInstAttribute.Text = string.Format("音色設定[{0}]", mInstPool[idx].Info.Name);
			tabControl.SelectedIndex = 1;

			if (null != mInstPool[idx].ArtPool) {
				foreach (var art in mInstPool[idx].ArtPool.Art.List) {
					switch (art.Source) {
					case DLS.CONN_SRC_TYPE.KEY_NUMBER:
						switch (art.Destination) {
						case DLS.CONN_DST_TYPE.EG1_ATTACK_TIME:
							numAmpAttack.Value = (decimal)art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_DECAY_TIME:
							numAmpDecay.Value = (decimal)art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_RELEASE_TIME:
							numAmpRelease.Value = (decimal)art.Value;
							break;
						case DLS.CONN_DST_TYPE.EG1_SUSTAIN_LEVEL:
							if (0 == art.Value) {
								numAmpSustain.Value = 100;
							}
							else {
								numAmpSustain.Value = (decimal)art.Value;
							}
							break;
						}
						break;
					default:
						break;
					}
				}
			}
		}

		private void AddInst()
		{
			InstAddForm fm = new InstAddForm();
			fm.mInstPool = mInstPool;
			fm.ShowDialog();
			mInstPool = fm.mInstPool;

			DispInstList();
		}

		private void DeleteInst()
		{
			var idx = lstInst.SelectedIndex;
			if (idx < 0) return;

			mInstPool.Del(idx);
			DispInstList();

			if (lstInst.Items.Count <= idx)
			{
				lstInst.SelectedIndex = lstInst.Items.Count - 1;
			}
			else
			{
				lstInst.SelectedIndex = idx;
			}
		}

		private void CopyInst()
		{
			var idx = lstInst.SelectedIndex;
			if (idx < 0) return;

			mClipboardInst = mInstPool[idx];
		}

		private void PasteInst()
		{
			if (null == mClipboardInst) return;

			InstAddForm fm = new InstAddForm();
			fm.mSelectedInst = mClipboardInst;
			fm.mInstPool = mInstPool;
			fm.ShowDialog();
			mInstPool = fm.mInstPool;

			DispInstList();
		}
		#endregion

		private void DispPcmList()
		{
			WaveListForm fm = new WaveListForm();
			fm.mInstPool = mInstPool;
			fm.mWave = mWavePool;
			fm.ShowDialog();
		}
	}
}
