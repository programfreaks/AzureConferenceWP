using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AzureConferenceLib.Models;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AzureConferenceWP
{
    public partial class SpeakerPage : PhoneApplicationPage
    {
        public SpeakerPage()
        {
            InitializeComponent();
        }

        private void SessionList_ItemClicked(object sender, Infragistics.Controls.Grids.ListItemEventArgs e)
        {
            App.CurrentSession = e.Item.Data as Session;
            if (App.CurrentSession != null)
                NavigationService.Navigate(new System.Uri("/SessionPage.xaml", System.UriKind.Relative));
        }
    }
}