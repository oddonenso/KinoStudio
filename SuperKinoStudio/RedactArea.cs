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
    public partial class RedactArea : Form
    {
        private Area area;
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public RedactArea(Area area)
        {
            InitializeComponent();
            this.area = area;
            textBox1.Text = area.AreaName;
            comboBox2.Text = area?.Studios.NameStudio;

            if (area.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(area.Image))
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

        private void RedactArea_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            openFileDialog.Title = "Select Image File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                area.Image = imageBytes;

                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
