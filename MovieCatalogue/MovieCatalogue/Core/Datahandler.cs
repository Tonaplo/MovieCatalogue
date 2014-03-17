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
    class Datahandler
    {
        #region Save and Load Movies

        public static void SaveMovie(string path, BindingList<Movie> movieList)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (!fi.Directory.Exists) fi.Directory.Create();

            if (fi.Exists)
                fi.Delete();
            try
            {
                StreamWriter w = new StreamWriter(path);

                XmlSerializer xmlserializer = new XmlSerializer(typeof(BindingList<Movie>));
                xmlserializer.Serialize(w, movieList); 
                
                w.Close(); 
            }
            catch (Exception exp) 
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            finally {}
        }

        public static BindingList<Movie> LoadMovie(string filename)
        {
            string path = filename;

            BindingList<Movie> t = null;

            if (System.IO.File.Exists(path))
            {
                TextReader textReader;
                XmlSerializer deserializer = new XmlSerializer(typeof(BindingList<Movie>));
                textReader = new StreamReader(path);

                try
                {
                    t = (BindingList<Movie>)deserializer.Deserialize(textReader);
                }
                catch (Exception exp)
                {
                   MissingInfoForm missingInfo = new MissingInfoForm(exp.Message);
                   missingInfo.ShowDialog();
                   textReader.Close();
                }
                finally
                {
                    textReader.Close();
                }
                return t;
            }
            else
                return new BindingList<Movie>();

        }
        #endregion

        #region Save and Load Actors

        public static void SaveActors(string path, BindingList<Actor> actorList)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(path);
            if (!fi.Directory.Exists) fi.Directory.Create();

            if (fi.Exists)
                fi.Delete();

            StreamWriter w = new StreamWriter(path);

            XmlSerializer xmlserializer = new XmlSerializer(typeof(BindingList<Actor>));
            xmlserializer.Serialize(w, actorList);
            w.Close();
        }

        public static BindingList<Actor> LoadActors(string filename)
        {
            string path = filename;

            BindingList<Actor> t = null;

            if (System.IO.File.Exists(path))
            {
                TextReader textReader;
                XmlSerializer deserializer = new XmlSerializer(typeof(BindingList<Actor>));
                textReader = new StreamReader(path);

                try
                {
                    t = (BindingList<Actor>)deserializer.Deserialize(textReader);
                }
                catch (Exception exp)
                {
                    MissingInfoForm missingInfo = new MissingInfoForm(exp.Message);
                    missingInfo.ShowDialog();
                    textReader.Close();
                }
                finally
                {
                    textReader.Close();
                }
                return t;
            }
            else
                return new BindingList<Actor>();

        }

        #endregion

        #region Export and Import functionality

        public static void ExportMovie(string fullFileName, BindingList<Movie> MovieList) 
        {
            
            System.IO.FileInfo fi = new System.IO.FileInfo(fullFileName);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            if (fi.Exists)
                fi.Delete();
            try
            {
                StreamWriter w = new StreamWriter(fullFileName);

                XmlSerializer xmlserializer = new XmlSerializer(typeof(BindingList<Movie>));
                xmlserializer.Serialize(w, MovieList);

                w.Close();
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + exp.InnerException.Message);
            }
            finally { }
        }

        public static void ExportActors(string fullFileNameA, BindingList<Actor> actorList)
        {

            System.IO.FileInfo fiA = new System.IO.FileInfo(fullFileNameA);
            if (!fiA.Directory.Exists) fiA.Directory.Create();

            if (fiA.Exists)
                fiA.Delete();
            try
            {
                StreamWriter w = new StreamWriter(fullFileNameA);

                XmlSerializer xmlserializer = new XmlSerializer(typeof(BindingList<Actor>));
                xmlserializer.Serialize(w, actorList);

                w.Close();
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message + exp.InnerException.Message);
            }
            finally { }
        }

        public static BindingList<Movie> LoadFromFolderMovie(string moviepath, bool showMessage)
        {
            BindingList<Movie> t = null;
            TextReader textReader = null;
            XmlSerializer deserializer = new XmlSerializer(typeof(BindingList<Movie>));

            try
            {
                textReader = new StreamReader(moviepath);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
            

            try
            {
                t = (BindingList<Movie>)deserializer.Deserialize(textReader);
                textReader.Close();
                if (showMessage)
                {
                    MissingInfoForm mif = new MissingInfoForm("Movies where imported!");
                    mif.ShowDialog();
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }

            finally
            {
            }

            if (t == null)
                return new BindingList<Movie>();
            return t;
            
            
        }

        public static BindingList<Actor> LoadFromFolderActors(string actorpath, bool showMessage)
        {
            BindingList<Actor> t = null;

            TextReader textReader;
            XmlSerializer deserializer = new XmlSerializer(typeof(BindingList<Actor>));
            

            try
            {
                textReader = new StreamReader(actorpath);
                t = (BindingList<Actor>)deserializer.Deserialize(textReader);
                textReader.Close();
                if (showMessage)
                {
                    MissingInfoForm mif = new MissingInfoForm("Actors where imported!");
                    mif.ShowDialog();
                }
            }
            catch (Exception exp)
            {
                System.Windows.Forms.MessageBox.Show(exp.Message);
            }
            finally
            {
               
            }
            if (t == null)
                return new BindingList<Actor>();
            return t;

            
        }
        #endregion

    }

    }