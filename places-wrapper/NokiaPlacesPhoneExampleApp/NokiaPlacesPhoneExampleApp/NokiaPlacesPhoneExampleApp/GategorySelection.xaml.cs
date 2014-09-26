using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace NokiaPlacesPhoneExampleApp
{
    public partial class GategorySelection : PhoneApplicationPage
    {
        public GategorySelection()
        {
            InitializeComponent();

            addBoxItem("eat-drink");
            addBoxItem("going-out");
            addBoxItem("sights-museums");
            addBoxItem("transport");
            addBoxItem("accommodation");
            addBoxItem("shopping");
            addBoxItem("leisure-outdoor");
            addBoxItem("administrative-areas-buildings");
            addBoxItem("natural-geographical");

            //we should check the (Application.Current as App).SelectedCategories
            //to mark current category selection
        }

        private void addBoxItem(string categoryString)
        {
            CheckBox addbox = new CheckBox();
            addbox.Content = categoryString;
            stackBox.Items.Add(addbox);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OkBut == sender)
            {
                string hjelp = "";

                for (var i = 0; i < stackBox.Items.Count; i++)
                {
                    CheckBox boxItem = (stackBox.Items[i] as CheckBox);
                    if (boxItem != null)
                    {
                        if (boxItem.IsChecked == true)
                        {
                            if (hjelp.Length > 0)
                            {
                                hjelp = hjelp + "," + boxItem.Content;
                            }
                            else
                            {
                                hjelp = boxItem.Content.ToString();
                            }
                        }
                    }
                }

                (Application.Current as App).SelectedCategories = hjelp;
            }

            this.NavigationService.GoBack();
        }
    }
}