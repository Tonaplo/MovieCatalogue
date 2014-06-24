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

namespace MovieCatalogue
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.AddMovieButton = new System.Windows.Forms.Button();
            this.Searchbox = new System.Windows.Forms.TextBox();
            this.DeleteMovieButton = new System.Windows.Forms.Button();
            this.MovieDetailsLabel = new System.Windows.Forms.Label();
            this.MovieTitleLabel = new System.Windows.Forms.Label();
            this.MovieYearLabel = new System.Windows.Forms.Label();
            this.MovieGenreLabel = new System.Windows.Forms.Label();
            this.MovieDescriptionBox = new System.Windows.Forms.TextBox();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.MovieActorLabel = new System.Windows.Forms.Label();
            this.MovieCountryLabel = new System.Windows.Forms.Label();
            this.MovieDirectorLabel = new System.Windows.Forms.Label();
            this.WatchMovieButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.MoviePlayTimeLabel = new System.Windows.Forms.Label();
            this.MoviePosterBox = new System.Windows.Forms.PictureBox();
            this.NumberoOfMoviesLabel = new System.Windows.Forms.Label();
            this.Exportbutton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.labelRented = new System.Windows.Forms.Label();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.listBoxTitle = new System.Windows.Forms.ListBox();
            this.comboBoxSearchBy = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.movieBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.comboBoxGenre = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.MoviePosterBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.movieBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // AddMovieButton
            // 
            this.AddMovieButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddMovieButton.Location = new System.Drawing.Point(430, 447);
            this.AddMovieButton.Name = "AddMovieButton";
            this.AddMovieButton.Size = new System.Drawing.Size(100, 40);
            this.AddMovieButton.TabIndex = 3;
            this.AddMovieButton.Text = "Add Movie";
            this.AddMovieButton.UseVisualStyleBackColor = true;
            this.AddMovieButton.Click += new System.EventHandler(this.AddMovieButton_Click);
            // 
            // Searchbox
            // 
            this.Searchbox.Location = new System.Drawing.Point(13, 68);
            this.Searchbox.Name = "Searchbox";
            this.Searchbox.Size = new System.Drawing.Size(400, 21);
            this.Searchbox.TabIndex = 2;
            this.Searchbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Searchbox_KeyDown);
            // 
            // DeleteMovieButton
            // 
            this.DeleteMovieButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DeleteMovieButton.Location = new System.Drawing.Point(671, 447);
            this.DeleteMovieButton.Name = "DeleteMovieButton";
            this.DeleteMovieButton.Size = new System.Drawing.Size(100, 40);
            this.DeleteMovieButton.TabIndex = 5;
            this.DeleteMovieButton.Text = "Delete Movie";
            this.DeleteMovieButton.UseVisualStyleBackColor = true;
            this.DeleteMovieButton.Click += new System.EventHandler(this.DeleteMovieButton_Click);
            // 
            // MovieDetailsLabel
            // 
            this.MovieDetailsLabel.AutoSize = true;
            this.MovieDetailsLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieDetailsLabel.Font = new System.Drawing.Font("Century", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDetailsLabel.ForeColor = System.Drawing.Color.White;
            this.MovieDetailsLabel.Location = new System.Drawing.Point(484, 9);
            this.MovieDetailsLabel.Name = "MovieDetailsLabel";
            this.MovieDetailsLabel.Size = new System.Drawing.Size(102, 16);
            this.MovieDetailsLabel.TabIndex = 7;
            this.MovieDetailsLabel.Text = "Movie Details";
            // 
            // MovieTitleLabel
            // 
            this.MovieTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieTitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieTitleLabel.Font = new System.Drawing.Font("Century", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieTitleLabel.ForeColor = System.Drawing.Color.White;
            this.MovieTitleLabel.Location = new System.Drawing.Point(167, 9);
            this.MovieTitleLabel.Name = "MovieTitleLabel";
            this.MovieTitleLabel.Size = new System.Drawing.Size(250, 36);
            this.MovieTitleLabel.TabIndex = 8;
            this.MovieTitleLabel.Text = "Welcome to the MovieCatalogue!";
            this.MovieTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MovieYearLabel
            // 
            this.MovieYearLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieYearLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieYearLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieYearLabel.ForeColor = System.Drawing.Color.White;
            this.MovieYearLabel.Location = new System.Drawing.Point(430, 56);
            this.MovieYearLabel.Name = "MovieYearLabel";
            this.MovieYearLabel.Size = new System.Drawing.Size(207, 18);
            this.MovieYearLabel.TabIndex = 9;
            this.MovieYearLabel.Text = "Year: <Year>";
            // 
            // MovieGenreLabel
            // 
            this.MovieGenreLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieGenreLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieGenreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieGenreLabel.ForeColor = System.Drawing.Color.White;
            this.MovieGenreLabel.Location = new System.Drawing.Point(430, 78);
            this.MovieGenreLabel.Name = "MovieGenreLabel";
            this.MovieGenreLabel.Size = new System.Drawing.Size(207, 18);
            this.MovieGenreLabel.TabIndex = 10;
            this.MovieGenreLabel.Text = "Genre: <Genre>";
            // 
            // MovieDescriptionBox
            // 
            this.MovieDescriptionBox.BackColor = System.Drawing.SystemColors.Control;
            this.MovieDescriptionBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieDescriptionBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDescriptionBox.Location = new System.Drawing.Point(430, 283);
            this.MovieDescriptionBox.Multiline = true;
            this.MovieDescriptionBox.Name = "MovieDescriptionBox";
            this.MovieDescriptionBox.ReadOnly = true;
            this.MovieDescriptionBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.MovieDescriptionBox.Size = new System.Drawing.Size(207, 152);
            this.MovieDescriptionBox.TabIndex = 11;
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.BackColor = System.Drawing.Color.Transparent;
            this.DescriptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DescriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DescriptionLabel.ForeColor = System.Drawing.Color.White;
            this.DescriptionLabel.Location = new System.Drawing.Point(430, 260);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(76, 18);
            this.DescriptionLabel.TabIndex = 12;
            this.DescriptionLabel.Text = "Description:";
            // 
            // MovieActorLabel
            // 
            this.MovieActorLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieActorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieActorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieActorLabel.ForeColor = System.Drawing.Color.White;
            this.MovieActorLabel.Location = new System.Drawing.Point(430, 166);
            this.MovieActorLabel.Name = "MovieActorLabel";
            this.MovieActorLabel.Size = new System.Drawing.Size(207, 90);
            this.MovieActorLabel.TabIndex = 13;
            this.MovieActorLabel.Text = "Actors: <Actors in Movie>";
            // 
            // MovieCountryLabel
            // 
            this.MovieCountryLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieCountryLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieCountryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieCountryLabel.ForeColor = System.Drawing.Color.White;
            this.MovieCountryLabel.Location = new System.Drawing.Point(430, 122);
            this.MovieCountryLabel.Name = "MovieCountryLabel";
            this.MovieCountryLabel.Size = new System.Drawing.Size(207, 18);
            this.MovieCountryLabel.TabIndex = 14;
            this.MovieCountryLabel.Text = "Language: <Language>";
            // 
            // MovieDirectorLabel
            // 
            this.MovieDirectorLabel.BackColor = System.Drawing.Color.Transparent;
            this.MovieDirectorLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MovieDirectorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MovieDirectorLabel.ForeColor = System.Drawing.Color.White;
            this.MovieDirectorLabel.Location = new System.Drawing.Point(430, 144);
            this.MovieDirectorLabel.Name = "MovieDirectorLabel";
            this.MovieDirectorLabel.Size = new System.Drawing.Size(207, 18);
            this.MovieDirectorLabel.TabIndex = 15;
            this.MovieDirectorLabel.Text = "Director: <Director>";
            // 
            // WatchMovieButton
            // 
            this.WatchMovieButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.WatchMovieButton.Font = new System.Drawing.Font("Century", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WatchMovieButton.Location = new System.Drawing.Point(13, 9);
            this.WatchMovieButton.Name = "WatchMovieButton";
            this.WatchMovieButton.Size = new System.Drawing.Size(148, 36);
            this.WatchMovieButton.TabIndex = 1;
            this.WatchMovieButton.Text = "Watch This Movie!";
            this.WatchMovieButton.UseVisualStyleBackColor = true;
            this.WatchMovieButton.Click += new System.EventHandler(this.WatchMovieButton_Click);
            // 
            // ExitButton
            // 
            this.ExitButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExitButton.Location = new System.Drawing.Point(789, 447);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(100, 40);
            this.ExitButton.TabIndex = 6;
            this.ExitButton.Text = "Exit";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EditButton.Location = new System.Drawing.Point(547, 447);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(100, 40);
            this.EditButton.TabIndex = 4;
            this.EditButton.Text = "Edit Movie";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // MoviePlayTimeLabel
            // 
            this.MoviePlayTimeLabel.BackColor = System.Drawing.Color.Transparent;
            this.MoviePlayTimeLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MoviePlayTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MoviePlayTimeLabel.ForeColor = System.Drawing.Color.White;
            this.MoviePlayTimeLabel.Location = new System.Drawing.Point(430, 100);
            this.MoviePlayTimeLabel.Name = "MoviePlayTimeLabel";
            this.MoviePlayTimeLabel.Size = new System.Drawing.Size(207, 18);
            this.MoviePlayTimeLabel.TabIndex = 19;
            this.MoviePlayTimeLabel.Text = "Play Time: <Play Time>";
            // 
            // MoviePosterBox
            // 
            this.MoviePosterBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MoviePosterBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MoviePosterBox.InitialImage = null;
            this.MoviePosterBox.Location = new System.Drawing.Point(643, 57);
            this.MoviePosterBox.Name = "MoviePosterBox";
            this.MoviePosterBox.Size = new System.Drawing.Size(246, 372);
            this.MoviePosterBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.MoviePosterBox.TabIndex = 20;
            this.MoviePosterBox.TabStop = false;
            // 
            // NumberoOfMoviesLabel
            // 
            this.NumberoOfMoviesLabel.BackColor = System.Drawing.Color.Transparent;
            this.NumberoOfMoviesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumberoOfMoviesLabel.ForeColor = System.Drawing.Color.White;
            this.NumberoOfMoviesLabel.Location = new System.Drawing.Point(12, 121);
            this.NumberoOfMoviesLabel.Name = "NumberoOfMoviesLabel";
            this.NumberoOfMoviesLabel.Size = new System.Drawing.Size(151, 19);
            this.NumberoOfMoviesLabel.TabIndex = 22;
            this.NumberoOfMoviesLabel.Text = "Number of Movies: <>";
            this.NumberoOfMoviesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Exportbutton
            // 
            this.Exportbutton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Exportbutton.Location = new System.Drawing.Point(643, 5);
            this.Exportbutton.Name = "Exportbutton";
            this.Exportbutton.Size = new System.Drawing.Size(100, 40);
            this.Exportbutton.TabIndex = 24;
            this.Exportbutton.Text = "Export Data";
            this.Exportbutton.UseVisualStyleBackColor = true;
            this.Exportbutton.Click += new System.EventHandler(this.Exportbutton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ImportButton.Location = new System.Drawing.Point(789, 5);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(100, 40);
            this.ImportButton.TabIndex = 25;
            this.ImportButton.Text = "Import Data";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // labelRented
            // 
            this.labelRented.BackColor = System.Drawing.Color.Transparent;
            this.labelRented.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelRented.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRented.ForeColor = System.Drawing.Color.Red;
            this.labelRented.Location = new System.Drawing.Point(430, 35);
            this.labelRented.Name = "labelRented";
            this.labelRented.Size = new System.Drawing.Size(207, 18);
            this.labelRented.TabIndex = 26;
            this.labelRented.Text = "WARNING! Rented to <>";
            this.labelRented.Visible = false;
            // 
            // buttonSearch
            // 
            this.buttonSearch.Location = new System.Drawing.Point(14, 95);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(149, 23);
            this.buttonSearch.TabIndex = 27;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // listBoxTitle
            // 
            this.listBoxTitle.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBoxTitle.FormattingEnabled = true;
            this.listBoxTitle.ItemHeight = 17;
            this.listBoxTitle.Location = new System.Drawing.Point(13, 151);
            this.listBoxTitle.Name = "listBoxTitle";
            this.listBoxTitle.Size = new System.Drawing.Size(400, 327);
            this.listBoxTitle.Sorted = true;
            this.listBoxTitle.TabIndex = 0;
            this.listBoxTitle.SelectedIndexChanged += new System.EventHandler(this.listBoxTitle_SelectedIndexChanged);
            this.listBoxTitle.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxTitle_KeyDown);
            // 
            // comboBoxSearchBy
            // 
            this.comboBoxSearchBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchBy.FormattingEnabled = true;
            this.comboBoxSearchBy.Location = new System.Drawing.Point(167, 117);
            this.comboBoxSearchBy.Name = "comboBoxSearchBy";
            this.comboBoxSearchBy.Size = new System.Drawing.Size(246, 23);
            this.comboBoxSearchBy.TabIndex = 28;
            this.comboBoxSearchBy.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchBy_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(169, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(244, 19);
            this.label1.TabIndex = 29;
            this.label1.Text = "Search By:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // movieBindingSource
            // 
            this.movieBindingSource.DataSource = typeof(MovieCatalogue.Core.Movie);
            // 
            // comboBoxGenre
            // 
            this.comboBoxGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGenre.Enabled = false;
            this.comboBoxGenre.FormattingEnabled = true;
            this.comboBoxGenre.Location = new System.Drawing.Point(12, 66);
            this.comboBoxGenre.Name = "comboBoxGenre";
            this.comboBoxGenre.Size = new System.Drawing.Size(401, 23);
            this.comboBoxGenre.TabIndex = 30;
            this.comboBoxGenre.Visible = false;
            this.comboBoxGenre.SelectedIndexChanged += new System.EventHandler(this.comboBoxGenre_SelectedIndexChanged);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(910, 499);
            this.Controls.Add(this.comboBoxGenre);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxSearchBy);
            this.Controls.Add(this.listBoxTitle);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.labelRented);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.Exportbutton);
            this.Controls.Add(this.NumberoOfMoviesLabel);
            this.Controls.Add(this.MoviePosterBox);
            this.Controls.Add(this.MoviePlayTimeLabel);
            this.Controls.Add(this.EditButton);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.WatchMovieButton);
            this.Controls.Add(this.MovieDirectorLabel);
            this.Controls.Add(this.MovieCountryLabel);
            this.Controls.Add(this.MovieActorLabel);
            this.Controls.Add(this.DescriptionLabel);
            this.Controls.Add(this.MovieDescriptionBox);
            this.Controls.Add(this.MovieGenreLabel);
            this.Controls.Add(this.MovieYearLabel);
            this.Controls.Add(this.MovieTitleLabel);
            this.Controls.Add(this.MovieDetailsLabel);
            this.Controls.Add(this.DeleteMovieButton);
            this.Controls.Add(this.Searchbox);
            this.Controls.Add(this.AddMovieButton);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Century", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Main Window";
            ((System.ComponentModel.ISupportInitialize)(this.MoviePosterBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.movieBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //This is not done
        

        private System.Windows.Forms.Button AddMovieButton;
        private System.Windows.Forms.TextBox Searchbox;
        private System.Windows.Forms.Button DeleteMovieButton;
        private System.Windows.Forms.BindingSource movieBindingSource;
        private Label MovieDetailsLabel;
        private Label MovieTitleLabel;
        private Label MovieYearLabel;
        private Label MovieGenreLabel;
        private TextBox MovieDescriptionBox;
        private Label DescriptionLabel;
        private Label MovieActorLabel;
        private Label MovieCountryLabel;
        private Label MovieDirectorLabel;
        private Button WatchMovieButton;
        private Button ExitButton;
        private Button EditButton;
        private Label MoviePlayTimeLabel;
        private PictureBox MoviePosterBox;
        private Label NumberoOfMoviesLabel;
        private Button Exportbutton;
        private Button ImportButton;
        private Label labelRented;
        private Button buttonSearch;
        private ListBox listBoxTitle;
        private ComboBox comboBoxSearchBy;
        private Label label1;
        private ComboBox comboBoxGenre;
    }
}

