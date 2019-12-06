using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Management;
using System.Runtime.InteropServices;

namespace Common.Utility
{
    public class SystemInfo
    {
        private int m_ProcessorCount = 0;   //CPU个数 
        private PerformanceCounter pcCpuLoad;   //CPU计数器 
        private long m_PhysicalMemory = 0;   //物理内存 

        private const int GW_HWNDFIRST = 0;
        private const int GW_HWNDNEXT = 2;
        private const int GWL_STYLE = (-16);
        private const int WS_VISIBLE = 268435456;
        private const int WS_BORDER = 8388608;

        #region AIP声明 
        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        [DllImport("User32")]
        private extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32")]
        private extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        private static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private extern static int GetWindowTextLength(IntPtr hWnd);
        #endregion

        #region 构造函数 
        ///  
        /// 构造函数，初始化计数器等 
        ///  
        public SystemInfo()
        {
            //初始化CPU计数器 
            pcCpuLoad = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            pcCpuLoad.MachineName = ".";
            pcCpuLoad.NextValue();

            //CPU个数 
            m_ProcessorCount = Environment.ProcessorCount;

            //获得物理内存 
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                if (mo["TotalPhysicalMemory"] != null)
                {
                    m_PhysicalMemory = long.Parse(mo["TotalPhysicalMemory"].ToString());
                }
            }
        }
        #endregion

        #region CPU个数 

        /// <summary>
        /// 获取CPU个数 
        /// </summary>
        public int ProcessorCount
        {
            get
            {
                return m_ProcessorCount;
            }
        }
        #endregion

        #region CPU占用率 

        /// <summary>
        /// 获取CPU占用率 
        /// </summary>
        public float CpuLoad
        {
            get
            {
                return pcCpuLoad.NextValue();
            }
        }
        #endregion

        #region 可用内存 

        /// <summary>
        /// 获取可用内存 
        /// </summary>
        public long MemoryAvailable
        {
            get
            {
                long availablebytes = 0;
                ManagementClass mos = new ManagementClass("Win32_OperatingSystem");
                foreach (ManagementObject mo in mos.GetInstances())
                {
                    if (mo["FreePhysicalMemory"] != null)
                    {
                        availablebytes = 1024 * long.Parse(mo["FreePhysicalMemory"].ToString());
                    }
                }
                return availablebytes;
            }
        }

        /// <summary>
        /// 获取内存使用率
        /// </summary>
        /// <returns></returns>
        public double GetMemoryUsedRate()
        {
            double? MemoryUsedRate = (double?)(PhysicalMemory - MemoryAvailable) / PhysicalMemory;
            return MemoryUsedRate.HasValue ? Convert.ToDouble(MemoryUsedRate * 100) : 0;
        }
        #endregion

        #region 物理内存 
        /// <summary>
        /// 获取物理内存 
        /// </summary>
        public long PhysicalMemory
        {
            get
            {
                return m_PhysicalMemory;
            }
        }
        #endregion

        /// <summary>  
        /// 获取指定驱动器器的空间(单位为GB)  
        /// </summary>  
        /// <param name=”str_HardDiskName”>只需输入代表驱动器的字母即可 </param>  
        /// <returns>long[] index=0:可用空间,index=1:总空间</returns>  
        public static long[] GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long[] freeSpace = new long[2];
            str_HardDiskName = str_HardDiskName + ":\\";
            foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace[0] = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                    freeSpace[1] = drive.TotalSize / (1024 * 1024 * 2014);
                    break;
                }
            }
            return freeSpace;
        }
    }
}
