using System;
using System.Device.Location;
using System.Windows;
using System.Windows.Media;
using AzureConferenceLib.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace AzureConferenceWP
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            var myColor = (Color)App.Current.Resources["PhoneAccentColor"];
            App.Current.Resources.Remove("PhoneAccentColor");
            myColor.R = 0;
            myColor.G = 114;
            myColor.B = 188;
            myColor.A = 1;
            App.Current.Resources.Add("PhoneAccentColor", myColor);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void SessionList_ItemClicked(object sender, Infragistics.Controls.Grids.ListItemEventArgs e)
        {
            App.CurrentSession = e.Item.Data as Session;
            if (App.CurrentSession != null)
                NavigationService.Navigate(new System.Uri("/SessionPage.xaml", System.UriKind.Relative));
        }

        private void SpeakerList_ItemClicked(object sender, Infragistics.Controls.Grids.ListItemEventArgs e)
        {
            App.CurrentSpeaker = e.Item.Data as Speaker;
            if (App.CurrentSpeaker != null)
                NavigationService.Navigate(new System.Uri("/SpeakerPage.xaml", System.UriKind.Relative));
        }
        private void Appbar_locate_OnClick(object sender, EventArgs e)
        {
            launch();
        }
        private async void launch()
        {
            var task = new BingMapsTask();
            task.Center = new GeoCoordinate(12.992620, 77.582566);
            task.ZoomLevel = 16;
            task.SearchTerm = "The Lalit";
            task.Show();
        }

        private void Appbar_navigate_OnClick(object sender, EventArgs e)
        {
            navigate();
        }

        private async void navigate()
        {
            
                var bingMapsDirectionsTask = new BingMapsDirectionsTask();
                bingMapsDirectionsTask.End = new LabeledMapLocation("The Lalit", new GeoCoordinate(12.992620, 77.582566));
                bingMapsDirectionsTask.Show();
            
        }
        private void ApplicationBarAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }
    }
}