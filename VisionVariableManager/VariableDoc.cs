using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisionVariableManager
{
    /// <summary>
    /// 字典主要用于判断名称唯一性，因为字典的查询速度比顺序表要更快，计算与赋值主要使用顺序表。
    /// </summary>
   public class VariableDoc
    {
        public List<VariableInfoBase> GlobalVariablesInfoList;
        public List<VariableInfoBase> TaskVariablesInfoList;
        public List<VariableInfoBase> SystemVariableInfoList;
        [XmlIgnore]
        public Dictionary<string, VariableInfoBase> GlobaleVariablesInfoDic;
        //[XmlIgnore]
        //public Dictionary<string, VariableInfoBase> TaskVariablesInfoDic;
        [XmlIgnore]
        public Dictionary<string, VariableInfoBase> SystemVariablesInfoDic;
        public VariableDoc()
        {
            GlobalVariablesInfoList = new List<VariableInfoBase>();
            GlobaleVariablesInfoDic = new Dictionary<string, VariableInfoBase>();
            TaskVariablesInfoList = new List<VariableInfoBase>();
           // TaskVariablesInfoDic = new Dictionary<string, VariableInfoBase>();
            SystemVariableInfoList = new List<VariableInfoBase>();
            SystemVariablesInfoDic = new Dictionary<string, VariableInfoBase>();
        }
        public static VariableDoc LoadObj()
        {
            VariableDoc pDoc = new VariableDoc();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(VariableDoc));
                FileStream fsReader = null;
                fsReader = File.OpenRead(@".//Parameter/VisionVariableDoc.xml");
                pDoc = (VariableDoc)xmlSerializer.Deserialize(fsReader);
                fsReader.Close();
                pDoc.GlobaleVariablesInfoDic = pDoc.GlobalVariablesInfoList.ToDictionary(p => p.VariableName);
                pDoc.SystemVariablesInfoDic = pDoc.SystemVariableInfoList.ToDictionary(p => p.VariableName);
                //pDoc.TaskVariablesInfoDic = pDoc.TaskVariablesInfoList.ToDictionary(p=>p.VariableName);
            }
            catch
            {
                pDoc.GlobalVariablesInfoList.Clear();

            }

            return pDoc;
        }

        public bool SaveDoc()
        {
            if (!Directory.Exists(@".//Parameter/"))
            {
                Directory.CreateDirectory(@".//Parameter/");
            }
            FileStream fsWriter1 = new FileStream(@".//Parameter/VisionVariableDoc.xml", FileMode.Create, FileAccess.Write, FileShare.Read);
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(VariableDoc));
            xmlSerializer1.Serialize(fsWriter1, this);
            fsWriter1.Close();
            return true;
        }
    }
}
