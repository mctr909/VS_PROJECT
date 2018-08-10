using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor
{
	public partial class MainForm : Form
	{
		private WavePlayback mWaveOut;
		private DLS.DLS mDLS;

		public MainForm()
		{
			InitializeComponent();
			SetTabSize();
			mWaveOut = new WavePlayback();
			mDLS = new DLS.DLS();
		}

		private void Form1_SizeChanged(object sender, EventArgs e)
		{
			SetTabSize();
		}

		#region メニューバー[ファイル]
		private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		unsafe private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
			openFileDialog1.ShowDialog();
			var filePath = openFileDialog1.FileName;
			if (!File.Exists(filePath))
				return;

			using (var fs = new FileStream(filePath, FileMode.Open))
			using (var br = new BinaryReader(fs)) {
				br.ReadUInt32();
				var size = br.ReadUInt32();
				br.ReadUInt32();

				var mBuff = new byte[size - 4];
				fs.Read(mBuff, 0, mBuff.Length);

				fixed (byte* p = &mBuff[0])
				{
					mDLS = new DLS.DLS(p, (UInt32)(p + mBuff.Length));
				}

				fs.Close();
			}

			DispInstList();
			DispPcmList();
			tabControl.SelectedIndex = 0;
		}

		private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
		{
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

		private void tsbAddWave_Click(object sender, EventArgs e)
		{

		}

		private void tsbDeleteWave_Click(object sender, EventArgs e)
		{

		}

		private void tsbOutputWave_Click(object sender, EventArgs e)
		{
			WaveFileOut();
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
			SetWaveListSize();
			SetInstAttributeSize();
			SetInstRegionSize();
		}

		private void SetInstListSize()
		{
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			lstInst.Width = width;
			lstInst.Height = height;
		}

		private void SetWaveListSize()
		{
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			lstWave.Width = width;
			lstWave.Height = height;
		}

		private void SetInstAttributeSize()
		{
			var offsetX = 16;
			var offsetY = 36;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			grdArt.Width = width;
			grdArt.Height = height;
		}

		private void SetInstRegionSize()
		{
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			pnlRegion.Width = width;
			pnlRegion.Height = height;
		}
		#endregion

		#region 音色一覧
		private void lstInst_DoubleClick(object sender, EventArgs e)
		{
			DispInstInfo();
		}

		private void pictRange_DoubleClick(object sender, EventArgs e)
		{
			var cp = pictRange.PointToClient(Cursor.Position);
			cp.X = (int)(cp.X / 6 + 0.5);
			cp.Y = (int)((pictRange.Height - cp.Y) / 6 + 0.5);

			DLS.RGN rgn;
			var inst = mDLS.Instruments.List[lstInst.SelectedIndex];
			foreach (var region in inst.Regions.List.Values) {
				var key = region.RegionHeader.Key;
				var vel = region.RegionHeader.Velocity;
				if (key.Low <= cp.X && cp.X <= key.High
				&& vel.Low <= cp.Y && cp.Y <= vel.High) {
					rgn = region;
					break;
				}
			}

			tslPos.Text = string.Format("X:{0} Y:{1}", cp.X, cp.Y);
		}

		private void DispInstList()
		{
			lstInst.Items.Clear();
			lstInst.Font = new Font("ＭＳ ゴシック", 9.0f, FontStyle.Regular);
			foreach (var inst in mDLS.Instruments.List.Values) {
				lstInst.Items.Add(string.Format(
					"{0} {1} {2} {3} {4}",
					(inst.InstHeader.Locale.BankFlags & 0x80) == 0x80 ? "Drum" : "Note",
					inst.InstHeader.Locale.ProgramNo.ToString("000"),
					inst.InstHeader.Locale.BankMSB.ToString("000"),
					inst.InstHeader.Locale.BankLSB.ToString("000"),
					inst.Text.Name
				));
			}
		}

		private void DispInstInfo()
		{
			var inst = mDLS.Instruments.List[lstInst.SelectedIndex];

			tbpInstAttribute.Text = string.Format("音色設定[{0}]", inst.Text.Name);

			DataTable tb = new DataTable();
			tb.Columns.Add("Destination", typeof(DLS.Connection.DST_TYPE));
			tb.Columns.Add("Source", typeof(DLS.Connection.SRC_TYPE));
			tb.Columns.Add("Control", typeof(DLS.Connection.SRC_TYPE));
			tb.Columns.Add("Value", typeof(double));

			if (null != inst.Articulations) {
				foreach (var art in inst.Articulations.List.Values) {
					foreach (var conn in art.List.Values) {
						var row = tb.NewRow();
						row["Destination"] = conn.Destination;
						row["Source"] = conn.Source;
						row["Control"] = conn.Control;
						row["Value"] = conn.Value.ToString("0.000");
						tb.Rows.Add(row);
					}
				}
			}

			grdArt.DataSource = tb;
			grdArt.Columns["Destination"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Source"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Control"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			DispRegionInfo();
		}

		private void DispRegionInfo()
		{
			var inst = mDLS.Instruments.List[lstInst.SelectedIndex];

			tbpLayerAttribute.Text = string.Format("レイヤー設定[{0}]", inst.Text.Name);

			var bmp = new Bitmap(pictRange.Width, pictRange.Height);
			var g = Graphics.FromImage(bmp);
			var redLine = new Pen(Color.FromArgb(255, 255, 0, 0), 2.0f);
			var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

			foreach (var region in inst.Regions.List.Values) {
				var key = region.RegionHeader.Key;
				var vel = region.RegionHeader.Velocity;
				g.DrawRectangle(
					redLine,
					key.Low * 6,
					vel.Low * 6,
					(key.High - key.Low + 1) * 6,
					(vel.High - vel.Low + 1) * 6
				);
				g.FillRectangle(
					greenFill,
					key.Low * 6,
					vel.Low * 6,
					(key.High - key.Low + 1) * 6,
					(vel.High - vel.Low + 1) * 6
				);
			}

			pictRange.Image = bmp;
		}

		private void AddInst()
		{
			InstAddForm fm = new InstAddForm(mDLS);
			fm.ShowDialog();
			DispInstList();
		}

		private void DeleteInst()
		{
			DispInstList();
		}

		private void CopyInst()
		{
		}

		private void PasteInst()
		{
			DispInstList();
		}
		#endregion

		#region 波形一覧
		private void lstWave_DoubleClick(object sender, EventArgs e)
		{
			var idx = lstWave.SelectedIndex;
			var fm = new WaveInfoForm(mWaveOut, mDLS, idx);
			fm.ShowDialog();
		}

		private void DispPcmList()
		{
			lstWave.Items.Clear();
			int count = 0;
			foreach (var wave in mDLS.WavePool.List.Values) {
				var name = "";
				if (null == wave.Text || string.IsNullOrWhiteSpace(wave.Text.Name)) {
					name = string.Format("Wave[{0}]", count);
				}
				else {
					name = wave.Text.Name;
				}

				var use = false;
				foreach (var inst in mDLS.Instruments.List.Values) {
					foreach (var rgn in inst.Regions.List.Values) {
						if (count == rgn.WaveLink.TableIndex) {
							use = true;
							break;
						}
					}
				}

				lstWave.Items.Add(string.Format(
					"{0}{1}{2}",
					(use ? "[use]" : "     "),
					(0 < wave.Sampler.LoopCount ? "[loop]" : "      "),
					name
				));
				++count;
			}
		}

		private void WaveFileOut()
		{
			folderBrowserDialog1.ShowDialog();
			var folderPath = folderBrowserDialog1.SelectedPath;
			if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
				return;
			}

			var indices = lstWave.SelectedIndices;
			foreach (var idx in indices) {
				var wave = mDLS.WavePool.List[(int)idx];
				if (null == wave.Text || string.IsNullOrWhiteSpace(wave.Text.Name)) {
					wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
				}
				else {
					wave.ToFile(Path.Combine(folderPath, wave.Text.Name + ".wav"));
				}
			}
		}
		#endregion
	}
}
