using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace HNAssistant
{
    /// <summary>
    /// 引用：
    /// System.Drawing
    /// using System.Management;
    /// </summary>
    public partial class Download : Form
    {
        int dcount;
        public Download()
        {
            InitializeComponent();
        }

        private void ShowTask(int num, string project)
        {
            this.Invoke((Action)delegate
            {
                this.progressBar.Value = num;
                double percent = (float)num / (float)this.progressBar.Maximum;
                this.lbPercent.Text = string.Format("已完成 {0}", percent.ToString("P"));
                this.lbPercentBig.Text = string.Format("已完成 {0}", percent.ToString("P"));
                this.lbProject.Text = project;
            });
        }

        #region 界面
        #region 无边框移动基础
        Point mPoint;
        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }

        #endregion

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Min_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        public bool isFinish;
        public async void DownLoad(string sourceFolder, string destFolder)
        {
            isFinish = false;
            //获取源文件夹下所有文件数量
            System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(sourceFolder);
            int count = GetFilesCount(dirInfo);
            this.Invoke((Action)delegate
            {
                this.progressBar.Maximum = count;
                this.timer1.Start();
            });
            ShowTask(0, "");
            //复制
            dcount = 0;
            bool result = await Task.Run(() => CopyFolder(sourceFolder, destFolder));
            //如果下载失败，则删除未下载完成的不完整文件夹
            if (result == false)
            {
                this.Invoke((Action)delegate
                {
                    this.progressBar.Maximum = 1;
                    this.timer1.Start();
                });
                ShowTask(0, destFolder);

                result = await Task.Run(() => DeleteFolder(destFolder));
                if (result == true) ShowTask(1, destFolder);
                else { }
            }
            isFinish = true;
        }

        public async void DeleteDirectory(string targetDirectory)
        {
            isFinish = false;
            this.Invoke((Action)delegate
            {
                this.progressBar.Maximum = 1;
                this.timer1.Start();
            });
            ShowTask(0, targetDirectory);

            bool result = await Task.Run(() => DeleteFolder(targetDirectory));
            if (result == true) ShowTask(1, targetDirectory);
            else { }
            isFinish = true;
        }

        #region 文件
        /// <summary>复制文件夹及文件
        /// 将目标文件夹里面的内容，复制目标文件夹里面
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public bool CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                if (Directory.Exists(sourceFolder) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", sourceFolder));
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件

                    dcount++;
                    ShowTask(dcount, name);
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\r\n即将删除该版本文件夹！");
                return false;
            }
            finally
            {
            }
        }

        /// <summary>删除该文件夹下所有子文件&子文件夹，但该文件夹不删除
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DeleteFolderAllChile(string folderPath)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(folderPath) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", folderPath));
                //删除文件
                foreach (string filepath in Directory.GetFiles(folderPath))
                {
                    File.Delete(filepath);
                }

                //删除文件夹
                foreach (string directory in Directory.GetDirectories(folderPath))
                {
                    Directory.Delete(directory, true);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>删除整个文件夹，该文件夹及其子文件&文件夹从此消失
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool DeleteFolder(string folderPath)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(folderPath) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", folderPath));
                Directory.Delete(folderPath, true);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        //string dirPath = @"\\192.168.6.5\软件发布\Temp\器件软件发布\Start Assistant\Debug";
        //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dirPath);
        //int count = df.GetFilesCount(dirInfo);
        public int GetFilesCount(DirectoryInfo dirInfo)
        {
            int totalFile = 0;
            //totalFile += dirInfo.GetFiles().Length;//获取全部文件
            totalFile += dirInfo.GetFiles().Length;
            foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(subdir);
            }
            return totalFile;
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.isFinish == true) this.Close();
        }
    }
    /// <summary>
    /// using:
    /// using System.Management
    /// using System.Net.NetworkInformation
    /// 引用：
    /// System.Management
    /// </summary>
    public class DirectoryFil
    {
        public Label lbPercent { get; set; }

        public Label lbPercentBig { get; set; }

        public ProgressBar progressBar { get; set; }

        public Label lbProject { get; set; }

        public string errorMsg { get; set; }

        public Dictionary<eUser, CloudUser> CloudFolder = new Dictionary<eUser, CloudUser>();

        public DirectoryFil()
        {
            CloudFolder.Add(eUser.服务器, new CloudUser() { ip = @"192.168.6.5", account = "", password = "" });
            CloudFolder.Add(eUser.软件发布, new CloudUser() { ip = @"192.168.1.13", account = "", password = "" });
        }

        /// <summary>复制文件夹及文件
        /// 
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public bool CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(sourceFolder) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", sourceFolder));
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //获取源文件夹下所有文件数量


                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        //string dirPath = @"\\192.168.6.5\软件发布\Temp\器件软件发布\Start Assistant\Debug";
        //System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dirPath);
        //int count = df.GetFilesCount(dirInfo);
        public int GetFilesCount(DirectoryInfo dirInfo)
        {

            int totalFile = 0;
            //totalFile += dirInfo.GetFiles().Length;//获取全部文件
            totalFile += dirInfo.GetFiles().Length;
            foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(subdir);
            }
            return totalFile;
        }

        /// <summary>复制文件夹及文件
        /// 
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public bool CopyFolder2(string sourceFolder, string destFolder)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(sourceFolder) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", sourceFolder));
                string folderName = System.IO.Path.GetFileName(sourceFolder);
                string destfolderdir = System.IO.Path.Combine(destFolder, folderName);
                string[] filenames = System.IO.Directory.GetFileSystemEntries(sourceFolder);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (System.IO.Directory.Exists(file))
                    {
                        string currentdir = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(currentdir))
                        {
                            System.IO.Directory.CreateDirectory(currentdir);
                        }
                        CopyFolder2(file, destfolderdir);
                    }
                    else
                    {
                        string srcfileName = System.IO.Path.Combine(destfolderdir, System.IO.Path.GetFileName(file));
                        if (!System.IO.Directory.Exists(destfolderdir))
                        {
                            System.IO.Directory.CreateDirectory(destfolderdir);
                        }
                        System.IO.File.Copy(file, srcfileName);
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>删除整个文件夹，该文件夹及其子文件&文件夹从此消失
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public bool DeleteFolder(string folderPath)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(folderPath) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", folderPath));
                Directory.Delete(folderPath, true);
                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>删除该文件夹下所有子文件&子文件夹，但该文件夹不删除
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DeleteFolderAllChile(string folderPath)
        {
            try
            {
                Monitor.Enter(this);
                if (Directory.Exists(folderPath) == false)
                    throw new Exception(string.Format("文件夹：{0}不存在", folderPath));
                //删除文件
                foreach (string filepath in Directory.GetFiles(folderPath))
                {
                    File.Delete(filepath);
                }

                //删除文件夹
                foreach (string directory in Directory.GetDirectories(folderPath))
                {
                    Directory.Delete(directory, true);
                }
                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        /// <summary>使可以访问带密码的共享文件夹
        /// using:
        /// using System.Management;
        /// 引用:
        /// System.Management.dll
        /// </summary>
        /// <returns></returns>
        private bool Verify(eUser user)
        {
            try
            {
                ManagementScope ms = new ManagementScope(CloudFolder[user].ip);
                ConnectionOptions conn = new ConnectionOptions();
                conn.Username = CloudFolder[user].account;
                conn.Password = CloudFolder[user].password;
                ms.Options = conn;
                ms.Connect();
                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {

            }
        }

        /// <summary>检测网络连接
        /// using:
        /// using System.Net.NetworkInformation;
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckNetConnect(eUser user)
        {
            try
            {
                Ping ping = new Ping();
                int errorCount = 0;
                PingReply reply = ping.Send(CloudFolder[user].ip);
                while (reply.Status != IPStatus.Success)
                {
                    errorCount++;
                    System.Threading.Thread.Sleep(100);
                    if (errorCount == 4)
                    {
                        throw new Exception(string.Format("连续4次未ping通地址：{0}", CloudFolder[user].ip));
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                this.errorMsg = e.Message;
                return false;
            }
            finally
            {

            }
        }

        public struct CloudUser
        {
            public string ip;
            public string account;
            public string password;
        }
    }
    public enum eUser
    {
        软件发布, 服务器
    }
}
