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
                
                foreach (Event ev in convert.results)
                {
                    if(ev.images.Count > 0)
                    {
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                        Uri myUri = new Uri(ev.images[0].image);
                        switch (myUri.Segments.Last().Substring(myUri.Segments.Last().Count() - 3, 3))
                        {
                            case "jpg":
                                {
                                    JpegBitmapDecoder dec = new JpegBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "peg":
                                {
                                    JpegBitmapDecoder dec = new JpegBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "bmp":
                                {
                                    BmpBitmapDecoder dec = new BmpBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "png":
                                {
                                    PngBitmapDecoder dec = new PngBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "gif":
                                {
                                    GifBitmapDecoder dec = new GifBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "iff":
                                {
                                    TiffBitmapDecoder dec = new TiffBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            case "wmp":
                                {
                                    WmpBitmapDecoder dec = new WmpBitmapDecoder(myUri, BitmapCreateOptions.None, BitmapCacheOption.Default);
                                    BitmapSource bs = dec.Frames[0];
                                    image.Source = bs;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        int n = g_Image.Children.Add(image);

                        Label l_Name = new Label();
                        l_Name.Content = ev.short_title;
                        l_Name.Margin = new Thickness(10, 20, 0, 0);
                        
                        Label l_Date = new Label();
                        l_Date.Content = ev.dates[0].StartDate.Date;

                        g_Image.RowDefinitions.Add(new RowDefinition());
                        
                        Grid.SetRow(g_Image.Children[n], n);
                        Grid.SetRow(l_Name, n);
                        Grid.SetRow(l_Date, n);


                    }
                }                
            }
        }
    }
}
