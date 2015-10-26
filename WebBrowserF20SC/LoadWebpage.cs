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
            //Create a WebRequest instance by calling Create with the URI
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
                rtb.Invoke(new MethodInvoker(delegate { rtb.Text = ((HttpWebResponse)e.Response).StatusCode.ToString();}));
            }
        }
    
        }
    }
