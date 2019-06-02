using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisionUtil.NLog
{
    public class LogFileDoc
    {
        public List<LogFileItem> logFileList;
        public LogFileDoc()
        {
            logFileList = new List<LogFileItem>();
        }
        public static LogFileDoc LoadDocument()
        {
            LogFileDoc m_Doc;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogFileDoc));
            FileStream fsReader = null;
            try
            {
                fsReader = File.OpenRead(@".//Parameter/LogFileSettingDoc.xml");
                m_Doc = (LogFileDoc)xmlSerializer.Deserialize(fsReader);
                foreach (LogFileItem item in m_Doc.logFileList)
                {
                    LogFileManager.LogFileDic.Add(item.fileName, item);
                }
                fsReader.Close();
            }
            catch
            {
                if (fsReader != null)
                    fsReader.Close();
                m_Doc = new LogFileDoc();
            }
            return m_Doc;
        }

        public bool SaveDocument()
        {
            if (!Directory.Exists(@".//Parameter/"))
            {
                Directory.CreateDirectory(@".//Parameter/");
            }
            FileStream fsWriter = new FileStream(@".//Parameter/LogFileSettingDoc.xml", FileMode.Create, FileAccess.Write, FileShare.Read);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LogFileDoc));
            xmlSerializer.Serialize(fsWriter, this);
            fsWriter.Close();
            return true;
        }
    }
}
