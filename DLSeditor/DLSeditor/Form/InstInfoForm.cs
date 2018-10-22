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

			pnlRegion.Width = width;
			pnlRegion.Height = height;
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

			tslPos.Text = string.Format("X:{0} Y:{1}", cp.X, cp.Y);
		}

		private void tsbAddRange_Click(object sender, EventArgs e) {

		}

		private void DispRegionInfo() {
			var inst = mDLS.Instruments.List[GetLocale(mList, mList.SelectedIndex)];

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
