using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HNAssistant;

namespace Demons
{
    public partial class UCDemon2 : UserControl
    {
        public UCDemon2()
        {
            InitializeComponent();
        }

        private void UCDemon2_Load(object sender, EventArgs e)
        {

        }

        #region 下载
        
        private void btDemo1_Click(object sender, EventArgs e)
        {
            Download download = new Download();
            string source = tbSource.Text;
            string target = tbTarget.Text;
            try
            {
                DownloadAssistant.Download(source, target);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //updateVersion.Close();
            }
        }
        private void btSource_Click(object sender, EventArgs e)
        {
            string  source = "";
            if (DialogAssistant.SelectDialogFolder(ref source) == true)
            {
                tbSource.Text=source;
            }
        }

        private void btTarget_Click(object sender, EventArgs e)
        {
            string target = "";
            if (DialogAssistant.SelectDialogFolder(ref target) == true)
            {
                tbSource.Text = target;
            }
        }

        #endregion

        private void btDemo2_Click(object sender, EventArgs e)
        {
            string file = "";
            if (DialogAssistant.SelectDialogFile(ref file) == true)
            {
                tbSelectFile.Text = file;
            }
        }

        private void btDemo3_Click(object sender, EventArgs e)
        {

        }

        private void btDemo4_Click(object sender, EventArgs e)
        {

        }

        private void btDemo5_Click(object sender, EventArgs e)
        {

        }

        

        #region


        #endregion

        #region


        #endregion

        #region


        #endregion
    }
}
