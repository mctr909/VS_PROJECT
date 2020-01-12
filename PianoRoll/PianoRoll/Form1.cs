using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
            SetSize();
        }

        private void Form1_Resize(object sender, EventArgs e) {
            SetSize();
        }

        private void SetSize() {
            pianoRoll1.Left = 0;
            pianoRoll1.Top = 0;
            pianoRoll1.Width = Width - 16;
            pianoRoll1.Height = Height - 40;
        }
    }
}
