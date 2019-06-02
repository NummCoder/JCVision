using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtil
{
   public static class TransAngle
    {
        public static double AngleToHu(double dAngle)
        {
            return ((dAngle / 180.0) * 3.1415926535897931);
        }
        public static double HuToAngle(double dHu)
        {
            return (dHu / 3.1415926535897931) * 180.0;
        }
    }
}
