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
    public partial class RedactMovie : Form
    {
        private Movie movie;
        public RedactMovie(Movie movie)
        {
            InitializeComponent();
            this.movie = movie;
            textBox1.Text = movie.MovieName;

            comboBox2.Text = movie.Studios?.NameStudio;
            comboBox3.Text = movie.Area?.AreaName;

            numericUpDown2.Value = movie.budgetFilm ?? 0;

            // Устанавливаем изображение в PictureBox
            if (movie.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(movie.Image))
                {
                    Image originalImage = Image.FromStream(ms);

                    if (ImageAnimator.CanAnimate(originalImage)) // Проверяем, является ли изображение GIF
                    {
                        ImageAnimator.Animate(originalImage, (sender, e) => {
                            pictureBox1.Invalidate();
                        });
                    }

                    // Зумируем изображение
                    Bitmap zoomedImage = new Bitmap(originalImage.Width * 2, originalImage.Height * 2);
                    Graphics g = Graphics.FromImage(zoomedImage);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(originalImage, 0, 0, originalImage.Width * 2, originalImage.Height * 2);
                    g.Dispose();

                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom; // Устанавливаем режим масштабирования
                    pictureBox1.Image = zoomedImage;
                    pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // Устанавливаем якоря
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void RedactMovie_Load(object sender, EventArgs e)
        {

        }
    }
}
