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
    public partial class SearchPage : PhoneApplicationPage
    {
        public SearchPage()
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

                    App.PlaceClient.Search((ListResponse<SearchResultItem> result) =>
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
                                    NavigationService.Navigate(new Uri("/PlaceResultPage.xaml?target=Search", UriKind.Relative));
                                }
                            }
                            else
                            {
                                MessageBox.Show("Search error: " + result.Error);
                            }
                        });
                    }, toGeo, StringBox.Text,-1,-1);
                }
                catch (Exception erno)
                {
                    MessageBox.Show("Error message: " + erno.Message);
                }
            }
            else if (sender == getGeoButton)
            {
                (Application.Current as App).SelectedLocation = null;
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
        }

        private void suggBut_Click_1(object sender, RoutedEventArgs e)
        {
            GeoCoordinate toGeo = null;
            try
            {
                toGeo = new GeoCoordinate();
                toGeo.Latitude = Double.Parse(LatitudeBox.Text);
                toGeo.Longitude = Double.Parse(LongittudeBox.Text);
            }
            catch {
                toGeo = null;
            }

            if (toGeo != null)
            {
                if (StringBox.Text.Length > 0)
                {
                    SuggestionBox.Items.Clear();
                    SuggResults.Visibility = System.Windows.Visibility.Visible;
                    ContentPanel.Visibility = System.Windows.Visibility.Collapsed;

                    App.PlaceClient.GetSuggestions((Response<SuggestionList> result) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            if (result.Error == null)
                            {
                                for (int i = 0; i < result.Result.Suggestions.Count(); i++)
                                {
                                    Button addbox = new Button();
                                    addbox.Content = result.Result.Suggestions[i];
                                    addbox.Click += ResultBut_Click;
                                    SuggestionBox.Items.Add(addbox);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Search error: " + result.Error);
                            }
                        });
                    }, StringBox.Text, toGeo);
                }
                else
                {
                    MessageBox.Show("Please define the search text part for which suggestions are searched for");
                }
            }
            else
            {
                MessageBox.Show("Please define the location for which suggestions are searched for");
            }
        }

        private void ResultBut_Click(object sender, RoutedEventArgs e)
        {
            Button suggbut = (sender as Button);
            if ((sender != CanCelBut) && (suggbut != null))
            {
                StringBox.Text = suggbut.Content.ToString();
            }
            ContentPanel.Visibility = System.Windows.Visibility.Visible;
            SuggResults.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}