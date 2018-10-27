using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor {
	public partial class MainForm : Form {
		private WavePlayback mWaveOut;
		private DLS.DLS mDLS;
		private string mFilePath;
		private bool onRange;

		public MainForm() {
			InitializeComponent();
			SetTabSize();
			mWaveOut = new WavePlayback();
			mDLS = new DLS.DLS();

			timer1.Interval = 50;
			timer1.Enabled = true;
			timer1.Start();
		}

		private void Form1_SizeChanged(object sender, EventArgs e) {
			SetTabSize();
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tstRegion.Text = "";
			if (onRange) {
				var posRegion = PosToRegeon();
				tstRegion.Text = string.Format("音程:{0} 強弱:{1}", posRegion.X.ToString("000"), posRegion.Y.ToString("000"));
			}
		}

		#region メニューバー[ファイル]
		private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
			mDLS = new DLS.DLS();
			DispInstList();
			DispWaveList();
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
			DispWaveList();
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
			var offsetX = 28;
			var offsetY = 70;
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
			var height = tabControl.Height - offsetY - 4;

			lstInst.Left = 0;
			lstInst.Top = toolStrip2.Height + 4;
			lstInst.Width = width;
			lstInst.Height = height;
		}

		private void SetWaveListSize() {
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY - 4;

			lstWave.Left = 0;
			lstWave.Top = toolStrip3.Height + 4;
			lstWave.Width = width;
			lstWave.Height = height;
		}

		private void SetInstAttributeSize() {
			var offsetX = 16;
			var offsetY = 36;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;
		}

		private void SetInstRegionSize() {
			var offsetX = 16;
			var offsetY = 60;
			var width = tabControl.Width - offsetX;
			var height = tabControl.Height - offsetY;

			pnlRegion.Left = 0;
			pnlRegion.Top = toolStrip1.Height + 4;
			pnlRegion.Width = width;
			pnlRegion.Height = height;
			lstRegion.Left = 0;
			lstRegion.Top = toolStrip1.Height + 4;
			lstRegion.Width = width;
			lstRegion.Height = height;
		}
		#endregion

		#region 音色一覧
		private void lstInst_DoubleClick(object sender, EventArgs e) {
			DispRegionInfo();
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
			var indices = lstInst.SelectedIndices;
			foreach (int idx in indices) {
				mDLS.Instruments.List.Remove(GetLocale(idx));
			}

			DispInstList();
			DispWaveList();
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

		private DLS.MidiLocale GetLocale(int index) {
			if (0 == lstInst.Items.Count) {
				return new DLS.MidiLocale();
			}
			if (index < 0) {
				return new DLS.MidiLocale();
			}

			var cols = lstInst.Items[index].ToString().Split('\t');

			var locale = new DLS.MidiLocale();
			locale.BankFlags = (byte)("Drum" == cols[0] ? 0x80 : 0x00);
			locale.ProgramNo = byte.Parse(cols[1]);
			locale.BankMSB = byte.Parse(cols[2]);
			locale.BankLSB = byte.Parse(cols[3]);

			return locale;
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
			DispWaveList();
			lstWave.SelectedIndex = index;
		}

		private void DispWaveList() {
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

			DispWaveList();
		}
		#endregion

		#region レイヤー設定
		private void pictRange_DoubleClick(object sender, EventArgs e) {
			if (lstInst.SelectedIndex < 0) {
				return;
			}

			var fm = new RegionInfoForm(mDLS, GetLocale(lstInst.SelectedIndex), PosToRegionId());
			fm.ShowDialog();
			DispRegionInfo();
		}

		private void pictRange_MouseEnter(object sender, EventArgs e) {
			onRange = true;
		}

		private void pictRange_MouseLeave(object sender, EventArgs e) {
			onRange = false;
		}

		private void lstRegion_DoubleClick(object sender, EventArgs e) {
			var fm = new RegionInfoForm(mDLS, GetLocale(lstInst.SelectedIndex), ListToRegeonId());
			fm.ShowDialog();
			DispRegionInfo();
		}

		private void tsbAddRange_Click(object sender, EventArgs e) {
			var region = new DLS.CK_RGNH();
			region.Key.Low = ushort.MaxValue;
			region.Key.High = ushort.MaxValue;
			region.Velocity.Low = ushort.MaxValue;
			region.Velocity.High = ushort.MaxValue;
			var fm = new RegionInfoForm(mDLS, GetLocale(lstInst.SelectedIndex), region);
			fm.ShowDialog();
			DispRegionInfo();
		}

		private void tsbDeleteRange_Click(object sender, EventArgs e) {
			var inst = mDLS.Instruments.List[GetLocale(lstInst.SelectedIndex)];

			var index = lstRegion.SelectedIndex;

			foreach (int idx in lstRegion.SelectedIndices) {
				var cols = lstRegion.Items[idx].ToString().Split(' ');

				var rgn = new DLS.CK_RGNH();
				rgn.Key.Low = byte.Parse(cols[1]);
				rgn.Key.High = byte.Parse(cols[2]);
				rgn.Velocity.Low = byte.Parse(cols[7]);
				rgn.Velocity.High = byte.Parse(cols[8]);

				if (inst.Regions.List.ContainsKey(rgn)) {
					inst.Regions.List.Remove(rgn);
				}
			}

			DispRegionInfo();

			if (index < lstRegion.Items.Count) {
				lstRegion.SelectedIndex = index;
			}
			else {
				lstRegion.SelectedIndex = lstRegion.Items.Count - 1;
			}
		}

		private void tsbRangeList_Click(object sender, EventArgs e) {
			pnlRegion.Visible = false;
			lstRegion.Visible = true;
			tsbRangeKey.Checked = false;
			tsbRangeList.Checked = true;
			tsbAddRange.Enabled = true;
			tsbDeleteRange.Enabled = true;
		}

		private void tsbRangeKey_Click(object sender, EventArgs e) {
			tsbAddRange.Enabled = false;
			tsbDeleteRange.Enabled = false;
			tsbRangeList.Checked = false;
			tsbRangeKey.Checked = true;

			lstRegion.Visible = false;
			pnlRegion.Visible = true;
		}

		private Point PosToRegeon() {
			var posRegion = pictRange.PointToClient(Cursor.Position);
			if (posRegion.X < 0) {
				posRegion.X = 0;
			}
			if (posRegion.Y < 0) {
				posRegion.Y = 0;
			}
			if (pictRange.Width <= posRegion.X) {
				posRegion.X = pictRange.Width - 1;
			}
			if (pictRange.Height <= posRegion.Y) {
				posRegion.Y = pictRange.Height - 1;
			}

			posRegion.Y = pictRange.Height - posRegion.Y - 1;
			posRegion.X = (int)(posRegion.X / 6.0 + 0.1);
			posRegion.Y = (int)(posRegion.Y / 3.0 + 0.1);

			return posRegion;
		}

		private DLS.CK_RGNH PosToRegionId() {
			var region = new DLS.CK_RGNH();
			var posRegion = PosToRegeon();
			var inst = mDLS.Instruments.List[GetLocale(lstInst.SelectedIndex)];
			foreach (var rgn in inst.Regions.List.Values) {
				var key = rgn.Header.Key;
				var vel = rgn.Header.Velocity;
				if (key.Low <= posRegion.X && posRegion.X <= key.High
				&& vel.Low <= posRegion.Y && posRegion.Y <= vel.High) {
					region = rgn.Header;
					break;
				}
			}

			return region;
		}

		private DLS.CK_RGNH ListToRegeonId() {
			if (lstRegion.SelectedIndex < 0) {
				return new DLS.CK_RGNH();
			}

			var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
			var region = new DLS.CK_RGNH();
			region.Key.Low = ushort.Parse(cols[1]);
			region.Key.High = ushort.Parse(cols[2]);
			region.Velocity.Low = ushort.Parse(cols[7]);
			region.Velocity.High = ushort.Parse(cols[8]);

			return region;
		}

		private void DispRegionInfo() {
			if (!mDLS.Instruments.List.ContainsKey(GetLocale(lstInst.SelectedIndex))) {
				lstRegion.Items.Clear();
				if (null != pictRange.Image) {
					pictRange.Image.Dispose();
					pictRange.Image = null;
				}
				tbpInstInfo.Text = "音色設定"; ;
				tbpRegion.Text = "レイヤー設定";
				return;
			}

			var inst = mDLS.Instruments.List[GetLocale(lstInst.SelectedIndex)];

			tbpInstInfo.Text = string.Format("音色設定[{0}]", inst.Info.Name.Trim());
			tbpRegion.Text = string.Format("レイヤー設定[{0}]", inst.Info.Name.Trim());

			var bmp = new Bitmap(pictRange.Width, pictRange.Height);
			var g = Graphics.FromImage(bmp);
			var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
			var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

			var index = lstRegion.SelectedIndex;
			lstRegion.Items.Clear();

			foreach (var region in inst.Regions.List.Values) {
				var key = region.Header.Key;
				var vel = region.Header.Velocity;
				g.DrawRectangle(
					blueLine,
					key.Low * 6,
					vel.Low * 3,
					(key.High - key.Low + 1) * 6,
					(vel.High - vel.Low + 1) * 3
				);
				g.FillRectangle(
					greenFill,
					key.Low * 6,
					vel.Low * 3,
					(key.High - key.Low + 1) * 6,
					(vel.High - vel.Low + 1) * 3
				);

				var wave = mDLS.WavePool.List[(int)region.WaveLink.TableIndex];

				lstRegion.Items.Add(string.Format(
					"音程 {0} {1}    強弱 {2} {3}    波形 {4} {5}",
					region.Header.Key.Low.ToString("000"),
					region.Header.Key.High.ToString("000"),
					region.Header.Velocity.Low.ToString("000"),
					region.Header.Velocity.High.ToString("000"),
					region.WaveLink.TableIndex.ToString("0000"),
					wave.Info.Name
				));
			}

			if (null != pictRange.Image) {
				pictRange.Image.Dispose();
				pictRange.Image = null;
			}
			pictRange.Image = bmp;

			if (lstRegion.Items.Count <= index) {
				index = lstRegion.Items.Count - 1;
			}
			lstRegion.SelectedIndex = index;
		}
		#endregion
	}
}