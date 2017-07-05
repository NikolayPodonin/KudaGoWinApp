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
        public MainWindow()
        {
            InitializeComponent();
            MainFunction();
        }

        public void MainFunction()
        {
            using (var webClient = new WebClient())
            {
                //webClient.BaseAddress = "https://kudago.com/public-api/v1.3/event-categories/";
                //webClient.QueryString.Add("lang", "en");
                //webClient.QueryString.Add("fields", "name");
                webClient.Encoding = Encoding.UTF8;
                var response = webClient.DownloadString(@"https://kudago.com/public-api/v1.3/events/?fields=id,publication_date,dates,title,short_title,slug,place,description,body_text,location,categories,tagline,age_restriction,price,is_free,images,favorites_count,comments_count,site_url,tags,participants&expand=place,dates,location");
                
                var convert = JsonConvert.DeserializeObject<EventsParsing>(response);
                //StringBuilder build = new StringBuilder();
                //foreach (var c in convert)
                //{
                //    foreach (var d in c)
                //    {
                //        build.Append(d.Key);
                //        build.Append(": ");
                //        build.Append(d.Value);
                //        build.AppendLine();
                //    }
                //}
                listBox.Items.Add(convert.results[0].short_title);
            }
        }
    }
}
