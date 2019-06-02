using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionInterface
{
   public interface IResult
    {
        string ResultName { get; set; }
        bool IsSuccess { get; set; }
        string Errormessage { get; set; }
        string ErrorCode { get; set; }
        IResult CopyResult();
    }
}
