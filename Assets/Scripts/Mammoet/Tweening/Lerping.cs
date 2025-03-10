using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweening
{
    /**
     * Lerping functions used by other Tweening classes
     * */

    public static class Lerping
    {
        public static float Lerp(float from, float to, float t)
        {
            return from + ((to - from) * t);
        }

        public static double Lerp(double from, double to, double t)
        {
            return from + ((to - from) * t);
        }

        public static double Lerp(byte from, byte to, double t)
        {
            return from + ((to - from) * t);
        }

        public static int Lerp(int from, int to, double t)
        {
            return (int)(from + ((to - from) * t));
        }
    }
}
