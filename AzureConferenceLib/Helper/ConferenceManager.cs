using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AzureConferenceLib.Models;
using AzureConferenceLib.ViewModels;
using Newtonsoft.Json;

namespace AzureConferenceLib.Helper
{
    public class ConferenceManager : ModelBase
    {
        public static string JsonDataFilename = "sessions.json";

        public string SessionData = String.Empty;

        public static Dictionary<int, Session> Sessions = new Dictionary<int, Session>();

        public static Dictionary<int, Speaker> Speakers = new Dictionary<int, Speaker>();

        public void LoadFromString(string jsonString)
        {
#if WINDOWS_PHONE
            jsonString = jsonString.Replace("0000-04:00", ""); // HACK: excuse this timezone hack
            var sessions = JsonConvert.DeserializeObject<List<Session>>(jsonString);
            Sessions = new Dictionary<int, Session>();
            Speakers = new Dictionary<int, Speaker>();
            var obsSessions = new ObservableCollection<Session>();
            var obsSpeakers = new ObservableCollection<Speaker>();
            foreach (var session in sessions)
            {
                Sessions.Add(session.Id, session);
                obsSessions.Add(session);
                //Console.WriteLine("Session: " + session.Title);
                foreach (var speaker in session.Speakers)
                {
                    speaker.Sessions.Add(session);
                    if (!Speakers.ContainsKey(speaker.Id))
                    {
                        Speakers.Add(speaker.Id, speaker);
                        obsSpeakers.Add(speaker);
                    }
                    //Console.WriteLine("Speaker: " + speaker.Name);
                }
            }

            SessionList = obsSessions;
            SpeakerList = obsSpeakers;


#else
			var jsonObject = JsonValue.Parse (jsonString);
			
			if (jsonObject != null)
			{
				for (var j = 0;j < jsonObject.Count; j++) {
					var jsonSession = jsonObject[j];// as JsonValue;
					var session = new Session(jsonSession);
					
					Sessions.Add(session.Id, session);
					Console.WriteLine ("Session: " + session.Title);
					
					var jsonSpeakers = jsonSession["speakers"];// as JsonValue;
					
					for (var k = 0; k < jsonSpeakers.Count; k++) {
						var jsonSpeaker = jsonSpeakers[k]; // as JsonValue;
						var speaker = new Speaker(jsonSpeaker);
						
						if (!Speakers.ContainsKey (speaker.Id)) {
							Speakers.Add (speaker.Id, speaker);
						} else {
							speaker = Speakers[speaker.Id];
						}
						speaker.Sessions.Add (session);
						session.Speakers.Add (speaker);
						
						Console.WriteLine ("Speaker: " + speaker.Name);
					}
				}
			}
#endif
            //Console.WriteLine("done");
        }

        public ObservableCollection<Session> SessionList { get; set; }
        public ObservableCollection<Speaker> SpeakerList { get; set; }
        //private ObservableCollection<TwitterStatusItemModel> TwitterFeed;

        public event LoadEventHandler DataLoaded;

        private bool _sessionsAreNotOnlineYet;

        public bool SessionsAreNotOnlineYet
        {
            get { return _sessionsAreNotOnlineYet; }
            set
            {
                _sessionsAreNotOnlineYet = value;
                NotifyPropertyChanged("SessionsAreNotOnlineYet");
            }
        }


        public ObservableCollection<Session> GetSessions()
        {
            return SessionList;
        }

        async public Task<bool> GetData(string conferenceUrl, bool isJson)
        {
            SessionList = new ObservableCollection<Session>();
            SpeakerList = new ObservableCollection<Speaker>();
            DateTime sessionLastDownload = DateTime.MinValue;

            

            // Get the data from Isolated storage if it is there
            //if (IsolatedStorageSettings.ApplicationSettings.Contains("SessionData"))
            //{
            //    var converted = (IsolatedStorageSettings.ApplicationSettings["SessionData"] as IEnumerable<Session>);

            //    SessionList = converted.ToObservableCollection(SessionList);
            //    var loadedEventArgs = new LoadEventArgs { IsLoaded = true, Message = string.Empty };
            //    OnDataLoaded(loadedEventArgs);
            //}
            // Get the data from Isolated storage if it is there
            //if (IsolatedStorageSettings.ApplicationSettings.Contains("SpeakerData"))
            //{
            //    var converted = (IsolatedStorageSettings.ApplicationSettings["SpeakerData"] as IEnumerable<Speaker>);

            //    SpeakerList = converted.ToObservableCollection(SpeakerList);
            //    var loadedEventArgs = new LoadEventArgs { IsLoaded = true, Message = string.Empty };
            //    OnDataLoaded(loadedEventArgs);
            //}

            // Get the last time the data was downloaded.
            if (IsolatedStorageSettings.ApplicationSettings.Contains("SessionLastDownload"))
            {
                sessionLastDownload = (DateTime)IsolatedStorageSettings.ApplicationSettings["SessionLastDownload"];
            }

            // Check if we need to download the latest data, or if we can just use the isolated storage data
            // Cache the data for 2 hours
            if (!isJson && !String.IsNullOrEmpty(conferenceUrl) && conferenceUrl.StartsWith("http") && (sessionLastDownload.AddHours(2) < DateTime.Now || !IsolatedStorageSettings.ApplicationSettings.Contains("SessionData")))
            {
                var responseString = string.Empty;
                // Download the session data
                var httpClient = new HttpClient(new HttpClientHandler()); //new SharpGIS.GZipWebClient();
                var response = await httpClient.GetAsync(conferenceUrl);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync();
                LoadFromString(responseString);

                //conferenceWebClient.DownloadStringCompleted += conferenceWebClient_DownloadStringCompleted;
                //conferenceWebClient.DownloadStringAsync(new Uri(Settings.ConferenceDataUri));

                // Download speaker data
                //var speakerWebClient = new SharpGIS.GZipWebClient();
                //speakerWebClient.DownloadStringCompleted += speakerWebClient_DownloadStringCompleted;
                //speakerWebClient.DownloadStringAsync(new Uri(Settings.SpeakerServiceUri));
            }
            else if (!String.IsNullOrEmpty(conferenceUrl) && isJson)
            {
                LoadFromString(conferenceUrl);
            }
            return true;
        }

        private void conferenceWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            LoadFromString(e.Result);
        }

        //void sessionWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    try
        //    {
        //        var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConferenceService.SessionsResponse>>(e.Result);
        //        {
        //            DateTime tempTime;
        //            var converted = (from s in deserialized
        //                             orderby s.Code
        //                             select new Session
        //                             {
        //                                 Id = s.Id,
        //                                 Code = s.Code,
        //                                 Title = s.Title,
        //                                 Description = StripHtmlTags(s.Description),
        //                                 Location = s.Room,
        //                                 Date = (DateTime.TryParse(s.Starts, out tempTime) ? DateTime.Parse(s.Starts) : DateTime.MinValue),
        //                                 //Speakers = s.Speakers.Select(p => new Speaker { Id = p.SpeakerId, FirstName = p.First, LastName = p.Last, PictureUrl = p.SmallImage }).ToObservableCollection()

        //                                 //Build specific
        //                                 SpeakerIds = (s.SpeakerIds != null ? s.SpeakerIds.ToObservableCollection() : null),
        //                                 Link = s.Link,
        //                                 SlidesUri = s.Slides,
        //                                 Thumbnail = s.ThumbnailImage,
        //                                 WmvUri = s.Wmv,
        //                             }).ToList();

        //            // Display the data on the screen ONLY if we didn't already load from the cache
        //            // Don't bother about rebinding everything, just wait until the user launches the next time.
        //            if (SessionList.Count < 1)
        //                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
        //                {
        //                    SessionList = converted.ToObservableCollection(SessionList);
        //                    //SpeakerList = SessionList.SelectMany(p => p.Speakers).Distinct().OrderBy(p => p.SurnameFirstname).ToObservableCollection(SpeakerList);
        //                    var loadedEventArgs = new LoadEventArgs { IsLoaded = true, Message = string.Empty };
        //                    OnDataLoaded(loadedEventArgs);
        //                });

        //            // Make sure the sessions are online before caching
        //            if (converted.Count > 1)
        //            {
        //                // Save the results into the cache.
        //                // First save the data
        //                if (IsolatedStorageSettings.ApplicationSettings.Contains("SessionData"))
        //                    IsolatedStorageSettings.ApplicationSettings.Remove("SessionData");
        //                IsolatedStorageSettings.ApplicationSettings.Add("SessionData", converted);

        //                // then update the last updated key
        //                if (IsolatedStorageSettings.ApplicationSettings.Contains("SessionLastDownload"))
        //                    IsolatedStorageSettings.ApplicationSettings.Remove("SessionLastDownload");
        //                IsolatedStorageSettings.ApplicationSettings.Add("SessionLastDownload", DateTime.Now);
        //                IsolatedStorageSettings.ApplicationSettings.Save(); // trigger a save
        //            }
        //            else
        //                SessionsAreNotOnlineYet = true;
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
        //        {
        //            var loadedEventArgs = new LoadEventArgs { IsLoaded = false, Message = "There was a network error. Close the app and try again." };
        //            OnDataLoaded(loadedEventArgs);
        //            System.Windows.MessageBox.Show("There was a network error. Close the app and try again.");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //void speakerWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    try
        //    {
        //        var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConferenceService.SpeakerResponse>>(e.Result);
        //        {
        //            var converted = (from s in deserialized
        //                             orderby s.Last
        //                             select new Speaker
        //                             {
        //                                 Id = s.Id,
        //                                 Bio = StripHtmlTags(s.Bio ?? ""),
        //                                 FirstName = s.First,
        //                                 LastName = s.Last,
        //                                 PictureUrl = s.Photo,

        //                                 //Build specific
        //                                 SessionIds = (s.SessionIds != null ? s.SessionIds.ToObservableCollection() : null),

        //                             }).ToList();

        //            // Display the data on the screen ONLY if we didn't already load from the cache
        //            // Don't bother about rebinding everything, just wait until the user launches the next time.
        //            if (SpeakerList.Count < 1)
        //                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
        //                {
        //                    SpeakerList = converted.ToObservableCollection(SpeakerList);
        //                    var loadedEventArgs = new LoadEventArgs { IsLoaded = true, Message = string.Empty };
        //                    OnDataLoaded(loadedEventArgs);
        //                });

        //            // Make sure the sessions are online before caching
        //            if (converted.Count > 1)
        //            {
        //                // Save the results into the cache.
        //                // First save the data
        //                if (IsolatedStorageSettings.ApplicationSettings.Contains("SpeakerData"))
        //                    IsolatedStorageSettings.ApplicationSettings.Remove("SpeakerData");
        //                IsolatedStorageSettings.ApplicationSettings.Add("SpeakerData", converted);

        //                // then update the last updated key
        //                if (IsolatedStorageSettings.ApplicationSettings.Contains("SpeakerLastDownload"))
        //                    IsolatedStorageSettings.ApplicationSettings.Remove("SpeakerLastDownload");
        //                IsolatedStorageSettings.ApplicationSettings.Add("SpeakerLastDownload", DateTime.Now);
        //                IsolatedStorageSettings.ApplicationSettings.Save(); // trigger a save
        //            }
        //        }
        //    }
        //    catch (WebException)
        //    {
        //        System.Windows.Deployment.Current.Dispatcher.BeginInvoke(() =>
        //        {
        //            var loadedEventArgs = new LoadEventArgs { IsLoaded = false, Message = "There was a network error. Close the app and try again." };
        //            OnDataLoaded(loadedEventArgs);
        //            System.Windows.MessageBox.Show("There was a network error. Close the app and try again.");
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        protected virtual void OnDataLoaded(LoadEventArgs e)
        {
            if (DataLoaded != null)
            {
                DataLoaded(this, e);
            }
        }

        public ObservableCollection<Speaker> GetSpeakers()
        {
            return SpeakerList;
        }
        #region Twitter
        //public ObservableCollection<TwitterStatusItemModel> GetTwitterFeed(string TwitterId)
        //{
        //    TwitterFeed = new ObservableCollection<TwitterStatusItemModel>();
        //    WebClient wc = new WebClient();
        //    wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(TwitterStatusInformationCompleted);
        //    wc.DownloadStringAsync(new Uri(string.Format("{0}{1}", Settings.TwitterServiceUri, TwitterId)));
        //    return TwitterFeed;
        //}

        //void TwitterStatusInformationCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    try
        //    {
        //        var doc = XDocument.Parse(e.Result);
        //        foreach (var item in doc.Descendants("status"))
        //        {
        //            var model = new TwitterStatusItemModel()
        //            {
        //                Date = item.Element("created_at").Value.Substring(0, item.Element("created_at").Value.IndexOf('+')),
        //                Text = item.Element("text").Value,
        //            };
        //            TwitterFeed.Add(model);
        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }


        //    TwitterFeed = null;
        //}

        //private string StripHtmlTags(string value)
        //{
        //    const string HTML_TAG_PATTERN = "<.*?>";
        //    var result = Regex.Replace(value, HTML_TAG_PATTERN, string.Empty);
        //    return result;
        //}

        #endregion


#if !WINDOWS_PHONE
        public static void LoadFromFile ()
		{
			string xmlPath = JsonDataFilename;
			var basedir = Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			
			if (File.Exists (Path.Combine (basedir, JsonDataFilename))) {	// load a downloaded copy
				xmlPath = Path.Combine (basedir, JsonDataFilename);
			}
			
			var jsonString = File.ReadAllText(xmlPath);
			
			LoadFromString(jsonString);
		}
#endif
    }
}
