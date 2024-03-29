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
    public partial class RedactActor : Form
    {
        private Actors actor;

        public RedactActor(Actors actor)
        {
            InitializeComponent();
            this.actor = actor;
            textBox1.Text = actor.NameActor;
            textBox2.Text = actor.SurnameActor;
            textBox3.Text = actor.MidnameActor;

            // Устанавливаем значения для ComboBox
            comboBox1.Text = actor.Movie?.MovieName;
            comboBox2.Text = actor.Studios?.NameStudio;
            comboBox3.Text = actor.Area?.AreaName;

            // Устанавливаем значения для NumericUpDown
            numericUpDown1.Value = actor.Age ?? 0;
            numericUpDown2.Value = actor.SalaryActor ?? 0;

            // Устанавливаем изображение в PictureBox
            if (actor.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(actor.Image))
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RedactActor_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
