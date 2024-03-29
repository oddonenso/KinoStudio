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

namespace SuperKinoStudio
{
    public partial class Autho : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Users users = new Users();
        public Autho()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Registration().ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            var user = entities.Users.FirstOrDefault(u => u.Login == username);

            if (user != null)
            {
                if (user.Password == password)
                {
                    if (user.RoleId == 1)
                    {
                        new Client(user.UserId).Show();
                    }
                    else if (user.RoleId == 2)
                    {
                        new Employee(user.UserId).Show();
                    }
                    else if (user.RoleId == 3)
                    {
                        new President(user.UserId).Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неправильный пароль");
                }
            }
            else
            {
                MessageBox.Show("Пользователь с указанным логином не найден");
            }

        }

        private void Autho_Load(object sender, EventArgs e)
        {

        }

       
    }
}
