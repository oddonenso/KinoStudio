﻿using SuperKinoStudio.Models;
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
    public partial class President : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Users users = new Users();
        public President(int userId)
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
        private void Check_Click(object sender, EventArgs e)
        {
            new AllForPresidents().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Registration().Show();
        }

        private void InviteStudio_Click(object sender, EventArgs e)
        {
            new StudioInvite().Show();

        }

        private void inviteMovie_Click(object sender, EventArgs e)
        {
            new FilmInvite().Show();

        }

        private void InviteArea_Click(object sender, EventArgs e)
        {
            new InviteArea().Show();
        }

        private void InviteActor_Click(object sender, EventArgs e)
        {
            new ActorInvite().Show();
        }
    }
}
