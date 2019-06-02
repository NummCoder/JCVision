using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionDisplayTool.DisplayContents
{
  public class GenericityContent:ContentBase
    {
        public HObject Current { get; set; }

        public GenericityContent()
        {

        }
        public GenericityContent(string name, HWindow window):base(name,window)
        {

        }
        public override void Display()
        {
            try
            {
                if (Current != null)
                {
                    base.SetDefaultSetting();
                    Current.DispObj(Window);
                }
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex.ToString());
            }
        }
    }
}
