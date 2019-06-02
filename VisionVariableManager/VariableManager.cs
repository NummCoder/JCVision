using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.NLog;

namespace VisionVariableManager
{
   public class VariableManager
    {
        #region Properties
        public static VariableDoc doc { get; set; }
        #endregion

        public static void Init()
        {
            doc = VariableDoc.LoadObj();
        }
        public static void SaveDoc()
        {
            if (doc!=null)
            {
                doc.SaveDoc();
            }
        }

        #region About TaskVariables
        public static bool AddTaskVarInfo(VariableInfoBase variable)
        {
            if (doc!=null)
            {
                doc.TaskVariablesInfoList.Add(variable);
                return true;
            }
            return false;
        }
        public static VariableInfoBase GetTaskVarInfo(string taskName,string varialbleName)
        {
            if (doc!=null)
            {
                return doc.TaskVariablesInfoList.Find(p => p.TaskName == taskName && p.VariableName == varialbleName);
            }
            return null;
        }
        public static bool RemoveTaskVarInfo(string taskName,string varName)
        {
            if (doc!=null)
            {
                var info = doc.TaskVariablesInfoList.Find(p=>p.TaskName==taskName&&p.VariableName==varName);
                if (info!=null)
                {
                    doc.TaskVariablesInfoList.Remove(info);
                }
            }
            return false;
        }
        public static bool CheckTaskVarUnique(string taskName,string varName)
        {
            if (doc!=null)
            {
                var info = doc.TaskVariablesInfoList.Find(p => p.TaskName == taskName && p.VariableName == varName);
                if (info!=null)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool TrySetTaskVarValue(string taskName, string varialbleName,object value)
        {
            try
            {
                if (doc != null)
                {
                    var variable = doc.TaskVariablesInfoList.Find(p => p.TaskName == taskName && p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    dinfo.Value = Convert.ToDouble(value);                                   
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    sinfo.Value = Convert.ToString(value);
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    iinfo.Value = (HImage)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    rinfo.Value = (HRegion)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    xinfo.Value = (HXLDCont)value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog",ex.ToString());
            }
            return false;
        }
        public static object TryGetTaskVarValue(string taskName, string varialbleName)
        {
            object value = new object();
            try
            {
                if (doc != null)
                {
                    var variable = doc.TaskVariablesInfoList.Find(p => p.TaskName == taskName && p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    value = dinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    value = sinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    value = iinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    value = rinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    value = xinfo.Value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog", ex.ToString());
            }
            return value;
        }
        #endregion

        #region About SystemVariables
        public static bool AddSystemVarInfo(VariableInfoBase variable)
        {
            if (doc != null)
            {
                if (!doc.SystemVariablesInfoDic.ContainsKey(variable.VariableName))
                {
                    doc.TaskVariablesInfoList.Add(variable);
                    doc.SystemVariablesInfoDic.Add(variable.VariableName,variable);
                    return true;
                }
            }
            return false;
        }
        public static VariableInfoBase GetSystemVarInfo(string varialbleName)
        {
            if (doc != null)
            {
                if (doc.SystemVariablesInfoDic.ContainsKey(varialbleName))
                {
                    //主要是用顺序表来计算，此处不返回字典里的对象。
                    return doc.SystemVariableInfoList.Find(p=>p.VariableName==varialbleName);
                }
            }
            return null;
        }
        public static bool RemoveSystemVarInfo(string varName)
        {
            if (doc != null)
            {
                if (doc.SystemVariablesInfoDic.ContainsKey(varName))
                {
                    doc.SystemVariablesInfoDic.Remove(varName);
                    doc.SystemVariableInfoList.Remove(doc.SystemVariableInfoList.Find(p=>p.VariableName==varName));
                }
            }
            return false;
        }
        public static bool CheckSystemVarUnique(string varName)
        {
            if (doc != null)
            {
                if (doc.SystemVariablesInfoDic.ContainsKey(varName))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool TrySetSystemVarValue(string varialbleName, object value)
        {
            try
            {
                if (doc != null)
                {
                    if (!doc.SystemVariablesInfoDic.ContainsKey(varialbleName))
                    {
                        return false;
                    }
                    var variable = doc.SystemVariableInfoList.Find(p=> p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    dinfo.Value = Convert.ToDouble(value);
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    sinfo.Value = Convert.ToString(value);
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    iinfo.Value = (HImage)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    rinfo.Value = (HRegion)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    xinfo.Value = (HXLDCont)value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog", ex.ToString());
            }
            return false;
        }
        public static object TryGetSystemVarValue(string varialbleName)
        {
            object value = new object();
            try
            {
                if (doc != null)
                {
                    var variable = doc.SystemVariableInfoList.Find(p => p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    value = dinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    value = sinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    value = iinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    value = rinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    value = xinfo.Value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog", ex.ToString());
            }
            return value;
        }
        #endregion

        #region About GlobalVariables
        public static bool AddGlobalVarInfo(VariableInfoBase variable)
        {
            if (doc != null)
            {
                if (!doc.GlobaleVariablesInfoDic.ContainsKey(variable.VariableName))
                {
                    doc.GlobalVariablesInfoList.Add(variable);
                    doc.GlobaleVariablesInfoDic.Add(variable.VariableName, variable);
                    return true;
                }
            }
            return false;
        }
        public static VariableInfoBase GetGlobalVarInfo(string varialbleName)
        {
            if (doc != null)
            {
                if (doc.GlobaleVariablesInfoDic.ContainsKey(varialbleName))
                {
                    //主要是用顺序表来计算，此处不返回字典里的对象。
                    return doc.GlobalVariablesInfoList.Find(p => p.VariableName == varialbleName);
                }
            }
            return null;
        }
        public static bool RemoveGlobalVarInfo(string varName)
        {
            if (doc != null)
            {
                if (doc.GlobaleVariablesInfoDic.ContainsKey(varName))
                {
                    doc.GlobaleVariablesInfoDic.Remove(varName);
                    doc.GlobalVariablesInfoList.Remove(doc.SystemVariableInfoList.Find(p => p.VariableName == varName));
                }
            }
            return false;
        }
        public static bool CheckGlobalVarUnique(string varName)
        {
            if (doc != null)
            {
                if (doc.GlobaleVariablesInfoDic.ContainsKey(varName))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool TrySetGlobalVarValue(string varialbleName, object value)
        {
            try
            {
                if (doc != null)
                {
                    if (!doc.GlobaleVariablesInfoDic.ContainsKey(varialbleName))
                    {
                        return false;
                    }
                    var variable = doc.GlobalVariablesInfoList.Find(p => p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    dinfo.Value = Convert.ToDouble(value);
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    sinfo.Value = Convert.ToString(value);
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    iinfo.Value = (HImage)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    rinfo.Value = (HRegion)value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    xinfo.Value = (HXLDCont)value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog", ex.ToString());
            }
            return false;
        }
        public static object TryGetGlobalVarValue(string varialbleName)
        {
            object value = new object();
            try
            {
                if (doc != null)
                {
                    var variable = doc.GlobalVariablesInfoList.Find(p => p.VariableName == varialbleName);
                    if (variable != null)
                    {
                        switch (variable.variableType)
                        {
                            case VisionUtil.EnumStrucks.VariableType.Double:
                                VariableInfo<double> dinfo = variable as VariableInfo<double>;
                                if (dinfo != null)
                                {
                                    value = dinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.String:
                                VariableInfo<string> sinfo = variable as VariableInfo<string>;
                                if (sinfo != null)
                                {
                                    value = sinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Image:
                                VariableInfo<HImage> iinfo = variable as VariableInfo<HImage>;
                                if (iinfo != null)
                                {
                                    value = iinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.Region:
                                VariableInfo<HRegion> rinfo = variable as VariableInfo<HRegion>;
                                if (rinfo != null)
                                {
                                    value = rinfo.Value;
                                }
                                break;
                            case VisionUtil.EnumStrucks.VariableType.XLD:
                                VariableInfo<HXLDCont> xinfo = variable as VariableInfo<HXLDCont>;
                                if (xinfo != null)
                                {
                                    value = xinfo.Value;
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileManager.Error("VariableLog", ex.ToString());
            }
            return value;
        }
        #endregion
    }
}
