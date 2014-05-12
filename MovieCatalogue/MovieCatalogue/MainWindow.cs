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
        BindingList<string> movieDisplayList = new BindingList<string>();
        BindingList<string> movieRentedList = new BindingList<string>();
        BindingList<Actor> actorList = Datahandler.LoadActors("actors.xml");
        private static ToolTip tt = new ToolTip();

        public MainWindow()
        {
            InitializeComponent();

            if (movieList == null)
                movieList = new BindingList<Movie>();

            for (int i = 0; i < movieList.Count; i++)
                movieDisplayList.Add(movieList[i].Title);

            foreach (var item in movieList)
            {
                if (item._lentOut == true)
                    movieRentedList.Add(item.Title);
            }

            listBoxTitle.DataSource = movieDisplayList;
            listBoxTitle.DisplayMember = "DisplayTitle";

            listBoxGenre.DataSource = movieDisplayList;
            listBoxGenre.DisplayMember = "DisplayGenre";
            listBoxGenre.ClearSelected();

            listBoxActor.DataSource = movieDisplayList;
            listBoxActor.DisplayMember = "DisplayTitle";
            listBoxActor.ClearSelected();

            listBoxRented.DataSource = movieRentedList;
            listBoxRented.DisplayMember = "DisplayTitle";
            listBoxRented.ClearSelected();

            ResetLabels();
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
                movieDisplayList.Clear();
                movieRentedList.Clear();

                if (Searchbox.Text.Length == 0)
                {
                    foreach (var item in movieList)
                    {
                        movieDisplayList.Add(item.Title);
                    }

                    foreach (var item in movieList)
                    {
                        if (item._lentOut == true)
                            movieRentedList.Add(item.Title);
                    }
                }

                if (Searchbox.Text.Length > 0)
                {
                    for (int i = 0; i < movieList.Count; i++)
                    {
                        if (Core.Search.MainSearch(Searchbox.Text, movieList[i], MainTabPane.SelectedTab.Text))
                        {
                            movieDisplayList.Add(movieList[i].Title);
                        }
                    }

                    foreach (var item in movieList)
                    {
                        if (item._lentOut == true)
                            movieRentedList.Add(item.Title);
                    }
                }

                switch (MainTabPane.SelectedIndex)
                {
                    case 0:
                        listBoxTitle.DataSource = null;
                        //listBoxTitle.SuspendLayout();
                        //listBoxTitle.ResumeLayout();
                        listBoxTitle.DataSource = movieDisplayList;
                        listBoxTitle.DisplayMember = "DisplayTitle";
                        break;
                    case 1:
                        listBoxGenre.DataSource = null;
                        //listBoxGenre.SuspendLayout();
                        //listBoxGenre.ResumeLayout();
                        listBoxGenre.DataSource = movieDisplayList;
                        listBoxGenre.DisplayMember = "DisplayGenre";
                        break;
                    case 2:
                        listBoxActor.DataSource = null;
                        //listBoxActor.SuspendLayout();
                        //listBoxActor.ResumeLayout();
                        listBoxActor.DataSource = movieDisplayList;
                        listBoxActor.DisplayMember = "DisplayTitle";
                        break;
                    case 3:
                        listBoxRented.DataSource = null;
                        //listBoxRented.SuspendLayout();
                        //listBoxRented.ResumeLayout();
                        listBoxRented.DataSource = movieRentedList;
                        listBoxRented.DisplayMember = "DisplayTitle";
                        break;
                    default:
                        break;
                }
                
                this.ResetLabels();
            }
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

            listBoxGenre.ClearSelected();
            listBoxTitle.ClearSelected();
            listBoxActor.ClearSelected();
            listBoxRented.ClearSelected();
        }

        /// <summary>
        /// This function sets all of the labels describing the Movie
        /// </summary>
        public void SetLabels(Movie movie)
        {
            MovieTitleLabel.Text = movie.Title;
            MovieYearLabel.Text = "Year: " + movie.Year.ToString();
            MovieGenreLabel.Text = "Genre: " + movie.Genre;
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

                movieList.Add(new Movie(newMovieForm.Title, newMovieForm.Year, newMovieForm.Genre, newMovieForm.Description, newMovieForm.ActorsInMovie, newMovieForm.Country, newMovieForm.Director, newMovieForm.CompendiumNumber, newMovieForm.PlayTime, ImageToBase64(newMovieForm.Poster, System.Drawing.Imaging.ImageFormat.Jpeg), newMovieForm.LentStatus, newMovieForm.LentToPerson));
                Datahandler.SaveMovie("movie.xml", movieList);

                listBoxTitle.DataSource = null;
                listBoxTitle.SuspendLayout();

                listBoxGenre.DataSource = null;
                listBoxGenre.SuspendLayout();

                listBoxActor.DataSource = null;
                listBoxActor.SuspendLayout();

                listBoxRented.DataSource = null;
                listBoxRented.SuspendLayout();

                movieDisplayList.Clear();
                movieRentedList.Clear();

                for (int i = 0; i < movieList.Count; i++)
                    movieDisplayList.Add(movieList[i].Title);

                foreach (var item in movieList)
                {
                    if (item._lentOut == true)
                        movieRentedList.Add(item.Title);
                }

                listBoxTitle.ResumeLayout();
                listBoxGenre.ResumeLayout();
                listBoxActor.ResumeLayout();
                listBoxRented.ResumeLayout();

                listBoxTitle.DataSource = movieDisplayList;
                listBoxTitle.DisplayMember = "DisplayTitle";

                listBoxGenre.DataSource = movieDisplayList;
                listBoxGenre.DisplayMember = "DisplayGenre";

                listBoxActor.DataSource = movieDisplayList;
                listBoxActor.DisplayMember = "DisplayTitle";

                listBoxRented.DataSource = movieRentedList;
                listBoxRented.DisplayMember = "DisplayTitle";
            }

            this.ResetLabels();
        }

        /// <summary>
        /// This function describes what happens when you click the DeleteMovieButton
        /// </summary>
        private void DeleteMovieButton_Click(object sender, EventArgs e)
        {
            string title;
            Movie toBeDeleted = new Movie();
            if (MainTabPane.SelectedIndex == 0 && listBoxTitle.SelectedItem != null)
            {
                title = listBoxTitle.SelectedItem.ToString();
                toBeDeleted = movieList.First(x => x.Title == title);
                movieList.Remove(toBeDeleted);
                movieDisplayList.Remove(listBoxTitle.SelectedItem.ToString());
            }

            else if (MainTabPane.SelectedIndex == 1 && listBoxGenre.SelectedItem != null)
            {
                title = listBoxGenre.SelectedItem.ToString();
                toBeDeleted = movieList.First(x => x.Title == title);
                movieList.Remove(toBeDeleted);
                movieDisplayList.Remove(listBoxGenre.SelectedItem.ToString());
            }

            else if (MainTabPane.SelectedIndex == 2 && listBoxActor.SelectedItem != null)
            {
                title = listBoxActor.SelectedItem.ToString();
                toBeDeleted = movieList.First(x => x.Title == title);
                movieList.Remove(toBeDeleted);
                movieDisplayList.Remove(listBoxActor.SelectedItem.ToString());
            }

            else if (MainTabPane.SelectedIndex == 3 && listBoxRented.SelectedItem != null)
            {
                title = listBoxRented.SelectedItem.ToString();
                toBeDeleted = movieList.First(x => x.Title == title);
                movieList.Remove(toBeDeleted);
                movieDisplayList.Remove(listBoxRented.SelectedItem.ToString());
            }
            else
            {
                MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                noMovie.ShowDialog();
                return;
            }

            if (toBeDeleted != null && toBeDeleted._lentOut == true)
            {
                movieRentedList.Remove(toBeDeleted.Title);
            }

            Datahandler.SaveMovie("movie.xml", movieList);

            ResetLabels();
        }

        /// <summary>
        /// This function describes what happens when you click an item in the main list box
        /// </summary>
        private void listBoxTitle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxTitle.SelectedItem != null)
            {
                string title = listBoxTitle.SelectedItem.ToString();
                selectedMovie = movieList.First(x => x.Title == title);
                SetLabels(selectedMovie);
            }
        }

        /// <summary>
        /// This function describes what happens when you click an item in the main list box
        /// </summary>
        private void listBoxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxGenre.SelectedItem != null)
            {
                string title = listBoxGenre.SelectedItem.ToString();
                selectedMovie = movieList.First(x => x.Title == title);
                SetLabels(selectedMovie);
            }
        }

        /// <summary>
        /// This function describes what happens when you click an item in the main list box
        /// </summary>
        private void listBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxActor.SelectedItem != null)
            {
                string title = listBoxActor.SelectedItem.ToString();
                selectedMovie = movieList.First(x => x.Title == title);
                SetLabels(selectedMovie);
            }
        }

        private void listBoxRented_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRented.SelectedItem != null)
            {
                string title = listBoxRented.SelectedItem.ToString();
                selectedMovie = movieList.First(x => x.Title == title);
                SetLabels(selectedMovie);
            }
        }

        /// <summary>
        /// This function sorts the Movie according to the text entered in the search box
        /// </summary>
        private void Searchbox_TextChanged(object sender, EventArgs e)
        {
            ResetListBoxDisplays();
        }

        /// <summary>
        /// This function describes what happens when you click the WatchMovieButton
        /// </summary>
        private void WatchMovieButton_Click(object sender, EventArgs e)
        {
            Movie seeThisMovie = new Movie();
            string title;

            if (MainTabPane.SelectedIndex == 0)
            {
                title = listBoxTitle.SelectedItem.ToString();
                seeThisMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 1)
            {
                title = listBoxGenre.SelectedItem.ToString();
                seeThisMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 2)
            {
                title = listBoxActor.SelectedItem.ToString();
                seeThisMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 3)
            {
                title = listBoxRented.SelectedItem.ToString();
                seeThisMovie = movieList.First(x => x.Title == title);
            }

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
            Datahandler.SaveMovie("movie.xml", movieList);
            this.Close();
        }

        /// <summary>
        /// This function describes what happens when you click the EditButton. It lets you edit the selected Movie.
        /// </summary>
        private void EditButton_Click(object sender, EventArgs e)
        {
            Movie editedMovie = null;
            string title;
            

            if (MainTabPane.SelectedIndex == 0)
            {
                title = listBoxTitle.SelectedItem.ToString();
                editedMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 1)
            {
                title = listBoxGenre.SelectedItem.ToString();
                editedMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 2)
            {
                title = listBoxGenre.SelectedItem.ToString();
                editedMovie = movieList.First(x => x.Title == title);
            }

            else if (MainTabPane.SelectedIndex == 3)
            {
                title = listBoxRented.SelectedItem.ToString();
                editedMovie = movieList.First(x => x.Title == title);
            }

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
                editedMovie.Title = editMovieForm.Title;
                editedMovie.Year = editMovieForm.Year;
                editedMovie.Genre = editMovieForm.Genre;
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

                Datahandler.SaveMovie("movie.xml", movieList);

                ResetListBoxDisplays();
            }

            this.ResetLabels();
        }

        private void MainTabPane_MouseClick(object sender, MouseEventArgs e)
        {
            ResetLabels();
            listBoxTitle.ClearSelected();
            listBoxActor.ClearSelected();
            listBoxGenre.ClearSelected();
            listBoxRented.ClearSelected();
            ResetListBoxDisplays();
        }

        private void Exportbutton_Click(object sender, EventArgs e)
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

            MissingInfoForm mif = new MissingInfoForm("Data exported succesfully!");
            mif.ShowDialog();

        }

        private void ImportButton_Click(object sender, EventArgs e)
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

        private void listBoxTitle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Movie seeThisMovie = new Movie();

                if (MainTabPane.SelectedIndex == 0)
                    seeThisMovie = (Movie)listBoxTitle.SelectedItem;

                else if (MainTabPane.SelectedIndex == 1)
                    seeThisMovie = (Movie)listBoxGenre.SelectedItem;

                else if (MainTabPane.SelectedIndex == 2)
                    seeThisMovie = (Movie)listBoxActor.SelectedItem;

                else if (MainTabPane.SelectedIndex == 3)
                    seeThisMovie = (Movie)listBoxRented.SelectedItem;

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
        #endregion

    }

}
