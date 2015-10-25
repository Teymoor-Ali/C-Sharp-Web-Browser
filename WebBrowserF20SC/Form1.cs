using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace WebBrowserF20SC
{
    public partial class Form1 : Form
    {

        private string historyFileName;
        private List<SessionItem> currentSessionHistory;
        private int currentPageIndex = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            init();
        }

        private void init()
        {
            //Create a new list for current history to be stoerd
            currentSessionHistory = new List<SessionItem>();
            //muteing forward and back buttons
            if (isFirstElement() || currentPageIndex == 0) btnBack.Enabled = false;
            if (isLastElement()) btnForward.Enabled = false;
            //load the hisory items and favourites on startup
            SavedHistoryItems();
            reloadFavourites();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //if the index of the current tab page is the last one then create a new tab
            if (e.TabPageIndex == this.tabControl1.TabPages.Count - 1)
            {

                try
                {
                    //create a new tab on the last index and select the index  as the second last one so that the user stays in the current tab
                    this.tabControl1.TabPages.Insert(this.tabControl1.TabPages.Count - 1, "untitled");
                    this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 2;

                    //fill the new tab with a rich text box
                    RichTextBox rtb = new RichTextBox();
                    rtb.Dock = DockStyle.Fill;

                    this.tabControl1.TabPages[this.tabControl1.TabPages.Count - 2].Controls.Add(rtb);
                    //add date and time to check the the rich text box is working
                    rtb.Text = DateTime.Now.ToString();


                    if (this.tabControl1.TabCount == 3)
                    {
                        try
                        {
                            StreamReader rdr = new StreamReader("homepage.txt");
                            //put the text as the line the homepage text file reads
                            this.urltextbox.Text = rdr.ReadLine();
                            //call the event handler for the Go button
                            this.btnGo.PerformClick();
                            rdr.Close();
                        }
                        catch (Exception)
                        {
                           
                        }
                    }
                }          
                catch (Exception)
                {
                    
                }
            }
        }
        void loadNewTabWithURL(string URL)
        {
            //create a new tab on the last index and choose the second last index as the current index so the user stays in the current tab
            this.tabControl1.TabPages.Insert(this.tabControl1.TabPages.Count - 1, "untitled");
            this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 2;
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;

            this.tabControl1.TabPages[this.tabControl1.TabPages.Count - 2].Controls.Add(rtb);

            rtb.Text = DateTime.Now.ToString();

            //the url text box will contain the string URL
            this.urltextbox.Text = URL;

            this.btnGo.PerformClick();
        }

        void refreshHistoryItems()
        {
            historyToolStripMenuItem.DropDownItems.Clear();

            for (int i = currentPageIndex - 1; i >= 0; --i)
            {
                SessionItem historyItem = currentSessionHistory.ElementAt(i);
                DateTime dateTime = historyItem.date;
                String url = historyItem.url;

                historyToolStripMenuItem.DropDownItems.Add(dateTime + " - " + url);
            }
        }
        void SavedHistoryItems()
        {
            historyFileName = "history.txt";

            //clear all items in the history menu item
            this.historyToolStripMenuItem.DropDownItems.Clear();

            string historyContents = "";
            try
            {
                //read the entire history items from the file in a single string
                StreamReader rdr = new StreamReader(historyFileName);
                historyContents = rdr.ReadToEnd();
                rdr.Close();

                //split each line of the input file from the string
                string[] splits = historyContents.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string entry in splits)
                {
                    //split the line in two, where the left side of the delimiter is the date and time and the right side is the url
                    string[] splits2 = entry.Split(new string[] { "!#!" }, StringSplitOptions.RemoveEmptyEntries);
                    if (splits2.Length == 2)
                    {
                        //the first part of the splitted line is the date and time
                        DateTime dt = DateTime.Parse(splits2[0]);


                        //add the date and time in hour and minutes format to the menu item with a '-' and the url
                        this.historyToolStripMenuItem.DropDownItems.Add(dt.ToString("hh:mm") + " - " + splits2[1]);

                    }
                    else
                    {
                        System.Console.WriteLine("Incorrect format of the history file");
                    }
                }

            }

            catch (Exception)
            {
                
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //the url text box contains the text of the selected tab 
                urltextbox.Text = tabControl1.SelectedTab.Text;
            }
            catch (Exception)
            {
                
            }
        }

        private void historyToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //if an item in the history drop down menu is clicked, the entire text that was clicked in the menu is saved to a string 'url'
            string url = e.ClickedItem.Text;

            // the string is split into two where the first part is the date and the second part is the url and the 'url' is extracted from it
            string[] splits = url.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            //call the loadNewTabWithURL function and pass the 'url' as a parameter to open a new tab and display the url clicked from the history item
            loadNewTabWithURL(splits[1]);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //check if the enter key is pressed
            if (e.KeyCode == Keys.Return)
            {
                //if the enter key is pressed call the event handler for clicking on the Go button
                this.btnGo.PerformClick();
            }
        }

        //when the Go button is clicked 
        private void btnGo_Click(object sender, EventArgs e)
        {
            //save the index of the current tab
            int currentTabIndex = this.tabControl1.SelectedIndex;
            //remove any spaces if present in the text written in the textbox
            string url = this.urltextbox.Text.Trim();
            //if the text box is empty then return
            if (url.Length == 0)
            {
                return;
            }
            //check if the url starts with "http", if not then add "http://"
            if (url.StartsWith("http") == false)
            {
                url = "http://" + url;
            }
            currentSessionHistory.Add(new SessionItem(DateTime.Now, url));
            ++currentPageIndex;
            refreshHistoryItems();
            loadPage(url, currentTabIndex);
            //save the current date and time, the delimiter and the url into a string called historyEntry
            string historyEntry = DateTime.Now.ToString() + "!#!" + url;
            //open the history file and add the entry inside it
            StreamWriter rtr = new StreamWriter(historyFileName, true);
            rtr.WriteLine(historyEntry);
            rtr.Close();
            //refresh the history items so it gets updated and the item gets added
            SavedHistoryItems();
        }

        private void loadPage(String url, int currentTabIndex)
        {
            if (url.Length == 0) return;

            tabControl1.SelectedTab.Text = urltextbox.Text = url;
            btnBack.Enabled = !isFirstElement();
            btnForward.Enabled = !isLastElement();

            Console.WriteLine("currPageIndex: " + currentPageIndex);

            //create an object of the class URLLoader
            LoadWebpage loader = new LoadWebpage();
            //save the current url into that class's URL variable
            loader.URL = url;
            //save the rich text box of the current tab to the loader class so the raw html can be displayed
            loader.rtb = (RichTextBox)this.tabControl1.TabPages[currentTabIndex].Controls[0];
            //start a new thread for this tab
            Thread t = new Thread(new ThreadStart(loader.loadURL));
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }

        //read the url from the home page file and call the load url function to load the url inot the tab
        private void btnHome_Click(object sender, EventArgs e)
        {
            try
            {
                //
                StreamReader rdr = new StreamReader("homepage.txt");
                loadNewTabWithURL(rdr.ReadLine().Trim());
            }
            catch (Exception exp)
            {
                System.Console.WriteLine(exp.Message.ToString());
            }
        }

        private void editHomePage_Click(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void editHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dsiplay the home page dialog box to set or edit the home page
            HomePageDialog dlg = new HomePageDialog();
            dlg.ShowDialog();
        }

        private void addCurrentPageToFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        void reloadFavourites()
        {
            // prevent the user from choosing one of the options available on the associated ToolStripDropDown control
            this.favortiToolStripMenuItem.HideDropDown();
            //clear all current data from the drop down list
            this.favortiToolStripMenuItem.DropDownItems.Clear();

            try
            {
                //read from the favourites.text file
                StreamReader rdr = new StreamReader("favourites.txt");
                //read all the entries in the text file and save to a string
                string allEntries = rdr.ReadToEnd();

                //split line by line
                string[] splits = allEntries.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                rdr.Close();


                foreach (string item in splits)
                {
                    //split each line further into two parts separated by a delimiter where the left side is the Name of the Favourites and the right side is the url
                    string[] splits2 = item.Split(new string[] { "!#!" }, StringSplitOptions.RemoveEmptyEntries);


                    ToolStripMenuItem ddMenu = new ToolStripMenuItem(splits2[0]);
                    //add 3 further menu items, "Open", "Edit" and "Remove" to the favourite menu item
                    ddMenu.DropDownItemClicked += new ToolStripItemClickedEventHandler(ddMenu_DropDownItemClicked);

                    ddMenu.DropDownItems.Add("Open");
                    ddMenu.DropDownItems.Add("Edit");
                    ddMenu.DropDownItems.Add("Remove");
                    this.favortiToolStripMenuItem.DropDownItems.Add(ddMenu);
                }
            }
            catch (Exception)
            {
                
            }
        }

        void ddMenu_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //save the index of the favourite menu item in the design
            int favindex = this.favortiToolStripMenuItem.DropDownItems.IndexOf(e.ClickedItem.OwnerItem);
            //save the text of the clicked menu item
            string subMenuText = e.ClickedItem.Text;

            //if the menu item clicked is Open
            if (subMenuText == "Open")
            {
                try
                {
                    //open the favourites text file and read the line number from favindex
                    StreamReader rdr = new StreamReader("favourites.txt");
                    string line = rdr.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[favindex];

                    //split that line into two further parts and read the right part whcih is the url
                    string[] splits = line.Split(new string[] { "!#!" }, StringSplitOptions.RemoveEmptyEntries);

                    //load the url once extracted from the text file
                    loadNewTabWithURL(splits[1]);
                }
                catch (Exception)
                {
                   
                }
            }
            //if the menu item clicked is "Edit"
            else if (subMenuText == "Edit")
            {
                //set the index of the favourites
                AddToFavourites frmAddToFavourites = new AddToFavourites();
                frmAddToFavourites.setmyIndex(favindex);
                //call the dialog box so the user can edit the favourites
                frmAddToFavourites.ShowDialog();
                //refresh the favourites items so they can get updated
                reloadFavourites();
            }
            //if the menu item clicked is "Remove"
            else if (subMenuText == "Remove")
            {
                try
                {
                    //read the entire text file into a string and split each line
                    StreamReader rdr = new StreamReader("favourites.txt");
                    string allFavourites = rdr.ReadToEnd();
                    string[] splits = allFavourites.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    rdr.Close();
                    //find the line which needs to be removed through favindex and rewrite
                    //the entire string again line by line except for the line that needs to be removed
                    StreamWriter rtr = new StreamWriter("favourites.txt", false);
                    for (int i = 0; i < splits.Length; i++)
                    {
                        if (i != favindex)
                        {
                            rtr.WriteLine(splits[i]);
                        }
                    }

                    rtr.Close();

                    reloadFavourites();

                }
                catch (Exception)
                {
                   
                }
            }

        }

        //when the add to favourites is clicked from the toolbar
        private void addToFavouritesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            AddToFavourites frmFavourites = new AddToFavourites();
            //remove any spaces from the url and save the url to the class's object
            frmFavourites.setMyURL(this.urltextbox.Text.Trim());
            //show the dialog box for Add to Favourites
            frmFavourites.ShowDialog();
            //refresh the favourites so that hey get updated
            reloadFavourites();
        }

        //when the close button is clicked, it closes the current tab
        private void btnCloseTab_Click(object sender, EventArgs e)
        {
            //remove the current index that is selected
            this.tabControl1.TabPages.RemoveAt(this.tabControl1.SelectedIndex);
            //the selected index should now be the second last one
            this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 2;
        }

        private void Backbtn(object sender, EventArgs e)
        {
            //Back button moves the index
            if (isFirstElement()) return;
            currentPageIndex -= 1;
            String url = currentSessionHistory.ElementAt(currentPageIndex - 1).url;
            loadPage(url, tabControl1.SelectedIndex);
        }

        private void Forwardbtn(object sender, EventArgs e)
        {
            //Forward button moves the index
            if (isLastElement()) return;
            currentPageIndex += 1;
            String url = currentSessionHistory.ElementAt(currentPageIndex - 1).url;
            loadPage(url, tabControl1.SelectedIndex);
        }

        private void Refreshbtn(object sender, EventArgs e)
        {
            String url = urltextbox.Text.ToString();
            loadPage(url, tabControl1.SelectedIndex);
        }

        private Boolean isFirstElement()
        {
            return currentPageIndex == 1;
        }

        private Boolean isLastElement()
        {
            return currentPageIndex == currentSessionHistory.Count;
        }

        private void ClearHistorybtn_Click(object sender, EventArgs e)
        {
            // Clears the history file and refreshes the history
            File.WriteAllText("history.txt", "");
            MessageBox.Show("History Cleared");
            SavedHistoryItems();
        }
    }
}
