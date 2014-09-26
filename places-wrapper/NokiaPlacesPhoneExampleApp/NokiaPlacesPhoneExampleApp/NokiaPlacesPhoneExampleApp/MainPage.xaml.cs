using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NokiaPlacesPhoneExampleApp.Resources;

using System.Diagnostics;
using System.Device.Location;

using Nokia.Places.Phone;
using Nokia.Places.Phone.Types;

namespace NokiaPlacesPhoneExampleApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void Button_gridbut_Click(object sender, RoutedEventArgs e)
        {
            if (sender == SearchBut)
            {
                NavigationService.Navigate(new Uri("/SearchPage.xaml", UriKind.Relative));
            }
            else if (sender == ExplocBut)
            {
                NavigationService.Navigate(new Uri("/ExploreLocationPage.xaml", UriKind.Relative));
            }
            else if (sender == ExpAreBut)
            {
                NavigationService.Navigate(new Uri("/ExploreArea.xaml", UriKind.Relative));
            }
            else if (sender == DiscovBut)
            {
                NavigationService.Navigate(new Uri("/DiscoverHerePage.xaml", UriKind.Relative));
            }
        }
         
    }

}