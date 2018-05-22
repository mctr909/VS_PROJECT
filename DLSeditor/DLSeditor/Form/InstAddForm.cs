using System;
using System.Windows.Forms;

namespace DLSeditor
{
    public partial class InstAddForm : Form
    {
        public DLS.LINS mInstPool;
        public DLS.INS_ mSelectedInst;

        public InstAddForm()
        {
            InitializeComponent();
        }

        private void InstAddForm_Load(object sender, EventArgs e)
        {
            string[] prgList = new string[] {
                "000:Piano 1",
                "001:Piano 2",
                "002:Piano 3",
                "003:Honky-tonk",
                "004:E.Piano 1",
                "005:E.Piano 2",
                "006:Harpsichord",
                "007:Clav.",
                "008:Celesta",
                "009:Glockenspiel",
                "010:Music Box",
                "011:Vibraphone",
                "012:Marimba",
                "013:Xylophone",
                "014:Tubular-bell",
                "015:Santur",
                "016:Organ 1",
                "017:Organ 2",
                "018:Organ 3",
                "019:Church Org.1",
                "020:Reed Organ",
                "021:Accordion Fr",
                "022:Harmonica",
                "023:Bandoneon",
                "024:Nylon-str.Gt",
                "025:Steel-str.Gt",
                "026:Jazz Gt.",
                "027:Clean Gt.",
                "028:Muted Gt.",
                "029:Overdrive Gt",
                "030:DistotionGt",
                "031:Gt.Harmonics",
                "032:Acoustic Bs.",
                "033:Fingered Bs.",
                "034:Picked Bass",
                "035:Fretless Bs.",
                "036:Slap Bass 1",
                "037:Slap Bass 2",
                "038:Synth Bass 1",
                "039:Synth Bass 2",
                "040:Violin",
                "041:Viola",
                "042:Cello",
                "043:Contrabass",
                "044:Tremolo Str",
                "045:PizzicatoStr",
                "046:Harp",
                "047:Timpani",
                "048:Strings",
                "049:Slow Strings",
                "050:Syn.Strings1",
                "051:Syn.Strings2",
                "052:Choir Aahs",
                "053:Voice Oohs",
                "054:SynVox",
                "055:OrchestraHit",
                "056:Trumpet",
                "057:Trombone",
                "058:Tuba",
                "059:MutedTrumpet",
                "060:French Horns",
                "061:Brass 1",
                "062:Synth Brass1",
                "063:Synth Brass2",
                "064:Soprano Sax",
                "065:Alto Sax",
                "066:Tenor Sax",
                "067:Baritone Sax",
                "068:Oboe",
                "069:English Horn",
                "070:Bassoon",
                "071:Clarinet",
                "072:Piccolo",
                "073:Flute",
                "074:Recorder",
                "075:Pan Flute",
                "076:Bottle Blow",
                "077:Shakuhachi",
                "078:Whistle",
                "079:Ocarina",
                "080:Square Wave",
                "081:Saw Wave",
                "082:Syn.Calliope",
                "083:Chiffer Lead",
                "084:Charang",
                "085:Solo Vox",
                "086:5th Saw Wave",
                "087:Bass & Lead",
                "088:Fantasia",
                "089:Warm Pad",
                "090:Polysynth",
                "091:Space Voice",
                "092:Bowed Glass",
                "093:Metal Pad",
                "094:Halo Pad",
                "095:Sweep Pad",
                "096:Ice Rain",
                "097:Soundtrack",
                "098:Crystal",
                "099:Atmosphere",
                "100:Brightness",
                "101:Goblin",
                "102:Echo Drops",
                "103:Star Theme",
                "104:Sitar",
                "105:Banjo",
                "106:Shamisen",
                "107:Koto",
                "108:Kalimba",
                "109:Bagpipe",
                "110:Fiddle",
                "111:Shanai",
                "112:Tinkle Bell",
                "113:Agogo",
                "114:Steel Drums",
                "115:Woodblock",
                "116:Taiko",
                "117:Melo.Tom 1",
                "118:Synth Drum",
                "119:Reverse Cym.",
                "120:Gt.FretNoise",
                "121:Breath Noize",
                "122:Seashore",
                "123:Bird",
                "124:Telephone 1",
                "125:Helicopter",
                "126:Applause",
                "127:Gun Shot"
            };

            for (int i = 0; i < 128; ++i)
            {
                lstPrgNo.Items.Add(prgList[i]);
                lstBankMSB.Items.Add(i.ToString("000"));
                lstBankLSB.Items.Add(i.ToString("000"));
            }

            if (null != mSelectedInst)
            {
                rbDrum.Checked = mSelectedInst.InstHeader.IsDrum;
                rbNote.Checked = !rbDrum.Checked;
                rbDrum.Enabled = false;
                rbNote.Enabled = false;
                txtInstName.Text = mSelectedInst.Info.Name;
                Text = "音色コピー";
            }
        }

        private void lstPrgNo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstBankMSB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstBankLSB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DLS.INS_ inst;

            if (null == mSelectedInst)
            {
                inst = new DLS.INS_();
            }
            else
            {
                inst = mSelectedInst;
            }

			inst.InstHeader = new DLS.INSH();
			inst.InstHeader.IsDrum = rbDrum.Checked;
			inst.InstHeader.ProgramNo = (byte)lstPrgNo.SelectedIndex;
			inst.InstHeader.BankMSB = (byte)lstBankMSB.SelectedIndex;
			inst.InstHeader.BankLSB = (byte)lstBankLSB.SelectedIndex;
			inst.Info.Name = txtInstName.Text;

            if (null == mInstPool)
            {
                mInstPool = new DLS.LINS();
            }

            mInstPool.Add(inst);
            Close();
        }
    }
}
