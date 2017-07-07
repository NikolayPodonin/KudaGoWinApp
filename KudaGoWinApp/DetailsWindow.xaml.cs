using KudaGoClassLibrary;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;


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

                i_Prewiew.Source = StaticClass.ImageSourceReturn(ev.images[0].image);
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

        private void b_Right_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(i_Prewiew.Uid) + 1;
            if (i < ev.images.Count)
            {
                var newImage = StaticClass.ImageSourceReturn(ev.images[i].image);
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
                var newImage = StaticClass.ImageSourceReturn(ev.images[i].image);
                if (newImage != null)
                {
                    i_Prewiew.Source = newImage;
                    i_Prewiew.Uid = i.ToString();
                }
            }
        }
    }
}
