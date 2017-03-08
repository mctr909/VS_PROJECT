using System;
using System.Drawing;

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

    public static void RGBtoHSL(Color rgb, ref HSL hsl)
    {
        byte max = Math.Max(rgb.R, Math.Max(rgb.G, rgb.B));
        byte min = Math.Min(rgb.R, Math.Min(rgb.G, rgb.B));

        #region Hue
        if (rgb.R == rgb.G && rgb.G == rgb.B)
        {
            hsl.H = 0;
        }
        else if (rgb.R == max)
        {
            hsl.H = 60 * (rgb.G - rgb.B) / (max - min);
        }
        else if (rgb.G == max)
        {
            hsl.H = 60 * (rgb.B - rgb.R) / (max - min) + 120;
        }
        else if (rgb.B == max)
        {
            hsl.H = 60 * (rgb.R - rgb.G) / (max - min) + 240;
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

        hsl.A = rgb.A;
    }

    public static void HSLtoRGB(ref HSL hsl, ref Color rgb)
    {
        float max, min, width;

        if (hsl.L == 100)
        {
            rgb = Color.FromArgb(hsl.A, 255, 255, 255);
            return;
        }
        else if (hsl.L == 0)
        {
            rgb = Color.FromArgb(hsl.A, 0, 0, 0);
            return;
        }

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
                rgb = Color.FromArgb(hsl.A, (int)max, (int)(hsl.H * width + min), (int)min);
                return;
            case 1:
                rgb = Color.FromArgb(hsl.A, (int)((120 - hsl.H) * width + min), (int)max, (int)min);
                return;
            case 2:
                rgb = Color.FromArgb(hsl.A, (int)min, (int)max, (int)((hsl.H - 120) * width + min));
                return;
            case 3:
                rgb = Color.FromArgb(hsl.A, (int)min, (int)((240 - hsl.H) * width + min), (int)max);
                return;
            case 4:
                rgb = Color.FromArgb(hsl.A, (int)((hsl.H - 240) * width + min), (int)min, (int)max);
                return;
            default:
                rgb = Color.FromArgb(hsl.A, (int)max, (int)min, (int)((360 - hsl.H) * width + min));
                return;
        }
    }

    public static Bitmap ChangeColor(Bitmap bmp, Config config)
    {
        Bitmap ret = new Bitmap(bmp.Width, bmp.Height);
        HSL hsl = new HSL();
        Color rgb = Color.FromArgb(0, 0, 0, 0);
        
        int hDiff;
        float sWidth = (config.After.SMax - config.After.SMin) / 100.0f;
        float lWidth = (config.After.LMax - config.After.LMin) / 100.0f;

        for (int py = 0; py < bmp.Height; ++py)
        {
            for (int px = 0; px < bmp.Width; ++px)
            {
                rgb = bmp.GetPixel(px, py);
                RGBtoHSL(rgb, ref hsl);

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

                HSLtoRGB(ref hsl, ref rgb);
                ret.SetPixel(px, py, rgb);
            }
        }

        return ret;
    }
}