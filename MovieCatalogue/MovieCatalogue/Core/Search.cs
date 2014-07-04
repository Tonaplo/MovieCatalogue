using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace MovieCatalogue.Core
{
    class Search
    {
        public static bool MainSearch(string search, Movie movie, string tab)
        {
            int index=0;
            if (tab == "Title" && movie.Title.ToLower().Contains(search.ToLower()))
                return true;
            else if (tab == "Genre" && GenreSearch(movie._genres, search))
                return true;
            else if (tab == "Actor" && ActorSearch(search, movie._actorList))
                return true;
            else if (tab == "Rented" && movie._lentOut)
                return true;
            else if (tab == "Compendium" && movie.CompendiumNumber.compendium == int.Parse(search))
                return true;

            return false;
        }

        private static bool GenreSearch(List<Core.Genre> genres, string search)
        {
            foreach (var item in genres)
            {
                if (item.ToString().ToLower() == search.ToLower())
                    return true;
            }

            return false;
        }

        public static bool ActorSearch(string search, List<Actor> actors)
        {
            for (int i = 0; i < actors.Count; i++)
            {
                if (actors[i]._name.ToLower().Contains(search.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ActorSearch(string search, BindingList<Actor> actors, out int index)
        {
            index = -1;
            for (int i = 0; i < actors.Count; i++)
            {
                if (actors[i]._name.ToLower().Contains(search.ToLower()))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }

        public static bool ActorSearch(string search, ListBox.ObjectCollection list, out int index)
        {
            index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if ((list[i] as Actor)._name.ToString().ToLower().Contains(search.ToLower()))
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }
    }
}
