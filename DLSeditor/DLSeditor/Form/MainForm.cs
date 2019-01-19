using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace DLSeditor {
    public partial class MainForm : Form {
        private DLS.DLS mDLS;
        private string mFilePath;
        private bool onRange;
        private DLS.INS mClipboardInst;

        private readonly string[] NoteName = new string[] {
            "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B"
        };

        public MainForm() {
            InitializeComponent();
            SetTabSize();
            mDLS = new DLS.DLS();

            timer1.Interval = 50;
            timer1.Enabled = true;
            timer1.Start();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        private void timer1_Tick(object sender, EventArgs e) {
            tstRegion.Text = "";
            if (onRange) {
                var posRegion = PosToRegion();
                tstRegion.Text = string.Format(
                    "強弱:{0} 音程:{1}({2}{3})",
                    posRegion.Y.ToString("000"),
                    posRegion.X.ToString("000"),
                    NoteName[posRegion.X % 12],
                    (posRegion.X / 12 - 2)
                );
            }
        }

        #region メニューバー[ファイル]
        private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
            mDLS = new DLS.DLS();
            DispInstList();
            DispWaveList();
            DispRegionInfo();
            tabControl.SelectedIndex = 0;
            mFilePath = "";
        }

        unsafe private void 開くOToolStripMenuItem_Click(object sender, EventArgs e) {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
            openFileDialog1.ShowDialog();
            var filePath = openFileDialog1.FileName;
            if (!File.Exists(filePath)) {
                return;
            }

            using (var fs = new FileStream(filePath, FileMode.Open))
            using (var br = new BinaryReader(fs)) {
                br.ReadUInt32();
                var size = br.ReadUInt32();
                br.ReadUInt32();

                var mBuff = new byte[size - 4];
                fs.Read(mBuff, 0, mBuff.Length);

                fixed (byte* p = &mBuff[0]) {
                    mDLS = new DLS.DLS(p, p + mBuff.Length);
                }

                fs.Close();
            }

            txtInstSearch.Text = "";
            txtWaveSearch.Text = "";

            DispWaveList();
            DispInstList();
            DispRegionInfo();
            DispInstAttr();

            if(0 < lstInst.Items.Count) {
                lstInst.SelectedIndex = 0;
            }

            tabControl.SelectedIndex = 0;
            mFilePath = filePath;
        }

        private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            if (string.IsNullOrWhiteSpace(mFilePath) || !File.Exists(mFilePath)) {
                名前を付けて保存ToolStripMenuItem_Click(sender, e);
            }
            mDLS.Save(mFilePath);
        }

        private void 名前を付けて保存ToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "DLSファイル(*.dls)|*.dls";
            saveFileDialog1.ShowDialog();
            var filePath = saveFileDialog1.FileName;
            if (!Directory.Exists(Path.GetDirectoryName(filePath))) {
                return;
            }

            mDLS.Save(filePath);
            mFilePath = filePath;
        }
        #endregion

        #region メニューバー[編集]
        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    AddWave();
                    break;
                case "tbpInstList":
                    AddInst();
                    break;
                case "tbpRegion":
                    AddRegion();
                    break;
            }
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    DeleteWave();
                    break;
                case "tbpInstList":
                    DeleteInst();
                    break;
                case "tbpRegion":
                    DeleteRegion();
                    break;
            }
        }

        private void コピーCToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
                case "tbpInstList":
                    CopyInst();
                    break;
            }
        }


        private void 貼り付けPToolStripMenuItem_Click(object sender, EventArgs e) {
            switch (tabControl.SelectedTab.Name) {
                case "tbpWaveList":
                    break;
                case "tbpInstList":
                    PasteInst();
                    break;
            }
        }
        #endregion

        #region ツールストリップ
        private void tsbAddInst_Click(object sender, EventArgs e) {
            AddInst();
        }

        private void tsbDeleteInst_Click(object sender, EventArgs e) {
            DeleteInst();
        }

        private void tsbCopyInst_Click(object sender, EventArgs e) {
            CopyInst();
        }

        private void tsbPasteInst_Click(object sender, EventArgs e) {
            PasteInst();
        }

        private void tsbAddWave_Click(object sender, EventArgs e) {
            AddWave();
        }

        private void tsbDeleteWave_Click(object sender, EventArgs e) {
            DeleteWave();
        }

        private void tsbOutputWave_Click(object sender, EventArgs e) {
            WaveFileOut();
        }

        private void txtWaveSearch_Leave(object sender, EventArgs e) {
            DispWaveList();
        }

        private void txtWaveSearch_TextChanged(object sender, EventArgs e) {
            DispWaveList();
        }

        private void txtInstSearch_Leave(object sender, EventArgs e) {
            DispInstList();
        }

        private void txtInstSearch_TextChanged(object sender, EventArgs e) {
            DispInstList();
        }

        private void tsbAddRange_Click(object sender, EventArgs e) {
            AddRegion();
        }

        private void tsbDeleteRange_Click(object sender, EventArgs e) {
            DeleteRegion();
        }

        private void tsbRangeList_Click(object sender, EventArgs e) {
            tsbRangeKey.Checked = false;
            tsbRangeList.Checked = true;
            tsbAddRange.Enabled = true;
            tsbDeleteRange.Enabled = true;

            pnlRegion.Visible = false;
            lstRegion.Visible = true;
        }

        private void tsbRangeKey_Click(object sender, EventArgs e) {
            tsbAddRange.Enabled = false;
            tsbDeleteRange.Enabled = false;
            tsbRangeList.Checked = false;
            tsbRangeKey.Checked = true;

            lstRegion.Visible = false;
            pnlRegion.Visible = true;
        }
        #endregion

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

            SetInstListSize();
            SetWaveListSize();
            SetInstAttributeSize();
            SetInstRegionSize();
        }

        private void SetInstListSize() {
            var offsetX = 16;
            var offsetY = 60;
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY - 4;

            lstInst.Left = 0;
            lstInst.Top = toolStrip2.Height + 4;
            lstInst.Width = width;
            lstInst.Height = height;
        }

        private void SetWaveListSize() {
            var offsetX = 16;
            var offsetY = 60;
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY - 4;

            lstWave.Left = 0;
            lstWave.Top = toolStrip3.Height + 4;
            lstWave.Width = width;
            lstWave.Height = height;
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
            var width = tabControl.Width - offsetX + 6;
            var height = tabControl.Height - offsetY + 4;

            picRegion.Width = picRegion.BackgroundImage.Width;
            picRegion.Height = picRegion.BackgroundImage.Height;

            pnlRegion.Left = 0;
            pnlRegion.Top = toolStrip1.Height + 4;
            pnlRegion.Width = width;
            pnlRegion.Height = height;

            lstRegion.Left = 0;
            lstRegion.Top = toolStrip1.Height + 4;
            lstRegion.Width = width;
            lstRegion.Height = height;
        }
        #endregion

        #region 波形一覧
        private void lstWave_DoubleClick(object sender, EventArgs e) {
            if (0 == lstWave.Items.Count) {
                return;
            }

            var cols = lstWave.SelectedItem.ToString().Split('\t');
            var idx = int.Parse(cols[0]);
            var fm = new WaveInfoForm(mDLS, idx);
            var index = lstWave.SelectedIndex;
            fm.ShowDialog();
            DispWaveList();
            lstWave.SelectedIndex = index;
        }

        private void WaveFileOut() {
            folderBrowserDialog1.ShowDialog();
            var folderPath = folderBrowserDialog1.SelectedPath;
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
                return;
            }

            var indices = lstWave.SelectedIndices;
            foreach (var idx in indices) {
                var cols = lstWave.Items[(int)idx].ToString().Split('\t');
                var wave = mDLS.WavePool.List[int.Parse(cols[0])];
                if (null == wave.Info || string.IsNullOrWhiteSpace(wave.Info.Name)) {
                    wave.ToFile(Path.Combine(folderPath, string.Format("Wave{0}.wav", idx)));
                }
                else {
                    wave.ToFile(Path.Combine(folderPath, wave.Info.Name + ".wav"));
                }
            }
        }

        private void AddWave() {
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "wavファイル(*.wav)|*.wav";
            openFileDialog1.Multiselect = true;
            openFileDialog1.ShowDialog();
            var filePaths = openFileDialog1.FileNames;

            foreach (var filePath in filePaths) {
                if (!File.Exists(filePath)) {
                    continue;
                }

                var wave = new DLS.WAVE(filePath);
                mDLS.WavePool.List.Add(mDLS.WavePool.List.Count, wave);
            }

            DispWaveList();
        }

        private void DeleteWave() {
            //
            var deleteList = new Dictionary<int, bool>();
            foreach (int selectedIndex in lstWave.SelectedIndices) {
                var useFlag = false;
                foreach (var inst in mDLS.Instruments.List.Values) {
                    foreach (var rgn in inst.Regions.List.Values) {
                        if (selectedIndex == rgn.WaveLink.TableIndex) {
                            useFlag = true;
                            break;
                        }
                    }
                }

                if (!useFlag) {
                    deleteList.Add(selectedIndex, useFlag);
                }
            }

            //
            var count = 0;
            var renumberingList = new Dictionary<int, int>();
            foreach (var wave in mDLS.WavePool.List) {
                if (!deleteList.ContainsKey(wave.Key)) {
                    if (wave.Key != count) {
                        renumberingList.Add(wave.Key, count);
                    }
                    ++count;
                }
            }

            //
            var waveList = new Dictionary<int, DLS.WAVE>();
            foreach (var wave in mDLS.WavePool.List) {
                if (!deleteList.ContainsKey(wave.Key)) {
                    waveList.Add(waveList.Count, wave.Value);
                }
            }
            mDLS.WavePool.List.Clear();
            mDLS.WavePool.List = waveList;

            //
            foreach (var inst in mDLS.Instruments.List.Values) {
                foreach (var rgn in inst.Regions.List.Values) {
                    var index = (int)rgn.WaveLink.TableIndex;
                    if (renumberingList.ContainsKey(index)) {
                        mDLS.Instruments.List[inst.Header.Locale]
                            .Regions.List[rgn.Header].WaveLink.TableIndex = (uint)renumberingList[index];
                    }
                }
            }

            DispWaveList();
        }

        private void DispWaveList() {
            var idx = lstWave.SelectedIndex;

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

                if (!string.IsNullOrEmpty(txtWaveSearch.Text)
                    && name.IndexOf(txtWaveSearch.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
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

            if (lstWave.Items.Count <= idx) {
                idx = lstWave.Items.Count - 1;
            }
            lstWave.SelectedIndex = idx;
        }
        #endregion

        #region 音色一覧
        private void lstInst_DoubleClick(object sender, EventArgs e) {
            DispRegionInfo();
            DispInstAttr();
        }

        private void AddInst() {
            var fm = new InstForm(mDLS);
            fm.ShowDialog();
            DispInstList();
        }

        private void DeleteInst() {
            if (0 == lstInst.Items.Count) {
                return;
            }

            var index = lstInst.SelectedIndex;
            var indices = lstInst.SelectedIndices;
            foreach (int idx in indices) {
                mDLS.Instruments.List.Remove(GetLocale(idx));
            }

            DispInstList();
            DispWaveList();
            DispRegionInfo();
            DispInstAttr();

            if (index < lstInst.Items.Count) {
                lstInst.SelectedIndex = index;
            }
            else {
                lstInst.SelectedIndex = lstInst.Items.Count - 1;
            }
        }

        private void CopyInst() {
            var inst = GetSelectedInst();
            if(null == inst) {
                return;
            }

            mClipboardInst = new DLS.INS();
            mClipboardInst.Header = inst.Header;

            // Regions
            mClipboardInst.Regions = new DLS.LRGN();
            foreach (var rgn in inst.Regions.List) {
                var tempRgn = new DLS.RGN();
                tempRgn.Header = rgn.Value.Header;
                tempRgn.WaveLink = rgn.Value.WaveLink;

                // Sampler
                tempRgn.Sampler = rgn.Value.Sampler;
                tempRgn.Loops = new Dictionary<int, DLS.WaveLoop>();
                foreach (var loop in rgn.Value.Loops) {
                    var tempLoop = new DLS.WaveLoop();
                    tempLoop.Size = loop.Value.Size;
                    tempLoop.Type = loop.Value.Type;
                    tempLoop.Start = loop.Value.Start;
                    tempLoop.Length = loop.Value.Length;
                    tempRgn.Loops.Add(loop.Key, tempLoop);
                }

                // Articulations
                if (null != rgn.Value.Articulations && null != rgn.Value.Articulations.ART) {
                    tempRgn.Articulations = new DLS.LART();
                    tempRgn.Articulations.ART = new DLS.ART();
                    foreach (var art in rgn.Value.Articulations.ART.List) {
                        tempRgn.Articulations.ART.List.Add(art.Key, art.Value);
                    }
                }

                mClipboardInst.Regions.List.Add(rgn.Key, tempRgn);
            }

            // Articulations
            if (null != inst.Articulations && null != inst.Articulations.ART) {
                mClipboardInst.Articulations = new DLS.LART();
                mClipboardInst.Articulations.ART = new DLS.ART();
                foreach (var art in inst.Articulations.ART.List) {
                    mClipboardInst.Articulations.ART.List.Add(art.Key, art.Value);
                }
            }

            // Info
            mClipboardInst.Info = new DLS.INFO();
            mClipboardInst.Info.Name = inst.Info.Name;
            mClipboardInst.Info.Keywords = inst.Info.Keywords;
            mClipboardInst.Info.Comments = inst.Info.Comments;
        }

        private void PasteInst() {
            if (null == mClipboardInst) {
                return;
            }

            var fm = new InstForm(mDLS, mClipboardInst);
            fm.ShowDialog();

            DispInstList();
            DispRegionInfo();
            DispInstAttr();
        }

        private void DispInstList() {
            var idx = lstInst.SelectedIndex;

            lstInst.Items.Clear();
            foreach (var inst in mDLS.Instruments.List.Values) {
                if (!string.IsNullOrEmpty(txtInstSearch.Text)
                    && inst.Info.Name.IndexOf(txtInstSearch.Text, StringComparison.InvariantCultureIgnoreCase) < 0
                ) {
                    continue;
                }

                lstInst.Items.Add(string.Format(
                    "{0}\t{1}\t{2}\t{3}\t{4}",
                    (inst.Header.Locale.BankFlags & 0x80) == 0x80 ? "Drum" : "Note",
                    inst.Header.Locale.ProgramNo.ToString("000"),
                    inst.Header.Locale.BankMSB.ToString("000"),
                    inst.Header.Locale.BankLSB.ToString("000"),
                    inst.Info.Name
                ));
            }

            if (lstInst.Items.Count <= idx) {
                idx = lstInst.Items.Count - 1;
            }
            lstInst.SelectedIndex = idx;
        }
        #endregion

        #region 音色情報
        private void txtInstName_Leave(object sender, EventArgs e) {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            if (null == inst.Info) {
                inst.Info = new DLS.INFO();
            }

            inst.Info.Name = txtInstName.Text;
            Text = txtInstName.Text;
            DispInstList();
        }

        private void txtInstKeyword_Leave(object sender, EventArgs e) {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            if (null == inst.Info) {
                inst.Info = new DLS.INFO();
            }

            inst.Info.Keywords = txtInstKeyword.Text;
        }

        private void txtInstComment_Leave(object sender, EventArgs e) {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            if (null == inst.Info) {
                inst.Info = new DLS.INFO();
            }

            inst.Info.Comments = txtInstComment.Text;
        }
        #endregion

        #region 音色属性
        private void DispInstAttr() {
            var inst = GetSelectedInst();
            if (null == inst || null == inst.Articulations || null == inst.Articulations.ART) {
                ampEnvelope.Visible = false;
            }
            else {
                ampEnvelope.Visible = true;
            }
        }

        private void tbpInstAttribute_Leave(object sender, EventArgs e) {
            var inst = GetSelectedInst();
            if (null == inst || null == inst.Articulations || null == inst.Articulations.ART) {
                return;
            }

            inst.Articulations.ART.List.Clear();
            ampEnvelope.SetList(inst.Articulations.ART.List);
        }
        #endregion

        #region 音程/強弱割り当て
        private void pictRange_DoubleClick(object sender, EventArgs e) {
            if (lstInst.SelectedIndex < 0) {
                return;
            }

            EditRegion(PosToRegionId());
        }

        private void pictRange_MouseEnter(object sender, EventArgs e) {
            onRange = true;
        }

        private void pictRange_MouseLeave(object sender, EventArgs e) {
            onRange = false;
        }

        private void lstRegion_DoubleClick(object sender, EventArgs e) {
            if (lstInst.SelectedIndex < 0) {
                return;
            }

            EditRegion(ListToRegeonId());
        }

        private void AddRegion() {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            var region = new DLS.RGN();
            region.Header.Key.Low = ushort.MaxValue;
            region.WaveLink.TableIndex = uint.MaxValue;
            var fm = new RegionInfoForm(mDLS, region);
            fm.ShowDialog();

            if (ushort.MaxValue != region.Header.Key.Low) {
                inst.Regions.List.Add(region.Header, region);
                DispRegionInfo();
            }
        }

        private void EditRegion(DLS.CK_RGNH regionId) {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            if (inst.Regions.List.ContainsKey(regionId)) {
                var region = inst.Regions.List[regionId];
                var fm = new RegionInfoForm(mDLS, region);
                fm.ShowDialog();
                DispRegionInfo();
            }
            else {
                AddRegion();
            }
        }

        private void DeleteRegion() {
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }

            var index = lstRegion.SelectedIndex;

            foreach (int idx in lstRegion.SelectedIndices) {
                var cols = lstRegion.Items[idx].ToString().Split(' ');

                var rgn = new DLS.CK_RGNH();
                rgn.Key.Low = byte.Parse(cols[1]);
                rgn.Key.High = byte.Parse(cols[2]);
                rgn.Velocity.Low = byte.Parse(cols[7]);
                rgn.Velocity.High = byte.Parse(cols[8]);

                if (inst.Regions.List.ContainsKey(rgn)) {
                    inst.Regions.List.Remove(rgn);
                }
            }

            DispRegionInfo();

            if (index < lstRegion.Items.Count) {
                lstRegion.SelectedIndex = index;
            }
            else {
                lstRegion.SelectedIndex = lstRegion.Items.Count - 1;
            }
        }

        private Point PosToRegion() {
            var posRegion = picRegion.PointToClient(Cursor.Position);
            if (posRegion.X < 0) {
                posRegion.X = 0;
            }
            if (posRegion.Y < 0) {
                posRegion.Y = 0;
            }
            if (picRegion.Width <= posRegion.X) {
                posRegion.X = picRegion.Width - 1;
            }
            if (picRegion.Height <= posRegion.Y) {
                posRegion.Y = picRegion.Height - 1;
            }

            posRegion.Y = picRegion.Height - posRegion.Y - 1;
            posRegion.X = (int)(posRegion.X / 7.0);
            posRegion.Y = (int)(posRegion.Y / 4.0);

            return posRegion;
        }

        private DLS.CK_RGNH PosToRegionId() {
            var region = new DLS.CK_RGNH();
            var posRegion = PosToRegion();
            var inst = GetSelectedInst();
            foreach (var rgn in inst.Regions.List.Values) {
                var key = rgn.Header.Key;
                var vel = rgn.Header.Velocity;
                if (key.Low <= posRegion.X && posRegion.X <= key.High
                && vel.Low <= posRegion.Y && posRegion.Y <= vel.High) {
                    region = rgn.Header;
                    break;
                }
            }

            return region;
        }

        private DLS.CK_RGNH ListToRegeonId() {
            if (lstRegion.SelectedIndex < 0) {
                return new DLS.CK_RGNH();
            }

            var cols = lstRegion.Items[lstRegion.SelectedIndex].ToString().Split(' ');
            var region = new DLS.CK_RGNH();
            region.Key.Low = ushort.Parse(cols[1]);
            region.Key.High = ushort.Parse(cols[2]);
            region.Velocity.Low = ushort.Parse(cols[7]);
            region.Velocity.High = ushort.Parse(cols[8]);

            return region;
        }

        private void DispRegionInfo() {
            var inst = GetSelectedInst();
            if (null == inst) {
                lstRegion.Items.Clear();
                if (null != picRegion.Image) {
                    picRegion.Image.Dispose();
                    picRegion.Image = null;
                }
                return;
            }

            ampEnvelope.Art = inst.Articulations.ART;

            txtInstName.Text = inst.Info.Name.Trim();
            txtInstKeyword.Text = inst.Info.Keywords.Trim();
            txtInstComment.Text = inst.Info.Comments.Trim();

            Text = inst.Info.Name.Trim();

            var bmp = new Bitmap(picRegion.Width, picRegion.Height);
            var g = Graphics.FromImage(bmp);
            var blueLine = new Pen(Color.FromArgb(255, 0, 0, 255), 2.0f);
            var greenFill = new Pen(Color.FromArgb(64, 0, 255, 0), 1.0f).Brush;

            var idx = lstRegion.SelectedIndex;
            lstRegion.Items.Clear();

            foreach (var region in inst.Regions.List.Values) {
                var key = region.Header.Key;
                var vel = region.Header.Velocity;
                g.FillRectangle(
                    greenFill,
                    key.Low * 7,
                    bmp.Height - (vel.High + 1) * 4 - 1,
                    (key.High - key.Low + 1) * 7,
                    (vel.High - vel.Low + 1) * 4
                );
                g.DrawRectangle(
                    blueLine,
                    key.Low * 7,
                    bmp.Height - (vel.High + 1) * 4,
                    (key.High - key.Low + 1) * 7,
                    (vel.High - vel.Low + 1) * 4
                );
                var waveName = "";
                if (mDLS.WavePool.List.ContainsKey((int)region.WaveLink.TableIndex)) {
                    var wave = mDLS.WavePool.List[(int)region.WaveLink.TableIndex];
                    waveName = wave.Info.Name;
                }

                var regionInfo = string.Format(
                    "音程 {0} {1}    強弱 {2} {3}",
                    region.Header.Key.Low.ToString("000"),
                    region.Header.Key.High.ToString("000"),
                    region.Header.Velocity.Low.ToString("000"),
                    region.Header.Velocity.High.ToString("000")
                );
                if (uint.MaxValue != region.WaveLink.TableIndex) {
                    regionInfo = string.Format(
                        "{0}    波形 {1} {2}",
                        regionInfo,
                        region.WaveLink.TableIndex.ToString("0000"),
                        waveName
                    );
                }
                lstRegion.Items.Add(regionInfo);
            }

            if (null != picRegion.Image) {
                picRegion.Image.Dispose();
                picRegion.Image = null;
            }
            picRegion.Image = bmp;

            if (lstRegion.Items.Count <= idx) {
                idx = lstRegion.Items.Count - 1;
            }
            lstRegion.SelectedIndex = idx;
        }
        #endregion

        private DLS.MidiLocale GetLocale(int index) {
            if (0 == lstInst.Items.Count) {
                return new DLS.MidiLocale();
            }
            if (index < 0) {
                return new DLS.MidiLocale();
            }

            var cols = lstInst.Items[index].ToString().Split('\t');

            var locale = new DLS.MidiLocale();
            locale.BankFlags = (byte)("Drum" == cols[0] ? 0x80 : 0x00);
            locale.ProgramNo = byte.Parse(cols[1]);
            locale.BankMSB = byte.Parse(cols[2]);
            locale.BankLSB = byte.Parse(cols[3]);

            return locale;
        }

        private DLS.INS GetSelectedInst() {
            var locale = GetLocale(lstInst.SelectedIndex);
            if (!mDLS.Instruments.List.ContainsKey(locale)) {
                return null;
            }

            return mDLS.Instruments.List[locale];
        }
    }
}