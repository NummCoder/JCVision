using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionInterface
{
    //抽象出所有使用Region处理的工具
   public interface IRegion
    {
        void SetRegion(HRegion region);
    }
}
