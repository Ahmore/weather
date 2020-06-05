using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using Plugin.Geolocator.Abstractions;
using Plugin.Geolocator;

namespace App3
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public Position CurrentPosition { get; set; }
        WebView webView;

        public MainPage()
        {
            CurrentPosition = new Position();
            StartListening();

            webView = new WebView
            {
                Source = new HtmlWebViewSource
                {
                    Html = "<b>Waiting for location...</b>"
                },
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    webView
                }
            };
        }

        async private void StartListening()
        {
            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(0), 10, true);
            CrossGeolocator.Current.PositionChanged += PositionChanging;
            CrossGeolocator.Current.PositionError += PositionError;
        }
        private async void PositionChanging(object sender, PositionEventArgs e)
        {
            CurrentPosition = e.Position;

            var url = String.Format(
                       "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&mode=html&appid=4526d487f12ef78b82b7a7d113faea64",
                       CurrentPosition.Latitude,
                       CurrentPosition.Longitude);

            var client2 = new HttpClient();
            var res2 = await client2.GetAsync(url);
            var str2 = res2.Content.ReadAsStringAsync().Result;

            webView.Source = new HtmlWebViewSource
            {
                Html = str2
            };
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            Console.WriteLine(e.Error);
        }
    }
}
