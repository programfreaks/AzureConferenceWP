using System;
using System.Collections.ObjectModel;
using AzureConferenceLib.ViewModels;

namespace AzureConferenceLib.Models
{
    public class Session : ModelBase
    {
        public Session()
        {
            Speakers = new ObservableCollection<Speaker>();
        }

#if !WINDOWS_PHONE
		public Session (JsonValue json) : this()
		{
			Id = json["id"];
			Title = json["title"];
			Abstract = json["abstract"];
			Location = json["location"];

			var begins = json["begins"].ToString().Trim ('"').Substring (0, 19).Replace ("T", " ");
			Begins = DateTime.Parse (begins);
			var ends = json["ends"].ToString ().Trim ('"').Substring (0, 19).Replace ("T", " ");
			Ends = DateTime.Parse (ends, System.Globalization.CultureInfo.InvariantCulture);
		}
#endif
        //public int Id { get; set; }
        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                    return;
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        private string _title { get; set; }
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value)
                    return;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
        private string _abstract { get; set; }
        public string Abstract
        {
            get
            {
                return _abstract;
            }
            set
            {
                if (value != _abstract)
                {
                    _abstract = value;
                    NotifyPropertyChanged("Abstract");
                }
            }
        }
        private string _location { get; set; }
        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (value != _location)
                {
                    _location = value;
                    NotifyPropertyChanged("Location");
                }
            }
        }
        private DateTime _begins { get; set; }
        public DateTime Begins
        {
            get
            {
                return _begins;
            }
            set
            {
                if (value != _begins)
                {
                    _begins = value;
                    NotifyPropertyChanged("Begins");
                }
            }
        }
        private DateTime _ends { get; set; }
        public DateTime Ends
        {
            get
            {
                return _ends;
            }
            set
            {
                if (value != _ends)
                {
                    _ends = value;
                    NotifyPropertyChanged("Ends");
                }
            }
        }

        public string LocationDisplay
        {
            get
            {
                if (Location.ToLower().Contains("tbd")
                    || Location.ToLower().Contains("room")
                    || Location.ToLower().Contains("hall")
                    || Title.ToLower().Contains("party"))
                    return Location;
                else
                    return Location + " room";
            }
        }

        // compat
        public string Code { get { return Id.ToString(); } }
        public DateTime Start { get { return Begins; } }
        public DateTime End { get { return Ends; } }
        public string DateTimeDisplay { get { return Begins.ToString("ddd MMM dd H:mm"); } }
        public string DateTimeQuickJumpDisplay
        {
            get { return Begins.ToString("ddd MMM dd H:mm"); }
        }
        public string DateTimeQuickJumpSubtitle
        {
            get { return Begins.ToString("ddd MMM dd H:mm"); }
        }
        public string TimeQuickJumpDisplay
        {
            get { return Start.ToString("ddd H:mm"); }
        }
        public bool HasTag(string tag)
        {
            return false;
        }

        //public List<Speaker> Speakers { get; set; }
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

        public string SpeakerList { get { return GetSpeakerList(); } }
        public string GetSpeakerList()
        {
            var speakers = "";
            foreach (var s in _speakers)
            {
                speakers += s.Name + ", ";
            }
            return speakers.TrimEnd(new char[] { ' ', ',' });
        }
    }
}
