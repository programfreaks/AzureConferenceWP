using AzureConferenceLib.Models;

namespace AzureConferenceWP.ViewModels
{
    public class SpeakerViewModel
    {
        public Speaker Speaker { get; private set; }
        //public ObservableCollection<TwitterStatusItemModel> Twitter { get; private set; }

        public SpeakerViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            Speaker = App.CurrentSpeaker;

                // App.Sessions.Where(p => p.Speakers==Speaker.Id).ToObservableCollection();
            //Twitter = Service.GetTwitterFeed(Speaker.Twitter);
        }
    }
}
