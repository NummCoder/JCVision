using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionInterface
{
   public interface IToolInfo
    {
        string ToolName { get; set; }
        string TaskName { get; set; }
        IToolInfo CopyInfo();
        /// <summary>
        /// 获取工具的中文描述
        /// </summary>
        /// <returns></returns>
        string GetToolType();
        /// <summary>
        /// 获取工具的命名空间
        /// </summary>
        /// <returns></returns>
        string GetToolNameSpace();
        /// <summary>
        /// 获取工具类名
        /// </summary>
        /// <returns></returns>
        string GetToolClassName();
    }
}
