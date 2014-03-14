using System;

namespace AzureConferenceLib.Helper
{
    public delegate void LoadEventHandler(object sender, LoadEventArgs e);

    public class LoadEventArgs : EventArgs
    {
        public LoadEventArgs() : base() { }

        public LoadEventArgs(bool IsLoaded, string Message)
        {
            this.IsLoaded = IsLoaded;
            this.Message = Message;
        }
        public bool IsLoaded { get; set; }
        public string Message { get; set; }
    }
}
