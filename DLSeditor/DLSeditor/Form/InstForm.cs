﻿using System;
using System.Windows.Forms;

namespace DLSeditor {
	public partial class InstForm : Form {
		private DLS.DLS mDLS;

		private readonly string[] GM_INST_NAME = new string[] {
			"Acoustic Grand Piano",
			"Bright Acoustic Piano",
			"Electoric Grand Piano",
			"Honky-tonk Piano",
			"Electoric Piano 1",
			"Electoric Piano 2",
			"Harpsichord",
			"Clavi",
			"Celesta",
			"Glockenspiel",
			"Music Box",
			"Vibraphone",
			"Marimba",
			"Xylophone",
			"Tubular Bells",
			"Dulcimer",
			"Drawbar Organ",
			"Percussive Organ",
			"Rock Organ",
			"Church Organ",
			"Reed Organ",
			"Accordion",
			"Harmonica",
			"Tango Accordion",
			"Acoustic Giutar (Nylon)",
			"Acoustic Giutar (Steel)",
			"Electoric Giutar (Jazz)",
			"Electoric Giutar (Clean)",
			"Electoric Giutar (Muted)",
			"Overdriven Guitar",
			"Distortion Guitar",
			"Guitar Harmonics",
			"Acoustic Bass",
			"Electoric Bass (Fingar)",
			"Electoric Bass (Pick)",
			"Fretless Bass",
			"Slap Bass 1",
			"Slap Bass 2",
			"Synth Bass 1",
			"Synth Bass 2",
			"Violin",
			"Viola",
			"Cello",
			"Contrabass",
			"Tremolo Strings",
			"Pizzicato Strings",
			"Orchestral Harp",
			"Timpani",
			"String Ensemble 1",
			"String Ensemble 2",
			"Synth Strings 1",
			"Synth Strings 2",
			"Choir Aahs",
			"Voice Oohs",
			"Synth Voice",
			"Orchestra Hit",
			"Trumpet",
			"Trombone",
			"Tuba",
			"Muted Trumpet",
			"French Horn",
			"Brass Section",
			"Synth Brass 1",
			"Synth Brass 2",
			"Soprano Sax",
			"Alto Sax",
			"Tenor Sax",
			"Baritone Sax",
			"Oboe",
			"English Horn",
			"Bassoon",
			"Clarinet",
			"Piccolo",
			"Flute",
			"Recorder",
			"Pan Flute",
			"Blown Bottle",
			"Shakuhach",
			"Whistle",
			"Ocarina",
			"Square",
			"Sawtooth",
			"Calliope",
			"Chiff",
			"Charang",
			"Voice",
			"Fifths",
			"Bass+Lead",
			"New age",
			"Warm",
			"Polysynth",
			"Choir",
			"Bowed",
			"Metallic",
			"Halo",
			"Sweep",
			"Rain",
			"Soundtrack",
			"Crystal",
			"Atmosphere",
			"Brightness",
			"Goblins",
			"Echoes",
			"Sci-Fi",
			"Sitar",
			"Banjo",
			"Shamisen",
			"Koto",
			"Kalimba",
			"Bag Pipe",
			"Fiddle",
			"Shanai",
			"Tinkle Bell",
			"Agogo",
			"Steel Drums",
			"Woodblock",
			"Taiko Drum",
			"Melodic Tom",
			"Synth Drum",
			"Reverse Cymbal",
			"Guitar Fret Noise",
			"Breath Noise",
			"Seashore",
			"Bird Tweet",
			"Telephone Ring",
			"Helicopter",
			"Applause",
			"Gunshot"
		};

		public InstForm(DLS.DLS dls) {
			InitializeComponent();
			StartPosition = FormStartPosition.CenterParent;
			mDLS = dls;
		}

		private void InstAddForm_Load(object sender, EventArgs e) {
			setList();
		}

		private void rbDrum_CheckedChanged(object sender, EventArgs e) {
			var index = lstPrgNo.SelectedIndex;
			if (rbDrum.Checked) {
				lstPrgNo.Items.Clear();
				for (byte i = 0; i < 128; ++i) {
					lstPrgNo.Items.Add(string.Format("{0} {1}", i.ToString("000"), ""));
				}
			}
			else {
				lstPrgNo.Items.Clear();
				for (byte i = 0; i < 128; ++i) {
					lstPrgNo.Items.Add(string.Format("{0} {1}", i.ToString("000"), GM_INST_NAME[i]));
				}
			}
			lstPrgNo.SelectedIndex = index;
		}

		private void lstPrgNo_SelectedIndexChanged(object sender, EventArgs e) {
			if (!string.IsNullOrWhiteSpace(txtInstName.Text)) {
				return;
			}
			if (rbDrum.Checked) {

			}
			else {
				txtInstName.Text = GM_INST_NAME[lstPrgNo.SelectedIndex];
			}
		}

		private void lstBankMSB_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void lstBankLSB_SelectedIndexChanged(object sender, EventArgs e) {

		}

		private void btnAdd_Click(object sender, EventArgs e) {
			var inst = new DLS.INS(
				(byte)lstPrgNo.SelectedIndex,
				(byte)lstBankMSB.SelectedIndex,
				(byte)lstBankLSB.SelectedIndex,
				rbDrum.Checked
			);
			inst.Info.Name = txtInstName.Text;

			if (mDLS.Instruments.List.ContainsKey(inst.Header.Locale)) {
				return;
			}

			mDLS.Instruments.List.Add(inst.Header.Locale, inst);
			Close();
		}

		private void setList() {
			for (byte i = 0; i < 128; ++i) {
				lstPrgNo.Items.Add(string.Format("{0} {1}", i.ToString("000"), GM_INST_NAME[i]));
				lstBankMSB.Items.Add(i.ToString("000"));
				lstBankLSB.Items.Add(i.ToString("000"));
			}
			lstPrgNo.SelectedIndex = 0;
			lstBankMSB.SelectedIndex = 0;
			lstBankLSB.SelectedIndex = 0;
			txtInstName.Text = "";
		}
	}
}