using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionDisplayTool;
using VisionInterface;
using VisionUtil;

namespace VisionBlobTool
{
    public class BlobTool : LogBaseClass, ITool,IRegion
    {
        public string ToolName { get ; set ; }
        public string TaskName { get; set; }
        private BlobToolInfo info;
        public IToolInfo Info
        {
            get { return info; }
            set
            {
                if (value.GetType() == typeof(BlobToolInfo))
                {
                    info = (BlobToolInfo)value;
                }
            }
        }
        public HImage Image { get ; set ; }
        public DisplayControl Window { get ; set ; }
        private HRegion region { get; set; }
        public BlobTool()
        {

        }
        public BlobTool(IToolInfo info, DisplayControl window)
        {
            this.ToolName = info.ToolName;
            this.Info = info;
            this.Window = window;
        }
        public BlobTool(IToolInfo info, HImage image, DisplayControl window)
        {
            this.ToolName = info.ToolName;
            this.Info = info;
            this.Window = window;
            this.Image = image;
        }
        /// <summary>
        /// 该接口用于框架
        /// </summary>
        /// <returns></returns>
        public ToolResult GetResult()
        {
            HiPerfTimer timer = new HiPerfTimer();
            timer.Start();
            try
            {
                HRegion hRegion = new HRegion();
                hRegion.GenEmptyRegion();
                if (region!=null)
                {
                    hRegion = region.Connection();
                    if (info.IsUseArea)
                    {
                        hRegion = hRegion.SelectShape("area", "and", info.MinArea, info.MaxArea);
                    }
                    if (info.IsUseWidth)
                    {
                        hRegion = hRegion.SelectShape("width", "and", info.MinWidth, info.MaxWidth);
                    }
                    if (info.IsUseHeight)
                    {
                        hRegion = hRegion.SelectShape("height", "and", info.MinHeight, info.MaxHeight);
                    }
                    if (info.IsUseRetanglarity)
                    {
                        hRegion = hRegion.SelectShape("rectangularity", "and", info.MinRetanglarity, info.MaxRetanglarity);
                    }
                    if (info.IsUseCircularity)
                    {
                        hRegion = hRegion.SelectShape("circularity", "and", info.MinCircularity, info.MaxCircularity);
                    }
                    if (region!=null)
                    {
                        ToolResult result = new ToolResult();
                        result.ResultName = ToolName;
                        result.Region = region;
                        int area= region.AreaCenter(out double row,out double col);
                        result.ImageX = col;
                        result.ImageY = row;
                        HTuple tuple= region.OrientationRegion();
                        if (tuple.Length>0)
                        {
                            result.ImageAngle = tuple[0].D;
                        }
                        result.ElapsedTime = timer.Duration;
                        result.GetResultTime = DateTime.Now;
                        return result;
                    }
                    else
                    {
                        return new ToolResult() { ResultName = ToolName, Errormessage = "没有找到合适的区域", GetResultTime = DateTime.Now };
                    }
                }
                return new ToolResult() { ResultName=ToolName,Errormessage="需要筛选的区域为空区域！",GetResultTime=DateTime.Now};
            }
            catch (Exception ex)
            {
                WriteErrorLog("VisionTool",$"{this.ToolName} has occured some mistakes with {ex.ToString()}");
                return new ToolResult() {ResultName=ToolName,Errormessage=ex.ToString(),GetResultTime=DateTime.Now };
            }

        }
        /// <summary>
        /// 该接口用于独立嵌入其他程序使用
        /// </summary>
        /// <param name="image"></param>
        public ToolResult RunTool()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return null;
        }
        public void SetImage(HImage image)
        {
            this.Image = image;
        }

        public void SetRegion(HRegion region)
        {
            this.region = region;
        }

        public void SetWindow(DisplayControl window)
        {
            this.Window=window;
        }
    }
}
