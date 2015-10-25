using System;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace WebBrowserF20SC
{
    public class LoadWebpage
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
                //MethodInvoker provides a simple delegate that is used to invoke a method with a void parameter list
                rtb.Invoke(new MethodInvoker(delegate { rtb.Text = sr.ReadToEnd(); }));
                sr.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine(((HttpWebResponse)e.Response).StatusCode.ToString());
                //Exceptions for individual error codes returned from server
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "NotFound")
                {
                    rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "page not found"; }));
                }
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "BadRequest")
                {
                    rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "Bad Request"; }));
                }
                if (((HttpWebResponse)e.Response).StatusCode.ToString() == "Forbidden")
                {
                    rtb.Invoke(new MethodInvoker(delegate { rtb.Text = "Forbidden"; }));
                }
            }

        }
    }
}