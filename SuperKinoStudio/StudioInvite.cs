using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SuperKinoStudio
{
    public partial class StudioInvite : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Studios studios = new Studios();   
        OpenFileDialog openFileDialog = new OpenFileDialog();
        public StudioInvite()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            openFileDialog.Title = "Select Image File";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                studios.Image = imageBytes;

                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NameStudio = textBox1.Text;
            DateTime date = dateTimePicker1.Value;
            string address = textBox3.Text;
            decimal salaryDecimal = numericUpDown2.Value; // Получаем значение как decimal
            int salary = (int)salaryDecimal; // Преобразуем decimal в int

            studios.NameStudio = NameStudio;
            studios.FoundingDate = date;
            studios.location = address;
            studios.AnnualIncome = salary;

            entities.Studios.Add(studios);

            MessageBox.Show("Студия Добавлена!");

            entities.SaveChanges();
            ClearFields();
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            numericUpDown2.Value = 0;
            pictureBox1.Image = null;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void StudioInvite_Load(object sender, EventArgs e)
        {

        }
    }
}
