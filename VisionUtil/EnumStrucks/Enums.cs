using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtil.EnumStrucks
{
    /// <summary>
    /// 用于模板匹配
    /// </summary>
    public enum Optimization
    {
        auto,
        none,
        no_pregeneration,
        point_reduction_high,
        point_reduction_low,
        point_reduction_medium,
        pregeneration,
        sort
    }
    /// <summary>
    /// 用于模板匹配
    /// </summary>
    public enum Metric
    {
        use_polarity,
        ignore_color_polarity,
        ignore_global_polarity,
        ignore_local_polarity
    }
    /// <summary>
    /// 用于画ROI
    /// </summary>
    public enum ROI
    {
        Line,
        Circle,
        Ellipse,
        Rectangle1,
        Rectangle2
    }
    /// <summary>
    /// 用于灰度匹配
    /// </summary>
    public enum GrayValues
    {
        original,
        gradient,
        normalized,
        sobel
    }
    /// <summary>
    /// 用于找园找线
    /// </summary>
    public enum Translation
    {
        all,
        positive,
        negative
    }
    public enum CameraType
    {
        Basler,
        HiK
    }
    public enum VariableType
    {
        Double,
        String,
        Image,
        Region,
        XLD
    }
    public enum VariableRange
    {
        全局变量 ,
        任务变量,
        系统变量
    }
    public enum ImageFormat
    {
        RGB8Planar,
        YUV411Packed,
        RGB10V2Packed,
        RGB10V1Packed,
        BGR12Packed,
        RGB12Packed,
        BGR10Packed,
        RGB10Packed,
        BGRA8Packed,
        RGBA8Packed,
        BGR8Packed,
        RGB8Packed,
        BayerBG12,
        BayerGB12,
        BayerRG12,
        BayerGR12,
        BayerBG10,
        BayerGB10,
        BayerRG10,
        BayerGR10,
        BayerBG8,
        BayerGB8,
        BayerRG8,
        BayerGR8,
        Mono16,
        Mono12Packed,
        Mono12,
        Mono10Packed,
        YUV422Packed,
        YUV444Packed,
        Mono8,
        RGB10Planar,
        YCbCr422_8,
        BGR8,
        RGB8,
        BayerBG12p,
        BayerGB12p,
        BayerRG12p,
        BayerGR12p,
        Mono12p,
        BayerBG10p,
        BayerGB10p,
        BayerRG10p,
        BayerGR10p,
        Mono10p,
        Mono8Signed,
        RGB12V1Packed,
        BayerBG16,
        BayerGB16,
        BayerRG16,
        BayerGR16,
        BayerBG12Packed,
        BayerRG12Packed,
        BayerGR12Packed,
        BayerGB12Packed,
        YUV422_YUYV_Packed,
        RGB16Planar,
        RGB12Planar,
        Mono10
    }
}
