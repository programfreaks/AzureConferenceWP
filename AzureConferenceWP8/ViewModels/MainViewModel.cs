using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using AzureConferenceLib.Helper;
using AzureConferenceLib.Models;
using AzureConferenceLib.ViewModels;
using AzureConferenceWP8.Resources;

namespace AzureConferenceWP8.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            LoadData();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        /// 
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private ObservableCollection<Session> _sessions;
        public ObservableCollection<Session> Sessions
        {
            get
            {
                return _sessions;
            }
            set
            {
                if (value != _sessions)
                {
                    _sessions = value;
                    NotifyPropertyChanged("Sessions");
                }
            }
        }


        private ObservableCollection<Speaker> _speakers;
        public ObservableCollection<Speaker> Speakers
        {
            get
            {
                return _speakers;
            }
            set
            {
                if (value != _speakers)
                {
                    _speakers = value;
                    NotifyPropertyChanged("Speakers");
                }
            }
        }

        private ObservableCollection<Session> _savedSessions;
        public ObservableCollection<Session> SavedSessions
        {
            get
            {
                return _savedSessions;
            }
            set
            {
                if (value != _savedSessions)
                {
                    _savedSessions = value;
                    NotifyPropertyChanged("SavedSessions");
                }
            }
        }

        public IEnumerable<Group<Session>> SessionsByTimeSlot { get; private set; }
        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            var manager = new ConferenceManager();
            //var sessionFile = await Windows.Storage.StorageFile.GetFileFromPathAsync("/SampleData/Sessions.txt");
            if (String.IsNullOrEmpty(App.sessionData))
            {
                var sessionData = HelperMethods.ReadFile(Settings.StaticData);
                App.sessionData = sessionData;
                var res = await manager.GetData(sessionData, true);
            }
            else
            {
                await manager.GetData(App.sessionData, true);   
            }
            //var res = await manager.GetData(Settings.ConferenceDataUri);
            //Service.GetData();

            Sessions = manager.GetSessions();
            Speakers = manager.GetSpeakers();
            if(App.SavedSessionIds == null)
                App.SavedSessionIds = new List<int>();
            else if (App.SavedSessionIds != null && App.SavedSessionIds.Count > 0)
            {
                App.SavedSessions = new ObservableCollection<Session>();
                foreach (var sessionId in App.SavedSessionIds)
                {
                    int id = sessionId;
                    var session = Sessions.FirstOrDefault(s => s.Id == id);
                    if(session != null)
                        App.SavedSessions.Add(session);
                }
            }
            //if (Service.SessionsAreNotOnlineYet)
            //    DiscoveredThatSessionsAreNotOnlineYet = true;

            //needs to get it from iso if available...
            App.Sessions = Sessions;
            App.Speakers = Speakers;

            if (App.SavedSessions == null)
                App.SavedSessions = new ObservableCollection<Session>();

            SavedSessions = App.SavedSessions;


            // Sample data; replace with real data
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime one", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime two", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}