using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisionTaskManager
{
   public class VisionTaskInfoDoc
    {
        public List<VisionTaskInfo> TasksInfoList;
        [XmlIgnore]
        public Dictionary<string, VisionTaskInfo> TasksInfoDic;
        public VisionTaskInfoDoc()
        {
            TasksInfoList = new List<VisionTaskInfo>();
            TasksInfoDic = new Dictionary<string, VisionTaskInfo>();
        }
        public static VisionTaskInfoDoc LoadObj()
        {
            VisionTaskInfoDoc pDoc = new VisionTaskInfoDoc();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VisionTaskInfoDoc));
                FileStream fsReader = null;
                fsReader = File.OpenRead(@".//Parameter/VisionTaskInfoDoc.xml");
                pDoc = (VisionTaskInfoDoc)xmlSerializer.Deserialize(fsReader);
                fsReader.Close();
                pDoc.TasksInfoDic = pDoc.TasksInfoList.ToDictionary(p => p.TaskName);
                foreach (var item in pDoc.TasksInfoList)
                {
                   
                    item.TaskToolInfoDic = item.TaskToolsInfo.ToDictionary(p=>p.ToolName);
                }
            }
            catch
            {
                pDoc.TasksInfoList.Clear();

            }

            return pDoc;
        }

        public bool SaveDoc()
        {
            if (!Directory.Exists(@".//Parameter/"))
            {
                Directory.CreateDirectory(@".//Parameter/");
            }
            FileStream fsWriter1 = new FileStream(@".//Parameter/VisionTaskInfoDoc.xml", FileMode.Create, FileAccess.Write, FileShare.Read);
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(VisionTaskInfoDoc));
            xmlSerializer1.Serialize(fsWriter1, this);
            fsWriter1.Close();
            return true;
        }
    }
}
