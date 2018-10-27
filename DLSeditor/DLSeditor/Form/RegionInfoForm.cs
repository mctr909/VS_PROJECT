using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

			if (ushort.MaxValue != mRegionId.Key.Low) {
				numKeyLow.Value = mRegionId.Key.Low;
				numKeyHigh.Value = mRegionId.Key.High;
				numVelocityLow.Value = mRegionId.Velocity.Low;
				numVelocityHigh.Value = mRegionId.Velocity.High;
				numKeyLow.Enabled = false;
				numKeyHigh.Enabled = false;
				numVelocityLow.Enabled = false;
				numVelocityHigh.Enabled = false;

				var inst = mDLS.Instruments.List[mKey];
				var region = inst.Regions.List[mRegionId];
				var wave = mDLS.WavePool.List[(int)region.WaveLink.TableIndex];

				txtWave.Text = string.Format(
					"{0} {1}",
					region.WaveLink.TableIndex.ToString("0000"),
					wave.Info.Name
				);
			}
			else {
				numKeyLow.Value = 63;
				numKeyHigh.Value = 63;
				numVelocityLow.Value = 63;
				numVelocityHigh.Value = 63;
				btnEditWave.Enabled = false;
			}
		}

		private void numKeyLow_ValueChanged(object sender, EventArgs e) {
			var oct = (int)numKeyLow.Value / 12 - 2;
			var note = (int)numKeyLow.Value % 12;
			lblKeyLow.Text = string.Format("{0}{1}", NoteName[note], oct);

			if (numKeyHigh.Value < numKeyLow.Value) {
				numKeyHigh.Value = numKeyLow.Value;
			}
		}

		private void numKeyHigh_ValueChanged(object sender, EventArgs e) {
			var oct = (int)numKeyHigh.Value / 12 - 2;
			var note = (int)numKeyHigh.Value % 12;
			lblKeyHigh.Text = string.Format("{0}{1}", NoteName[note], oct);

			if (numKeyHigh.Value < numKeyLow.Value) {
				numKeyLow.Value = numKeyHigh.Value;
			}
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

		private void btnSelectWave_Click(object sender, EventArgs e) {

		}

		private void btnEditWave_Click(object sender, EventArgs e) {

		}

		private void SetPosition() {
			numKeyLow.Top = 12;
			numKeyHigh.Top = 12;
			lblKeyLow.Top = numKeyLow.Top + numKeyLow.Height + 2;
			lblKeyHigh.Top = numKeyHigh.Top + numKeyHigh.Height + 2;
			
			glbKey.Top = 4;
			glbKey.Height
				= numKeyLow.Top
				+ numKeyLow.Height + 2
				+ lblKeyLow.Height + 4
			;

			numVelocityLow.Top = numKeyLow.Top;
			numVelocityHigh.Top = numKeyHigh.Top;
			glbVelocity.Left = glbKey.Left + glbKey.Width + 36;
			glbVelocity.Top = glbKey.Top;
			glbVelocity.Height = glbKey.Height;

			txtWave.Top = 12;
			btnSelectWave.Top = 12;
			btnEditWave.Top = 12;
			btnSelectWave.Left = txtWave.Left + txtWave.Width + 4;
			btnEditWave.Left = btnSelectWave.Left + btnSelectWave.Width + 4;
			glbWave.Top = glbVelocity.Top + glbVelocity.Height + 6;
			glbWave.Width = glbKey.Width + glbVelocity.Width + 36;
			glbWave.Height = txtWave.Top + txtWave.Height + 6;
		}
	}
}
