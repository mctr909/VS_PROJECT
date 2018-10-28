using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class RegionInfoForm : Form {
		private DLS.DLS mDLS;
		private DLS.MidiLocale mKey;
		private DLS.CK_RGNH mRegionId;

		private readonly string[] NoteName = new string[] {
			"C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
		};

		public RegionInfoForm(DLS.DLS dls, DLS.MidiLocale key, DLS.CK_RGNH regionId) {
			InitializeComponent();

			SetPosition();

			mDLS = dls;
			mKey = key;
			mRegionId = regionId;

			var inst = mDLS.Instruments.List[mKey];

			if (ushort.MaxValue != mRegionId.Key.Low && inst.Regions.List.ContainsKey(mRegionId)) {
				numKeyLow.Value = mRegionId.Key.Low;
				numKeyHigh.Value = mRegionId.Key.High;
				numVelocityLow.Value = mRegionId.Velocity.Low;
				numVelocityHigh.Value = mRegionId.Velocity.High;
				numKeyLow.Enabled = false;
				numKeyHigh.Enabled = false;
				numVelocityLow.Enabled = false;
				numVelocityHigh.Enabled = false;

				var region = inst.Regions.List[mRegionId];
				var wave = mDLS.WavePool.List[(int)region.WaveLink.TableIndex];

				txtWave.Text = string.Format(
					"{0} {1}",
					region.WaveLink.TableIndex.ToString("0000"),
					wave.Info.Name
				);

				numUnityNote.Value = region.Sampler.UnityNote;
				numFineTune.Value = region.Sampler.FineTune;

				btnAdd.Text = "反映";
				SetKeyLowName();
				SetKeyHighName();
			}
			else {
				numKeyLow.Value = 63;
				numKeyHigh.Value = 63;
				numVelocityLow.Value = 63;
				numVelocityHigh.Value = 63;
				btnEditWave.Enabled = false;
				btnAdd.Text = "追加";
				SetKeyLowName();
				SetKeyHighName();
			}
		}

		private void numKeyLow_ValueChanged(object sender, EventArgs e) {
			SetKeyLowName();
		}

		private void numKeyHigh_ValueChanged(object sender, EventArgs e) {
			SetKeyHighName();
		}

		private void numVelocityLow_ValueChanged(object sender, EventArgs e) {
			if (numVelocityHigh.Value < numVelocityLow.Value) {
				numVelocityHigh.Value = numVelocityLow.Value;
			}
		}

		private void numVelocityHigh_ValueChanged(object sender, EventArgs e) {
			if (numVelocityHigh.Value < numVelocityLow.Value) {
				numVelocityLow.Value = numVelocityHigh.Value;
			}
		}

		private void numUnityNote_ValueChanged(object sender, EventArgs e) {
			var oct = (int)numUnityNote.Value / 12 - 2;
			var note = (int)numUnityNote.Value % 12;
			lblUnityNote.Text = string.Format("{0}{1}", NoteName[note], oct);
		}

		private void btnSelectWave_Click(object sender, EventArgs e) {

		}

		private void btnEditWave_Click(object sender, EventArgs e) {
			var inst = mDLS.Instruments.List[mKey];
			var region = inst.Regions.List[mRegionId];
			var fm = new WaveInfoForm(new WavePlayback(), mDLS, (int)region.WaveLink.TableIndex);
			fm.ShowDialog();
		}

		private void SetPosition() {
			numKeyLow.Top = 12;
			numKeyHigh.Top = 12;
			lblKeyLow.Top = numKeyLow.Top + numKeyLow.Height + 2;
			lblKeyHigh.Top = numKeyHigh.Top + numKeyHigh.Height + 2;
			
			grbKey.Top = 4;
			grbKey.Height
				= numKeyLow.Top
				+ numKeyLow.Height + 2
				+ lblKeyLow.Height + 4
			;

			numVelocityLow.Top = numKeyLow.Top;
			numVelocityHigh.Top = numKeyHigh.Top;
			grbVelocity.Left = grbKey.Left + grbKey.Width + 36;
			grbVelocity.Top = grbKey.Top;
			grbVelocity.Height = grbKey.Height;

			txtWave.Top = 12;
			btnSelectWave.Top = 12;
			btnEditWave.Top = 12;
			btnSelectWave.Left = txtWave.Left + txtWave.Width + 4;
			btnEditWave.Left = btnSelectWave.Left + btnSelectWave.Width + 4;
			grbWave.Top = grbVelocity.Top + grbVelocity.Height + 6;
			grbWave.Width = grbKey.Width + grbVelocity.Width + 36;
			grbWave.Height = txtWave.Top + txtWave.Height + 6;

			grbUnityNote.Top = grbWave.Top + grbWave.Height + 6;
			grbFineTune.Top = grbUnityNote.Top;
			grbFineTune.Left = grbUnityNote.Left + grbUnityNote.Width + 6;
			btnAdd.Top = grbUnityNote.Top + 4;
			btnAdd.Left = grbWave.Right - btnAdd.Width;

			Width = grbWave.Left + grbWave.Width + 24;
			Height = btnAdd.Top + grbUnityNote.Height + 48;
		}

		private void SetKeyLowName() {
			var oct = (int)numKeyLow.Value / 12 - 2;
			var note = (int)numKeyLow.Value % 12;
			lblKeyLow.Text = string.Format("{0}{1}", NoteName[note], oct);

			if (numKeyHigh.Value < numKeyLow.Value) {
				numKeyHigh.Value = numKeyLow.Value;
			}
		}

		private void SetKeyHighName() {
			var oct = (int)numKeyHigh.Value / 12 - 2;
			var note = (int)numKeyHigh.Value % 12;
			lblKeyHigh.Text = string.Format("{0}{1}", NoteName[note], oct);

			if (numKeyHigh.Value < numKeyLow.Value) {
				numKeyLow.Value = numKeyHigh.Value;
			}
		}
	}
}
