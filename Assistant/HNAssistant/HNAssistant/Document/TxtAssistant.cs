using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace HNAssistant
{
    /// <summary>
    /// System.IO;
    /// System.Diagnostics;
    /// </summary>
    public class TxtAssistant
    {
        public string errorMsg { get; set; }

        private string pathLog;

        public TxtAssistant()
        {
            string root = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Log", DateTime.Now.ToString("yyyy_MM"));
            if (Directory.Exists(root) == false) Directory.CreateDirectory(root);
            this.pathLog = Path.Combine(root, DateTime.Now.ToString("yyyy_MM_dd") + ".txt");
            //if (Directory.Exists(root) == false) Directory.CreateDirectory(root);
        }

        /// <summary>向给定地址path文件后添加一行数据saveMsg
        /// 只需给定地址，在执行程序时，如果不存在，则会自行新建,overRideData=true,则覆盖原有文件
        /// </summary>
        /// <param name="saveMsg"></param>
        /// <returns></returns>
        public bool TxtSave(string path,string saveMsg, bool overRideData = false)
        {
            try
            {
                FileMode mymode = overRideData ? FileMode.Create : FileMode.Append;
                using (FileStream mylogfile = new FileStream(path, mymode, FileAccess.Write, FileShare.Read))
                using (StreamWriter mylogstream = new StreamWriter(mylogfile))
                {
                    mylogstream.WriteLine(saveMsg);
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
            return true;
        }

        /// <summary>向给定地址path文件后添加一行数据saveMsg
        /// 只需给定地址，在执行程序时，如果不存在，则会自行新建,overRideData=true,则覆盖原有文件
        /// </summary>
        /// <param name="saveMsg"></param>
        /// <returns></returns>
        public bool TxtSave(string saveMsg, bool overRideData = false)
        {
            try
            {
                
                FileMode mymode = overRideData ? FileMode.Create : FileMode.Append;
                using (FileStream mylogfile = new FileStream(pathLog, mymode, FileAccess.Write, FileShare.Read))
                using (StreamWriter mylogstream = new StreamWriter(mylogfile))
                {
                    mylogstream.WriteLine(saveMsg);
                }
                return true;
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return false;
            }
            
        }

        /// <summary>读取给定路径文本的所有数据
        /// 
        /// </summary>
        /// <param name="readData"></param>
        /// <returns></returns>
        public bool TxtRead(out List<string> readData)
        {
            readData = new List<string>();
            try
            {
                if (File.Exists(pathLog) == false)
                {
                    throw new Exception("输入路径未找到文件！");
                }
                using (StreamReader myreader = new StreamReader(pathLog))
                {
                    string result;
                    while (myreader.Peek() != -1)
                    {
                        result = myreader.ReadLine();
                        readData.Add(result);
                    }
                }
            }
            catch (Exception e)
            {
                readData.Add("error");
                errorMsg = e.Message;
                return false;
            }
            return true;
        }
    }
    
}
