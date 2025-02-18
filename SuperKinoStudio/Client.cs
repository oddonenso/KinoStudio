﻿using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperKinoStudio
{
    public partial class Client : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Users users = new Users();
        public Client(int userId)
        {
            InitializeComponent();
            LoadUser(userId);

        }
        private void LoadUser(int userId)
        {
            users = entities.Users.FirstOrDefault(x => x.UserId == userId);
            if (users != null)
            {
                label1.Text = users.Name;
                label2.Text = users.Surname;
                label3.Text = users.Midname;
                label4.Text = users.Role.RoleName;
                label5.Text = users.Email;
                label6.Text = users.PhoneNumber;

                //загрузка фото

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
            else
            {
                MessageBox.Show("Пользователь не найден");
            }


        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Check_Click(object sender, EventArgs e)
        {
            new Allinformation().Show();
        }
    }
}
