using System;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor
{
	public partial class WaveListForm : Form
	{
		private WavePlayback mWaveOut;
		private DLS.File mDLS;

		public WaveListForm(WavePlayback waveOut, DLS.File dls)
		{
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mWaveOut = waveOut;
			mDLS = dls;
		}

		private void WaveListForm_Load(object sender, EventArgs e)
		{
			SetListSize();
			DispList();
		}

		#region メニューバー
		private void 追加AToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 削除DToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void ファイル出力ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OutPutWave();
		}

		private void 詳細表示ToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
		#endregion

		#region ツールストリップ
		private void tsbAddWave_Click(object sender, EventArgs e)
		{

		}

		private void tsbDeleteWave_Click(object sender, EventArgs e)
		{

		}

		private void tsbOutputWave_Click(object sender, EventArgs e)
		{
			OutPutWave();
		}
		#endregion

		private void WaveListForm_SizeChanged(object sender, EventArgs e)
		{
			SetListSize();
		}

		private void SetListSize()
		{
			var offsetX = 40;
			var offsetY = 108;
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

			lstWave.Width = width;
			lstWave.Height = height;
		}

		private void DispList()
		{
			lstWave.Items.Clear();
			int count = 0;
			foreach (var wave in mDLS.WavePool.Values) {
				var name = "";
				if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
					name = string.Format("Wave[{0}]", count);
				}
				else {
					name = wave.Info.Name;
				}

				var use = false;
				foreach (var inst in mDLS.InstList.Values) {
					foreach (var rgn in inst.Regions.Values) {
						if (count == rgn.WaveLink.WaveIndex) {
							use = true;
							break;
						}
					}
				}

				lstWave.Items.Add((use ? "[use]" : "     ") + name);
				++count;
			}
		}

		private void OutPutWave()
		{
			folderBrowserDialog1.ShowDialog();
			var folderPath = folderBrowserDialog1.SelectedPath;
			if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
				return;
			}

			var indices = lstWave.SelectedIndices;
			foreach (var idx in indices) {
				var wave = mDLS.WavePool[(int)idx];
				if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
					wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
				}
				else {
					wave.ToFile(Path.Combine(folderPath, wave.Info.Name + ".wav"));
				}
			}
		}

		private void lstWave_DoubleClick(object sender, EventArgs e)
		{
			var idx = lstWave.SelectedIndex;
			var fm = new WaveInfoForm(mWaveOut, mDLS, idx);
			fm.ShowDialog();
		}
	}
}
