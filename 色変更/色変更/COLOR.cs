using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

public class COLOR
{
    public struct HSL
    {
        public int H;
        public int S;
        public int L;
        public byte A;
    }

    public struct Param
    {
        public int H;
        public int HWidth;
        public int SMin;
        public int SMax;
        public int LMin;
        public int LMax;
    }

    public struct Config
    {
        public Config(Param before, Param after)
        {
            Before = before;
            After = after;
        }
        public Param Before;
        public Param After;
    }

    public static void RGBtoHSL(ref byte b, ref byte g, ref byte r, ref byte a, ref HSL hsl)
    {
        byte max = Math.Max(r, Math.Max(g, b));
        byte min = Math.Min(r, Math.Min(g, b));

        #region Hue
        if (r == g && g == b)
        {
            hsl.H = 0;
        }
        else if (r == max)
        {
            hsl.H = 60 * (g - b) / (max - min);
        }
        else if (g == max)
        {
            hsl.H = 60 * (b - r) / (max - min) + 120;
        }
        else if (b == max)
        {
            hsl.H = 60 * (r - g) / (max - min) + 240;
        }

        if (hsl.H < -180)
        {
            hsl.H += 360;
        }
        else if(hsl.H > 180)
        {
            hsl.H -= 360;
        }
        #endregion

        #region Saturation
        int cnt = (max + min) / 2;

        if (cnt == 255)
        {
            hsl.S = 100;
        }
        else if (cnt == 0)
        {
            hsl.S = 0;
        }
        else if (cnt >= 128)
        {
            hsl.S = 100 * (max - cnt) / (255 - cnt);
        }
        else
        {
            hsl.S = 100 * (cnt - min) / cnt;
        }
        #endregion

        // Light
        hsl.L = 100 * cnt / 255;

        hsl.A = a;
    }

    public static void HSLtoRGB(ref HSL hsl, ref byte[] pix, ref int offset)
    {
        float max, min, width;

        if (hsl.L < 50)
        {
            max = 2.55f * (hsl.L + hsl.L * hsl.S / 100.0f);
            min = 2.55f * (hsl.L - hsl.L * hsl.S / 100.0f);
            width = (max - min) / 60.0f;
        }
        else
        {
            max = 2.55f * (hsl.L + (100 - hsl.L) * hsl.S / 100.0f);
            min = 2.55f * (hsl.L - (100 - hsl.L) * hsl.S / 100.0f);
            width = (max - min) / 60.0f;
        }

        if (hsl.H < 0)
        {
            hsl.H += 360;
        }

        switch (hsl.H / 60)
        {
            case 0:
                pix[offset] = (byte)min;
                pix[offset + 1] = (byte)(hsl.H * width + min);
                pix[offset + 2] = (byte)max;
                pix[offset + 3] = hsl.A;
                return;
            case 1:
                pix[offset] = (byte)min;
                pix[offset + 1] = (byte)max;
                pix[offset + 2] = (byte)((120 - hsl.H) * width + min);
                pix[offset + 3] = hsl.A;
                return;
            case 2:
                pix[offset] = (byte)((hsl.H - 120) * width + min);
                pix[offset + 1] = (byte)max;
                pix[offset + 2] = (byte)min;
                pix[offset + 3] = hsl.A;
                return;
            case 3:
                pix[offset] = (byte)max;
                pix[offset + 1] = (byte)((240 - hsl.H) * width + min);
                pix[offset + 2] = (byte)min;
                pix[offset + 3] = hsl.A;
                return;
            case 4:
                pix[offset] = (byte)max;
                pix[offset + 1] = (byte)min;
                pix[offset + 2] = (byte)((hsl.H - 240) * width + min);
                pix[offset + 3] = hsl.A;
                return;
            default:
                pix[offset] = (byte)((360 - hsl.H) * width + min);
                pix[offset + 1] = (byte)min;
                pix[offset + 2] = (byte)max;
                pix[offset + 3] = hsl.A;
                return;
        }
    }

    public static Bitmap ChangeColor(Bitmap bmp, Config config)
    {
        if (bmp == null) return bmp;

        HSL hsl = new HSL();

        Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
        BitmapData bmpRetData = ret.LockBits(
            new Rectangle(0, 0, ret.Width, ret.Height)
            , ImageLockMode.ReadOnly
            , ret.PixelFormat
        );

        BitmapData bmpSrcData = bmp.LockBits(
            new Rectangle(0, 0, bmp.Width, bmp.Height)
            , ImageLockMode.ReadOnly
            , bmp.PixelFormat
        );
        byte[] pix = new byte[bmp.Width * bmp.Height * 4];
        Marshal.Copy(bmpSrcData.Scan0, pix, 0, pix.Length);
        bmp.UnlockBits(bmpSrcData);

        int hDiff;
        float sWidth = (config.After.SMax - config.After.SMin) / 100.0f;
        float lWidth = (config.After.LMax - config.After.LMin) / 100.0f;

        for (int py = 0; py < bmp.Height; ++py)
        {
            for (int px = 0, s = bmpRetData.Stride * py; px < bmp.Width; ++px, s += 4)
            {
                RGBtoHSL(ref pix[s], ref pix[s + 1], ref pix[s + 2], ref pix[s + 3], ref hsl);

                hDiff = config.Before.H - hsl.H;
                if (hDiff > 180) hDiff -= 360;
                else if (hDiff < -180) hDiff += 360;

                if (Math.Abs(hDiff) <= config.Before.HWidth
                && hsl.S >= config.Before.SMin && hsl.S <= config.Before.SMax
                && hsl.L >= config.Before.LMin && hsl.L <= config.Before.LMax)
                {
                    hsl.H = config.After.H;
                    hsl.S = (int)(hsl.S * sWidth) + config.After.SMin;
                    hsl.L = (int)(hsl.L * lWidth) + config.After.LMin;
                }

                HSLtoRGB(ref hsl, ref pix, ref s);
            }
        }

        Marshal.Copy(pix, 0, bmpRetData.Scan0, pix.Length);
        ret.UnlockBits(bmpRetData);
        return ret;
    }
}