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
    public partial class RedactStudio : Form
    {
        private Studios studios;
        public RedactStudio(Studios studios)
        {
            InitializeComponent();
            this.studios = studios;
            textBox1.Text = studios.NameStudio;
            dateTimePicker1.Value = studios.FoundingDate.Value;
            textBox2.Text = studios.location;
            numericUpDown2.Value = studios.AnnualIncome ?? 0;

            if (studios.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(studios.Image))
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

        private void RedactStudio_Load(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
