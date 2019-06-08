﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

namespace DLSeditor {
    public partial class MainForm : Form {
        private string mFilePath;
        private DLS.DLS mDLS;
        private DLS.INS mClipboardInst;

        public MainForm() {
            InitializeComponent();
            SetTabSize();
            mDLS = new DLS.DLS();
        }

        private void Form1_SizeChanged(object sender, EventArgs e) {
            SetTabSize();
        }

        #region メニューバー[ファイル]
        private void 新規作成NToolStripMenuItem_Click(object sender, EventArgs e) {
            mDLS = new DLS.DLS();
            DispInstList();
            DispWaveList();
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
            var inst = GetSelectedInst();
            if (null == inst) {
                return;
            }
            var fm = new InstInfoForm(mDLS, inst);
            fm.ShowDialog();
            DispInstList();
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
                    "{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                    (inst.Header.Locale.BankFlags & 0x80) == 0x80 ? "Drum" : "Note",
                    inst.Header.Locale.ProgramNo.ToString("000"),
                    inst.Header.Locale.BankMSB.ToString("000"),
                    inst.Header.Locale.BankLSB.ToString("000"),
                    inst.Info.Keywords.PadRight(10, ' ').Substring(0, 10),
                    inst.Info.Name
                ));
            }

            if (lstInst.Items.Count <= idx) {
                idx = lstInst.Items.Count - 1;
            }
            lstInst.SelectedIndex = idx;
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