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
    public partial class AllForPresidents : Form
    {
        private GenerateListViewPresident president = new GenerateListViewPresident();
        KinoStudioEntities2 entities2;
        public AllForPresidents()
        {
            InitializeComponent();
            comboBox1.Items.Add("Actors");
            comboBox1.Items.Add("Areas");
            comboBox1.Items.Add("Studios");
            comboBox1.Items.Add("Movies");
            comboBox1.Items.Add("Users");
            entities2 = new KinoStudioEntities2();
            textBox1.TextChanged += TextBox1_TextChanged;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            SearchItems(comboBox1.SelectedItem.ToString(), textBox1.Text);
        }
        public void SearchItems(string tableName, string searchText)
        {
            ListView listView = (ListView)listView1.Controls[0];//получаем доступ к таблицам, и первый элемент[0]
            listView.Items.Clear();
            switch (tableName)
            {
                case "Users":
                    foreach (var user in entities2.Users.Where(u => u.Name.StartsWith(searchText) || u.Surname.StartsWith(searchText)))
                    {
                        ListViewItem item = new ListViewItem(user.UserId.ToString());
                        item.SubItems.Add(user.Name);
                        item.SubItems.Add(user.Surname);
                        item.SubItems.Add(user.Midname);
                        item.SubItems.Add(user.Login);
                        item.SubItems.Add(user.Role.RoleName); // добавлено
                        item.SubItems.Add(user.Gender.GenderName); // добавлено

                        if (user.Image != null && user.Image.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(user.Image))
                            {
                                listView.SmallImageList.Images.Add(Image.FromStream(ms));
                                item.ImageIndex = listView.SmallImageList.Images.Count - 1;
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        item.SubItems.Add(user.PhoneNumber);
                        item.SubItems.Add(user.Age.ToString());
                        item.SubItems.Add(user.Email);

                        item.Tag = user;
                        listView.Items.Add(item);
                    }
                    break;

                case "Actors":
                    foreach (var actor in entities2.Actors.Where(a => a.NameActor.StartsWith(searchText) || a.SurnameActor.StartsWith(searchText)))
                    {
                        ListViewItem item = new ListViewItem(actor.ActorId.ToString());
                        item.SubItems.Add(actor.NameActor);
                        item.SubItems.Add(actor.SurnameActor);
                        item.SubItems.Add(actor.MidnameActor);
                        item.SubItems.Add(actor.Age.ToString());
                        item.SubItems.Add(actor.SalaryActor.ToString());

                        if (actor.Image != null && actor.Image.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(actor.Image))
                            {
                                listView.SmallImageList.Images.Add(Image.FromStream(ms));
                                item.ImageIndex = listView.SmallImageList.Images.Count - 1;
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        var movie = entities2.Movie.FirstOrDefault(m => m.MovieId == actor.MovieId);
                        if (movie != null)
                        {
                            item.SubItems.Add(movie.Studios.NameStudio);
                            item.SubItems.Add(movie.Area.AreaName);
                            item.SubItems.Add(movie.MovieName);
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                            item.SubItems.Add(string.Empty);
                            item.SubItems.Add(string.Empty);
                        }

                        item.Tag = actor;
                        listView.Items.Add(item);
                    }
                    break;
                case "Areas":
                    foreach (var area in entities2.Area.Where(a => a.AreaName.StartsWith(searchText)))
                    {
                        ListViewItem item = new ListViewItem(area.AreaId.ToString());
                        item.SubItems.Add(area.AreaName);

                        if (area.Image != null && area.Image.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(area.Image))
                            {
                                listView.SmallImageList.Images.Add(Image.FromStream(ms));
                                item.ImageIndex = listView.SmallImageList.Images.Count - 1;
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        item.Tag = area;
                        listView.Items.Add(item);
                    }
                    break;
                case "Studios":
                    foreach (var studio in entities2.Studios.Where(s => s.NameStudio.StartsWith(searchText)))
                    {
                        ListViewItem item = new ListViewItem(studio.StudioId.ToString());
                        item.SubItems.Add(studio.NameStudio);
                        item.SubItems.Add(studio.FoundingDate.ToString());
                        item.SubItems.Add(studio.location);
                        item.SubItems.Add(studio.AnnualIncome.ToString());

                        if (studio.Image != null && studio.Image.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(studio.Image))
                            {
                                listView.SmallImageList.Images.Add(Image.FromStream(ms));
                                item.ImageIndex = listView.SmallImageList.Images.Count - 1;
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        item.Tag = studio;
                        listView.Items.Add(item);
                    }
                    break;
                case "Movies":
                    foreach (var movie in entities2.Movie.Where(m => m.MovieName.StartsWith(searchText)))
                    {
                        ListViewItem item = new ListViewItem(movie.MovieId.ToString());
                        item.SubItems.Add(movie.MovieName);

                        if (movie.Image != null && movie.Image.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(movie.Image))
                            {
                                listView.SmallImageList.Images.Add(Image.FromStream(ms));
                                item.ImageIndex = listView.SmallImageList.Images.Count - 1;
                            }
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        var studio = entities2.Studios.FirstOrDefault(s => s.StudioId == movie.StudioId);
                        if (studio != null)
                        {
                            item.SubItems.Add(studio.NameStudio);
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        var area = entities2.Area.FirstOrDefault(a => a.AreaId == movie.AreaId);
                        if (area != null)
                        {
                            item.SubItems.Add(area.AreaName);
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        item.SubItems.Add(movie.budgetFilm.ToString());

                        item.Tag = movie;
                        listView.Items.Add(item);
                    }
                    break;
            }
        }
        private void president_ListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (president.SelectedItem != null)
            {
                if (president.SelectedItem is Users)
                {
                    new RedactUser((Users)president.SelectedItem).Show();
                }
                else if (president.SelectedItem is Actors)
                {
                    new RedactActor((Actors)president.SelectedItem).Show();
                }
                else
                {
                    switch (comboBox1.SelectedItem.ToString())
                    {
                        case "Actors":
                            new RedactActor((Actors)president.SelectedItem).Show();
                            break;

                        case "Areas":
                            new RedactArea((Area)president.SelectedItem).Show();
                            break;

                        case "Studios":
                            RedactStudio formStudio = new RedactStudio((Studios)president.SelectedItem);
                            formStudio.Show();
                            break;

                        case "Movies":
                            new RedactMovie((Movie)president.SelectedItem).Show();
                            break;
                        case "Users":
                            new RedactUser((Users)president.SelectedItem).Show();
                            break;
                    }
                }
            }
        }

        private void AllForPresidents_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //событие для выбора таблицы
        {
            string selectedTable = comboBox1.SelectedItem.ToString();
            listView1.Controls.Clear();
            ListView generatedListView = president.GenerateListView(selectedTable, listView1.Size); //генерация выбранной таблицы из cmb
            generatedListView.SelectedIndexChanged += president_ListViewSelectedIndexChanged;
            listView1.Controls.Add(generatedListView);
        }
    }
}
