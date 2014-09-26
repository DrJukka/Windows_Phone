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
    public partial class DiscoverHerePage : PhoneApplicationPage
    {
        public DiscoverHerePage()
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

                    App.PlaceClient.Explore((ListResponse<SearchResultItem> result) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            if (result.Error == null)
                            {
                                string showString = "Result.Count: " + result.Result.Count;
                                MessageBox.Show(showString);

                                if (result.Result.Count > 0){
                                    (Application.Current as App).PlaceResult = result.Result;

                                    NavigationService.Navigate(new Uri("/PlaceResultPage.xaml?target=Discover", UriKind.Relative));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Search error: " + result.Error);
                            }
                        });
                    }, toGeo, StringBox.Text, -1, -1, -1);
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

        private void localCatButton_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                string hjelp = "";

                for (var i = 0; i < LocalCatBox.Items.Count; i++)
                {
                    CheckBox boxItem = (LocalCatBox.Items[i] as CheckBox);
                    if (boxItem != null)
                    {
                        if (boxItem.IsChecked == true)
                        {
                            // for some reason local categories start with uppercase letter
                            // but we don't get any results, if we don't use all lower case letters..
                            if (hjelp.Length > 0)
                            {
                                hjelp = hjelp + "," + boxItem.Content.ToString().ToLower();
                            }
                            else
                            {
                                hjelp = boxItem.Content.ToString().ToLower();
                            }
                        }
                    }
                }

                StringBox.Text = hjelp;
            }

            ContentPanel.Visibility = System.Windows.Visibility.Visible;
            CategorySelector.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void localCatBut_Click(object sender, RoutedEventArgs e)
        {
            GeoCoordinate toGeo = null;
            try
            {
                toGeo = new GeoCoordinate();
                toGeo.Latitude = Double.Parse(LatitudeBox.Text);
                toGeo.Longitude = Double.Parse(LongittudeBox.Text);
            }
            catch
            {
                toGeo = null;
            }

            if (toGeo != null)
            {
                LocalCatBox.Items.Clear();
                CategorySelector.Visibility = System.Windows.Visibility.Visible;
                ContentPanel.Visibility = System.Windows.Visibility.Collapsed;

                App.PlaceClient.GetLocalCategories((ListResponse<CategoryItem> result) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        if (result.Error == null)
                        {
                            for (int i = 0; i < result.Result.Count; i++)
                            {
                            /*    public string Id { get; private set; }
                                public string Title { get; private set; }
                                public Uri Href { get; private set; }

                                public string Type { get; private set; }
                                public Uri Icon { get; private set; }

                                public List<string> Within { get; private set; }
                             */
                                // lets only select main categories here
                                if ((result.Result[i].Within == null) || (result.Result[i].Within.Count() == 0)){
                                    CheckBox addbox = new CheckBox();
                                    addbox.Content = result.Result[i].Title;
                                    LocalCatBox.Items.Add(addbox);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Search error: " + result.Error);
                        }
                    });
                }, toGeo);
            }
            else
            {
                MessageBox.Show("Please define the location for which categories are searched for");
            }
        }
    }
}