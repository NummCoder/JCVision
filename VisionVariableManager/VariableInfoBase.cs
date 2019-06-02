using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionUtil.EnumStrucks;

namespace VisionVariableManager
{
    public abstract class VariableInfoBase
    {
        public string VariableName { get; set; }
        public string TaskName { get; set; }

        public VariableRange Range { get; set; }

        public VariableType variableType { get; set; }

        public string VariableAnnotation { get; set; }
    }
    public class VariableInfo<T> : VariableInfoBase
    {
        public T Value { get; set; }

        public VariableInfo()
        {

        }
        public VariableInfo(string name, VariableType type)
        {
            this.VariableName = name;
            this.variableType = type;
        }
    }
}
