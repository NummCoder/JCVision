using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VisionInterface;

namespace VisionTaskManager
{
    public class VisionTaskInfo
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public string TaskRunFormName { get; set; }
        public List<IToolInfo> TaskToolsInfo { get; set; }

        public bool IsSaveNGImage { get; set; }
        public bool IsSaveOKImage { get; set; }
        public string TaskRegistImagePath { get; set; }      
        [XmlIgnore]
        public Dictionary<string,IToolInfo> TaskToolInfoDic { get; set; }
        public VisionTaskInfo()
        {
            TaskToolsInfo = new List<IToolInfo>();
           
            TaskToolInfoDic = new Dictionary<string, IToolInfo>();
        }
        public bool AddToolInfo(IToolInfo info)
        {
            if (!TaskToolInfoDic.ContainsKey(info.ToolName))
            {
                TaskToolInfoDic.Add(info.ToolName,info);
                TaskToolsInfo.Add(info);
                return true;
            }
            return false;
        }
        public IToolInfo GetToolInfo(int index)
        {
            if (TaskToolsInfo.Count>=index)
            {
                return TaskToolsInfo[index];
            }
            return null;
        }
        public IToolInfo GetToolInfo(string toolName)
        {
            if (TaskToolInfoDic.ContainsKey(toolName))
            {
                return TaskToolsInfo.Find(p=>p.TaskName==toolName);
            }
            return null;
        }
        public bool RemoveToolInfo(string toolName)
        {
            if (TaskToolInfoDic.ContainsKey(toolName))
            {
                TaskToolInfoDic.Remove(toolName);
                TaskToolsInfo.RemoveAll(p=>p.ToolName==toolName);
                return true;
            }
            return false;
        }
        public bool InsertToolInfo(int index, IToolInfo info)
        {
            if (!TaskToolInfoDic.ContainsKey(info.ToolName))
            {
                TaskToolInfoDic.Add(info.ToolName, info);
                TaskToolsInfo.Insert(index,info);
                return true;
            }
            return false;
        }
       
        public bool CheckToolUnique(string toolName)
        {
            if (TaskToolInfoDic.ContainsKey(toolName))
            {
                return false;
            }
            return true; 
        }
    }
}
