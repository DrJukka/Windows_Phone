
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Maps.Controls;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Device.Location;
using System.Collections.ObjectModel;
using Microsoft.Phone.Maps.Services;

using Nokia.Places.Phone;
using Nokia.Places.Phone.Types;

namespace NokiaPlacesPhoneExampleApp
{
    public partial class PlaceResultPage : PhoneApplicationPage
    {
        MapLayer markerLayer = null;
        PlaceItem selPlace = null;

        public PlaceResultPage()
        {
            InitializeComponent();
            markerLayer = new MapLayer();
            map1.Layers.Add(markerLayer);

            map1.ZoomLevelChanged += map1_ZoomLevelChanged;
        }

        void map1_ZoomLevelChanged(object sender, MapZoomLevelChangedEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            zoomSlider.Value = map1.ZoomLevel;

            string target = "";

            if (this.NavigationContext.QueryString.ContainsKey("target"))
            {
                target = this.NavigationContext.QueryString["target"];
            }

            Debug.WriteLine("OnNavigatedTo, target: " + target);

            base.OnNavigatedTo(e);

            pivvot.Title = target + " Results";

            this.ResultsBox.ItemsSource = (Application.Current as App).PlaceResult;
            
            if ((Application.Current as App).PlaceResult != null)
            {
                if ((Application.Current as App).PlaceResult.Count() > 0)
                {
                    for (int i = 0; i < (Application.Current as App).PlaceResult.Count(); i++)
                    {
                        string numNum = "0" + i;
                        if (i > 9)
                        {
                            numNum = "" + i;
                        }

                        if ((Application.Current as App).PlaceResult[i].Location != null)
                        {
                            if ((Application.Current as App).PlaceResult[i].Location.GeoCoordinate != null)
                            {
                                AddResultToMap(numNum, (Application.Current as App).PlaceResult[i].Location.GeoCoordinate);
                            }
                            else if ((Application.Current as App).PlaceResult[i].Location.BoundingArea != null)
                            {
                                // add polygon for the area
                            }
                        }
                    }
                    if ((markerLayer != null)) // fit all
                    {
                        if (markerLayer.Count() == 1)
                        {
                            map1.Center = markerLayer[0].GeoCoordinate;
                        }
                        else
                        {

                            bool gotRect = false;
                            double north = 0;
                            double west = 0;
                            double south = 0;
                            double east = 0;

                            for (var p = 0; p < markerLayer.Count(); p++)
                            {
                                if (markerLayer[p].GeoCoordinate != null)
                                {
                                    if (!gotRect)
                                    {
                                        gotRect = true;
                                        north = south = markerLayer[p].GeoCoordinate.Latitude;
                                        west = east = markerLayer[p].GeoCoordinate.Longitude;
                                    }
                                    else
                                    {
                                        if (north < markerLayer[p].GeoCoordinate.Latitude) north = markerLayer[p].GeoCoordinate.Latitude;
                                        if (west > markerLayer[p].GeoCoordinate.Longitude) west = markerLayer[p].GeoCoordinate.Longitude;
                                        if (south > markerLayer[p].GeoCoordinate.Latitude) south = markerLayer[p].GeoCoordinate.Latitude;
                                        if (east < markerLayer[p].GeoCoordinate.Longitude) east = markerLayer[p].GeoCoordinate.Longitude;
                                    }
                                }
                            }

                            if (gotRect)
                            {
                              //  MessageBox.Show("got rect, north: " + north + ", west: " + west + ", south: "+ south + ", east: " + east);
                                map1.SetView(new LocationRectangle(north, west, south, east));
                            }
                        }
                    }
                }
            }
        }

        private void zoomSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoomSlider != null)
            {
                map1.ZoomLevel = zoomSlider.Value;
            }
        }

        private void AddResultToMap(String text, GeoCoordinate location)
        {

            MapOverlay oneMarker = new MapOverlay();
            oneMarker.GeoCoordinate = location;

            Canvas canCan = new Canvas();

            Ellipse Circhegraphic = new Ellipse();
            Circhegraphic.Fill = new SolidColorBrush(Colors.Brown);
            Circhegraphic.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
            Circhegraphic.StrokeThickness = 5;
            Circhegraphic.Opacity = 0.8;
            Circhegraphic.Height = 70;
            Circhegraphic.Width = 70;

            canCan.Children.Add(Circhegraphic);
            TextBlock textt = new TextBlock { Text = text };
            textt.HorizontalAlignment = HorizontalAlignment.Center;
            textt.FontSize = 55;
            Canvas.SetLeft(textt, 5);
            Canvas.SetTop(textt, -5);
            Canvas.SetZIndex(textt, 5);


            canCan.Children.Add(textt);
            oneMarker.Content = canCan;

            oneMarker.PositionOrigin = new Point(0.5, 0.5);
            textt.MouseLeftButtonUp += textt_MouseLeftButtonUp;

            markerLayer.Add(oneMarker);
        }

        void textt_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("textt_MouseLeftButtonUp");
            TextBlock textt = sender as TextBlock;
            if (textt != null && ((Application.Current as App).PlaceResult != null))
            {
                GetMoreOnItem(int.Parse(textt.Text));
            }

        }

        private void ShowItem(object sender, SelectionChangedEventArgs e)
        {
            GetMoreOnItem(this.ResultsBox.SelectedIndex);
        }

        private void GetMoreOnItem(int index)
        {
            if (index >= 0 && index < (Application.Current as App).PlaceResult.Count())
            {
                selPlace = null;
                PlaceDetailBox.Items.Clear();

                if (string.Equals((Application.Current as App).PlaceResult[index].Type, "urn:nlp-types:place", StringComparison.OrdinalIgnoreCase)
                && ((Application.Current as App).PlaceResult[index].Id.Length > 0))
                {
                    App.PlaceClient.GetPlace((Response<PlaceItem> result) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            if (result.Error == null)
                            {
                                pivvot.SelectedIndex = 2;
                                selPlace = result.Result;
                                addBoxString("Name: " + selPlace.Name);

                                if (selPlace.View != null)
                                {
                                    addBoxButton("View Place");
                                }
/*
        
        public string Name { get; private set; }
        public Uri View { get; internal set; }
        public LocationItem Location { get; internal set; }
        public ContactsItem Contacts { get; internal set; }
        public List<CategoryItem> Categories { get; private set; }
        public RatingItem Rating { get; private set; }
        public Uri Icon { get; internal set; }
        public Dictionary<string, string> AlternativeNames { get; private set; }
        public string Attribution { get; private set; }
        public LinkItem Supplier { get; private set; }
        public List<ImageItem> Images { get; private set; }
        public LinkItem CreateImage { get; private set; }
        public List<EditorialItem> Editorials { get; private set; }
        public List<ReviewItem> Reviews { get; private set; }
        public LinkItem CreateReview { get; private set; }
        public ExtendedItem Extended { get; private set; }
        public RelatedItem Related { get; private set; }
        */
                   
                            }
                            else
                            {
                                MessageBox.Show("GetPlace error: " + result.Error);
                            }
                        });
                    }, (Application.Current as App).PlaceResult[index].Id);
                }
                else
                {
                    ShowDetailsMsg(index);
                }
            }
        }

        private void addBoxString(string addString)
        {
            PlaceDetailBox.Items.Add(addString);
        }

        private void addBoxButton(string addString)
        {
            Button addbox = new Button();
            addbox.Content = addString;
            addbox.Click += ResultBut_Click;
            PlaceDetailBox.Items.Add(addbox);
        }
        
        private void ResultBut_Click(object sender, RoutedEventArgs e)
        {
            Button suggbut = (sender as Button);
            if (suggbut != null && suggbut.Content.ToString() == "view")
            {
                Windows.System.Launcher.LaunchUriAsync(selPlace.View);
            }
            else
            {

            }

        }

        private void ShowDetailsMsg(int index)
        {
            if (index >= 0 && index < (Application.Current as App).PlaceResult.Count())
            {

                string showString = "";

                showString = showString + "\nName: " + (Application.Current as App).PlaceResult[index].Title;
                showString = showString + "\nId: " + (Application.Current as App).PlaceResult[index].Id;
                showString = showString + "\nDistanceTo: " + (Application.Current as App).PlaceResult[index].DistanceTo;

                if ((Application.Current as App).PlaceResult[index].Location != null)
                {
                    if ((Application.Current as App).PlaceResult[index].Location.GeoCoordinate != null)
                    {
                        showString = showString + "\nCoordinate.Latitude: " + (Application.Current as App).PlaceResult[index].Location.GeoCoordinate.Latitude + ", Longitude" + (Application.Current as App).PlaceResult[index].Location.GeoCoordinate.Longitude;
                    }
                    else
                    {
                        //print the area as well!!
                    }
                }
                else
                {
                    showString = showString + "\nCoordinate.Latitude is NULL";
                }
                if ((Application.Current as App).PlaceResult[index].Icon != null)
                {
                    showString = showString + "\nIcon: " + (Application.Current as App).PlaceResult[index].Icon.ToString();
                }
                else
                {
                    showString = showString + "\nPlaceResult.Icon is NULL";
                }

                if ((Application.Current as App).PlaceResult[index].Href != null)
                {
                    showString = showString + "\nPlaceUrl: " + (Application.Current as App).PlaceResult[index].Href.ToString();
                }
                else
                {
                    showString = showString + "\nPlaceResult.PlaceUrl is NULL";
                }

                showString = showString + "\nVisinity: " + (Application.Current as App).PlaceResult[index].Visinity;

                if ((Application.Current as App).PlaceResult[index].Category != null)
                {
                    showString = showString + "\nCategory.Id: " + (Application.Current as App).PlaceResult[index].Category.Id;
                    showString = showString + "\nCategory.Title: " + (Application.Current as App).PlaceResult[index].Category.Title;
                    showString = showString + "\nCategory.Type: " + (Application.Current as App).PlaceResult[index].Category.Type;
                    if ((Application.Current as App).PlaceResult[index].Category.Href != null)
                    {
                        showString = showString + "\nCategory.CategoryUrl: " + (Application.Current as App).PlaceResult[index].Category.Href.ToString();
                    }
                    else
                    {
                        showString = showString + "\nCategory.CategoryUrl is NULL";
                    }
                }
                else
                {
                    showString = showString + "\nCategory is NULL";
                }

                showString = showString + "\nType: " + (Application.Current as App).PlaceResult[index].Type;
                showString = showString + "\nRating: " + (Application.Current as App).PlaceResult[index].Rating;

                MessageBox.Show(showString);
            }
        }
    }
}