﻿using System;
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
        BindingList<Movie> movieRentedList = new BindingList<Movie>();
        BindingList<Actor> actorList = Datahandler.LoadActors("actors.xml");
        private static ToolTip tt = new ToolTip();

        public MainWindow()
        {
            InitializeComponent();

            if (movieList == null)
                movieList = new BindingList<Movie>();

            for (int i = 0; i < movieList.Count; i++)
                movieDisplayList.Add(movieList[i]);

            foreach (var item in movieList)
            {
                if (item._lentOut == true)
                    movieRentedList.Add(item);
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

                foreach (var item in movieList)
                {
                    movieDisplayList.Add(item);
                }
                        
                foreach (var item in movieDisplayList)
                {
                    if (item._lentOut == true)
                        movieRentedList.Add(item);
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

        public void SearchInitiated()
        {
            List<int> tobeRemoved = new List<int>();
            for (int i = 0; i < movieDisplayList.Count; i++)
            {
                if (!Core.Search.MainSearch(Searchbox.Text, movieDisplayList[i], MainTabPane.SelectedTab.Text))
                {
                    tobeRemoved.Add(i);
                }
            }

            var li = tobeRemoved.OrderByDescending(x => x);

            foreach (var item in li)
            {
                movieDisplayList.RemoveAt(item);
            }
            this.ResetLabels();
        }

        public ListBox returnFocusedListBox(int pane)
        {
            switch (MainTabPane.SelectedIndex)
            {
                case 0:
                    return listBoxTitle;
                case 1:
                    return listBoxGenre;
                case 2:
                    return listBoxActor;
                case 3:
                    return listBoxRented;
                default:
                    return null;
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
                    movieDisplayList.Add(movieList[i]);

                foreach (var item in movieList)
                {
                    if (item._lentOut == true)
                        movieRentedList.Add(item);
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
            Movie toBeDeleted = new Movie();
            if (MainTabPane.SelectedIndex == 0 && listBoxTitle.SelectedItem != null)
            {
                toBeDeleted = (Movie)listBoxTitle.SelectedItem;
                movieList.Remove((Movie)listBoxTitle.SelectedItem);
                movieDisplayList.Remove((Movie)listBoxTitle.SelectedItem);
            }

            else if (MainTabPane.SelectedIndex == 1 && listBoxGenre.SelectedItem != null)
            {
                toBeDeleted = (Movie)listBoxGenre.SelectedItem;
                movieList.Remove((Movie)listBoxGenre.SelectedItem);
                movieDisplayList.Remove((Movie)listBoxGenre.SelectedItem);
            }

            else if (MainTabPane.SelectedIndex == 2 && listBoxActor.SelectedItem != null)
            {
                toBeDeleted = (Movie)listBoxActor.SelectedItem;
                movieList.Remove((Movie)listBoxActor.SelectedItem);
                movieDisplayList.Remove((Movie)listBoxActor.SelectedItem);
            }

            else if (MainTabPane.SelectedIndex == 3 && listBoxRented.SelectedItem != null)
            {
                toBeDeleted = (Movie)listBoxRented.SelectedItem;
                movieList.Remove((Movie)listBoxRented.SelectedItem);
                movieDisplayList.Remove((Movie)listBoxRented.SelectedItem);
            }
            else
            {
                MissingInfoForm noMovie = new MissingInfoForm("You have not selected a movie!");
                noMovie.ShowDialog();
                return;
            }

            if (toBeDeleted != null && toBeDeleted._lentOut == true)
            {
                movieRentedList.Remove(toBeDeleted);
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
                selectedMovie = (Movie)listBoxTitle.SelectedItem;
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
                selectedMovie = (Movie)listBoxGenre.SelectedItem;
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
                selectedMovie = (Movie)listBoxActor.SelectedItem;
                SetLabels(selectedMovie);
            }
        }

        private void listBoxRented_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxRented.SelectedItem != null)
            {
                selectedMovie = (Movie)listBoxRented.SelectedItem;
                SetLabels(selectedMovie);
            }
        }

        /// <summary>
        /// This function sorts the Movie according to the text entered in the search box
        /// </summary>
        private void Searchbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                Searchbox.Clear();
            }
            else
            {
                SearchInitiated();
            }
        }

        /// <summary>
        /// This function describes what happens when you click the WatchMovieButton
        /// </summary>
        private void WatchMovieButton_Click(object sender, EventArgs e)
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

            if (MainTabPane.SelectedIndex == 0)
                editedMovie = (Movie)listBoxTitle.SelectedItem;

            else if (MainTabPane.SelectedIndex == 1)
                editedMovie = (Movie)listBoxGenre.SelectedItem;

            else if (MainTabPane.SelectedIndex == 2)
                editedMovie = (Movie)listBoxActor.SelectedItem;

            else if (MainTabPane.SelectedIndex == 3)
                editedMovie = (Movie)listBoxRented.SelectedItem;

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

        private void Searchbox_TextChanged(object sender, EventArgs e)
        {
            if (Searchbox.Text.Length == 0)
            {
                ResetListBoxDisplays();
            }
        }
        #endregion

        
    }

}
