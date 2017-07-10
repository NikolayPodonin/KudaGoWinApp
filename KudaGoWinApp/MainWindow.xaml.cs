using KudaGoClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KudaGoWinApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        WebClient webClient;
        int i = 0;
        Style labelStyle;
        DateTime time;
        EventsParsing nextPageEvents;
        string categorySuffics;
        Thread downloader;

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                using (webClient = new WebClient())
                {
                    webClient.Encoding = Encoding.UTF8;
                    var response = webClient.DownloadString(@"https://kudago.com/public-api/v1.3/event-categories/");
                    var categories = JsonConvert.DeserializeObject<List<EventCategorie>>(response);
                    foreach (EventCategorie cat in categories)
                    {
                        lv_Categories.Items.Add(cat);
                    }
                }

                categorySuffics = "";

                labelStyle = new Style();
                labelStyle.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
                labelStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = new SolidColorBrush(Colors.White) });
                labelStyle.Setters.Add(new Setter { Property = Control.BackgroundProperty, Value = new SolidColorBrush(Colors.Brown) });
                labelStyle.Setters.Add(new Setter { Property = Control.HorizontalContentAlignmentProperty, Value = HorizontalAlignment.Left });
                labelStyle.Setters.Add(new Setter { Property = Control.HorizontalAlignmentProperty, Value = HorizontalAlignment.Left });
                labelStyle.Setters.Add(new Setter { Property = Control.VerticalAlignmentProperty, Value = VerticalAlignment.Top });

                string link = @"https://kudago.com/public-api/v1.3/events/?fields=id,dates,short_title,categories,images,&expand=dates&page_size=10";
                var events = DownloadEvents(link);
                FillEvents(events.results);

                nextPageEvents = DownloadEvents(events.next);

                time = DateTime.Now;

                downloader = new Thread(DownloaderEventsThread);
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка: " + e.Message, "KudaGoWinApp");
            }
        }

        private void DownloaderEventsThread(object link)
        {
            lock (nextPageEvents)
            {
                if(link.GetType() == typeof(string))
                {
                    nextPageEvents = DownloadEvents((string)link);
                }
                else
                {
                    throw new ArgumentException("Неверный запрос");
                }                
            }
        }

        private EventsParsing DownloadEvents(string link)
        {
            using (webClient)
            {
                var response = webClient.DownloadString(link);
                return JsonConvert.DeserializeObject<EventsParsing>(response);
            }
        }

        private void FillEvents(List<Event> results)
        {
            foreach (Event ev in results)
            {
                if (ev.images.Count > 0)
                {
                    g_Image.RowDefinitions.Add(new RowDefinition());

                    System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                    ImageSource source = new BitmapImage();

                    source = StaticClass.ImageSourceReturn(ev.images[0].image);
                    if (source != null)
                    {
                        image.Source = source;
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri(@"resources\no_image.jpg", UriKind.Relative));
                    }
                     

                    int n = g_Image.Children.Add(image);
                    Grid.SetRow(g_Image.Children[n], i);
                    g_Image.Children[n].Uid = "_1_" + ev.id;
                    g_Image.Children[n].MouseUp += NewRD_MouseUp;
                    g_Image.Children[n].TouchUp += NewRD_MouseUp;

                    Label l_Name = new Label();
                    l_Name.Content = ev.short_title;
                    l_Name.Margin = new Thickness(10, 20, 0, 0);
                    l_Name.Style = labelStyle;
                    n = g_Image.Children.Add(l_Name);
                    Grid.SetRow(g_Image.Children[n], i);
                    g_Image.Children[n].Uid = "_2_" + ev.id;
                    g_Image.Children[n].MouseUp += NewRD_MouseUp;
                    g_Image.Children[n].TouchUp += NewRD_MouseUp;

                    Label l_Date = new Label();
                    l_Date.Content = ev.dates[0].StartDate.ToShortDateString();
                    l_Date.VerticalAlignment = VerticalAlignment.Bottom;
                    l_Date.HorizontalAlignment = HorizontalAlignment.Right;
                    l_Date.Margin = new Thickness(0, 0, 10, 20);
                    l_Date.Style = labelStyle;
                    n = g_Image.Children.Add(l_Date);
                    Grid.SetRow(g_Image.Children[n], i);
                    g_Image.Children[n].Uid = "_3_" + ev.id;
                    g_Image.Children[n].MouseUp += NewRD_MouseUp;
                    g_Image.Children[n].TouchUp += NewRD_MouseUp;

                    i++;
                }
            }
        }

        private void NewRD_MouseUp(object sender, EventArgs e)
        {
            try
            {
                DetailsWindow dw = new DetailsWindow(((UIElement)sender).Uid.Remove(0, 3));
                dw.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "KudaGoWinApp");
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                if (downloader.IsAlive)
                {
                    return;
                }
                if (DateTime.Now - time < new TimeSpan(0, 0, 0, 1))
                {
                    return;
                }
                var scrollViewer = (ScrollViewer)sender;
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                {
                    lock (nextPageEvents)
                    {
                        FillEvents(nextPageEvents.results);
                    }

                    downloader = new Thread(DownloaderEventsThread);
                    downloader.Start(nextPageEvents.next + categorySuffics);

                    time = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "KudaGoWinApp");
            }
        }

        private void b_Categories_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lv_Categories.SelectedItems.Count > 0)
                {
                    var newCategSuff = @"&categories=";
                    foreach (var cat in lv_Categories.SelectedItems)
                    {
                        newCategSuff += ((EventCategorie)cat).slug + ",";
                    }
                    newCategSuff = newCategSuff.Remove(newCategSuff.Count() - 1, 1);
                    if (newCategSuff == categorySuffics)
                    {
                        return;
                    }
                    categorySuffics = newCategSuff;
                }
                else
                {
                    categorySuffics = "";
                }
                g_Image.Children.Clear();
                string link = @"https://kudago.com/public-api/v1.3/events/?fields=id,dates,short_title,categories,images,&expand=dates&page_size=10" + categorySuffics;
                var eventList = DownloadEvents(link);
                FillEvents(eventList.results);
                if (downloader.IsAlive)
                {
                    downloader.Join();
                }
                downloader = new Thread(DownloaderEventsThread);
                downloader.Start(link + @"&page=2");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "KudaGoWinApp");
            }
        }
    }
}
