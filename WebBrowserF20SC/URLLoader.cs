using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebBrowserF20SC
{
    public class URLLoader
    {

        public string URL;
        public RichTextBox rtb;

        public void loadURL()
        {
            WebClient wc = new WebClient();
            string htmlRaw = "";
            try
            {
                //get the raw html
                htmlRaw = wc.DownloadString(this.URL);
            }
            catch (Exception exp)
            {
                //display any error messages including the status codes
                htmlRaw = exp.Message;
            }
            try
            {
                //display the raw html in the rich text box
                this.rtb.Invoke(new MethodInvoker(delegate { rtb.Text = htmlRaw; }));
            }
            catch (Exception)
            {
                
            }
        }
    }
}