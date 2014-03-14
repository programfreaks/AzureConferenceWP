using System.Collections.ObjectModel;
using AzureConferenceLib.Helper;
using AzureConferenceLib.Models;

namespace AzureConferenceWP8.ViewModels
{
    public class SessionViewModel
    {
        public ObservableCollection<Speaker> Speakers { get; private set; }
        public Session Session { get; private set; }

        public SessionViewModel()
        {
            LoadData();
        }

        public void LoadData()
        {
            if (App.CurrentSession != null)
            {
                Session = App.CurrentSession;
                Speakers = Session.Speakers.ToObservableCollection();

            }
        }
    }
}
