using KudaGoClassLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KudaGoWinApp
{
    /// <summary>
    /// Логика взаимодействия для DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        Event ev;

        public DetailsWindow()
        {
            InitializeComponent();
        }

        public DetailsWindow(string eventId)
        {
            InitializeComponent();
            
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                
                var response = webClient.DownloadString(@"https://kudago.com/public-api/v1.3/events/" + eventId + "/?expand=dates,location,images,place");
                Regex reg = new Regex(@"<[^>]+>"); 
                response = reg.Replace(response, "");
                
                ev = JsonConvert.DeserializeObject<Event>(response);

                l_Head.Content = ev.title;
                tb_Descript.Text = ev.description;
                tb_Descript.TextWrapping = TextWrapping.Wrap;
                tb_FullDescript.Text = ev.body_text;
                tb_FullDescript.TextWrapping = TextWrapping.Wrap;

                i_Prewiew.Source = ImageSourceReturn(ev.images[0].image);
                i_Prewiew.Uid = 0.ToString();

                foreach (var date in ev.dates)
                {
                    tb_Dates.Text += date.StartDate.ToShortDateString() + "\n";
                }

                if (ev.place != null)
                {
                    if (ev.place.title != null && ev.place.title != "")
                    {
                        tb_Place.Text = "Место проведения: " + ev.place.title + "\n";
                    }
                    if (ev.place.address != null && ev.place.address != "")
                    {
                        tb_Place.Text += "Адрес: " + ev.place.address + "\n";
                    }
                    if (ev.place.phone != null && ev.place.phone != "")
                    {
                        tb_Place.Text += "Телефон: " + ev.place.phone + "\n";
                    }
                    if (ev.place.subway != null && ev.place.subway != "")
                    {
                        tb_Place.Text += "Ближайшее метро: " + ev.place.subway;
                    }
                }
                
                l_Price.Content = "Цена: " + ev.price;
                l_Age.Content = "Возрастные ограничения: " + ev.age_restriction;

                
                
            }
        }
               

        private BitmapSource ImageSourceReturn(string uri)
        {
            Uri myUri = new Uri(uri);
            switch (myUri.Segments.Last().Substring(myUri.Segments.Last().Count() - 3, 3))
            {
                case "jpg":
                    {
                        JpegBitmapDecoder dec = new JpegBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "peg":
                    {
                        JpegBitmapDecoder dec = new JpegBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "bmp":
                    {
                        BmpBitmapDecoder dec = new BmpBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "png":
                    {
                        PngBitmapDecoder dec = new PngBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "gif":
                    {
                        GifBitmapDecoder dec = new GifBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "iff":
                    {
                        TiffBitmapDecoder dec = new TiffBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                case "wmp":
                    {
                        WmpBitmapDecoder dec = new WmpBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                        BitmapSource bs = dec.Frames[0];
                        return bs;
                    }
                default:
                    {
                        return null;
                    }
            }
        }

        private void b_Right_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(i_Prewiew.Uid) + 1;
            if (i < ev.images.Count)
            {
                var newImage = ImageSourceReturn(ev.images[i].image);
                if (newImage != null)
                {
                    i_Prewiew.Source = newImage;
                    i_Prewiew.Uid = i.ToString();
                }
            }            
        }

        private void b_Left_Click(object sender, RoutedEventArgs e)
        {

            int i = int.Parse(i_Prewiew.Uid) - 1;
            if (i >= 0)
            {
                var newImage = ImageSourceReturn(ev.images[i].image);
                if (newImage != null)
                {
                    i_Prewiew.Source = newImage;
                    i_Prewiew.Uid = i.ToString();
                }
            }
        }
    }
}
