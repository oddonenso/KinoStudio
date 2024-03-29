using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SuperKinoStudio
{
    public partial class FilmInvite : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Movie movie = new Movie();
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public FilmInvite()
        {
            InitializeComponent();
            LoadComboboxStudio();
            LoadComboboxArea();

            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStudio = comboBox1.Text;

            int studioid = GetNameStudio(selectedStudio);

            var areas = entities.Area.Where(a => a.StudioId == studioid).ToList();

            comboBox2.DisplayMember = "AreaName";
            comboBox2.DataSource = areas;
        }

        private void LoadComboboxStudio()
        {
            comboBox1.DisplayMember = "NameStudio";
            comboBox1.DataSource = entities.Studios.ToList();
        }
        private void LoadComboboxArea()
        {
            comboBox2.DisplayMember = "AreaName";
            comboBox2.DataSource = entities.Area.ToList();
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            pictureBox1.Image = null;
            numericUpDown2.Value = 0;
        }
        private int GetNameStudio(string studioname)
        {
            var studios = entities.Studios.FirstOrDefault(r => r.NameStudio == studioname);
            if (studios != null)
            {
                return studios.StudioId;
            }
            else
            {
                throw new InvalidOperationException($"Студио {studioname} не найдена");
            }
        }
        private int GetNameArea(string namearea)
        {
            var area = entities.Area.FirstOrDefault(r=>r.AreaName== namearea);
            if (area!=null)
            {
                return area.AreaId;
            }
            else
            {
                throw new InvalidOperationException($"Площадки {namearea} не существует");
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string NameStudio = comboBox1.Text;
            string NameMovie = textBox1.Text;
            string NameArea = comboBox2.Text;
            int boudjet = (int)numericUpDown2.Value;

            var Studioid = GetNameStudio(NameStudio);
            if (Studioid==0)
            {
                MessageBox.Show($"Студия {NameStudio} не найдена");
                return;
            }

            var Areaid = GetNameArea(NameArea);
            if (Areaid==0)
            {
                MessageBox.Show($"Площадка {NameArea} не найдена");
                return;
            }

            var existingMovie = entities.Movie.FirstOrDefault(m => m.MovieName == NameMovie && m.StudioId == Studioid && m.AreaId == Areaid);
            if (existingMovie != null)
            {
                MessageBox.Show($"Фильм {NameMovie} уже был снят на этой киностудии и на этой площадке");
                return;
            }

            Movie movie1 = new Movie
            {
                StudioId = Studioid,
                MovieName = NameMovie,
                AreaId = Areaid,
                budgetFilm = boudjet,
                Image = movie.Image
            };
            entities.Movie.Add(movie1);
            MessageBox.Show("Фильм добавлен!");

            entities.SaveChanges();
            ClearFields();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                movie.Image = imageBytes;

                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
