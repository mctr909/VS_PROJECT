using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class RegionInfoForm : Form {
		private DLS.DLS mDLS;
		private DLS.RGN mRegion;

		private readonly string[] NoteName = new string[] {
			"C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
		};

		public RegionInfoForm(DLS.DLS dls, DLS.RGN region) {
			InitializeComponent();

			mDLS = dls;
			mRegion = region;

			SetPosition();
			DispRegionInfo();
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
			var fm = new WaveSelectForm(mDLS, mRegion);
			fm.ShowDialog();

			if (mDLS.WavePool.List.ContainsKey((int)mRegion.WaveLink.TableIndex)) {
				var wave = mDLS.WavePool.List[(int)mRegion.WaveLink.TableIndex];
				btnEditWave.Enabled = true;
				txtWave.Text = string.Format(
					"{0} {1}",
					mRegion.WaveLink.TableIndex.ToString("0000"),
					wave.Info.Name
				);
			}
			else {
				btnEditWave.Enabled = false;
				txtWave.Text = "";
			}
		}

		private void btnEditWave_Click(object sender, EventArgs e) {
			var fm = new WaveInfoForm(mDLS, (int)mRegion.WaveLink.TableIndex);
			fm.ShowDialog();
		}

		private void btnAdd_Click(object sender, EventArgs e) {
			if (ushort.MaxValue == mRegion.Header.Key.Low) {
				mRegion.Header.Key.Low = (ushort)numKeyLow.Value;
				mRegion.Header.Key.High = (ushort)numKeyHigh.Value;
				mRegion.Header.Velocity.Low = (ushort)numVelocityLow.Value;
				mRegion.Header.Velocity.High = (ushort)numVelocityHigh.Value;
			}

			if (!chkLoop.Checked) {
				mRegion.Loops.Clear();
				mRegion.Sampler.LoopCount = 0;
			}

			mRegion.Sampler.UnityNote = (ushort)numUnityNote.Value;
			mRegion.Sampler.FineTune = (short)numFineTune.Value;
			mRegion.Sampler.Gain = (double)numVolume.Value / 100.0;
			Close();
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
			grbVolume.Top = grbUnityNote.Top;
			grbVolume.Left = grbFineTune.Left + grbFineTune.Width + 6;

			chkLoop.Top = grbVolume.Top + grbVolume.Height + 6;

			btnAdd.Top = chkLoop.Top;
			btnAdd.Left = grbWave.Right - btnAdd.Width;

			Width = grbWave.Left + grbWave.Width + 24;
			Height = btnAdd.Top + btnAdd.Height + 48;
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

		private void DispRegionInfo() {
			if (ushort.MaxValue == mRegion.Header.Key.Low) {
				numKeyLow.Value = 63;
				numKeyHigh.Value = 63;
				numVelocityLow.Value = 0;
				numVelocityHigh.Value = 127;
				btnEditWave.Enabled = false;

				btnAdd.Text = "追加";
				SetKeyLowName();
				SetKeyHighName();
			}
			else {
				numKeyLow.Value = mRegion.Header.Key.Low;
				numKeyHigh.Value = mRegion.Header.Key.High;
				numVelocityLow.Value = mRegion.Header.Velocity.Low;
				numVelocityHigh.Value = mRegion.Header.Velocity.High;
				numKeyLow.Enabled = false;
				numKeyHigh.Enabled = false;
				numVelocityLow.Enabled = false;
				numVelocityHigh.Enabled = false;

				var waveName = "";
				if (mDLS.WavePool.List.ContainsKey((int)mRegion.WaveLink.TableIndex)) {
					var wave = mDLS.WavePool.List[(int)mRegion.WaveLink.TableIndex];
					waveName = wave.Info.Name;
					btnEditWave.Enabled = true;
				}
				else {
					btnEditWave.Enabled = false;
				}

				if (uint.MaxValue == mRegion.WaveLink.TableIndex) {
					txtWave.Text = "";
				}
				else {
					txtWave.Text = string.Format(
						"{0} {1}",
						mRegion.WaveLink.TableIndex.ToString("0000"),
						waveName
					);
				}

				if(0 < mRegion.Loops.Count) {
					chkLoop.Checked = true;
				}
				else {
					chkLoop.Checked = false;
				}

				numUnityNote.Value = mRegion.Sampler.UnityNote;
				numFineTune.Value = mRegion.Sampler.FineTune;
				numVolume.Value = (decimal)(mRegion.Sampler.Gain * 100.0);

				btnAdd.Text = "反映";
				SetKeyLowName();
				SetKeyHighName();
			}
		}
	}
}
