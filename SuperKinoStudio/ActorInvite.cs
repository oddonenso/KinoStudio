using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperKinoStudio
{
    public partial class ActorInvite : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Actors actors = new Actors();
        OpenFileDialog openFileDialog = new OpenFileDialog();   
        public ActorInvite()
        {
            InitializeComponent();
            LoadComboBoxFilm();
            LoadComboBoxStudio();
            LoadComboboxArea();
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBox2_SelectedIndexChanged;
        }

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedStudio = comboBox2.Text;

            int studioid = GetNameStudio(selectedStudio);

            var areas = entities.Area.Where(a => a.StudioId == studioid).ToList();

            comboBox3.DisplayMember = "AreaName";
            comboBox3.DataSource = areas;
        }
        private void LoadComboboxArea()
        {
            comboBox3.DisplayMember = "AreaName";
            comboBox3.DataSource = entities.Area.ToList();
        }

        private int GetNameFilm(string namefilm)
        {
            var film = entities.Movie.FirstOrDefault(m => m.MovieName == namefilm);
            if (film != null)
            {
                return film.MovieId;
            }
            else
            {
                return 0;
            }
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilm = comboBox1.Text;
            int filmId = GetNameFilm(selectedFilm);

            var movie = entities.Movie.FirstOrDefault(m => m.MovieId == filmId);

            if (movie != null)
            {
                var studioName = movie.Studios?.NameStudio; 
                var areaName = movie.Area?.AreaName;

                // Setting selected studio and area if available
                comboBox2.Text = studioName;
                comboBox3.Text = areaName;

                //  Если выбранный фильм связан со студией, обновите поле со списком областей соответствующим образом
                if (!string.IsNullOrEmpty(studioName))
                {
                    int studioId = GetNameStudio(studioName);
                    var areas = entities.Area.Where(a => a.StudioId == studioId).ToList();
                    comboBox3.DisplayMember = "AreaName";
                    comboBox3.DataSource = areas;
                }
            }
        }
        private void LoadComboBoxFilm()
        {
            comboBox1.DisplayMember = "MovieName";
            comboBox1.DataSource = entities.Movie.ToList();
        }

        private void LoadComboBoxStudio()
        {
            comboBox2.DisplayMember = "NameStudio";
            comboBox2.DataSource = entities.Studios.ToList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string Namemovie = comboBox1.Text;
            string Namestudio = comboBox2.Text;
            string Namearea = comboBox3.Text;
            string Nameactor = textBox1.Text;
            string Namesurname = textBox2.Text;
            string Namemidname = textBox3.Text;
            int age = (int)numericUpDown1.Value;
            int salary = (int)numericUpDown2.Value;

            var movieId = GetNameFilm(Namemovie);
            if (movieId == 0)
            {
                MessageBox.Show($"Фильм {Namemovie} не найден");
                return; // При необходимости выполнения дополнительных действий можно использовать break; или другую логику
            }


            var studioId = GetNameStudio(Namestudio);
            if (studioId==0)
            {
                MessageBox.Show($"Студия {Namestudio} не найдена");
                return;
            }

            var areaId = GetNameArea(Namearea);
            if (areaId==0)
            {
                MessageBox.Show($"Площадка {Namearea} не найдена");
            }

            var existingActor = entities.Actors.FirstOrDefault(a => a.NameActor == Nameactor && a.StudioId == studioId && a.MovieId == movieId && a.AreaId == areaId);
            if (existingActor != null)
            {
                MessageBox.Show("Актер уже снимается на этой киностудии/площадке и в фильме");
                return;
            }

            Actors actors1 = new Actors
            {
                NameActor = Nameactor,
                SurnameActor = Namesurname,
                MidnameActor = Namemidname,
                StudioId = studioId,
                MovieId = movieId,
                AreaId = areaId,
                Age = age, // присваиваем значение age
                SalaryActor = salary, // присваиваем значение salary
                Image = pictureBox1.Image != null ? ImageToByteArray(pictureBox1.Image) : null // присваиваем изображение
            };
            entities.Actors.Add(actors1);
            MessageBox.Show("Актер добавлен");
            entities.SaveChanges();
            ClearFields();
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
            pictureBox1.Image = null;
            numericUpDown2.Value = 0;
        }
        private int GetNameStudio(string nameStudio)
        {
            var studio = entities.Studios.FirstOrDefault(s => s.NameStudio == nameStudio);
            if (studio!=null)
            {
                return studio.StudioId;
            }
            else
            {
                throw new InvalidOperationException($"Студии {nameStudio} не сущесвтует");
            }
        }
        private int GetNameArea(string nameArea)
        {
            var areas = entities.Area.FirstOrDefault(a => a.AreaName == nameArea);

            if (areas!=null)
            {
                return areas.AreaId;
            }
            else
            {
                throw new InvalidOperationException($"Площадки {nameArea} не существует");
            }
        }
        public byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                actors.Image = imageBytes;

                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
