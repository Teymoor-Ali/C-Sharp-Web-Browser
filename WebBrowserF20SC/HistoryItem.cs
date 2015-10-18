using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBrowserF20SC
{
    class SessionItem
    {
        public DateTime date { get; set; }
        public String url { get; set; }

        public SessionItem() {  }

        public SessionItem(DateTime date, String url)
        {
            this.date = date;
            this.url = url;
        }
    }
}
