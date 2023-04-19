using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HNAssistant
{
    /// <summary>该类为使用参考类
    /// 完全复制该类，后续根据需求添加
    /// 命名空间：
    /// using System.IO;
    /// using Newtonsoft.Json;
    /// using Newtonsoft.Json.Linq;
    /// 引用：
    /// Newtonsoft.Json
    /// dll：
    /// Newtonsoft.Json.dll
    /// Newtonsoft.Json.xml
    /// </summary>
    public class JsonAssistant
    {
        public string errorMsg { get; set; }

        /// <summary>向指定路径下的json文件中写入数据
        /// 数据为类形式的属性，结构体，其余变量将只保存值，而没有对应键
        /// </summary>
        /// <param name="path"></param>
        /// <param name="saveData"></param>
        /// <returns></returns>
        public bool JsonWrite(string path, object saveData)
        {
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;
                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                {
                    serializer.Serialize(jsonWriter, saveData);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>根据输入地址的Json文件，返回指定key的值
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool JsonRead(string path, string key, ref object value)
        {
            try
            {
                using (System.IO.StreamReader file = System.IO.File.OpenText(path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o = (JObject)JToken.ReadFrom(reader);
                        value = o[key].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                value = "error";
                errorMsg = e.Message;
                return false;
            }
            return true;
        }

    }
}
