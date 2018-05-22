using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLSeditor
{
	public partial class WaveListForm : Form
	{
		public DLS.LINS mInstPool;
		public DLS.WVPL mWave;

		public WaveListForm()
		{
			InitializeComponent();
		}

		private void WaveListForm_Load(object sender, EventArgs e)
		{
			lstWave.Items.Clear();
			foreach(var wave in mWave.List)
			{
				lstWave.Items.Add(wave.Info.Name);
			}
			SetListSize();
		}

		private void WaveListForm_SizeChanged(object sender, EventArgs e)
		{
			SetListSize();
		}

		private void SetListSize()
		{
			var offsetX = 40;
			var offsetY = 108;
			var width = Width - offsetX;
			var height = Height - offsetY;

			if (width < 100)
			{
				return;
			}

			if (height < 100)
			{
				return;
			}

			lstWave.Width = width;
			lstWave.Height = height;
		}
	}
}
