using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNAssistant
{
    public class DownloadAssistant
    {
        public static void Download(string source, string target)
        {
            Download download = new Download();
            try
            {
                if (download.IsDisposed == true)
                {
                    download = new Download();
                }
                download.Show();
                download.DownLoad(source, target);
            }
            catch (Exception ex)
            {
                download.DeleteDirectory(target);
                throw new Exception(ex.Message);
            }
            finally
            {
                //updateVersion.Close();
            }
        }
    }
}
