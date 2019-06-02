using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionInterface
{
   public interface IAffine
    {

        HXLDCont TransContXLD(HXLDCont xldCout);
        HImage TransImage(HImage image);
        bool TransPoint(double row, double col, out double outRow, out double outCol);
        HXLDPoly TransPolygonXLD(HXLDPoly xldPoly);
        HRegion TransRegion(HRegion region);
        double GetRefAngle();
        double GetRefRowCoordinate();
        double GetRefColCoodinate();
    }
}
