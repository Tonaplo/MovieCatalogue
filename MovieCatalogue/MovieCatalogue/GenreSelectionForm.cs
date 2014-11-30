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

        public GenreSelectionForm(List<Core.Genre> genres)
        {
            InitializeComponent();
            listBoxGenre.DataSource = Enum.GetValues(typeof(MovieCatalogue.Core.Genre));
            if (genres.Count == 0)
                listBoxGenre.ClearSelected();
            else
            {

                for (int i = 0; i < listBoxGenre.Items.Count; i++)
                {
                    if (genres.Contains((Core.Genre)listBoxGenre.Items[i]))
                    {
                        listBoxGenre.SetSelected(i, true);
                    }
                }
            }
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
