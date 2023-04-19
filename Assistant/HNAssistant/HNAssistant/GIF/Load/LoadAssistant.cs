using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNAssistant
{
    public class LoadAssistant
    {
        Loading load = new Loading();

        public void Show()
        {
            if (load.IsDisposed == true)
            {
                load = new Loading();
            }
            load.Show();
            load.BringToFront();
        }

        public void Hide()
        {
            if (load.IsDisposed == false)
            {
                load.Close();
                load.Dispose();
            }
        }
    }
}
