using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DLSeditor {
	partial class InstInfoForm : Form {
		private DLS.DLS mDLS;
		private ListBox mList;
		private int mIndex;

		public InstInfoForm(DLS.DLS dls, ListBox list, int index) {
			InitializeComponent();
			SetTabSize();
			mDLS = dls;
			mList = list;
			mIndex = index;
			DispInstInfo();
		}

		private void InstInfoForm_SizeChanged(object sender, EventArgs e) {
			SetTabSize();
		}

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
			SetInstAttributeSize();
			SetInstRegionSize();
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

		#region 音色設定
		private void DispInstInfo() {
			if (0 == mList.Items.Count) {
				return;
			}

			var inst = mDLS.Instruments.List[GetLocale(mList, mList.SelectedIndex)];

			Text = inst.Info.Name;

			if (null != inst.Articulations && null != inst.Articulations.ART) {
				foreach (var conn in inst.Articulations.ART.List.Values) {
				}
			}

			DispRegionInfo();
		}
		#endregion

		#region レイヤー設定
		private void lstRegion_DoubleClick(object sender, EventArgs e) {

		}

		private void pictRange_DoubleClick(object sender, EventArgs e) {
			if (mList.SelectedIndex < 0) {
				return;
			}

			var cp = pictRange.PointToClient(Cursor.Position);
			cp.X = (int)(cp.X / 6 + 0.5);
			cp.Y = (int)((pictRange.Height - cp.Y) / 3 + 0.5);

			DLS.RGN rgn;
			var inst = mDLS.Instruments.List[GetLocale(mList, mList.SelectedIndex)];
			foreach (var region in inst.Regions.List.Values) {
				var key = region.Header.Key;
				var vel = region.Header.Velocity;
				if (key.Low <= cp.X && cp.X <= key.High
				&& vel.Low <= cp.Y && cp.Y <= vel.High) {
					rgn = region;
					break;
				}
			}
		}

		private void pictRange_MouseHover(object sender, EventArgs e) {

		}

		private void tsbAddRange_Click(object sender, EventArgs e) {
			var fm = new RegionInfoForm();
			fm.ShowDialog();
		}

		private void tsbDeleteRange_Click(object sender, EventArgs e) {
			var inst = mDLS.Instruments.List[GetLocale(mList, mList.SelectedIndex)];

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

		private void DispRegionInfo() {
			var inst = mDLS.Instruments.List[GetLocale(mList, mList.SelectedIndex)];

			var bmp = new Bitmap(pictRange.Width, pictRange.Height);
			var g = Graphics.FromImage(bmp);
			var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
			var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

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

			pictRange.Image = bmp;
		}
		#endregion

		public static DLS.MidiLocale GetLocale(ListBox list, int index) {
			var cols = list.Items[index].ToString().Split('\t');

			var locale = new DLS.MidiLocale();
			locale.BankFlags = (byte)("Drum" == cols[0] ? 0x80 : 0x00);
			locale.ProgramNo = byte.Parse(cols[1]);
			locale.BankMSB = byte.Parse(cols[2]);
			locale.BankLSB = byte.Parse(cols[3]);

			return locale;
		}
	}
}
