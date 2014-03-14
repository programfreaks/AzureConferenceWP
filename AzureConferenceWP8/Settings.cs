namespace AzureConferenceWP8
{
    public class Settings
    {
        public const string SessionServiceUri = @"http://channel9.msdn.com/events/build/2012/sessions";
        //public const string SessionServiceUri = "https://eup84q.blu.livefilestore.com/y1piNwHiDJPW_mojkUSUKd6nfTndoiAT_h6d10zApUKusCfdgEEYUzWmjW6ABJNzX3Tl40cas3I8REzKw6UbzBsbA/ete2011.json?download&psid=1";
        public const string SpeakerServiceUri = @"http://channel9.msdn.com/events/build/2012/speakers?json=true";
        public const string TwitterServiceUri = "http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=";
        public const string ConferenceDataUri = @"http://www.programfreaks.com/sessions.txt";
        public const string StaticData = @"SampleData/sessiondetails.json";
        public const string ApplicationName = "Build 2012";
        public const string EmailAddress = "David.Burela@gmail.com";
        public const string Subject = "please provide feedback";
        public const string DefaulImage = "/Images/defaultimage.png";
    }
}
