using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VisionInterface;

namespace VisionCameraManager
{
   public class CameraDoc
    {
        public List<IVisionCameraInfo> CamerasInfoList;
        [XmlIgnore]
        public Dictionary<string, IVisionCameraInfo> CamerasInfoDic;
        public CameraDoc()
        {
            CamerasInfoList = new List<IVisionCameraInfo>();
            CamerasInfoDic = new Dictionary<string, IVisionCameraInfo>();
        }
        public static CameraDoc LoadObj()
        {
            CameraDoc pDoc = new CameraDoc();
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(CameraDoc));
                FileStream fsReader = null;
                fsReader = File.OpenRead(@".//Parameter/CameraDoc.xml");
                pDoc = (CameraDoc)xmlSerializer.Deserialize(fsReader);
                fsReader.Close();
                pDoc.CamerasInfoDic = pDoc.CamerasInfoList.ToDictionary(p => p.UserID);
            }
            catch
            {
                pDoc.CamerasInfoList.Clear();

            }

            return pDoc;
        }

        public bool SaveDoc()
        {
            if (!Directory.Exists(@".//Parameter/"))
            {
                Directory.CreateDirectory(@".//Parameter/");
            }
            FileStream fsWriter1 = new FileStream(@".//Parameter/CameraDoc.xml", FileMode.Create, FileAccess.Write, FileShare.Read);
            XmlSerializer xmlSerializer1 = new XmlSerializer(typeof(CameraDoc));
            xmlSerializer1.Serialize(fsWriter1, this);
            fsWriter1.Close();
            return true;
        }
    }
}
