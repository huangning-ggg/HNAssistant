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
using System.IO;

namespace Demons
{
    public partial class UCDemon1 : UserControl
    {
        public UCDemon1()
        {
            InitializeComponent();
        }

        private void UCDemon1_Load(object sender, EventArgs e)
        {
            InitXML();
        }

        #region XML演示
        XmlAssistant myXML = new XmlAssistant();
        public void InitXML()
        {
            myXML.Init();
        }
        private void btDemo1_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region TXT演示
        private void btDemo2_Click(object sender, EventArgs e)
        {
            TxtAssistant txt = new TxtAssistant();


            string path = Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "resume");
            foreach (string name in comboBox1.Items)
            {
                int count1 = comboBox2.Items.Count;
                int count2 = comboBox3.Items.Count;
                Random r1 = new Random();
                Random r2 = new Random();
                int index1 = r1.Next(0, count1) - 1;
                int index2 = r2.Next(0, count2) - 1;
                index1 = index1 < 0 ? 0 : index1;
                index2 = index2 < 0 ? 0 : index2;
                string pa = string.Format("{0}-{1}-{2}.pdf", name, comboBox2.Items[index1].ToString().Trim(), comboBox3.Items[index2].ToString().Trim());

                bool rtl = txt.TxtSave(Path.Combine(path, pa), "");
                System.Threading.Thread.Sleep(r1.Next(5,50));
            }

        }
        #endregion

        #region INI演示
        INIHelper myINI = new INIHelper();
        private void btDemo3_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region EXCEL演示
        private void btDemo4_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region JSON演示
        private void btDemo5_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region


        #endregion

        #region


        #endregion

        #region


        #endregion
    }
}
