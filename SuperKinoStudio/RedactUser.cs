using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SuperKinoStudio
{
    public partial class RedactUser : Form
    {
        private Users users;
        OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public RedactUser(Users users)
        {
            InitializeComponent();
            this.users = users;
            InitializeUI();


        }
        private void InitializeUI()
        {
            textBox1.Text = users.Name;
            textBox2.Text = users.Surname;
            textBox3.Text = users.Midname;
            textBox4.Text = users.Login;
            textBox5.Text = users.Password;
            textBox6.Text = users.Email;
            textBox7.Text = users.PhoneNumber;

            RoleBox.Text = users.Role?.RoleName;
            genderBox.Text = users.Gender?.GenderName;

            numericUpDown1.Value = users.Age ?? 0;

            if (users.Image != null)
            {
                using (MemoryStream ms = new MemoryStream(users.Image))
                {
                    Image originalImage = Image.FromStream(ms);

                    if (ImageAnimator.CanAnimate(originalImage)) // Проверяем, является ли изображение GIF
                    {
                        ImageAnimator.Animate(originalImage, (sender, e) =>
                        {
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


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        

       

       

        

        private void UpdateInfo_Click(object sender, EventArgs e)
        {
            try
            {
                using (var context = new KinoStudioEntities2())
                {
                    var userToUpdate = context.Users.FirstOrDefault(u => u.UserId == users.UserId);

                    if (userToUpdate != null)
                    {
                        userToUpdate.Name = textBox1.Text;
                        userToUpdate.Surname = textBox2.Text;
                        userToUpdate.Midname = textBox3.Text;
                        userToUpdate.Login = textBox4.Text;
                        userToUpdate.Password = textBox5.Text;
                        userToUpdate.Email = textBox6.Text;
                        userToUpdate.PhoneNumber = textBox7.Text;
                        userToUpdate.RoleId = RoleBox.SelectedIndex + 1;
                        userToUpdate.RoleId = genderBox.SelectedIndex + 1;
                        userToUpdate.Age = (int)numericUpDown1.Value;


                        // Check if a new image is added
                        if (users.Image != null && users.Image.Length > 0)
                        {
                            userToUpdate.Image = users.Image; // Update the image
                        }

                        // Save changes to the database
                        context.SaveChanges();

                        MessageBox.Show("Данные о пользователе сохранены.", "Success", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных пользователя: " + ex.Message, "Error", MessageBoxButtons.OK);
            }
            this.Close();

        }
        private void DeleteUser()
        {
            try
            {
                using (var context = new KinoStudioEntities2())
                {
                    var userToDelete = context.Users.FirstOrDefault(u => u.UserId == users.UserId);

                    if (userToDelete != null)
                    {
                        context.Users.Remove(userToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Пользователь удален", "УСПЕХ", MessageBoxButtons.OK);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message, "Ошибка", MessageBoxButtons.OK);
            }
            this.Close();

        }
        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            openFileDialog1.Title = "Select Image File";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Read image to byte array
                byte[] imageBytes = File.ReadAllBytes(openFileDialog1.FileName);
                users.Image = imageBytes;

                // Отображаем изображение в PictureBox
                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));
                // Зумируем изображение
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                DeleteUser();
            }
        }
    }
}
