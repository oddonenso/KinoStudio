using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperKinoStudio.Models;

namespace SuperKinoStudio
{
    public class ListViewAdd
    {
        private KinoStudioEntities2 entities = new KinoStudioEntities2();
        public object SelectedItem { get; private set; }
        public event EventHandler SelectedItemChanged;

        public void OnSelectedItemChanged(EventArgs e)
        {
            SelectedItemChanged?.Invoke(this, e);
        }

        public ListView GenerateListView(string tableName, Size size)
        {
            ListView listView = new ListView();
            listView.View = View.Details;
            listView.GridLines = true;
            listView.FullRowSelect = true;
            listView.SmallImageList = new ImageList();
            listView.SmallImageList.ImageSize = new Size(255, 255); // Можно задать нужный размер
            listView.Size = size;


            switch (tableName)
            {
                case "Actors":
                    listView.Columns.Add("ActorId", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("NameActor", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("SurnameActor", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("MidnameActor", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Age", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("SalaryActor", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Studio", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Area", -2, HorizontalAlignment.Left);

                    foreach (var actor in entities.Actors)
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
                            item.SubItems.Add(string.Empty); // Добавляем пустую строку для столбца Image, если изображение отсутствует
                        }

                        // Предполагая, что у актёра есть свойство MovieId, которое связано с фильмом
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

                        item.Tag = actor; // Добавляем тег для элемента ListViewItem, чтобы потом можно было получить выбранного актёра
                        listView.Items.Add(item);
                    }
                    listView.SelectedIndexChanged += (sender, e) =>
                    {
                        if (listView.SelectedItems.Count > 0)
                        {
                            SelectedItem = listView.SelectedItems[0].Tag;
                            OnSelectedItemChanged(e);
                        }
                    };
                    break;
                case "Areas":
                    listView.Columns.Add("AreaId", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("AreaName", -2, HorizontalAlignment.Left);

                    foreach (var area in entities.Area)
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
                            item.SubItems.Add(string.Empty); // Добавляем пустую строку для столбца Image, если изображение отсутствует
                        }

                        item.Tag = area; // Добавляем тег для элемента ListViewItem, чтобы потом можно было получить выбранную площадку
                        listView.Items.Add(item);
                    }
                    listView.SelectedIndexChanged += (sender, e) =>
                    {
                        if (listView.SelectedItems.Count > 0)
                        {
                            SelectedItem = listView.SelectedItems[0].Tag;
                            OnSelectedItemChanged(e);
                        }
                    };
                    break;
                case "Studios":
                    listView.Columns.Add("StudioId", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("NameStudio", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("FoundingDate", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Location", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("AnnualIncome", -2, HorizontalAlignment.Left);

                    foreach (var studio in entities.Studios)
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
                            item.SubItems.Add(string.Empty); // Добавляем пустую строку для столбца Image, если изображение отсутствует
                        }

                        item.Tag = studio; // Добавляем тег для элемента ListViewItem, чтобы потом можно было получить выбранную студию
                        listView.Items.Add(item);
                    }
                    listView.SelectedIndexChanged += (sender, e) =>
                    {
                        if (listView.SelectedItems.Count > 0)
                        {
                            SelectedItem = listView.SelectedItems[0].Tag;
                            OnSelectedItemChanged(e);
                        }
                    };
                    break;
                case "Movies":
                    listView.Columns.Add("MovieId", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("MovieName", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Studio", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Area", -2, HorizontalAlignment.Left);
                    listView.Columns.Add("Budget", -2, HorizontalAlignment.Left);

                    foreach (var movie in entities.Movie)
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
                            item.SubItems.Add(string.Empty); // Добавляем пустую строку для столбца Image, если изображение отсутствует
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

                        item.Tag = movie; // Добавляем тег для элемента ListViewItem, чтобы потом можно было получить выбранный фильм
                        listView.Items.Add(item);
                    }
                    listView.SelectedIndexChanged += (sender, e) =>
                    {
                        if (listView.SelectedItems.Count > 0)
                        {
                            SelectedItem = listView.SelectedItems[0].Tag;
                            OnSelectedItemChanged(e);
                        }
                    };
                    break;
            }

            return listView;
        }
    }
}
