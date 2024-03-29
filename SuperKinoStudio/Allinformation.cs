using System.Windows.Forms;
using System;
using SuperKinoStudio.Models;
using System.Linq;
using System.IO;
using System.Drawing;

namespace SuperKinoStudio
{
    public partial class Allinformation : Form
    {

        private ListViewAdd listViewGenerator = new ListViewAdd();
        private KinoStudioEntities2 entities;
        public Allinformation()
        {
            InitializeComponent();
            comboBox1.Items.Add("Actors");
            comboBox1.Items.Add("Areas");
            comboBox1.Items.Add("Studios");
            comboBox1.Items.Add("Movies");
            entities = new KinoStudioEntities2();
            textBox1.TextChanged += TextBox1_TextChanged;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            SearchItems(comboBox1.SelectedItem.ToString(), textBox1.Text);

        }
        public void SearchItems(string tableName, string searchText)
        {
            ListView listView = (ListView)listView1.Controls[0];
            listView.Items.Clear();

            switch (tableName)
            {
                case "Actors":
                    foreach (var actor in entities.Actors.Where(a => a.NameActor.StartsWith(searchText) || a.SurnameActor.StartsWith(searchText)))
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

                        var movie = entities.Movie.FirstOrDefault(m => m.MovieId == actor.MovieId);
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
                    foreach (var area in entities.Area.Where(a => a.AreaName.StartsWith(searchText)))
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
                    foreach (var studio in entities.Studios.Where(s => s.NameStudio.StartsWith(searchText)))
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
                    foreach (var movie in entities.Movie.Where(m => m.MovieName.StartsWith(searchText)))
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

                        var studio = entities.Studios.FirstOrDefault(s => s.StudioId == movie.StudioId);
                        if (studio != null)
                        {
                            item.SubItems.Add(studio.NameStudio);
                        }
                        else
                        {
                            item.SubItems.Add(string.Empty);
                        }

                        var area = entities.Area.FirstOrDefault(a => a.AreaId == movie.AreaId);
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


        private void ListViewGenerator_ListViewSelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewGenerator.SelectedItem != null)
            {
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "Actors":
                        new RedactActor((Actors)listViewGenerator.SelectedItem).Show();
                        break;

                    case "Areas":
                        RedactArea formArea = new RedactArea((Area)listViewGenerator.SelectedItem);
                        formArea.Show();
                        break;

                    case "Studios":
                        RedactStudio formStudio = new RedactStudio((Studios)listViewGenerator.SelectedItem);
                        formStudio.Show();
                        break;

                    case "Movies":
                        new RedactMovie((Movie)listViewGenerator.SelectedItem).Show();
                        break;
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedTable = comboBox1.SelectedItem.ToString();
            listView1.Controls.Clear();
            ListView generatedListView = listViewGenerator.GenerateListView(selectedTable, listView1.Size);
            generatedListView.SelectedIndexChanged += ListViewGenerator_ListViewSelectedIndexChanged;
            listView1.Controls.Add(generatedListView);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Allinformation_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
