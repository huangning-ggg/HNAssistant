using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HNAssistant
{
    /// <summary>该类为使用参考类
    /// 完全复制该类，后续根据需求添加
    /// 命名空间：   
    /// using System.IO;
    /// 引用：
    /// ...
    /// dll：
    /// (DllImport)kernel32   
    /// </summary>
    public class INIHelper
    {
        public string errorMsg { get; set; }

        public Dictionary<FileNameXml, string> filePath { get; set; }

        public INIHelper()
        {
            //文件统一放置在Debug目录下，命名为Parameter
            string BasePath = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ParameterIni");
            if (Directory.Exists(BasePath) == false)
            {
                Directory.CreateDirectory(BasePath);
            }
            filePath = new Dictionary<FileNameXml, string>();
            filePath.Add(FileNameXml.DevConfig, Path.Combine(BasePath, "DeviceConfig.ini"));
            filePath.Add(FileNameXml.TestConfig, Path.Combine(BasePath, "TestConfig.ini"));
            filePath.Add(FileNameXml.SystemConfig, Path.Combine(BasePath, "SystemConfig.ini"));
        }

        #region 基础函数
        // 声明INI文件的写操作函数 WritePrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);
        #endregion


    }
    //操作INI文件时，方便并清晰指定文件
    public enum FileNameIni
    {
        DevConfig,
        TestConfig,
        SystemConfig
    }

    //使用一个类作为段，在该类下创建属性作为键，若需要创建多个段，则创建多个类
    public class data1
    {
        
    }

    public class data2
    {
       
    }
}
