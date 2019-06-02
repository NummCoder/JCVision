using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtil.NLog
{
   public class LogFileItem
    {
        public bool bUsing;
        public string fileName { get; set; }
        public string filePath { get; set; }
        public LogFileItem()
        {
            fileName = "";
            filePath = "";
            bUsing = true;
        }
    }
}
