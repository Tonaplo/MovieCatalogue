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
    public partial class GenreSelectionForm : Form
    {
        List<Core.Genre> selectedGenres = new List<Core.Genre>();

        public GenreSelectionForm()
        {
            InitializeComponent();
            listBoxGenre.DataSource = Enum.GetValues(typeof(MovieCatalogue.Core.Genre));
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listBoxGenre.SelectedItems.Count > 0)
            {
                foreach (var item in listBoxGenre.SelectedItems)
                {
                    selectedGenres.Add((Core.Genre)item);
                }

                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("You havent selected any genres!");
            }
        }

        public List<Core.Genre> SelectedGenres
        {
            get { return selectedGenres; }
        }
    }
}
