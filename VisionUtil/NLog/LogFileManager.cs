using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace VisionUtil.NLog
{
   public static class LogFileManager
    {
        public static LogFileDoc pDoc = new LogFileDoc();
        public static Dictionary<string, LogFileItem> LogFileDic = new Dictionary<string, LogFileItem>();
        public static Dictionary<string, Logger> loggerDic = new Dictionary<string, Logger>();
        public static void InitLogFiles()
        {
            pDoc = LogFileDoc.LoadDocument();
            foreach (LogFileItem item in pDoc.logFileList)
            {
                loggerDic.Add(item.fileName, LogManager.GetLogger(item.fileName));
            }
        }

        public static void Info(string name, object value)
        {
            try
            {
                if (LogFileDic[name].bUsing)
                {
                    loggerDic[name].Info(value);
                }
            }
            catch
            {

            }
        }

        public static void Warn(string name, object value)
        {
            try
            {
                if (LogFileDic[name].bUsing)
                {
                    loggerDic[name].Warn(value);
                }
            }
            catch
            {

            }
        }

        public static void Error(string name, object value)
        {
            try
            {
                if (LogFileDic[name].bUsing)
                {
                    loggerDic[name].Error(value);
                }
            }
            catch
            {

            }
        }

        public static void Debug(string name, object value)
        {
            try
            {
                if (LogFileDic[name].bUsing)
                {
                    loggerDic[name].Debug(value);
                }
            }
            catch
            {

            }
        }

        public static void Fatal(string name, object value)
        {
            try
            {
                if (LogFileDic[name].bUsing)
                {
                    loggerDic[name].Fatal(value);
                }
            }
            catch
            {

            }
        }
    }
}
