using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using MovieCatalogue;
using MovieCatalogue.Core;
using DeadDog.Movies;
using DeadDog.Movies.IMDB;
using DeadDog.Movies.IO;

namespace MovieCatalogue
{
    public partial class AddMovieForm : Form
    {
        BindingList<Movie> IMBDMovieList = new BindingList<Movie>();
        BindingList<Actor> actorsInMovie = new BindingList<Actor>();
        BindingList<Actor> actorList = Datahandler.LoadActors("actors.xml");
        BindingList<Actor> actorDisplayList = new BindingList<Actor>();
        static bool erroresWhileParsing = false;
        List<Image> IMBDPosterList = new List<Image>();
        IMDBBuffer bf = new IMDBBuffer();

        /// <summary>
        /// It is needed to have these functions to pass infomation from the Form back to the calling form.
        /// </summary>
        #region Functions returning infomation about the created/edited movie
        public string Title
        {
            get { return AddMovieTitleBox.Text; }
        }

        public Core.Genre Genre
        {
            get { return (Core.Genre)AddMovieGenreBox.SelectedItem; }
        }

        public int Year
        {
            get {

                try
                {
                    return int.Parse(AddMovieYearBox.Text);
                }
                catch (Exception exp) 
                {
                    MissingInfoForm missingInfo = new MissingInfoForm(exp.Message + "Please enter numbers only in the text box");
                    missingInfo.ShowDialog();
                    return 0;
                }

                finally{}
                
                }
        }

        public string Description
        {
            get { return AddMovieDescriptionBox.Text; }
        }

        public BindingList<Actor> ActorsInMovie
        {
            get { return actorsInMovie; }
        }

        public string Country
        {
            get { return AddMovieCountryBox.Text; }
        }

        public string Director
        {
            get { return AddMovieDirectorBox.Text; }
        }

        public string CompendiumNumber
        {
            get { return AddMovieCompendiumBox.Text; }
        }

        public int PlayTime
        {
            get
            {

                try
                {
                    return int.Parse(AddMoviePlayTimeTextBox.Text);
                }
                catch (Exception exp)
                {
                    MissingInfoForm missingInfo = new MissingInfoForm(exp.Message + "Please enter numbers only in the text box");
                    missingInfo.ShowDialog();
                    return 0;
                }

                finally { }

            }
        }

        public Image Poster
        {
            get { return IMDBMoviePictureBox.Image; }
        }

        public bool LentStatus
        {
            get 
            {
                if(checkBox_lentOut.Checked == true)
                    return true;
                else
                    return false;
            }
        }

        public string LentToPerson
        {
            get { return lentToTextBox.Text; }
        }
        #endregion

        #region Internal Functions

        private Movie AddMovieFromSearch(MainPage movie)
        {
            Movie newMovie = new Movie();

            newMovie.Title = movie.Title.Succes ? movie.Title.Data : "Error!";
            try {newMovie.Title = movie.Title;}
            catch (Exception exp) { newMovie.Title = "Error!"; erroresWhileParsing = true; }

            try {newMovie.Year = movie.Year;}
	        catch (Exception exp){newMovie.Year = 0;}

            newMovie.Genre = Core.Genre.Action;

            try {newMovie.Description = movie.Plot.ToString() + " Genre:" + movie.Genres.ToString();}
            catch (Exception exp) { newMovie.Description = "Error!"; erroresWhileParsing = true; }

            try {
                actorsInMovie.Clear();
                actorsInMovie.Add(new Actor(movie.Credits.Cast[0].Person.Name));
                actorsInMovie.Add(new Actor(movie.Credits.Cast[1].Person.Name));
                actorsInMovie.Add(new Actor(movie.Credits.Cast[2].Person.Name));
                newMovie.ActorList = new List<Actor>();
                newMovie.ActorList.AddRange(actorsInMovie);
            }
            catch (Exception exp) { erroresWhileParsing = true; }

            newMovie.Country = "English";

            try {newMovie.Director = movie.Credits.Directors[0].Person.Name;}
            catch (Exception exp) { newMovie.Director = "Error!"; erroresWhileParsing = true; }

            try {newMovie.Director = movie.Credits.Directors[0].Person.Name;}
            catch (Exception exp) { newMovie.Director = "Error!"; erroresWhileParsing = true; }

            newMovie.CompendiumNumber = "0";

            try {newMovie.PlayTime = movie.Runtime.Data.Minutes + (movie.Runtime.Data.Hours * 60);}
            catch (Exception exp) { newMovie.PlayTime = 0; erroresWhileParsing = true; }

            try {newMovie.PlayTime = movie.Runtime.Data.Minutes + (movie.Runtime.Data.Hours * 60);}
            catch (Exception exp) { newMovie.PlayTime = 0; erroresWhileParsing = true; }
            
            newMovie.Poster = "";
            try { newMovie.Poster = movie.PosterURL.ToString(); }
            catch (Exception exp) { newMovie.Poster = ""; erroresWhileParsing = true; }

            newMovie.LentOut = false;
            newMovie.LendPerson = "";

            try { IMBDPosterList.Add(movie.PosterURL.Data.GetImage()); ; }
            catch (Exception exp) { erroresWhileParsing = true; }
            
            return newMovie;
        }
        #endregion

        /// <summary>
        /// Here you can find the constructors for the Form. Some calls to the Form requieres a Movie to be passed as a parameter, that's why there's a need for two.
        /// </summary>
        #region Constructors for the Form
        public AddMovieForm()
        {
            InitializeComponent();
            CommonInitialiser();           
        }

        public AddMovieForm(Movie movie)
        {
            InitializeComponent();

            #region Set text fields from the imported Movie
            AddMovieTitleBox.Text = movie.Title;
            AddMovieYearBox.Text = movie.Year.ToString();

            for (int i = 0; i < movie.ActorList.Count; i++)
                actorsInMovie.Add(movie.ActorList[i]);

            AddMovieCountryBox.Text = movie.Country;
            AddMovieDirectorBox.Text = movie.Director;
            AddMovieCompendiumBox.Text = movie.CompendiumNumber.ToString();
            AddMovieDescriptionBox.Text = movie.Description;
            AddMoviePlayTimeTextBox.Text = movie.PlayTime.ToString();
            IMDBMoviePictureBox.Image = Base64ToImage(movie.Poster);

            if (movie.LentOut == true)
            {
                checkBox_lentOut.Checked = true;
                lentToTextBox.Text = movie.LendPerson;
            }

            #endregion

            CommonInitialiser();
        }

        public void CommonInitialiser()
        {
            IMBDSearchListBox.DataSource = IMBDMovieList;
            IMBDSearchListBox.DisplayMember = "DisplayTitle";
            IMBDSearchListBox.ClearSelected();

            AddMovieGenreBox.DataSource = Enum.GetValues(typeof(MovieCatalogue.Core.Genre));

            if(actorList == null)
                actorList = new BindingList<Actor>();

            for (int i = 0; i < actorList.Count; i++)
                actorDisplayList.Add(actorList[i]);

            AddMovieActorsInMovieBox.DataSource = actorsInMovie;
            AddMovieActorsInMovieBox.DisplayMember = "DisplayActor";
            AddMovieAllActorsBox.DataSource = actorDisplayList;
            AddMovieAllActorsBox.DisplayMember = "DisplayActor";
        }

        public BindingList<Actor> ActorList
        {
            get { return actorList; }
            set { actorList = value; }
        }
        
        #endregion



        #region Eventhandling functions.

        /// <summary>
        /// This function describes what happens when you click the DoneButton. It check if the info is correct and return the
        /// info if it is. If it isn't, it'll throw an error message.
        /// </summary>
        private void DoneButton_Click(object sender, EventArgs e)
        {
            if (this.AddMovieTitleBox.Text == "" || this.AddMovieYearBox.Text == "" || this.AddMovieGenreBox.Text == "" || this.AddMovieDescriptionBox.Text == "" || this.actorsInMovie.Count == 0 || this.AddMovieCountryBox.Text == "" || this.AddMovieDirectorBox.Text == "" || this.AddMovieCompendiumBox.Text == "" || this.AddMoviePlayTimeTextBox.Text == "")
            {
                MissingInfoForm missingInfo = new MissingInfoForm("Some infomation is missing! Make sure you have filled out all of the required fields. They are marked with a star!");
                missingInfo.ShowDialog();
            }

            else
            {
                 for(int i = 0; i < actorsInMovie.Count; i++)
                {
                    if (!CheckActor(actorsInMovie[i], actorList))
                    {
                        actorList.Add(actorsInMovie[i]);
                        actorDisplayList.Add(actorsInMovie[i]);
                    }
                }

                Datahandler.SaveActors("actors.xml", actorList);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }
        
        private void Addbutton_Click(object sender, EventArgs e)
        {
            actorsInMovie.Add((Actor)AddMovieAllActorsBox.SelectedItem);

            AddMovieActorsInMovieBox.BeginUpdate();
            AddMovieActorsInMovieBox.DataSource = actorsInMovie;
            AddMovieActorsInMovieBox.EndUpdate();


        }

        private void AddMovieDeleteActorButton_Click(object sender, EventArgs e)
        {
            actorsInMovie.RemoveAt(AddMovieActorsInMovieBox.SelectedIndex);

            if (SearchBox.Text.Length == 0)
            {
                actorDisplayList.Clear();
                for (int i = 0; i < actorList.Count; i++)
                    actorDisplayList.Add(actorList[i]);
            }
        }

        private void AddMovieNewActorButton_Click(object sender, EventArgs e)
        {
            actorList.Add(new Actor(AddMovieNewActorBox.Text));
            Datahandler.SaveActors("actors.xml", actorList);
            AddMovieNewActorBox.ResetText();

            if (SearchBox.Text.Length == 0)
            {
                actorDisplayList.Clear();
                for (int i = 0; i < actorList.Count; i++)
                    actorDisplayList.Add(actorList[i]);
            }

        }

        private void AddMovieDeleteActor_Click(object sender, EventArgs e)
        {
            actorList.Remove((Actor)AddMovieAllActorsBox.SelectedItem);
            Datahandler.SaveActors("actors.xml", actorList);


            actorDisplayList.Clear();
            for (int i = 0; i < actorList.Count; i++)
                actorDisplayList.Add(actorList[i]);
        }

        private void IMBDSearchListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
            
            Movie IMBDSelectedMovie = (Movie)IMBDSearchListBox.SelectedItem;
            int index = IMBDSearchListBox.SelectedIndex;

            AddMovieTitleBox.Text = IMBDSelectedMovie.Title;
            AddMovieYearBox.Text = IMBDSelectedMovie.Year.ToString();
            AddMovieCountryBox.Text = IMBDSelectedMovie.Country;
            AddMovieDirectorBox.Text = IMBDSelectedMovie.Director;
            AddMovieDescriptionBox.Text = IMBDSelectedMovie.Description;
            AddMoviePlayTimeTextBox.Text = IMBDSelectedMovie.PlayTime.ToString();

            if (IMBDPosterList.Count >= index)
                IMDBMoviePictureBox.Image = IMBDPosterList[index];

            actorsInMovie.Clear();
            if (IMBDSelectedMovie.ActorList.Count == 3)
            {
                actorsInMovie.Add(IMBDSelectedMovie.ActorList[0]);
                actorsInMovie.Add(IMBDSelectedMovie.ActorList[1]);
                actorsInMovie.Add(IMBDSelectedMovie.ActorList[2]);
            }

            }
            catch (Exception exp) 
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            finally { }
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public static bool CheckActor(Actor actor, BindingList<Actor> list)
        {
            int i = 0; 
            do
            {
                if (list.Count == 0)
                    return false;

                if (actor.Name == list[i].Name)
                {
                    return true;
                }
                 i++;
            }
            while(i < list.Count);

            return false;
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (SearchBox.Text.Length > 0)
            {
                int index = 0;
                if (Core.Search.ActorSearch(SearchBox.Text, AddMovieAllActorsBox.Items, out index))
                {
                    AddMovieAllActorsBox.SelectedIndex = index;
                }
                else 
                {
                    MissingInfoForm mif = new MissingInfoForm("There was no actor with that name in the list! Please add the person to the list first!");
                    mif.ShowDialog();
                }
            }
        }

        private void btImportPoster_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            openFileDialog1.Title = "Select an Image File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.IMDBMoviePictureBox.Image = Image.FromFile(openFileDialog1.FileName);
                this.IMDBMoviePictureBox.Refresh();
            }
        }

        private void IMDBSearch_textbox_TextChanged(object sender, EventArgs e)
        {
            if(IMDBSearch_textbox.Text == "Search for a movie here...")
                IMDBSearch_textbox.ForeColor = Color.DarkGray;
            else
                IMDBSearch_textbox.ForeColor = Color.Black;

            
        }

        private void IMDBSearch_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                IMBDMovieList.Clear();
                IMBDPosterList.Clear();

                if (IMDBSearch_textbox.Text != "")
                {
                    IMBDMovieList.Clear();
                    int moviecount = 5;
                    if (bf.SearchTitle(IMDBSearch_textbox.Text).Results.Count < 5)
                        moviecount = bf.SearchTitle(IMDBSearch_textbox.Text).Results.Count;

                    erroresWhileParsing = false;

                    for (int i = 0; i < moviecount; i++)
                    {
                        try
                        {
                            var mainMovie = bf.ReadMain(bf.SearchTitle(IMDBSearch_textbox.Text).Results[i].Id);
                            if(mainMovie.MediaType == MediaType.Movie)
                                IMBDMovieList.Add(AddMovieFromSearch(mainMovie));
                        }

                        catch (Exception exp)
                        {
                            if (exp.Message == "File could not be loaded after 3 attempts.")
                            {
                                MissingInfoForm missingInfo = new MissingInfoForm("It seems you are not connected to the Internet. Please check your internet connection.");
                                missingInfo.ShowDialog();
                            }
                        }

                        finally { }
                    }

                    if (erroresWhileParsing)
                    {
                        MissingInfoForm missingInfo = new MissingInfoForm("Some data was not found and might not be shown! Check boxes for the message \"Error!\"");
                        missingInfo.ShowDialog();
                    }

                    if (IMBDMovieList.Count == 0)
                    {
                        MissingInfoForm missingInfo = new MissingInfoForm("No movies with the specified title was found! Maybe you miss-spelled the title?");
                        missingInfo.ShowDialog();
                    }
                }
                else
                {
                    MissingInfoForm missingInfo = new MissingInfoForm("When using the IMBD search function, make sure you have filled in a Title to search for!");
                    missingInfo.ShowDialog();
                }
            }
        }

        private void IMDBSearch_textbox_MouseDown(object sender, MouseEventArgs e)
        {
            if (IMDBSearch_textbox.Text == "Search for a movie here...")
                IMDBSearch_textbox.Text = "";
        }

        private void checkBox_lentOut_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_lentOut.Checked == true)
            {
                lentToLabel.Visible = true;
                lentToTextBox.Visible = true;
            }
            else
            {
                lentToLabel.Visible = false;
                lentToTextBox.Visible = false;
            }
        }

        private void IMDBSearch_textbox_Leave(object sender, EventArgs e)
        {
            if (IMDBSearch_textbox.Text == "")
                IMDBSearch_textbox.Text = "Search for a movie here...";
        }

        #endregion
    }
}
