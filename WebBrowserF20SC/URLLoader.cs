using System;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace WebBrowserF20SC
{
    public class URLLoader
    {

        public string URL;
        public RichTextBox rtb;

        public void loadURL()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                //rtb.Text = sr.ReadToEnd();
                this.rtb.Invoke(new MethodInvoker(delegate { rtb.Text = sr.ReadToEnd(); }));
                sr.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(((HttpWebResponse)e.Response).StatusCode.ToString());

                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "NotFound")
                {
                    this.rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "page not found"; }));
                }
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "BadRequest")
                {
                    this.rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "Bad Request"; }));
                }
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "403")
                {
                    this.rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "Forbidden"; }));
                }
            }

        }
    }
}