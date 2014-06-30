using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MovieCatalogue.Core;
using MovieCatalogue;
using DeadDog.Movies.IO;

namespace MovieCatalogue
{
    public partial class MainWindow : Form
    {
        Movie selectedMovie = null;
        BindingList<Movie> movieList = Datahandler.LoadMovie("movie.xml");
        BindingList<Movie> movieDisplayList = new BindingList<Movie>();
        BindingList<Actor> actorList = Datahandler.LoadActors("actors.xml");
        BindingList<Movie> previousMovieList = Datahandler.LoadMovie("movie.xml");

        // This variable is set to true if any changes are made to the data.
        bool changesMade = false;

        private static ToolTip tt = new ToolTip();

        public MainWindow()
        {
            InitializeComponent();
            this.ControlBox = false;

            if (movieList == null)
                movieList = new BindingList<Movie>();

            ResetDisplayedListOfMovies();

            listBoxTitle.DataSource = movieDisplayList;
            listBoxTitle.DisplayMember = "DisplayTitle";

            ResetLabels();

            comboBoxSearchBy.Items.Add("Title");
            comboBoxSearchBy.Items.Add("Genre");
            comboBoxSearchBy.Items.Add("Actor");
            comboBoxSearchBy.Items.Add("Rented");
            comboBoxSearchBy.SelectedItem = "Title";
            comboBoxGenre.DataSource = Enum.GetValues(typeof(MovieCatalogue.Core.Genre));
        }

        #region Functions

        /// <summary>
        /// This function makes sure the label showing the Actors for the chosen Movie is
        /// grammatically correct.
        /// </summary>
        private string MovieActorLabelString(Movie movie)
        {
            string returnstring = "";
            for (int i = 0; i < movie.ActorList.Count - 2; i++)
            {
                returnstring += movie.ActorList[i].DisplayActor + ", ";
            }
            if (movie.ActorList.Count > 2)
                returnstring += movie.ActorList[movie.ActorList.Count - 2].DisplayActor + " and " + movie.ActorList[movie.ActorList.Count - 1].DisplayActor;

            else if (movie.ActorList.Count == 2)
                returnstring += movie.ActorList[0].DisplayActor + " and " + movie.ActorList[1].DisplayActor;

            else if (movie.ActorList.Count == 1)
                returnstring += movie.ActorList[0].DisplayActor;

            return returnstring;
        }

        private void ResetListBoxDisplays()
        {
            if (movieList != null)
            {
                ResetDisplayedListOfMovies();

                UpdateListBox();

                this.ResetLabels();
            }
        }

        private void ResetDisplayedListOfMovies()
        {
            movieDisplayList = new BindingList<Movie>(movieList);
        }

        private void UpdateListBox()
        {
            listBoxTitle.DataSource = null;
            listBoxTitle.DataSource = movieDisplayList;
            listBoxTitle.DisplayMember = "DisplayTitle";
        }

        public void SearchInitiated()
        {
            movieDisplayList = new BindingList<Movie>();
            List<int> tobeRemoved = new List<int>();
            string searchItem = "";

            if (comboBoxSearchBy.SelectedItem != "Genre")
                searchItem = Searchbox.Text;
            else
                searchItem = comboBoxGenre.SelectedItem.ToString();

            for (int i = 0; i < movieList.Count; i++)
            {
                if (Core.Search.MainSearch(searchItem, movieList[i], comboBoxSearchBy.SelectedItem.ToString()))
                {
                    tobeRemoved.Add(i);
                }
            }

            var li = tobeRemoved.OrderByDescending(x => x);
            NumberoOfMoviesLabel.Text = "This search yielded " + tobeRemoved.Count + " results.";

            foreach (var item in li)
            {
                movieDisplayList.Add(movieList[item]);
            }

            this.UpdateListBox();
        }

        /// <summary>
        /// This function resets all of the labels describing the Movie
        /// </summary>
        public void ResetLabels()
        {
            MovieTitleLabel.Text = "Welcome to the MovieCatalogue!";
            MovieYearLabel.Text = "Year: <Year>";
            MovieGenreLabel.Text = "Genre: <Genre>";
            MovieCountryLabel.Text = "Language: <Language>";
            MovieDirectorLabel.Text = "Director: <Director>";
            MovieActorLabel.Text = "Actors: <Actors in Movie>";
            MovieDescriptionBox.Text = "";
            MoviePlayTimeLabel.Text = "Playtime: <Play Time>";
            MoviePosterBox.Image = MoviePosterBox.InitialImage;
            NumberoOfMoviesLabel.Text = "Number of Movies: " + movieList.Count.ToString();

            listBoxTitle.ClearSelected();
        }

        private string ReturnGenreString(List<Core.Genre> list)
        {
            string ret = "";
            if (list.Count > 2)
            {
                for (int i = 0; i < list.Count - 2; i++)
                {
                    ret += list[i].ToString() + ", ";
                }
                ret += list[list.Count - 2].ToString() + " and " + list[list.Count - 1].ToString();
            }
            else if (list.Count == 2)
                ret = list[0].ToString() + " and " + list[1].ToString();
            else if (list.Count == 1)
                ret = list[0].ToString();
            else if (list.Count == 0)
                ret = "No genre listed!";

            return ret;
        }

        /// <summary>
        /// This function sets all of the labels describing the Movie
        /// </summary>
        public void SetLabels(Movie movie)
        {
            MovieTitleLabel.Text = movie.Title;
            MovieYearLabel.Text = "Year: " + movie.Year.ToString();
            MovieGenreLabel.Text = "Genre: " + ReturnGenreString(movie.Genres);
            MovieDescriptionBox.Text = movie.Description;
            MovieActorLabel.Text = "Actors: " + MovieActorLabelString(movie);
            MovieCountryLabel.Text = "Language: " + movie.Country;
            MovieDirectorLabel.Text = "Director: " + movie.Director;
            MoviePlayTimeLabel.Text = "Playtime: " + movie.PlayTime + " mins.";
            try
            {
                MoviePosterBox.Image = Base64ToImage(movie.Poster);
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            finally { }

            if (movie.LentOut == true)
            {
                labelRented.Visible = true;
                labelRented.Text = "WARNING! Lent to " + movie.LendPerson;
            }
            else
            {
                labelRented.Visible = false;
            }
        }

        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
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

        private void SetCorrectSearchBox(bool searchByGenre)
        {
            Searchbox.Enabled = !searchByGenre;
            Searchbox.Visible = !searchByGenre;
            comboBoxGenre.Enabled = searchByGenre;
            comboBoxGenre.Visible = searchByGenre;
        }

        private void LaunchImportDialogue()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Please choose a folder to load your movie and actor lists from";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                movieList = Datahandler.LoadFromFolderMovie(Path.Combine(fbd.SelectedPath, "Movies.xml"), true);
                actorList = Datahandler.LoadFromFolderActors(Path.Combine(fbd.SelectedPath, "Actors.xml"), true);
                Datahandler.SaveActors("actors.xml", actorList);

                ResetListBoxDisplays();
            }
        }

        private void ExportData()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose a location to save your list of movies";

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Datahandler.ExportMovie(Path.Combine(fbd.SelectedPath, "Movies.xml"), movieList);
                Datahandler.ExportActors(Path.Combine(fbd.SelectedPath, "Actors.xml"), actorList);
                Datahandler.SaveActors("actors.xml", actorList);
            }
            ResetLabels();

            System.Windows.Forms.MessageBox.Show("Data exported succesfully!");
        }

        #endregion


        #region Events

        /// <summary>
        /// This function describes what happens when you press the AddMovieButton
        /// </summary>
        private void AddMovieButton_Click(object sender, EventArgs e)
        {
            AddMovieForm newMovieForm = new AddMovieForm();
            if (newMovieForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                changesMade = true;
                movieList.Add(new Movie(newMovieForm.Title, newMovieForm.Year, newMovieForm.Genres, newMovieForm.Description, newMovieForm.ActorsInMovie, newMovieForm.Country, newMovieForm.Director, newMovieForm.CompendiumNumber, newMovieForm.PlayTime, ImageToBase64(newMovieForm.Poster, System.Drawing.Imaging.ImageFormat.Jpeg), newMovieForm.LentStatus, newMovieForm.LentToPerson));

                SaveLoadDataForm form = new SaveLoadDataForm(true, movieList);
                form.ShowDialog();

                ResetListBoxDisplays();
                
            }

        }

        /// <summary>
        /// This function describes what happens when you click the DeleteMovieButton
        /// </summary>
        private void DeleteMovieButton_Click(object sender, EventArgs e)
        {
            Movie toBeDeleted = new Movie();
            if (listBoxTitle.SelectedItem != null)
            {
                changesMade = true;
                toBeDeleted = (Movie)listBoxTitle.SelectedItem;
                movieList.Remove((Movie)listBoxTitle.SelectedItem);
                movieDisplayList.Remove((Movie)listBoxTitle.SelectedItem);
            }
            else
            {
                MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                noMovie.ShowDialog();
                return;
            }

            SaveLoadDataForm form = new SaveLoadDataForm(true, movieList);
            form.ShowDialog();

            ResetLabels();
        }

        /// <summary>
        /// This function describes what happens when you click an item in the main list box
        /// </summary>
        private void listBoxTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTitle.SelectedItem != null)
            {
                selectedMovie = (Movie)listBoxTitle.SelectedItem;
                SetLabels(selectedMovie);
            }
        }

        /// <summary>
        /// This function describes what happens when you click the WatchMovieButton
        /// </summary>
        private void WatchMovieButton_Click(object sender, EventArgs e)
        {
            Movie seeThisMovie = new Movie();

            seeThisMovie = (Movie)listBoxTitle.SelectedItem;

            if (seeThisMovie != null)
            {
                WatchMovie watchMovieForm = new WatchMovie(seeThisMovie.CompendiumNumber, seeThisMovie.Title);
                watchMovieForm.ShowDialog();
            }

            else
            {
                MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                noMovie.ShowDialog();
                return;
            }
        }

        /// <summary>
        /// This function describes what happens when you click the ExitButton. It exits the program
        /// </summary>
        private void ExitButton_Click(object sender, EventArgs e)
        {
            SaveLoadDataForm form = new SaveLoadDataForm(true, movieList);
            form.ShowDialog();

            if (changesMade)
            {
                var result = System.Windows.Forms.MessageBox.Show("It seems you have made some changes to your data! Would if you to export it?",
                    "Data was changed", MessageBoxButtons.YesNo);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    ExportData();
            }

            this.Close();
            
        }

        /// <summary>
        /// This function describes what happens when you click the EditButton. It lets you edit the selected Movie.
        /// </summary>
        private void EditButton_Click(object sender, EventArgs e)
        {
            Movie editedMovie = null;

            editedMovie = (Movie)listBoxTitle.SelectedItem;

            AddMovieForm editMovieForm;

            if (editedMovie != null)
                editMovieForm = new AddMovieForm(editedMovie);
            else
            {
                editMovieForm = new AddMovieForm();
                MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                noMovie.ShowDialog();
                AddMovieButton_Click(null, null);
                return;
            }

            if (editMovieForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                changesMade = true;
                editedMovie.Title = editMovieForm.Title;
                editedMovie.Year = editMovieForm.Year;
                editedMovie.Genres = editMovieForm.Genres;
                editedMovie.Description = editMovieForm.Description;
                editedMovie.ActorList.Clear();
                editedMovie.ActorList.AddRange(editMovieForm.ActorsInMovie);
                editedMovie.Country = editMovieForm.Country;
                editedMovie.Director = editMovieForm.Director;
                editedMovie.CompendiumNumber = editMovieForm.CompendiumNumber;
                editedMovie.PlayTime = editMovieForm.PlayTime;
                editedMovie.Poster = ImageToBase64(editMovieForm.Poster, System.Drawing.Imaging.ImageFormat.Jpeg);
                editedMovie.LentOut = editMovieForm.LentStatus;
                editedMovie.LendPerson = editMovieForm.LentToPerson;

                SaveLoadDataForm form = new SaveLoadDataForm(true, movieList);
                form.ShowDialog();

                ResetListBoxDisplays();
            }
        }

        private void Exportbutton_Click(object sender, EventArgs e)
        {
            ExportData();
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            changesMade = true;
            LaunchImportDialogue();
        }

        private void listBoxTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Movie seeThisMovie = new Movie();

                seeThisMovie = (Movie)listBoxTitle.SelectedItem;

                if (seeThisMovie != null)
                {
                    WatchMovie watchMovieForm = new WatchMovie(seeThisMovie.CompendiumNumber, seeThisMovie.Title);
                    watchMovieForm.ShowDialog();
                }

                else
                {
                    MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                    noMovie.ShowDialog();
                    return;
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            SearchInitiated();
        }

        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSearchBy.SelectedItem != "Genre")
                SetCorrectSearchBox(false);
            else
                SetCorrectSearchBox(true);

            Searchbox.Text = "";
            SearchInitiated();
        }

        private void Searchbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                SearchInitiated();
        }

        private void comboBoxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchInitiated();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            if (movieList == null || movieList.Count == 0)
            {
                string message = "It seems that you have no data in this catalogue! Would you like to import data?";
                var result = System.Windows.Forms.MessageBox.Show(message, "No data found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    LaunchImportDialogue();
            }
        }

        #endregion

        
    }

}
