using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MovieCatalogue.Core;
using DeadDog.Movies;
using DeadDog.Movies.IMDB;
using DeadDog.Movies.IO;

namespace MovieCatalogue
{
    /// <summary>
/// Simple progress form.
/// </summary>
    public partial class ProgressForm : Form
    {

        BackgroundWorker worker;
        int moviecount = 5;
        int lastPercent;
        string lastStatus;
        string searchText;
        static bool erroresWhileParsing = false;
        BindingList<Movie> movieList = new BindingList<Movie>();
        List<Image> posterList = new List<Image>();
        IMDBBuffer bf = new IMDBBuffer();

        /// <summary>
        /// Constructor.
        /// </summary>
        public ProgressForm(string search)
        {
            InitializeComponent();
            this.ControlBox = false;

            this.searchText = search;
            DefaultStatusText = "Searching IMDB.com, please wait...";
            CancellingText = "Cancelling operation...";

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new System.ComponentModel.DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                worker_RunWorkerCompleted);
        }

        /// <summary>
        /// Gets the progress bar so it is possible to customize it
        /// before displaying the form.
        /// Do not use it directly from the background worker function!
        /// </summary>
        public ProgressBar ProgressBar { get { return progressBar; } }

        public List<Image> PosterList { get { return posterList; } }
        public BindingList<Movie> MovieList { get { return movieList; } }
        public bool Errores { get { return erroresWhileParsing; } }

        /// <summary>
        /// Will be passed to the background worker.
        /// </summary>
        public object Argument { get; set; }
        /// <summary>
        /// Background worker's result.
        /// You may also check ShowDialog return value
        /// to know how the background worker finished.
        /// </summary>

        public RunWorkerCompletedEventArgs Result { get; private set; }
        /// <summary>
        /// True if the user clicked the Cancel button
        /// and the background worker is still running.
        /// </summary>
        public bool CancellationPending
        {
            get { return worker.CancellationPending; }
        }

        /// <summary>
        /// Text displayed once the Cancel button is clicked.
        /// </summary>
        public string CancellingText { get; set; }
        /// <summary>
        /// Default status text.
        /// </summary>
        public string DefaultStatusText { get; set; }
        /// <summary>
        /// Delegate for the DoWork event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Contains the event data.</param>
        public delegate void DoWorkEventHandler(ProgressForm sender, DoWorkEventArgs e);
        /// <summary>
        /// Occurs when the background worker starts.
        /// </summary>
        public event DoWorkEventHandler DoWork;

        /// <summary>
        /// Changes the status text only.
        /// </summary>
        /// <param name="status">New status text.</param>
        public void SetProgress(string status)
        {
            //do not update the text if it didn't change
            //or if a cancellation request is pending
            if (status != lastStatus && !worker.CancellationPending)
            {
                lastStatus = status;
                worker.ReportProgress(progressBar.Minimum - 1, status);
            }
        }

        /// <summary>
        /// Changes the progress bar value only.
        /// </summary>
        /// <param name="percent">New value for the progress bar.</param>
        public void SetProgress(int percent)
        {
            //do not update the progress bar if the value didn't change
            if (percent != lastPercent)
            {
                lastPercent = percent;
                worker.ReportProgress(percent);
            }
        }

        /// <summary>
        /// Changes both progress bar value and status text.
        /// </summary>
        /// <param name="percent">New value for the progress bar.</param>
        /// <param name="status">New status text.</param>
        public void SetProgress(int percent, string status)
        {
            //update the form is at least one of the values need to be updated
            if (percent != lastPercent || (status != lastStatus && !worker.CancellationPending))
            {
                lastPercent = percent;
                lastStatus = status;
                worker.ReportProgress(percent, status);
            }
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            //reset to defaults just in case the user wants to reuse the form
            Result = null;
            progressBar.Value = progressBar.Minimum;
            labelTop.Text = DefaultStatusText;
            lastStatus = DefaultStatusText;
            lastPercent = progressBar.Minimum;
            //start the background worker as soon as the form is loaded
            worker.RunWorkerAsync(Argument);
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (bf.SearchTitle(searchText).Results.Count < 5)
                moviecount = bf.SearchTitle(searchText).Results.Count;

            erroresWhileParsing = false;

            foreach (var main in bf.SearchTitle(searchText).Results)
            {
                try
                {
                    var mainMovie = bf.ReadMain(main.Id);
                    if(mainMovie.MediaType == MediaType.Movie)
                        movieList.Add(AddMovieFromSearch(mainMovie, movieList.Count));
                }

                catch (Exception exp)
                {
                    if (exp.Message == "File could not be loaded after 3 attempts.")
                    {
                        MissingInfoForm missingInfo = new MissingInfoForm("It seems you are not connected to the Internet. Please check your internet connection.");
                        missingInfo.ShowDialog();
                    }
                }

                if (movieList.Count == 5) 
                    break;
            }
            SetProgress(100);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //make sure the new value is valid for the progress bar and update it
            if (e.ProgressPercentage >= progressBar.Minimum &&
                e.ProgressPercentage <= progressBar.Maximum)
            {
                progressBar.Value = e.ProgressPercentage;
            }
            //do not update the text if a cancellation request is pending
            if (e.UserState != null && !worker.CancellationPending)
                labelTop.Text = e.UserState.ToString();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //the background worker completed
            //keep the resul and close the form
            
            Result = e;
            if (e.Error != null)
                DialogResult = DialogResult.Abort;
            else if (e.Cancelled)
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.OK;
            Close();
        }

        #region Internal Functions

        private Movie AddMovieFromSearch(MainPage movie, int i)
        {
            Movie newMovie = new Movie();
            double percentOfWhole = 100*((double)i / (double)moviecount);
            double percentPermovie = 100*(1/(double)(moviecount*10));

            newMovie.Title = movie.Title.Succes ? movie.Title.Data : "Error!";
            try { newMovie.Title = movie.Title; }
            catch (Exception exp) { newMovie.Title = "Error!"; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie*1 + (int)percentOfWhole, "Adding movie " + (i+1) + " of " + moviecount + "..."); }

            try { newMovie.Year = movie.Year; }
            catch (Exception exp) { newMovie.Year = 0; }
            finally { SetProgress((int)percentPermovie * 2 + (int)percentOfWhole); }

            try { newMovie.Description = movie.Plot.ToString() + " Genre:" + movie.Genres.ToString(); }
            catch (Exception exp) { newMovie.Description = "Error!"; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 3 + (int)percentOfWhole); }

            try
            {
                newMovie.ActorList = new List<Actor>();
                for (int j = 0; j < movie.Credits.Cast.Count; j++)
                {
                    newMovie.ActorList.Add(new Actor(movie.Credits.Cast[j].Person.Name));
                }
            }
            catch (Exception exp) { erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 4 + (int)percentOfWhole); }

            newMovie.Country = "English";

            try { newMovie.Director = movie.Credits.Directors[0].Person.Name; }
            catch (Exception exp) { newMovie.Director = "Error!"; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 5 + (int)percentOfWhole); }

            try { newMovie.Director = movie.Credits.Directors[0].Person.Name; }
            catch (Exception exp) { newMovie.Director = "Error!"; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 6 + (int)percentOfWhole); }

            try { newMovie.PlayTime = movie.Runtime.Data.Minutes + (movie.Runtime.Data.Hours * 60); }
            catch (Exception exp) { newMovie.PlayTime = 0; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 7 + (int)percentOfWhole); }

            try { newMovie.PlayTime = movie.Runtime.Data.Minutes + (movie.Runtime.Data.Hours * 60); }
            catch (Exception exp) { newMovie.PlayTime = 0; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 8 + (int)percentOfWhole); }

            newMovie.Poster = "";
            try { newMovie.Poster = movie.PosterURL.ToString(); }
            catch (Exception exp) { newMovie.Poster = ""; erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 9 + (int)percentOfWhole); }

            newMovie.LentOut = false;
            newMovie.LendPerson = "";

            try
            {
                newMovie.Genres = new List<Core.Genre>();
                foreach (var item in movie.Genres.Data)
	            {
                    Core.Genre newGenre;
		            if (CheckForGenre(item.Name.ToString(), out newGenre))
                        newMovie.Genres.Add(newGenre);
	            }
            }
            catch (Exception exp) { erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 9 + (int)percentOfWhole); }

            try { posterList.Add(movie.PosterURL.Data.GetImage()); ; }
            catch (Exception exp) { erroresWhileParsing = true; }
            finally { SetProgress((int)percentPermovie * 10 + (int)percentOfWhole); }

            return newMovie;
        }

        /// <summary>
        /// Searches through all the genres, to return the correct type.
        /// </summary>
        /// <param name="genre">The genre, passed as a String, that we are looking for</param>
        /// <param name="foundGenre">The genre found, if any</param>
        /// <returns>Whether or not a genre was found</returns>
        private bool CheckForGenre(String genre, out Core.Genre foundGenre)
        {

            foundGenre = Core.Genre.Action;

            if (genre.Equals(Core.Genre.Action.ToString()))
                return true;
            else if (genre.Equals(Core.Genre.Adventure.ToString()))
            {
                foundGenre = Core.Genre.Adventure;
                return true;
            }
            else if (genre.Equals(Core.Genre.Animation.ToString()))
            {
                foundGenre = Core.Genre.Animation;
                return true;
            }
            else if (genre.Equals(Core.Genre.Biography.ToString()))
            {
                foundGenre = Core.Genre.Biography;
                return true;
            }
            else if (genre.Equals(Core.Genre.Comedy.ToString()))
            {
                foundGenre = Core.Genre.Comedy;
                return true;
            }
            else if (genre.Equals(Core.Genre.Crime.ToString()))
            {
                foundGenre = Core.Genre.Crime;
                return true;
            }
            else if (genre.Equals(Core.Genre.Documentary.ToString()))
            {
                foundGenre = Core.Genre.Documentary;
                return true;
            }
            else if (genre.Equals(Core.Genre.Drama.ToString()))
            {
                foundGenre = Core.Genre.Drama;
                return true;
            }
            else if (genre.Equals(Core.Genre.Family.ToString()))
            {
                foundGenre = Core.Genre.Family;
                return true;
            }
            else if (genre.Equals(Core.Genre.Fantasy.ToString()))
            {
                foundGenre = Core.Genre.Fantasy;
                return true;
            }
            else if (genre.Equals(Core.Genre.FilmNoir.ToString()))
            {
                foundGenre = Core.Genre.FilmNoir;
                return true;
            }
            else if (genre.Equals(Core.Genre.History.ToString()))
            {
                foundGenre = Core.Genre.History;
                return true;
            }
            else if (genre.Equals(Core.Genre.Horror.ToString()))
            {
                foundGenre = Core.Genre.Horror;
                return true;
            }
            else if (genre.Equals(Core.Genre.Music.ToString()))
            {
                foundGenre = Core.Genre.Music;
                return true;
            }
            else if (genre.Equals(Core.Genre.Musical.ToString()))
            {
                foundGenre = Core.Genre.Musical;
                return true;
            }
            else if (genre.Equals(Core.Genre.Mystery.ToString()))
            {
                foundGenre = Core.Genre.Mystery;
                return true;
            }
            else if (genre.Equals(Core.Genre.Romance.ToString()))
            {
                foundGenre = Core.Genre.Romance;
                return true;
            }
            else if (genre.Equals(Core.Genre.ScienceFiction.ToString()))
            {
                foundGenre = Core.Genre.ScienceFiction;
                return true;
            }
            else if (genre.Equals(Core.Genre.Sport.ToString()))
            {
                foundGenre = Core.Genre.Sport;
                return true;
            }
            else if (genre.Equals(Core.Genre.Thriller.ToString()))
            {
                foundGenre = Core.Genre.Thriller;
                return true;
            }
            else if (genre.Equals(Core.Genre.War.ToString()))
            {
                foundGenre = Core.Genre.War;
                return true;
            }
            else if (genre.Equals(Core.Genre.Western.ToString()))
            {
                foundGenre = Core.Genre.Western;
                return true;
            }

            //No genre found to match - This should happen!
            System.Windows.Forms.MessageBox.Show("A Genre wasn't identified while searching! Something went wrong!");
            return false;
        }
        #endregion
    }
}
