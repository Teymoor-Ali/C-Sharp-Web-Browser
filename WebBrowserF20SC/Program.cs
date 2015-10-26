using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebBrowserF20SC
{
    static class Program
    {

        public static GUI GUI
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public static AddToFavourites AddToFavourites
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public static HomePageDialog HomePageDialog
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI());
        }
    }
}
