using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemBorder
{
    public class HSLColor
    {
        public float Hue;
        public float Saturation;
        public float Luminosity;

        public HSLColor(float H, float S, float L)
        {
            Hue = H;
            Saturation = S;
            Luminosity = L;
        }

        public static HSLColor FromRGB(Color Clr)
        {
            return FromRGB(Clr.R, Clr.G, Clr.B);
        }

        public static HSLColor FromRGB(Byte R, Byte G, Byte B)
        {
            float _R = (R / 255f);
            float _G = (G / 255f);
            float _B = (B / 255f);

            float _Min = Math.Min(Math.Min(_R, _G), _B);
            float _Max = Math.Max(Math.Max(_R, _G), _B);
            float _Delta = _Max - _Min;

            float H = 0;
            float S = 0;
            float L = (float)((_Max + _Min) / 2.0f);

            if (_Delta != 0)
            {
                if (L < 0.5f)
                {
                    S = (float)(_Delta / (_Max + _Min));
                }
                else
                {
                    S = (float)(_Delta / (2.0f - _Max - _Min));
                }

                float _Delta_R = (float)(((_Max - _R) / 6.0f + (_Delta / 2.0f)) / _Delta);
                float _Delta_G = (float)(((_Max - _G) / 6.0f + (_Delta / 2.0f)) / _Delta);
                float _Delta_B = (float)(((_Max - _B) / 6.0f + (_Delta / 2.0f)) / _Delta);

                if (_R == _Max)
                {
                    H = _Delta_B - _Delta_G;
                }
                else if (_G == _Max)
                {
                    H = (1.0f / 3.0f) + _Delta_R - _Delta_B;
                }
                else if (_B == _Max)
                {
                    H = (2.0f / 3.0f) + _Delta_G - _Delta_R;
                }

                if (H < 0) H += 1.0f;
                if (H > 1) H -= 1.0f;
            }

            return new HSLColor(H, S, L);
        }

        private float Hue_2_RGB(float v1, float v2, float vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if ((6 * vH) < 1) return (v1 + (v2 - v1) * 6 * vH);
            if ((2 * vH) < 1) return (v2);
            if ((3 * vH) < 2) return (v1 + (v2 - v1) * ((2 / 3) - vH) * 6);
            return (v1);
        }

        public Color ToRGB()
        {
            Color Clr = new Color();
            float var_1, var_2;

            if (Saturation == 0)
            {
                Clr.R = (Byte)(Luminosity * 255f);
                Clr.G = (Byte)(Luminosity * 255f);
                Clr.B = (Byte)(Luminosity * 255f);
            }
            else
            {
                if (Luminosity < 0.5) var_2 = Luminosity * (1 + Saturation);
                else var_2 = (Luminosity + Saturation) - (Saturation * Luminosity);

                var_1 = 2 * Luminosity - var_2;

                Clr.R = (Byte)(255f * Hue_2_RGB(var_1, var_2, Hue + (1 / 3)));
                Clr.G = (Byte)(255f * Hue_2_RGB(var_1, var_2, Hue));
                Clr.B = (Byte)(255f * Hue_2_RGB(var_1, var_2, Hue - (1 / 3)));
            }

            return Clr;
        }
    }
}
