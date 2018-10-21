using System;
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
		#endregion

		#region 音色一覧
		private void lstInst_DoubleClick(object sender, EventArgs e) {
			var fm = new InstInfoForm(mDLS, lstInst, lstInst.SelectedIndex);
			fm.ShowDialog();
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
				mDLS.Instruments.List.Remove(InstInfoForm.GetLocale(lstInst, idx));
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
	}
}