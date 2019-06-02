using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtil
{
    public class HiPerfTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
            out long lpPerformanceCount);
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);
        private long startTime, currentTime;
        private long freq;
        // 构造函数 
        public HiPerfTimer()
        {
            startTime = 0;
            currentTime = 0;
            if (QueryPerformanceFrequency(out freq) == false)
            {
                // 不支持高性能计数器 
                throw new Win32Exception();
            }
        }
        // 开始计时器 
        public void Start()
        {
            // 来让等待线程工作 
            QueryPerformanceCounter(out startTime);
        }
        // 停止计时器 
        public bool TimeUp(double lTime)
        {
            if (Duration > lTime)
                return true;
            else
                return false;
        }
        // 返回计时器经过时间(单位：秒) 
        public double Duration
        {
            get
            {
                QueryPerformanceCounter(out currentTime);
                return (double)(currentTime - startTime) / (double)freq;
            }
        }
    }
}
