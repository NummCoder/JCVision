using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.NLog;

namespace VisionInterface
{
   public class LogBaseClass
    {
        protected virtual void WriteInfoLog(string fileName,string infoMessage)
        {
            LogFileManager.Info(fileName, infoMessage);
        }

        protected virtual void WriteErrorLog(string fileName, string errorMessage)
        {
            LogFileManager.Error(fileName, errorMessage);
        }

        protected virtual void WriteFatalLog(string fileName, string fatalMessage)
        {
            LogFileManager.Fatal(fileName, fatalMessage);
        }
        protected virtual void WriteDebugLog(string fileName, string debugMessage)
        {
            LogFileManager.Debug(fileName, debugMessage);
        }
    }
}
