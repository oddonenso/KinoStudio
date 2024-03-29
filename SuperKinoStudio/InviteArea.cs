using SuperKinoStudio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SuperKinoStudio
{
    public partial class InviteArea : Form
    {
        KinoStudioEntities2 entities = new KinoStudioEntities2();
        Area area = new Area(); 
        public InviteArea()
        {
            InitializeComponent();
            LoadCombobox();
        }
        private void LoadCombobox()
        {
            comboBox1.DisplayMember = "NameStudio";
            comboBox1.DataSource = entities.Studios.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string NameStudio = comboBox1.Text;
            string NameArea = textBox1.Text;

            var Studioid = GetNameStudio(NameStudio);
            if (Studioid ==0)
            {
                MessageBox.Show($"Студия {NameStudio} не найдена");
                return;
            }

            var existingArea = entities.Area.FirstOrDefault(a => a.AreaName == NameArea && a.StudioId == Studioid);

            if (existingArea != null )
            {
                MessageBox.Show("Площадка уже такая есть на этой киностудии");
                return;
            }
            Area newArea = new Area
            {
                StudioId = Studioid,
                AreaName = NameArea,
                Image = area.Image
            };

            entities.Area.Add(newArea);
            entities.SaveChanges();

            MessageBox.Show("Площадка добавлена!");
            ClearFields();
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            comboBox1.Text = "";
            pictureBox1.Image = null;
        }
        private int GetNameStudio(string studioname)
        {
            var studios = entities.Studios.FirstOrDefault(r => r.NameStudio == studioname);
            if ( studios != null )
            {
                return studios.StudioId;
            }
            else
            {
                throw new InvalidOperationException($"Студия {studioname} не найдена ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.gif,*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";

            if (openFileDialog.ShowDialog()==DialogResult.OK)
            {
                byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                area.Image = imageBytes;

                pictureBox1.Image = Image.FromStream(new MemoryStream(imageBytes));

                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }
    }
}
