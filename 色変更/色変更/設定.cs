using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 色変更
{
    public partial class 設定 : Form
    {
        #region フィールド
        /// <summary>画像編集用ワーク</summary>
        public Bitmap[] BmpWork = new Bitmap[64];

        /// <summary>プレビュー子画面</summary>
        private static Form1 FmPreview = new 色変更.Form1();

        /// <summary>設定情報リスト</summary>
        private Dictionary<string, COLOR.Config> ConfigList;

        /// <summary>設定情報</summary>
        private COLOR.Config Config;

        /// <summary>色相環画像</summary>
        private Bitmap BmpHue;

        /// <summary>抽出条件の色相環がドラッグされているか</summary>
        private bool IsDraggingCondHue;

        /// <summary>変更色の色相環がドラッグされているか</summary>
        private bool IsDraggingChgHue;
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public 設定()
        {
            InitializeComponent();

            //*** プレビュー子画面を表示 ***//
            FmPreview.Show();

            //*** 色相環画像生成 ***//
            BmpHue = new Bitmap(picCondHue.Width - 2, picCondHue.Height - 2);
            double x, y, r;
            COLOR.HSL hsl = new COLOR.HSL();
            Color rgb = new Color();

            for (int py = 0; py < BmpHue.Height; ++py)
            {
                for (int px = 0; px < BmpHue.Width; ++px)
                {
                    x = 2.0 * px / BmpHue.Width - 1.0;
                    y = 1.0 - 2.0 * py / BmpHue.Height;
                    r = Math.Sqrt(x * x + y * y);

                    if (r >= 0.75 && r <= 0.9)
                    {
                        hsl.H = (int)(45 * Math.Atan2(y, x) / Math.Atan(1.0));
                        hsl.S = 100;
                        hsl.L = 50;
                        hsl.A = 255;
                        COLOR.HSLtoRGB(ref hsl, ref rgb);
                        BmpHue.SetPixel(px, py, rgb);
                    }
                }
            }

            //*** 設定の読み込み ***//
            InitSetting();

            //*** 値の反映 ***//
            ValueChenged();
        }

        /// <summary>
        /// 設定リストの選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            //*** 設定の読み込み ***//
            Config = ConfigList.ToArray()[cmbSetting.SelectedIndex].Value;

            //*** 抽出条件 ***//
            trkCondHueWidth.Value = Config.Before.HWidth;
            trkCondASaturation.Value = Config.Before.SMin;
            trkCondBSaturation.Value = Config.Before.SMax;
            trkCondALight.Value = Config.Before.LMin;
            trkCondBLight.Value = Config.Before.LMax;

            //*** 変更色 ***//
            trkChgASaturation.Value = Config.After.SMin;
            trkChgBSaturation.Value = Config.After.SMax;
            trkChgALight.Value = Config.After.LMin;
            trkChgBLight.Value = Config.After.LMax;

            chkHueNoChg.Checked = false;
            chkSaturationNoChg.Checked = false;
            chkLightNoChg.Checked = false;

            //*** 値の反映 ***//
            ValueChenged();
        }

        #region クリックイベント
        /// <summary>
        /// 画像の保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BmpWork[0] == null) return;

            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "PNGファイル(*.png)|*.png";
            saveFileDialog1.ShowDialog();

            string saveFileName = saveFileDialog1.FileName;
            if (string.IsNullOrEmpty(saveFileName)) return;

            BmpWork[0].Save(saveFileName, System.Drawing.Imaging.ImageFormat.Png);
        }

        /// <summary>
        /// プレビュー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPreview_Click(object sender, EventArgs e)
        {
            if (BmpWork[0] == null) return;

            for (int i = BmpWork.Length - 1; i > 0; --i)
            {
                BmpWork[i] = BmpWork[i - 1];
            }

            btnUndo.Enabled = (BmpWork[1] != null);
            btnRedo.Enabled = (BmpWork[BmpWork.Length - 1] != null);

            BmpWork[0] = COLOR.ChangeColor(BmpWork[0], Config);
            FmPreview.Draw(BmpWork[0]);
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            Bitmap tmp = BmpWork[0];
            for (int i = 1; i < BmpWork.Length; ++i)
            {
                BmpWork[i - 1] = BmpWork[i];
            }
            BmpWork[BmpWork.Length - 1] = tmp;

            btnUndo.Enabled = (BmpWork[1] != null);
            btnRedo.Enabled = (BmpWork[BmpWork.Length - 1] != null);

            FmPreview.Draw(BmpWork[0]);
        }

        /// <summary>
        /// Redo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRedo_Click(object sender, EventArgs e)
        {
            Bitmap tmp = BmpWork[BmpWork.Length - 1];
            for (int i = BmpWork.Length - 1; i > 0; --i)
            {
                BmpWork[i] = BmpWork[i - 1];
            }
            BmpWork[0] = tmp;

            btnUndo.Enabled = (BmpWork[1] != null);
            btnRedo.Enabled = (BmpWork[BmpWork.Length - 1] != null);

            FmPreview.Draw(BmpWork[0]);
        }

        /// <summary>
        /// 設定の保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            string name = cmbSetting.Text;

            if (name == "")
            {
                MessageBox.Show("名前をつけてください。");
                return;
            }

            int dupIndex = -1;
            for (var i = 0; i < cmbSetting.Items.Count; ++i)
            {
                if ((string)cmbSetting.Items[i] == name)
                {
                    dupIndex = i;
                    break;
                }
            }

            if (dupIndex >= 0)
            {
                if (DialogResult.No == MessageBox.Show("同じ名前の設定が存在します。\n上書きしますか？", "設定の保存", MessageBoxButtons.YesNo)) return;
                ConfigList[name] = Config;
                cmbSetting.SelectedIndex = dupIndex;
            }
            else
            {
                cmbSetting.Items.Add(name);
                ConfigList.Add(name, Config);
            }

            WriteSetting("");
        }

        /// <summary>
        /// 設定の削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteSetting_Click(object sender, EventArgs e)
        {
            if (cmbSetting.SelectedIndex < 0) return;
            ConfigList.Remove((string)cmbSetting.Items[cmbSetting.SelectedIndex]);
            cmbSetting.Items.Remove(cmbSetting.Items[cmbSetting.SelectedIndex]);
            WriteSetting("");
        }
        #endregion

        #region 抽出条件のイベント
        private void picCondHue_MouseDown(object sender, MouseEventArgs e)
        {
            IsDraggingCondHue = true;
        }

        private void picCondHue_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraggingCondHue = false;
        }

        private void picCondHue_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraggingCondHue)
            {
                GetHue(groupBox1, picCondHue, ref Config.Before);
                ValueChenged();
            }
        }

        private void trkCondHueWidth_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkCondASaturation_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkCondBSaturation_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkCondALight_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkCondBLight_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }
        #endregion

        #region 変更色のイベント
        private void picChgHue_MouseDown(object sender, MouseEventArgs e)
        {
            IsDraggingChgHue = true;
        }

        private void picChgHue_MouseUp(object sender, MouseEventArgs e)
        {
            IsDraggingChgHue = false;
        }

        private void picChgHue_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsDraggingChgHue)
            {
                GetHue(groupBox2, picChgHue, ref Config.After);
                ValueChenged();
            }
        }

        private void trkChgASaturation_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkChgBSaturation_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkChgALight_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void trkChgBLight_Scroll(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void chkHueNoChg_CheckedChanged(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void chkSaturationNoChg_CheckedChanged(object sender, EventArgs e)
        {
            ValueChenged();
        }

        private void chkLightNoChg_CheckedChanged(object sender, EventArgs e)
        {
            ValueChenged();
        }
        #endregion

        #region 共通
        /// <summary>
        /// 値の反映
        /// </summary>
        private void ValueChenged()
        {
            int tmp;

            #region 抽出条件
            Config.Before.HWidth = trkCondHueWidth.Value;
            Config.Before.SMin = trkCondASaturation.Value;
            Config.Before.SMax = trkCondBSaturation.Value;
            Config.Before.LMin = trkCondALight.Value;
            Config.Before.LMax = trkCondBLight.Value;
            if (Config.Before.SMin > Config.Before.SMax)
            {
                tmp = Config.Before.SMin;
                Config.Before.SMin = Config.Before.SMax;
                Config.Before.SMax = tmp;
            }
            if (Config.Before.LMin > Config.Before.LMax)
            {
                tmp = Config.Before.LMin;
                Config.Before.LMin = Config.Before.LMax;
                Config.Before.LMax = tmp;
            }

            lblCondHue.Text = string.Format("色相 {0}°", Config.Before.H);
            lblCondHueWidth.Text = string.Format("色相幅 ±{0}°", Config.Before.HWidth);

            if (Config.Before.SMin == Config.Before.SMax)
            {
                lblCondSaturation.Text = string.Format("彩度 {0}%", Config.Before.SMin);
            }
            else
            {
                lblCondSaturation.Text = string.Format("彩度 {0}～{1}%", Config.Before.SMin, Config.Before.SMax);
            }

            if (Config.Before.LMin == Config.Before.LMax)
            {
                lblCondLight.Text = string.Format("明度 {0}%", Config.Before.LMin);
            }
            else
            {
                lblCondLight.Text = string.Format("明度 {0}～{1}%", Config.Before.LMin, Config.Before.LMax);
            }
            #endregion

            #region 変更色
            if (chkHueNoChg.Checked)
            {
                Config.After.H = Config.Before.H;
            }

            if (chkSaturationNoChg.Checked)
            {
                Config.After.SMin = Config.Before.SMin;
                Config.After.SMax = Config.Before.SMax;
                trkChgASaturation.Value = trkCondASaturation.Value;
                trkChgBSaturation.Value = trkCondBSaturation.Value;
            }
            else
            {
                Config.After.SMin = trkChgASaturation.Value;
                Config.After.SMax = trkChgBSaturation.Value;
            }

            if (chkLightNoChg.Checked)
            {
                Config.After.LMin = Config.Before.LMin;
                Config.After.LMax = Config.Before.LMax;
                trkChgALight.Value = trkCondALight.Value;
                trkChgBLight.Value = trkCondBLight.Value;
            }
            else
            {
                Config.After.LMin = trkChgALight.Value;
                Config.After.LMax = trkChgBLight.Value;
            }

            if (Config.After.SMin > Config.After.SMax)
            {
                tmp = Config.After.SMin;
                Config.After.SMin = Config.After.SMax;
                Config.After.SMax = tmp;
            }

            if (Config.After.LMin > Config.After.LMax)
            {
                tmp = Config.After.LMin;
                Config.After.LMin = Config.After.LMax;
                Config.After.LMax = tmp;
            }

            lblChgHue.Text = string.Format("色相 {0}°", Config.After.H);

            if (Config.After.SMin == Config.After.SMax)
            {
                lblChgSaturation.Text = string.Format("彩度 {0}%", Config.After.SMin);
            }
            else
            {
                lblChgSaturation.Text = string.Format("彩度 {0}～{1}%", Config.After.SMin, Config.After.SMax);
            }

            if (Config.After.LMin == Config.After.LMax)
            {
                lblChgLight.Text = string.Format("明度 {0}%", Config.After.LMin);
            }
            else
            {
                lblChgLight.Text = string.Format("明度 {0}～{1}%", Config.After.LMin, Config.After.LMax);
            }
            #endregion

            // HSL色空間の描画
            DrawHSL(picCondHue, picCondSaturation, picCondLight, Config.Before);
            DrawHSL(picChgHue, picChgSaturation, picChgLight, Config.After);
        }

        /// <summary>
        /// 色相の取得
        /// </summary>
        /// <param name="grp"></param>
        /// <param name="pict"></param>
        /// <param name="param"></param>
        private void GetHue(GroupBox grp, PictureBox pict, ref COLOR.Param param)
        {
            int ix = Cursor.Position.X - (Location.X + grp.Location.X + pict.Location.X + 9);
            int iy = Cursor.Position.Y - (Location.Y + grp.Location.Y + pict.Location.Y + 31);
            double px = 2.0 * ix / pict.Width - 1.0;
            double py = 1.0 - 2.0 * iy / pict.Height;
            param.H = (int)(45 * Math.Atan2(py, px) / Math.Atan(1.0) + 0.5);
        }

        /// <summary>
        /// HSL色空間の描画
        /// </summary>
        /// <param name="pictH"></param>
        /// <param name="pictS"></param>
        /// <param name="pictL"></param>
        /// <param name="param"></param>
        private void DrawHSL(PictureBox pictH, PictureBox pictS, PictureBox pictL, COLOR.Param param)
        {
            Pen penNorm = new Pen(Color.Black, 1.0f);
            Pen penBold = new Pen(Color.Black, 4.0f);
            Pen penBlock = new Pen(Color.Black, 8.0f);

            //*** 色相環の描画 ***//
            Bitmap bmpHue = new Bitmap(BmpHue.Width, BmpHue.Height);
            Graphics gHue = Graphics.FromImage(bmpHue);
            RectangleF rectHue = new RectangleF(0.0f, 0.0f, bmpHue.Width, bmpHue.Height);

            double th = -Math.Atan(1.0) * param.H / 45.0;
            float ax = (float)(bmpHue.Width * Math.Cos(th) / 2.0);
            float ay = (float)(bmpHue.Height * Math.Sin(th) / 2.0);
            float bx = 0.75f * ax + bmpHue.Width / 2.0f;
            float by = 0.75f * ay + bmpHue.Height / 2.0f;
            ax += bmpHue.Width / 2.0f;
            ay += bmpHue.Height / 2.0f;

            gHue.DrawImage(BmpHue, 0, 0);
            gHue.DrawArc(penBold, rectHue, -param.H, param.HWidth);
            gHue.DrawArc(penBold, rectHue, -param.H - param.HWidth, param.HWidth);
            gHue.DrawLine(penNorm, new PointF(ax, ay), new PointF(bx, by));
            pictH.Image = bmpHue;

            //*** 彩度と明度の描画 ***//
            Bitmap bmpS = new Bitmap(pictS.Width, pictS.Height);
            Bitmap bmpL = new Bitmap(pictL.Width, pictL.Height);
            Graphics gS = Graphics.FromImage(bmpS);
            Graphics gL = Graphics.FromImage(bmpL);

            COLOR.HSL hsl = new COLOR.HSL();
            Color rgb = new Color();
            double k = 100.0 / bmpS.Width;

            for (float px = 0; px < bmpS.Width; px += penBlock.Width)
            {
                hsl.H = param.H;
                hsl.S = (int)(k * px);
                hsl.L = 50;
                hsl.A = 255;
                COLOR.HSLtoRGB(ref hsl, ref rgb);
                penBlock.Color = rgb;
                gS.DrawLine(penBlock, px, 0, px, bmpS.Height - 1);

                hsl.H = param.H;
                hsl.S = param.SMax;
                hsl.L = (int)(k * px);
                hsl.A = 255;
                COLOR.HSLtoRGB(ref hsl, ref rgb);
                penBlock.Color = rgb;
                gL.DrawLine(penBlock, px, 0, px, bmpL.Height - 1);
            }

            pictS.Image = bmpS;
            pictL.Image = bmpL;
        }

        /// <summary>
        /// 設定の初期化
        /// </summary>
        private void InitSetting()
        {
            string exePath = Application.ExecutablePath;
            string fileName = Path.GetDirectoryName(exePath) + "\\" + Path.GetFileNameWithoutExtension(exePath) + ".ini";

            if (File.Exists(fileName))
            {
                ReadSetting(fileName);
            }
            else
            {
                COLOR.Config config = new COLOR.Config();
                ConfigList = new Dictionary<string, COLOR.Config>();

                config.Before.H = 0;
                config.Before.HWidth = 180;
                config.Before.SMin = 0;
                config.Before.SMax = 100;
                config.Before.LMin = 0;
                config.Before.LMax = 100;
                config.After.H = 0;
                config.After.HWidth = 0;
                config.After.SMin = 0;
                config.After.SMax = 0;
                config.After.LMin = 0;
                config.After.LMax = 100;
                ConfigList.Add("モノクロ", config);

                config.Before.H = 0;
                config.Before.HWidth = 180;
                config.Before.SMin = 0;
                config.Before.SMax = 100;
                config.Before.LMin = 0;
                config.Before.LMax = 100;
                config.After.H = 50;
                config.After.HWidth = 0;
                config.After.SMin = 25;
                config.After.SMax = 70;
                config.After.LMin = 0;
                config.After.LMax = 100;
                ConfigList.Add("セピア", config);

                WriteSetting(fileName);
            }

            cmbSetting.Items.Clear();
            foreach (KeyValuePair<string, COLOR.Config> item in ConfigList.ToArray())
            {
                cmbSetting.Items.Add(item.Key);
            }
        }

        /// <summary>
        /// 設定をファイルから読み込む
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadSetting(string fileName)
        {
            if (!File.Exists(fileName)) return;

            ConfigList = new Dictionary<string, COLOR.Config>();

            COLOR.Config config = new COLOR.Config();
            StreamReader sr = new StreamReader(fileName);
            while(!sr.EndOfStream)
            {
                string[] cols = sr.ReadLine().Split('\t');
                config.Before.H = int.Parse(cols[0]);
                config.Before.HWidth = int.Parse(cols[1]);
                config.Before.SMin = int.Parse(cols[2]);
                config.Before.SMax = int.Parse(cols[3]);
                config.Before.LMin = int.Parse(cols[4]);
                config.Before.LMax = int.Parse(cols[5]);
                config.After.H = int.Parse(cols[6]);
                config.After.HWidth = int.Parse(cols[7]);
                config.After.SMin = int.Parse(cols[8]);
                config.After.SMax = int.Parse(cols[9]);
                config.After.LMin = int.Parse(cols[10]);
                config.After.LMax = int.Parse(cols[11]);

                ConfigList.Add(cols[12], config);
            }
            sr.Close();
        }

        /// <summary>
        /// 設定をファイルに書き出す
        /// </summary>
        /// <param name="fileName"></param>
        private void WriteSetting(string fileName)
        {
            if (fileName == "")
            {
                string exePath = Application.ExecutablePath;
                fileName = Path.GetDirectoryName(exePath) + "\\" + Path.GetFileNameWithoutExtension(exePath) + ".ini";
            }

            StreamWriter sw = new StreamWriter(fileName);
            foreach (KeyValuePair<string, COLOR.Config> item in ConfigList.ToArray())
            {
                sw.WriteLine(
                    "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\t{11}\t{12}"
                    , item.Value.Before.H.ToString("000")
                    , item.Value.Before.HWidth.ToString("000")
                    , item.Value.Before.SMin.ToString("000")
                    , item.Value.Before.SMax.ToString("000")
                    , item.Value.Before.LMin.ToString("000")
                    , item.Value.Before.LMax.ToString("000")
                    , item.Value.After.H.ToString("000")
                    , item.Value.After.HWidth.ToString("000")
                    , item.Value.After.SMin.ToString("000")
                    , item.Value.After.SMax.ToString("000")
                    , item.Value.After.LMin.ToString("000")
                    , item.Value.After.LMax.ToString("000")
                    , item.Key
                );
            }
            sw.Close();
        }
        #endregion
    }
}
