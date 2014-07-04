using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieCatalogue.Core
{
    public class Compendium
    {
        public int compendium, spot;

        public Compendium()
        { }

        public Compendium(int _compendium, int _spot)
        {
            this.compendium = _compendium;
            this.spot = _spot;
        }
    }
}
