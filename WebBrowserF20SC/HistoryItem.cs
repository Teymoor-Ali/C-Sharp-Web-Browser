using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBrowserF20SC
{
    class SessionHistory
    {
        //Accessors 
        public DateTime date { get; set; }
        public string url { get; set; }

        public SessionHistory() {  }

        public SessionHistory(DateTime date, String url)
        {
            this.date = date;
            this.url = url;
        }
    }
}
