using System;
using System.Device.Location;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AzureConferenceLib.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace AzureConferenceWP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
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
            if (!String.IsNullOrEmpty(DeviceStatus.DeviceManufacturer) &&
                DeviceStatus.DeviceManufacturer.Equals("nokia", StringComparison.InvariantCultureIgnoreCase))
            {
                string launchNokiaMaps =
                    "explore-maps://v2.0/show/place/?latlon=12.992620,77.582566&zoom=16&title=The%20Lalit";
                await Windows.System.Launcher.LaunchUriAsync(new Uri(launchNokiaMaps));
            }
            else
            {
                var task = new MapsTask();
                task.Center = new GeoCoordinate(12.992620, 77.582566);
                task.ZoomLevel = 16;
                task.SearchTerm = "The Lalit";
                task.Show();
            }
        }

        private void Appbar_navigate_OnClick(object sender, EventArgs e)
        {
            navigate();
        }

        private async void navigate()
        {
            if (!String.IsNullOrEmpty(DeviceStatus.DeviceManufacturer) &&
                DeviceStatus.DeviceManufacturer.Equals("nokia", StringComparison.InvariantCultureIgnoreCase))
            {
                string navigateNokiaMaps =
                    "guidance-drive://v2.0/navigate/destination/?latlon=12.992620,77.582566&zoom=16&title=The%20Lalit";
                await Windows.System.Launcher.LaunchUriAsync(new Uri(navigateNokiaMaps));
            }
            else
            {
                var mapsDirectionsTask = new MapsDirectionsTask();
                mapsDirectionsTask.End = new LabeledMapLocation("The Lalit", new GeoCoordinate(12.992620, 77.582566));
                mapsDirectionsTask.Show();
            }
        }

        private void ApplicationBarAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/YourLastAboutDialog;component/AboutPage.xaml", UriKind.Relative));
        }
    }
}