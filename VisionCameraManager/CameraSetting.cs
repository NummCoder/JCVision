using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionInterface;
using VisionDisplayTool;
using HalconDotNet;
using VisionUtil;
using VisionUtil.EnumStrucks;

namespace VisionCameraManager
{
    public partial class CameraSetting : Form
    {
        public HImage Image { get; set; }

        private IVisionCameraInfo info;
        public IVisionCameraInfo Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                FreshControlValue();
            }
        }
        /// <summary>
        /// 窗体是否处于打开状态
        /// </summary>
        public bool IsShowing { get; set; }

        private DisplayControl window;
        private void FreshControlValue()
        {
            if (info!=null)
            {
                this.textBoxCameraID.Text = info.UserID;
                this.textBoxCameraIP.Text = info.CameraIP;
                this.textBoxMacAddress.Text = info.CameraMac;
                this.UpDownShutter.Value = Convert.ToDecimal(info.CurExposureTime);
                this.trackBarShutter.Value = Convert.ToInt32(info.CurExposureTime);
                this.UpDownGain.Value = Convert.ToDecimal(info.CurGain);
                this.trackBarGain.Value = Convert.ToInt32(info.CurGain);
            }
        }

        public CameraSetting()
        {
            InitializeComponent();
            this.FormClosing += CameraSetting_FormClosing;
            window = new DisplayControl();
            window.Size = panel2.Size;
            panel2.Controls.Add(window);
            window.Show();
            this.ImageFormatCombox.Items.AddRange(new object[] {
            VisionUtil.EnumStrucks.ImageFormat.BayerRG8,
            VisionUtil.EnumStrucks.ImageFormat.BayerBG8,
            VisionUtil.EnumStrucks.ImageFormat.BayerGB8,
            VisionUtil.EnumStrucks.ImageFormat.BayerGR8,
            VisionUtil.EnumStrucks.ImageFormat.Mono8});
        }

        private void CameraSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (info!=null)
            {
                CameraManger.StopLive(info.UserID);
            }
            e.Cancel = true;
            this.Hide();
        }


        private void GrabBtn_Click(object sender, EventArgs e)
        {
            if (info!=null)
            {
                Image = CameraManger.GetImage(info.UserID);
                if (Image!=null)
                {
                    this.window.DisplayImage(Image.CopyImage());
                }
                else
                {
                    MessageHelper.ShowError("拍照失败，获取图像超时");
                }
            }
            else
            {
                MessageHelper.ShowError("拍照失败，无法获取相机信息！");
            }
        }

        private void LiveBtn_Click(object sender, EventArgs e)
        {
            if (info!=null)
            {
                if (LiveBtn.Text=="Live")
                {
                    LiveBtn.Text = "Stop";
                    if (!CameraManger.StartLive(info.UserID))                   
                       MessageHelper.ShowError("打开实时拍照失败，相机出现错误，请关闭连接后重新启动或者检查相机设置！");
                }
                else if (LiveBtn.Text == "Stop")
                {
                    LiveBtn.Text = "Live";
                    if (!CameraManger.StopLive(info.UserID))
                        MessageHelper.ShowError("打开实时拍照失败，相机出现错误，请关闭连接后重新启动或者检查相机设置！");
                }
            }
            else
            {
                MessageHelper.ShowError("拍照失败，无法获取相机信息！请关闭连接后重新启动或者检查相机设置！");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if (info!=null)
            {
                //相机曝光与增益设置
                int shutterValue = this.trackBarShutter.Value;
                int gainValue = this.trackBarGain.Value;
                CameraManger.SetShutter(info.UserID,shutterValue.ToString());
                CameraManger.SetGain(info.UserID,gainValue.ToString());
                //相机图像格式与延时设置
                if (ImageFormatCombox.SelectedItem!=null)
                {
                    ImageFormat format = (ImageFormat)ImageFormatCombox.SelectedItem;
                    CameraManger.SetImageFormat(info.UserID, format);
                    this.info.Format = format;
                }
                int heartBeatime = this.trackBarHeartBeatTime.Value;
                CameraManger.SetHearBeatTime(info.UserID,heartBeatime.ToString());
                //更新Info数据
                this.info.CurExposureTime = shutterValue.ToString();
                this.info.CurGain = gainValue.ToString();
                this.info.HeartbeatTime = heartBeatime.ToString();
            }
            CameraManger.SaveDoc();
        }

        private void trackBarShutter_Scroll(object sender, EventArgs e)
        {
            TrackBar bar = (TrackBar)sender;
            switch (bar.Name)
            {
                case "trackBarShutter":
                    AdjustExposureTimeValue(bar.Value);
                    break;
                case "trackBarGain":
                    AdjustGainValue(bar.Value);
                    break;
                case "trackBarTriggerDelayAbs":
                    AdjustTriggerDelayValue(bar.Value);
                    break;
                case "trackBarLineDebouncerTimeAbs":
                    AdjustLineDebouncerTimeValue(bar.Value);
                    break;
                case "trackBarOutLineTime":
                    AdjustOutLineTimeValue(bar.Value);
                    break;
                case "trackBarHeartBeatTime":
                    AdjustHeartBeatTimeValue(bar.Value);
                    break;
                default:
                    break;
            }
        }
        private void ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown bar = (NumericUpDown)sender;
            switch (bar.Name)
            {
                case "UpDownShutter":
                    AdjustExposureTimeValue(Convert.ToInt32(bar.Value));
                    break;
                case "UpDownGain":
                    AdjustGainValue(Convert.ToInt32(bar.Value));
                    break;
                case "UpDownTriggerDelayAbs":
                    AdjustTriggerDelayValue(Convert.ToInt32(bar.Value));
                    break;
                case "UpDownLineDebouncerTimeAbs":
                    AdjustLineDebouncerTimeValue(Convert.ToInt32(bar.Value));
                    break;
                case "UpDownOutLineTime":
                    AdjustOutLineTimeValue(Convert.ToInt32(bar.Value));
                    break;
                case "UpDownHeartBeatTime":
                    AdjustHeartBeatTimeValue(Convert.ToInt32(bar.Value));
                    break;
                default:
                    break;
            }
        }
        private void AdjustHeartBeatTimeValue(int value)
        {
            if (info!=null)
            {
                this.UpDownShutter.Value = Convert.ToDecimal(value);
                this.trackBarShutter.Value = value;
                info.HeartbeatTime = value.ToString();
                CameraManger.SetHearBeatTime(info.UserID,info.HeartbeatTime);
            }
        }

        private void AdjustOutLineTimeValue(int value)
        {
            if (info != null)
            {               
                this.UpDownOutLineTime.Value = Convert.ToDecimal(value);
                this.trackBarOutLineTime.Value = value;
                info.OutLineTime = value.ToString();
                //CameraManger.SetHearBeatTime(info.UserID, info.HeartbeatTime);
            }
        }

        private void AdjustLineDebouncerTimeValue(int value)
        {
            if (info != null)
            {
                this.UpDownLineDebouncerTimeAbs.Value = Convert.ToDecimal(value);
                this.trackBarLineDebouncerTimeAbs.Value = value;
                info.LineDebouncerTime = value.ToString();
            }
        }

        private void AdjustTriggerDelayValue(int value)
        {
            if (info != null)
            {
                this.UpDownTriggerDelayAbs.Value = Convert.ToDecimal(value);
                this.trackBarTriggerDelayAbs.Value = value;
                info.TriggerDelayTime = value.ToString();
            }
        }

        private void AdjustGainValue(int value)
        {
            if (info != null)
            {
                this.UpDownGain.Value = Convert.ToDecimal(value);
                this.trackBarGain.Value = value;
                info.CurGain = value.ToString();
                CameraManger.SetGain(info.UserID, info.CurGain);
            }
        }

        private void AdjustExposureTimeValue(int value)
        {
            if (info != null)
            {             
                this.UpDownShutter.Value = Convert.ToDecimal(value);
                this.trackBarShutter.Value = value;
                info.CurExposureTime = value.ToString();
                CameraManger.SetShutter(info.UserID, info.CurExposureTime);
            }
        }
    }
}
