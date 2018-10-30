using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class WaveSelectForm : Form {
		private DLS.DLS mDLS;
		private DLS.RGN mRegion;

		public WaveSelectForm(DLS.DLS dls, ref DLS.RGN region) {
			InitializeComponent();

			mDLS = dls;
			mRegion = region;
		}

		private void WaveSelectForm_Load(object sender, EventArgs e) {
			DispWaveList();
			SetSize();
		}

		private void WaveSelectForm_SizeChanged(object sender, EventArgs e) {
			SetSize();
		}

		private void lstWave_DoubleClick(object sender, EventArgs e) {
			if (0 == lstWave.Items.Count) {
				return;
			}

			var idx = lstWave.SelectedIndex;
			var fm = new WaveInfoForm(new WavePlayback(), mDLS, idx);
			var index = lstWave.SelectedIndex;
			fm.ShowDialog();
			DispWaveList();
			lstWave.SelectedIndex = index;
		}

		private void btnSelect_Click(object sender, EventArgs e) {
			if (0 <= lstWave.SelectedIndex) {
				mRegion.WaveLink.TableIndex = (uint)lstWave.SelectedIndex;
			}
			Close();
		}

		private void SetSize() {
			var offsetX = 24;
			var offsetY = 48;

			btnSelect.Width = 60;
			btnSelect.Height = 24;

			lstWave.Left = 4;
			lstWave.Top = 4;
			lstWave.Width = Width - offsetX;
			lstWave.Height = Height - btnSelect.Height - 6 - offsetY;

			btnSelect.Top = lstWave.Top + lstWave.Height + 6;
			btnSelect.Left = Width - btnSelect.Width - offsetX;
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
	}
}
