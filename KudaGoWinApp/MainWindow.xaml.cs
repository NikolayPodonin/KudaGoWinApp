using KudaGoClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
        string nextPage;
        int i = 0;

        public MainWindow()
        {
            InitializeComponent();
            string link = @"https://kudago.com/public-api/v1.3/events/?fields=id,dates,short_title,categories,images,&expand=dates";
            MainFunction(link);
        }        

        public void MainFunction(string link)
        {
            using (var webClient = new WebClient())
            {
                //webClient.BaseAddress = "https://kudago.com/public-api/v1.3/event-categories/";
                //webClient.QueryString.Add("lang", "en");
                //webClient.QueryString.Add("fields", "name");
                webClient.Encoding = Encoding.UTF8;
                var response = webClient.DownloadString(link);
                
                var events = JsonConvert.DeserializeObject<EventsParsing>(response);
                nextPage = events.next;

                Style labelStyle = new Style();
                labelStyle.Setters.Add(new Setter { Property = Control.FontFamilyProperty, Value = new FontFamily("Verdana") });
                labelStyle.Setters.Add(new Setter { Property = Control.ForegroundProperty, Value = new SolidColorBrush(Colors.White) });

                //l_Name.FontFamily = new FontFamily("Segoe UI Semibold");
                
                foreach (Event ev in events.results)
                {
                    if(ev.images.Count > 0)
                    {
                        g_Image.RowDefinitions.Add(new RowDefinition());

                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        image.Source = StaticClass.ImageSourceReturn(ev.images[0].image);
                        
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
        }


        
        private void NewRD_MouseUp(object sender, EventArgs e)
        {
            DetailsWindow dw = new DetailsWindow(((UIElement)sender).Uid.Remove(0, 3));
            dw.Show();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
            {
                MainFunction(nextPage);
            }
        }
    }
}
