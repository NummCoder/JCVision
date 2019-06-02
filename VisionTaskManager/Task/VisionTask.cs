using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;

namespace VisionTaskManager
{
   public class VisionTask
    {
        public string TaskName { get; set; }

        public VisionTaskInfo Info { get; set; }

        public DisplayControl Window { get; set; }

        public delegate void UpdataTaskResult();
        private event UpdataTaskResult updateRunTaskResult;
        /// <summary>
        /// 需要在取图工具中更新
        /// </summary>
        public HImage InputImage { get; set; }
        /// <summary>
        /// 需要在二值化工具或者斑点工具中更新
        /// </summary>
        public HRegion InputRegion { get; set; }
        private List<ITool> ToolList { get; set; }
        /// <summary>
        /// 主要用于检查工具名称的唯一性
        /// </summary>
        private Dictionary<string,ITool> ToolsDic { get; set; }
        /// <summary>
        /// 在工具执行的过程中，一直获取最新的工具结果。
        /// </summary>
        public ToolResult result { get; set; }
        /// <summary>
        /// 用于存储每一个工具执行时的结果
        /// </summary>
        public Dictionary<string,ToolResult> ToolResultDic { get; set; }
        public bool bContinueRunTask { get; set; }
        public VisionTask()
        {
            ToolsDic = new Dictionary<string, ITool>();
            result = new ToolResult();
            ToolList = new List<ITool>();
            ToolResultDic = new Dictionary<string, ToolResult>();
            InputRegion = new HRegion();
            InputRegion.GenEmptyRegion();
            InputImage = new HImage();
            InputImage.GenEmptyObj();
        }
        public VisionTask(VisionTaskInfo info)
        {
            #region MyRegion
            result = new ToolResult();
            ToolsDic = new Dictionary<string, ITool>();
            ToolList = new List<ITool>();
            ToolResultDic = new Dictionary<string, ToolResult>();
            InputRegion = new HRegion();
            InputRegion.GenEmptyRegion();
            InputImage = new HImage();
            InputImage.GenEmptyObj(); 
            #endregion

            this.Info = info;
            TaskName = info.TaskName;
            //显示界面布局   
            foreach (var item in Info.TaskToolsInfo)
            {
                object[] paras = new object[] { item};
                ITool tool = CreatingHelper<ITool>.CreateInstance(@".//VisionTools/"+item.GetToolNameSpace()+".dll",item.GetToolNameSpace(),item.GetToolClassName(),paras);
                if (tool!=null)
                {                  
                    ToolsDic.Add(tool.TaskName,tool);
                    ToolList.Add(tool);
                }
            }
        }

        /// <summary>
        /// 单次执行任务
        /// </summary>
        public void RunTask()
        {
            if (ToolsDic.Count!=0)
            {
                foreach (var item in ToolsDic.Values)
                {
                    ///如果是使用Region来处理的工具，比如斑点工具，则需要设置Region输入
                    if (item is IRegion)
                    {
                        IRegion tool = (IRegion)item;
                        tool.SetRegion(InputRegion);
                    }
                    //如果是通讯接口，则发送结果数据
                    if (item is ICommunicate)
                    {
                        //发送结果数据
                        continue;
                    }
                    ///如果是PLC接口，则根据绑定的地址进行赋值。
                    item.SetImage(InputImage);
                    item.SetWindow(Window);
                    item.GetResult();
                }
            }
        }
        /// <summary>
        /// 连续执行任务
        /// </summary>
        public void RunTaskLoop()
        {
            bContinueRunTask = true;
            Task.Factory.StartNew(()=>           
            {
                while (bContinueRunTask)
                {
                    RunTask();
                    //添加事件，更新界面
                    if (updateRunTaskResult!=null)
                    {
                        updateRunTaskResult();
                    }
                    System.Threading.Thread.Sleep(10);
                }
            });
        }
        public void RegisteRunTaskEvent(UpdataTaskResult func)
        {
            if (func!=null)
            {
                updateRunTaskResult += func;
            }
        }
        public void StopTask()
        {
            if (bContinueRunTask)
            {
                bContinueRunTask = false;
            }
        }
        public bool AddTool(ITool tool)
        {
            if (ToolsDic!=null)
            {
                if (!ToolsDic.ContainsKey(tool.ToolName))
                {
                    ToolsDic.Add(tool.ToolName,tool);
                    ToolList.Add(tool);
                    return true;
                }
            }
            return false;
        }

        public bool RemoverTool(string ToolName)
        {
            if (ToolsDic != null)
            {
                if (ToolsDic.ContainsKey(ToolName))
                {
                    ToolsDic.Remove(ToolName);
                    ToolList.Remove(ToolList.Find(p=>p.ToolName==ToolName));
                    return true;
                }
            }
            return false;
        }
        public bool InsertTool(int index,ITool tool)
        {
            if (ToolsDic!=null)
            {
                if (!ToolsDic.ContainsKey(tool.ToolName))
                {
                    ToolsDic.Add(tool.ToolName,tool);
                    ToolList.Insert(index, tool);
                    return true;
                }
            }
            return false;
        }
        //检查工具名称唯一性
        public bool CheckToolUnique(string toolName)
        {
            if (ToolsDic != null)
            {
                if (!ToolsDic.ContainsKey(toolName))
                {                  
                    return true;
                }
            }
            return false;
        }
        public void SetWindow(DisplayControl window)
        {
            this.Window = window;
        }
    }
}
