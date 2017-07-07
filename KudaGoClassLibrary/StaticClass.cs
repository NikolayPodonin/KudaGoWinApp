using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KudaGoClassLibrary
{
    public class StaticClass
    {
        public static BitmapSource ImageSourceReturn(string uri)
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
    }
}
