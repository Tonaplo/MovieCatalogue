using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MovieCatalogue
{
    public partial class SaveLoadDataForm : Form
    {
        BindingList<Core.Movie> loadedData = new BindingList<Core.Movie>();
        bool save;
        BackgroundWorker bgw = new BackgroundWorker();

        public SaveLoadDataForm(bool saving, BindingList<Core.Movie> movielist)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.ControlBox = false;
            save = saving;
            loadedData = movielist;
            bgw.DoWork += bw_DoWork;
            bgw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
        }

        public BindingList<Core.Movie> LoadedList
        {
            get { return loadedData; }
        }

        private void SaveLoadDataForm_Shown(object sender, EventArgs e)
        {
            if (save)
            {
                this.Text = "Autosave";
                labelText.Text = "Autosaving data, please wait...";
            }
            else
            {
                this.Text = "Loading Data";
                labelText.Text = "Autoloading data, please wait...";
            }

            bgw.RunWorkerAsync();
        }

        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (save)
            {
                Core.Datahandler.SaveMovie("movie.xml", loadedData);
            }
            else
            {
                loadedData = Core.Datahandler.LoadMovie("movie.xml");
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            labelText.Text = "Done!";
            this.Close();
        }
    }
}
