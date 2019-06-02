using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionInterface;
using VisionUtil;

namespace VisionTaskManager
{
    /// <summary>
    /// 任务管理类，用于管理任务的初始化以及任务的增删改查
    /// </summary>
    public class VisionTaskManger
    {
        /// <summary>
        /// 存储所有任务
        /// </summary>
        private static Dictionary<string, VisionTask> Tasks { get; set; }

        #region files operator 
        private static VisionTaskInfoDoc doc;
        public static void LoadDoc()
        {
            doc = VisionTaskInfoDoc.LoadObj();
        }
        public static void SaveDoc()
        {
            doc.SaveDoc();
        }
        public static List<VisionTaskInfo> GetTaskInfoList()
        {
            if (doc != null)
            {
                return doc.TasksInfoList;
            }
            return new List<VisionTaskInfo>();
        }
        public static VisionTaskInfo GetTaskInfoInstance(string key)
        {
            if (doc != null)
            {
                if (doc.TasksInfoDic.ContainsKey(key))
                {
                    return doc.TasksInfoDic[key];
                }
            }
            return null;
        }
      
        public static bool AddTaskInfo(VisionTaskInfo info)
        {
            if (doc != null)
            {
                if (!doc.TasksInfoDic.ContainsKey(info.TaskName))
                {
                    doc.TasksInfoList.Add(info);
                    doc.TasksInfoDic.Add(info.TaskName, info);
                    if (AddTask(info))
                    {
                        return true;
                    }
                    else
                    {
                        doc.TasksInfoList.Remove(info);
                        doc.TasksInfoDic.Remove(info.TaskName);
                        return false;
                    }
                }
            }
            return false;
        }
        public static bool RemoveTaskInfo(string taskName)
        {
            if (doc != null)
            {
                if (doc.TasksInfoDic.ContainsKey(taskName))
                {
                    doc.TasksInfoList.Remove(doc.TasksInfoList.Find(p => p.TaskName == taskName));
                    doc.TasksInfoDic.Remove(taskName);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Task Operator
        public static bool AddTask(VisionTaskInfo info)
        {
            if (Tasks!=null)
            {
                if (!Tasks.ContainsKey(info.TaskName))
                {
                    VisionTask task = new VisionTask(info);
                    Tasks.Add(task.TaskName,task);
                }
            }
            return false;
        }
        public static bool RemoveTask(string taskName)
        {
            if (Tasks!=null)
            {
                if (Tasks.ContainsKey(taskName))
                {
                    Tasks.Remove(taskName);
                    return true;
                }
            }
            return false;
        }
        public static VisionTask GetVisionTask(string taskName)
        {
            if (Tasks != null)
            {
                if (Tasks.ContainsKey(taskName))
                {
                    return Tasks[taskName];
                }
            }
            return null;
        }
        public  static void InitAllTask()
        {
            if (doc!=null)
            {
                foreach (var item in doc.TasksInfoDic.Values)
                {
                    VisionTask task = new VisionTask(item);
                    Tasks.Add(task.TaskName,task);
                }
            }
        }
        public static bool StartTask(string taskName)
        {
            if (Tasks.ContainsKey(taskName))
            {
                Tasks[taskName].RunTask();
                return true;
            }
            return false;
        }
        public static bool StartLoopTask(string taskName)
        {
            if (Tasks.ContainsKey(taskName))
            {
                Tasks[taskName].RunTaskLoop();
                return true;
            }
            return false;
        }
        public static bool StopTask(string taskName)
        {
            if (Tasks.ContainsKey(taskName))
            {
                Tasks[taskName].StopTask();
                return true;
            }
            return false;
        }


        #endregion

        #region Task Tool 
        public static bool AddTaskTool(string taskName,ITool tool)
        {
            if (Tasks.ContainsKey(taskName))
            {
                if (Tasks[taskName].CheckToolUnique(tool.ToolName))
                {                                      
                    return Tasks[taskName].AddTool(tool);
                }
            }
            return false;
        }
        public static bool RemoveTaskTool(string taskName,string toolName)
        {
            if (Tasks.ContainsKey(taskName))
            {
                if (Tasks[taskName].CheckToolUnique(toolName))
                {
                    return Tasks[taskName].RemoverTool(toolName);
                }
            }
            return false;
        }
        public static bool InsertTaskTool(string taskName, int index,ITool tool)
        {
            if (Tasks.ContainsKey(taskName))
            {
                if (Tasks[taskName].CheckToolUnique(tool.ToolName))
                {
                    return Tasks[taskName].InsertTool(index,tool);
                }
            }
            return false;
        }
        #endregion
    }
}
