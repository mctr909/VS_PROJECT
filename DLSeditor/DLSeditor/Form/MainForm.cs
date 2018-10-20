using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor {
	public partial class MainForm : Form {
		private WavePlayback mWaveOut;
		private DLS.DLS mDLS;
		private string mFilePath;

		public MainForm() {
			InitializeComponent();
			SetTabSize();
			mWaveOut = new WavePlayback();
			mDLS = new DLS.DLS();
		}

		private void Form1_SizeChanged(object sender, EventArgs e) {
			SetTabSize();
		}

		#region メニューバー[ファイル]
		private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
			mDLS = new DLS.DLS();
			DispInstList();
			DispPcmList();
			tabControl.SelectedIndex = 0;
			mFilePath = "";
		}

		unsafe private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
			openFileDialog1.ShowDialog();
			var filePath = openFileDialog1.FileName;
			if (!File.Exists(filePath)) {
				return;
			}

			using (var fs = new FileStream(filePath, FileMode.Open))
			using (var br = new BinaryReader(fs)) {
				br.ReadUInt32();
				var size = br.ReadUInt32();
				br.ReadUInt32();

				var mBuff = new byte[size - 4];
				fs.Read(mBuff, 0, mBuff.Length);

				fixed (byte* p = &mBuff[0]) {
					mDLS = new DLS.DLS(p, p + mBuff.Length);
				}

				fs.Close();
			}

			DispInstList();
			DispPcmList();
			tabControl.SelectedIndex = 0;
			mFilePath = filePath;
		}

		private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e) {
			if (string.IsNullOrWhiteSpace(mFilePath) || !File.Exists(mFilePath)) {
				名前を付けて保存ToolStripMenuItem_Click(sender, e);
			}
			mDLS.Save(mFilePath);
		}

		private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e) {
			saveFileDialog1.FileName = "";
			saveFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
			saveFileDialog1.ShowDialog();
			var filePath = saveFileDialog1.FileName;
			if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
				return;
			}

			mDLS.Save(filePath);
			mFilePath = filePath;
		}
		#endregion

		#region メニューバー[編集]
		private void 追加AToolStripMenuItem_Click(object sender, EventArgs e) {
			AddInst();
		}

		private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
			DeleteInst();
		}

		private void コピーCToolStripMenuItem_Click(object sender, EventArgs e) {
			CopyInst();
		}

		private void 貼り付けPToolStripMenuItem_Click(object sender, EventArgs e) {
			PasteInst();
		}
		#endregion

		#region ツールストリップ
		private void tsbAddInst_Click(object sender, EventArgs e) {
			AddInst();
		}

		private void tsbDeleteInst_Click(object sender, EventArgs e) {
			DeleteInst();
		}

		private void tsbCopyInst_Click(object sender, EventArgs e) {
			CopyInst();
		}

		private void tsbPasteInst_Click(object sender, EventArgs e) {
			PasteInst();
		}

		private void tsbAddWave_Click(object sender, EventArgs e) {
			WaveFileAdd();
		}

		private void tsbDeleteWave_Click(object sender, EventArgs e) {
			foreach (var inst in mDLS.Instruments.List.Values) {
				foreach (var rgn in inst.Regions.List.Values) {
					if (lstWave.SelectedIndex == rgn.WaveLink.TableIndex) {
						return;
					}
				}
			}
		}

		private void tsbOutputWave_Click(object sender, EventArgs e) {
			WaveFileOut();
		}
		#endregion

		#region サイズ調整
		private void SetTabSize() {
			var offsetX = 40;
			var offsetY = 80;
			var width = Width - offsetX;
			var height = Height - offsetY;

			if (width < 100) {
				return;
			}

			if (height < 100) {
				return;
			}

			tabControl.Width = width;
			tabControl.Height = height;

			SetInstListSize();
			SetWaveListSize();
			SetInstAttributeSize();
			SetInstRegionSize();
		}

		private void SetInstListSize() {
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			lstInst.Width = width;
			lstInst.Height = height;
		}

		private void SetWaveListSize() {
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			lstWave.Width = width;
			lstWave.Height = height;
		}

		private void SetInstAttributeSize() {
			var offsetX = 16;
			var offsetY = 36;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			grdArt.Width = width;
			grdArt.Height = height;
		}

		private void SetInstRegionSize() {
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			pnlRegion.Width = width;
			pnlRegion.Height = height;
		}
		#endregion

		#region 音色一覧
		private void lstInst_DoubleClick(object sender, EventArgs e) {
			DispInstInfo();
		}

		private void DispInstList() {
			lstInst.Items.Clear();
			lstInst.Font = new Font("ＭＳ ゴシック", 9.0f, FontStyle.Regular);
			foreach (var inst in mDLS.Instruments.List.Values) {
				lstInst.Items.Add(string.Format(
					"{0}\t{1}\t{2}\t{3}\t{4}",
					(inst.Header.Locale.BankFlags & 0x80) == 0x80 ? "Drum" : "Note",
					inst.Header.Locale.ProgramNo.ToString("000"),
					inst.Header.Locale.BankMSB.ToString("000"),
					inst.Header.Locale.BankLSB.ToString("000"),
					inst.Info.Name
				));
			}
		}

		private void DispInstInfo() {
			if (0 == lstInst.Items.Count) {
				return;
			}

			var inst = mDLS.Instruments.List[GetLocale()];

			tbpInstAttribute.Text = string.Format("音色設定[{0}]", inst.Info.Name);

			DataTable tb = new DataTable();
			tb.Columns.Add("Destination", typeof(DLS.Connection.DST_TYPE));
			tb.Columns.Add("Source", typeof(DLS.Connection.SRC_TYPE));
			tb.Columns.Add("Control", typeof(DLS.Connection.SRC_TYPE));
			tb.Columns.Add("Value", typeof(double));

			if (null != inst.Articulations && null != inst.Articulations.ART) {
				foreach (var conn in inst.Articulations.ART.List.Values) {
					var row = tb.NewRow();
					row["Destination"] = conn.Destination;
					row["Source"] = conn.Source;
					row["Control"] = conn.Control;
					row["Value"] = conn.Value.ToString("0.000");
					tb.Rows.Add(row);
				}
			}

			grdArt.DataSource = tb;
			grdArt.Columns["Destination"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Source"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Control"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grdArt.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			DispRegionInfo();
		}

		private void AddInst() {
			InstForm fm = new InstForm(mDLS);
			fm.ShowDialog();
			DispInstList();
		}

		private void DeleteInst() {
			if (0 == lstInst.Items.Count) {
				return;
			}

			var index = lstInst.SelectedIndex;
			mDLS.Instruments.List.Remove(GetLocale());
			DispInstList();
			if (index < lstInst.Items.Count) {
				lstInst.SelectedIndex = index;
			}
			else {
				lstInst.SelectedIndex = lstInst.Items.Count - 1;
			}
		}

		private void CopyInst() {
		}

		private void PasteInst() {
			DispInstList();
		}

		private DLS.MidiLocale GetLocale() {
			var index = lstInst.SelectedIndex;
			var cols = lstInst.Items[index].ToString().Split('\t');

			var locale = new DLS.MidiLocale();
			locale.BankFlags = (byte)("Drum" == cols[0] ? 0x80 : 0x00);
			locale.ProgramNo = byte.Parse(cols[1]);
			locale.BankMSB = byte.Parse(cols[2]);
			locale.BankLSB = byte.Parse(cols[3]);

			return locale;
		}
		#endregion

		#region レイヤー設定
		private void pictRange_DoubleClick(object sender, EventArgs e) {
			if(0 == lstInst.Items.Count) {
				return;
			}

			var cp = pictRange.PointToClient(Cursor.Position);
			cp.X = (int)(cp.X / 6 + 0.5);
			cp.Y = (int)((pictRange.Height - cp.Y) / 6 + 0.5);

			DLS.RGN rgn;
			var inst = mDLS.Instruments.List[GetLocale()];
			foreach (var region in inst.Regions.List.Values) {
				var key = region.Header.Key;
				var vel = region.Header.Velocity;
				if (key.Low <= cp.X && cp.X <= key.High
				&& vel.Low <= cp.Y && cp.Y <= vel.High) {
					rgn = region;
					break;
				}
			}

			tslPos.Text = string.Format("X:{0} Y:{1}", cp.X, cp.Y);
		}

		private void DispRegionInfo() {
			var inst = mDLS.Instruments.List[GetLocale()];

			tbpLayerAttribute.Text = string.Format("レイヤー設定[{0}]", inst.Info.Name);

			var bmp = new Bitmap(pictRange.Width, pictRange.Height);
			var g = Graphics.FromImage(bmp);
			var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
			var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

			foreach (var region in inst.Regions.List.Values) {
				var key = region.Header.Key;
				var vel = region.Header.Velocity;
				g.DrawRectangle(
					blueLine,
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

		private void tsbAddRange_Click(object sender, EventArgs e) {

		}
		#endregion

		#region 波形一覧
		private void lstWave_DoubleClick(object sender, EventArgs e) {
			if (0 == lstWave.Items.Count) {
				return;
			}

			var idx = lstWave.SelectedIndex;
			var fm = new WaveInfoForm(mWaveOut, mDLS, idx);
			var index = lstWave.SelectedIndex;
			fm.ShowDialog();
			DispPcmList();
			lstWave.SelectedIndex = index;
		}

		private void DispPcmList() {
			lstWave.Items.Clear();
			int count = 0;
			foreach (var wave in mDLS.WavePool.List) {
				var name = "";
				if (null == wave.Value.Info || string.IsNullOrWhiteSpace(wave.Value.Info.Name)) {
					name = string.Format("Wave[{0}]", count);
				}
				else {
					name = wave.Value.Info.Name;
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
					"{0}\t{1}\t{2}\t{3}",
					wave.Key.ToString("0000"),
					(use ? "use" : "   "),
					(0 < wave.Value.Sampler.LoopCount ? "loop" : "    "),
					name
				));
				++count;
			}
		}

		private void WaveFileOut() {
			folderBrowserDialog1.ShowDialog();
			var folderPath = folderBrowserDialog1.SelectedPath;
			if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
				return;
			}

			var indices = lstWave.SelectedIndices;
			foreach (var idx in indices) {
				var wave = mDLS.WavePool.List[(int)idx];
				if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
					wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
				}
				else {
					wave.ToFile(Path.Combine(folderPath, wave.Info.Name + ".wav"));
				}
			}
		}

		private void WaveFileAdd() {
			openFileDialog1.FileName = "";
			openFileDialog1.Filter = "wavファイル(*.wav)|*.wav";
			openFileDialog1.Multiselect = true;
			openFileDialog1.ShowDialog();
			var filePaths = openFileDialog1.FileNames;

			foreach (var filePath in filePaths) {
				if (!File.Exists(filePath)) {
					continue;
				}

				var wave = new DLS.WAVE(filePath);
				mDLS.WavePool.List.Add(mDLS.WavePool.List.Count, wave);
			}

			DispPcmList();
		}

		#endregion
	}
}