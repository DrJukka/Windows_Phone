using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using System.Device.Location;
using Nokia.Places.Phone;
using Nokia.Places.Phone.Types;

namespace NokiaPlacesPhoneExampleApp
{
    public partial class ExploreArea : PhoneApplicationPage
    {
        public ExploreArea()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ((Application.Current as App).SelectedLocation != null)
            {
                LatitudeBox.Text = (Application.Current as App).SelectedLocation.Latitude.ToString();
                LongittudeBox.Text = (Application.Current as App).SelectedLocation.Longitude.ToString();

                (Application.Current as App).SelectedLocation = null;
            }

            if (!string.IsNullOrEmpty((Application.Current as App).SelectedCategories))
            {
                StringBox.Text = (Application.Current as App).SelectedCategories;
                (Application.Current as App).SelectedCategories = "";
            }
        }
        private void Button_gridbut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == LaunchButton)
            {
                try
                {
                    (Application.Current as App).PlaceResult = null;

                    GeoCoordinate toGeo = new GeoCoordinate();
                    toGeo.Latitude = Double.Parse(LatitudeBox.Text);
                    toGeo.Longitude = Double.Parse(LongittudeBox.Text);

                    int radiusInMeters = int.Parse(DistanceBox.Text);

                    App.PlaceClient.Explore( (ListResponse<SearchResultItem> result) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            if (result.Error == null)
                            {
                                string showString = "Result.Count: " + result.Result.Count;
                                MessageBox.Show(showString);
                                if (result.Result.Count > 0)
                                {
                                    (Application.Current as App).PlaceResult = result.Result;
                                    NavigationService.Navigate(new Uri("/PlaceResultPage.xaml?target=Explore", UriKind.Relative));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Search error: " + result.Error);
                            }
                        });
                    },toGeo, StringBox.Text, radiusInMeters,-1,-1);
                }
                catch (Exception erno)
                {
                    MessageBox.Show("Error message: " + erno.Message);
                }
            }
            else if (sender == getGeoButton)
            {
                (Application.Current as App).SelectedLocation = null;
                (Application.Current as App).SelectedCategories = "";
                try
                {
                    GeoCoordinate toGeo = new GeoCoordinate();
                    toGeo.Latitude = Double.Parse(LatitudeBox.Text);
                    toGeo.Longitude = Double.Parse(LongittudeBox.Text);
                    (Application.Current as App).SelectedLocation = toGeo;
                }
                catch { }
                NavigationService.Navigate(new Uri("/LocationSelectorPage.xaml?target=Location", UriKind.Relative));
            }
            else if (sender == getCatButton)
            {
                (Application.Current as App).SelectedLocation = null;
                (Application.Current as App).SelectedCategories = "";
                try
                {
                    (Application.Current as App).SelectedCategories = StringBox.Text;
                }
                catch { }
                NavigationService.Navigate(new Uri("/GategorySelection.xaml", UriKind.Relative));
            }

        }
    }
}