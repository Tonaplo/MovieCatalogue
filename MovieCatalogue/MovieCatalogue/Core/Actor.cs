using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieCatalogue.Core
{
    public class Actor
    {
        public Actor(){}

        public Actor(string name)
        {
            this._name = name;
        }

        internal string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string DisplayActor
        {
            get { return string.Format(Name); }
        }
    }
}
