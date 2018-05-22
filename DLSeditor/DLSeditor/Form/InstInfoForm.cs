using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DLSeditor
{
	public partial class InstInfoForm : Form
	{
		public DLS.CINS_ mInst;
		public DLS.CWVPL mWave;

		public InstInfoForm()
		{
			InitializeComponent();
		}

		private Bitmap mRegion;
		private Pen mFillPen = new Pen(Color.FromArgb(64, 0, 255, 0));
		private Pen mLinePen = new Pen(Color.FromArgb(255, 0, 0, 0), 2.0f);
		private Brush mTextBrush = new Pen(Color.FromArgb(255, 0, 0, 0), 2.0f).Brush;
		private Brush mTextPanlBrush = new Pen(Color.FromArgb(168, 255, 255, 255), 2.0f).Brush;

		private void InstInfoForm_Load(object sender, EventArgs e)
		{
			TabSize();
			DispInfo();
			DispRegion();
			DispWaveList();
		}

		private void InstInfoForm_SizeChanged(object sender, EventArgs e)
		{
			TabSize();
		}

		private void tbRegion_SizeChanged(object sender, EventArgs e)
		{
			RegionSize();
		}

		private void tbWave_SizeChanged(object sender, EventArgs e)
		{
			WaveSize();
		}

		private void tsbSelectRegion_Click(object sender, EventArgs e)
		{

		}

		private void tsbAddRegion_Click(object sender, EventArgs e)
		{

		}

		private void tsbDeleteRegion_Click(object sender, EventArgs e)
		{

		}

		private void imgRegion_MouseDown(object sender, MouseEventArgs e)
		{
		}

		private void imgRegion_MouseUp(object sender, MouseEventArgs e)
		{
		}

		private void imgRegion_MouseMove(object sender, MouseEventArgs e)
		{
			DispRegionPos();
			imgRegion.Image = mRegion;
		}

		private void imgRegion_DoubleClick(object sender, EventArgs e)
		{
			var sp = Cursor.Position;
			var cp = imgRegion.PointToClient(sp);

			var noteNo = cp.X / 5;
			var exp = 127 - (cp.Y) / 4;

			if (noteNo < 0) noteNo = 0;
			if (exp < 0) exp = 0;
			if (127 < noteNo) noteNo = 127;
			if (127 < exp) exp = 127;

			foreach (var rgn in mInst.RegionPool.List)
			{
				var hd = rgn.RegionHeader;
				if (noteNo <= hd.KeyRangeHigh && hd.KeyRangeLow <= noteNo)
				{
					if (exp <= hd.VelocityRangeHigh && hd.VelocityRangeLow <= exp)
					{
						break;
					}
				}
			}
		}

		private void TabSize()
		{
			var width = Width - tbcInfo.Left - 24;
			var height = Height - tbcInfo.Top - 48;

			if (width < 2) return;
			if (height < 2) return;

			tbcInfo.Width = width;
			tbcInfo.Height = height;
		}

		private void RegionSize()
		{
			var width = tbcInfo.Width - pnlRegion.Left - 15;
			var height = tbcInfo.Height - pnlRegion.Top - 35;

			if (width < 2) return;
			if (height < 2) return;

			if (imgRegion.Width + 24 < width) width = imgRegion.Width + 24;
			if (imgRegion.Height + 24 < height) height = imgRegion.Height + 24;

			pnlRegion.Width = width;
			pnlRegion.Height = height;
		}

		private void WaveSize()
		{
			var width = tbcInfo.Width - pnlRegion.Left - 15;
			var height = tbcInfo.Height - pnlRegion.Top - 35;

			if (width < 2) return;
			if (height < 2) return;

			lstWave.Width = width;
			lstWave.Height = height;
		}

		private void DispInfo()
		{
			label1.Text = "";
			//if (null == mInst) return;
			Text = string.Format("音色情報[{0}]", mInst.Info.Name.Trim());

			#region Connection
			if (null == mInst.ArtPool || null == mInst.ArtPool.Art) return;
			foreach (var conn in mInst.ArtPool.Art.List)
			{
				label1.Text += string.Format(
					"Source:{0}\r\nControl:{1}\r\nDestination:{2}\r\nValue:{3}\r\n\r\n",
					conn.Source,
					conn.Control,
					conn.Destination,
					conn.Value.ToString("0.000")
				);
			}
			#endregion
		}

		private void DispRegion()
		{
			//if (null == mInst || null == mInst.RegionPool || null == mInst.RegionPool.List) return;
			if (null == mInst.RegionPool || null == mInst.RegionPool.List) return;

			mRegion = Properties.Resources.region;
			var graph = Graphics.FromImage(mRegion);

			foreach (var rgn in mInst.RegionPool.List)
			{
				var hd = rgn.RegionHeader;

				var keyLow = 5 * hd.KeyRangeLow;
				var keyHigh = 5 * hd.KeyRangeHigh;
				var volLow = 4 * hd.VelocityRangeLow;
				var volHigh = 4 * hd.VelocityRangeHigh;

				var width = keyHigh - keyLow + 5;
				var height = volHigh - volLow + 4;

				graph.FillRectangle(mFillPen.Brush, keyLow + 1, 514 - volHigh - 4, width - 1, height - 1);
				graph.DrawRectangle(mLinePen, keyLow, 514 - volHigh - 4, width, height);
			}

			imgRegion.Image = mRegion;
		}

		private void DispRegionPos()
		{
			var sp = Cursor.Position;
			var cp = imgRegion.PointToClient(sp);

			var noteNo = cp.X / 5;
			var exp = 127 - (cp.Y) / 4;

			if (noteNo < 0) noteNo = 0;
			if (exp < 0) exp = 0;
			if (127 < noteNo) noteNo = 127;
			if (127 < exp) exp = 127;

			var noteStr = "";

			switch (noteNo % 12)
			{
				case 0: noteStr = "C    "; break;
				case 1: noteStr = "Db/C#"; break;
				case 2: noteStr = "D    "; break;
				case 3: noteStr = "Eb/D#"; break;
				case 4: noteStr = "E    "; break;
				case 5: noteStr = "F    "; break;
				case 6: noteStr = "Gb/F#"; break;
				case 7: noteStr = "G    "; break;
				case 8: noteStr = "Ab/G#"; break;
				case 9: noteStr = "A    "; break;
				case 10: noteStr = "Bb/A#"; break;
				case 11: noteStr = "B    "; break;
			}

			tstRegionInfo.Text = string.Format(
				"Exp:{0} Oct:{1} Note:{2}",
				exp.ToString().PadRight(3),
				(noteNo / 12 - 2).ToString().PadLeft(2),
				noteStr
			);
		}

		private void DispWaveList()
		{
			lstWave.Items.Clear();
			foreach(var rgn in mInst.RegionPool.List)
			{
				lstWave.Items.Add(string.Format(
					"Note:{0}-{1} Exp.:{2}-{3} HasLoop:{4} Name:{5}",
					rgn.RegionHeader.KeyRangeLow.ToString("000"),
					rgn.RegionHeader.KeyRangeHigh.ToString("000"),
					rgn.RegionHeader.VelocityRangeLow.ToString("000"),
					rgn.RegionHeader.VelocityRangeHigh.ToString("000"),
					(0 < mWave[(int)rgn.WaveLink.WaveIndex].Sampler.List.Count).ToString().PadRight(5),
					mWave[(int)rgn.WaveLink.WaveIndex].Info.Name
				));
			}
		}

		private void tsbImportWaves_Click(object sender, EventArgs e)
		{

		}

		private void tsbDeleteWave_Click(object sender, EventArgs e)
		{

		}

		private void tsbExportWaves_Click(object sender, EventArgs e)
		{
			folderBrowserDialog1.ShowDialog();
			var folderPath = folderBrowserDialog1.SelectedPath;
			if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) return;

			folderPath = Path.Combine(folderPath, mInst.Info.Name);
			Directory.CreateDirectory(folderPath);

			foreach (var rgn in mInst.RegionPool.List)
			{
				mWave[(int)rgn.WaveLink.WaveIndex].ToFile(folderPath);
			}
		}

	}
}
