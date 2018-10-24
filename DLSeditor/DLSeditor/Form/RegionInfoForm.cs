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
		private readonly string[] NoteName = new string[] {
			"C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
		};

		public RegionInfoForm() {
			InitializeComponent();
			SetPosition();
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
			glbVelocity.Top = glbKey.Top;
			glbVelocity.Height = glbKey.Height;
		}
	}
}
