using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using MovieCatalogue.Core;
using MovieCatalogue;

namespace MovieCatalogue.Core
{
    public class Movie
    {
        public Movie() { }


        public Movie(string title, int year, Genre genre, string description, BindingList<Actor> actorList, string country, string director, string compendiumNumber, int playTime, string poster, bool lent, string lendPerson)
            :this()
        {
            this._title = title;
            this._year = year;
            this._genre = genre;
            this._description = description;
            this._actorList.AddRange(actorList);
            this._country = country;
            this._director = director;
            this._compendiumNumber = compendiumNumber;
            this._playTime = playTime;
            this._poster = poster;
            this._lentOut = lent;
            this._lendPerson = lendPerson;

        }

        internal string _title;
        internal int _year;
        internal Genre _genre;
        internal string _description;
        internal List<Actor> _actorList = new List<Actor>();
        internal string _country;
        internal string _director;
        internal string _compendiumNumber;
        internal int _playTime;
        internal string _poster;
        internal bool _lentOut;
        internal string _lendPerson;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public Genre Genre
        {
            get { return _genre; }
            set { _genre = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public List<Actor> ActorList
        {
            get { return _actorList; }
            set { _actorList = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string Director
        {
            get { return _director; }
            set { _director = value; }
        }

        public string CompendiumNumber
        {
            get { return _compendiumNumber; }
            set { _compendiumNumber = value; }
        }

        public int PlayTime
        {
            get { return _playTime; }
            set { _playTime = value; }
        }

        public string Poster
        {
            get { return _poster; }
            set { _poster = value; }
        }

        public bool LentOut
        {
            get { return _lentOut; }
            set { _lentOut = value; }
        }

        public string LendPerson
        {
            get { return _lendPerson; }
            set { _lendPerson = value; }
        }

        public string DisplayTitle
        {
            get { return string.Format(Title); }
        }

        public string DisplayGenre
        {
            get { return string.Format("{0}: {1}", Genre, Title); }
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
