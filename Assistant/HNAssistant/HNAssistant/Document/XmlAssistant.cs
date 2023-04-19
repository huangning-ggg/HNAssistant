using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace HNAssistant
{
    /// <summary>该类为使用参考类
    /// 完全复制该类，后续根据需求添加
    /// 命名空间：
    /// using System.Xml;
    /// using System.Xml.Serialization;
    /// using System.IO;
    /// 引用：
    /// ...
    /// dll：
    /// ...
    /// Tip:
    /// 1、xml与对应类中的数据量改变，只有变量名及类型不变，不会使反序列化失败，
    /// xml文件中存在的对应数据会返回，没有的忽略，但数据类型变化会引起反序列化失败！
    /// </summary>
    public class XmlAssistant
    {
        public string errorMsg { get; set; }

        public Dictionary<FileNameXml, string> filePath { get; set; }

        public DevConfig DevData { get; set; }

        public TestConfig TestData { get; set; }

        public SystemConfig SystemData { get; set; }

        public XmlAssistant()
        {
            //文件统一放置在Debug目录下，命名为Parameter
            string BasePath = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "ParameterXml");
            if (Directory.Exists(BasePath) == false)
            {
                Directory.CreateDirectory(BasePath);
            }
            filePath = new Dictionary<FileNameXml, string>();
            filePath.Add(FileNameXml.DevConfig, Path.Combine(BasePath, "DeviceConfig.xml"));
            filePath.Add(FileNameXml.TestConfig, Path.Combine(BasePath, "TestConfig.xml"));
            filePath.Add(FileNameXml.SystemConfig, Path.Combine(BasePath, "SystemConfig.xml"));
        }

        public bool Init()
        {
            try
            {
                DevData = new DevConfig();
                TestData = new TestConfig();
                SystemData = new SystemConfig();
                
                foreach (var item in this.filePath)
                {
                    if (File.Exists(item.Value) == false) SaveXmlData(item.Key);

                    else//反序列化一次文件，如果反序列化失败，表示文件有更改，则序列化为当前文件
                    {
                        if (ReadXmlData(item.Key) == false)
                        {
                            SaveXmlData(item.Key);
                        }
                        else { };
                    }
                }
                return true;
            }
            catch (Exception ex)
            { errorMsg = ex.Message; return false; }
        }

        public bool ReadXmlData(FileNameXml fn)
        {
            bool rtl;
            if (fn == FileNameXml.DevConfig)
            {
                DevConfig temp = new DevConfig();
                rtl = DeserializeFromXml<DevConfig>(filePath[fn], ref temp);
                DevData = temp;
            }
            else if (fn == FileNameXml.TestConfig)
            {
                TestConfig temp = new TestConfig();
                rtl = DeserializeFromXml<TestConfig>(filePath[fn], ref temp);
                TestData = temp;
            }
            else if (fn == FileNameXml.SystemConfig)
            {
                SystemConfig temp = new SystemConfig();
                rtl = DeserializeFromXml<SystemConfig>(filePath[fn], ref temp);
                SystemData = temp;
            }
            else
            {
                rtl = false;
                this.errorMsg = "未定义的文件";
            }
            return rtl;
        }

        public bool SaveXmlData(FileNameXml fn)
        {
            bool rtl;
            if (fn == FileNameXml.DevConfig)
                rtl = SerializeToXml<DevConfig>(filePath[fn], DevData);
            else if (fn == FileNameXml.TestConfig)
                rtl = SerializeToXml<TestConfig>(filePath[fn], TestData);
            else if (fn == FileNameXml.SystemConfig)
                rtl = SerializeToXml<SystemConfig>(filePath[fn], SystemData);
            else
            {
                rtl = false;
                this.errorMsg = "未定义的文件";
            }
            return rtl;
        }

        /// <summary>基础函数1
        /// XML序列化某一类型到指定的文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        private bool SerializeToXml<T>(string path, T obj)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                    xs.Serialize(writer, obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "SerializeToXml Fail," + ex.Message;
                return false;
            }
        }

        /// <summary>基础函数2
        /// 从某一XML文件反序列化到某一类型
        /// </summary>
        /// <param name="filePath">待反序列化的XML文件名称</param>
        /// <param name="type">反序列化出的</param>
        /// <returns></returns>
        private bool DeserializeFromXml<T>(string path, ref T xmlVal)
        {
            try
            {
                if (!System.IO.File.Exists(path)) throw new ArgumentNullException(path + " not Exists");
                using (System.IO.StreamReader reader = new System.IO.StreamReader(path))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(xmlVal.GetType());
                    xmlVal = (T)xs.Deserialize(reader);
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMsg = "DeserializeToXml Fail," + ex.Message;
                return false;
            }
        }      
    }

    public enum FileNameXml
    {
        DevConfig,
        TestConfig,
        SystemConfig
    }

    #region 设备配置
    /// <summary>设备参数
    /// 
    /// </summary>
    public class DevConfig
    {
        public DevConfig()
        {
        }
        /// <summary>
        /// IIC设备名称
        /// </summary>
        public string IICDevice;

        //光功率计
        public string OPMCom;

        //光源
        public string OPSComL;
        public string OPSComC;
    }
    #endregion

    #region 测试配置
    public class TestConfig
    {
        public TestConfig()
        { 
        
        }
        public TestMemory memory = new TestMemory();
    }

    public class TestMemory
    {
        public DateTime today;

        public string workNum;
        public string password;
        public string machineNum;
        public string OPMjumpwireNum;

        public string OPSjumpwireNumL;
        public string OPSjumpwireNumC;
        public string testBoardNum;
    }
    #endregion

    #region 系统配置
    public class SystemConfig
    {
        public SystemConfig()
        { 
        
        }
        public bool AutoStart;
    }

    #endregion
}
