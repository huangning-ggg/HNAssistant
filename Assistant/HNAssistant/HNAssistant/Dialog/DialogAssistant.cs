using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace HNAssistant
{
    /// <summary>该辅助类用于选择文件、文件夹，保存文件位置。
    /// 
    /// </summary>
    public class DialogAssistant
    {
        #region 选择文件

        #region Demonstrate
        /*1、选择文件（已存在）  
             DialogAssistant.Filters=new int{0,1};
             DialogAssistant.SelectDialogFile(ref filePath);
          2、选择文件（不存在）
             DialogAssistant.Filters=new int{0,1};
             DialogAssistant.SaveFile(ref filePath);
         */
        #endregion

        private static string Filters;
        private static string[] FiltersNum = {
                                             "Txt文件 (*.txt)|*.txt",                  //0
                                             "Microsoft Excel 97-2003(*.xls)|*.xls",   //1
                                             "Microsoft Excel(*.xlsx)|*.xlsx",         //2
                                             "Microsoft Word 97-2003(*.doc)|*.doc",    //3
                                             "Microsoft Word(*.docx)|*.docx",          //4
                                             "图像文件Image(*.BMP)|*.BMP",             //5
                                             "图像文件Image(*.JPG)|*.JPG",             //6
                                             "图像文件Image(*.GIF)|*.GIF",             //7
                                             "所有文件|*.*"                            //8
                                             };
        public static int[] selectFilter;

        private static void SetFilter()
        {
            Filters = "";
            if (selectFilter == null)
            {
                selectFilter = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            }
            else
            {
                if (selectFilter.Length == 0)
                {
                    selectFilter = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
                }
            }
            int count = selectFilter.Length;
            for (int i = 0; i < count; i++)
            {
                Filters += FiltersNum[selectFilter[i]];
                if (i != count - 1)
                { Filters += "|"; }
            }
            selectFilter = null;
        }

        /// <summary>选择文件，该文件是已经存在的。
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool SelectDialogFile(ref string filePath)
        {

            OpenFileDialog Filedialog = new OpenFileDialog();
            Filedialog.Multiselect = true;//该值确定是否可以选择多个文件
            Filedialog.AddExtension = true;
            Filedialog.CheckPathExists = false;
            Filedialog.InitialDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //@"..\..";//该值指示默认打开选择文件夹时的位置
            Filedialog.Title = "请选择文件";
            SetFilter();
            Filedialog.Filter = Filters;
            if (Filedialog.ShowDialog() == DialogResult.OK)
            {
                filePath = Filedialog.FileName;
                return true;
            }
            else
            {
                filePath = "";
                return false;
            }
        }

        /// <summary>在存储时用于选择文件存储位置，该位置一般是不存在的。
        /// 
        /// </summary>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool SaveFile(ref string savePath)
        {
            try
            {
                SaveFileDialog mySave = new SaveFileDialog();
                mySave.Title = "请选择存储的文件位置。";
                SetFilter();
                mySave.Filter = Filters;
                DialogResult myResult = mySave.ShowDialog();
                if (myResult == DialogResult.OK) { savePath = mySave.FileName; return true; }
                else if (myResult == DialogResult.Cancel) return false;
                else return false;
            }
            catch
            {
                savePath = "";
                return false;
            }
        }
        #endregion

        /// <summary>选择文件夹
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool SelectDialogFolder(ref string filePath)
        {
            FolderBrowserDialog Folderdialog = new FolderBrowserDialog();
            Folderdialog.RootFolder = Environment.SpecialFolder.MyComputer;
            Folderdialog.SelectedPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            Folderdialog.Description = "请选择文件夹";
            filePath = "";
            if (Folderdialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(Folderdialog.SelectedPath)) return false;
                else
                {
                    filePath = Folderdialog.SelectedPath;
                    return true;
                }
            }
            else return false;
        }

        //使用Process打开文件夹
        //只能打开文件夹浏览器，不能选中并返回选择的文件夹
        public static void OpenDialog(string path = "defult")
        {
            string directory = path == "defult" ? Directory.GetCurrentDirectory() : path;
            if (Directory.Exists(directory))
            {
                //以进程启动选择的文件
                Process.Start("explorer", "/select,\"" + directory + "\"");
            }
        }
    }
}
