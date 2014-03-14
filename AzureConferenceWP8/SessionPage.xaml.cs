using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using AzureConferenceWP8.ViewModels;
using Infragistics.Controls.Interactions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Reminder = Microsoft.Phone.Scheduler.Reminder;

namespace AzureConferenceWP8
{
    public partial class SessionPage : PhoneApplicationPage
    {
        public SessionPage()
        {
            InitializeComponent();
            if(App.CurrentSession!=null && (App.CurrentSession.Speakers == null || App.CurrentSession.Speakers.Count == 0))
                SpeakerPanoramaItem.Visibility = Visibility.Collapsed;
        }

        private void appbar_add_Click(object sender, EventArgs e)
        {
            var vm = (SessionViewModel)this.LayoutRoot.DataContext;
            bool found = false;

            foreach (var s in App.SavedSessions)
            {
                if (s.Title.Trim().Equals(vm.Session.Title.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    App.SavedSessions.Remove(s);
                    App.SavedSessionIds.Remove(vm.Session.Id);
                    found = false;
                    break;
                }
            }
            App.SavedSessions.Add(vm.Session);
            App.SavedSessionIds.Add(vm.Session.Id);
            //only add a reminder one time
            if (!found)
            {
                try
                {
                    Reminder reminder = new Reminder(vm.Session.Title);
                    reminder.BeginTime = vm.Session.Begins.AddMinutes(-5); // Add a reminder 5 mins before it starts
                    reminder.Content = vm.Session.Title;
                    reminder.RecurrenceType = RecurrenceInterval.None;

                    ScheduledActionService.Add(reminder);
                    XamMessageBox.Show("Favourite added", "Added to your favorites.",
                        () => { },
                        VerticalPosition.Center,
                        new XamMessageBoxCommand("OK", () => { }));
                }
                catch (Exception)
                {
                    //need to let use know we cound not add a reminder?
                    XamMessageBox.Show("Already a favorite", "This session is already a favorite.",
                        () => { },
                        VerticalPosition.Center,
                        new XamMessageBoxCommand("OK", () => { }));
                }

            }
        }

        private void appbar_send_Click(object sender, EventArgs e)
        {
            var vm = (SessionViewModel)this.LayoutRoot.DataContext;
            var session = vm.Session;

            var email = new EmailComposeTask();
            email.Subject = "This session at Azure Conference 2014 might be of interest";
            var sb = new StringBuilder();
            sb.Append("Title: ");
            sb.AppendLine(session.Title);
            sb.Append("Time: ");
            sb.AppendLine(session.Begins.ToShortDateString() + " " + session.Ends.ToShortTimeString());
            sb.Append("Location: ");
            sb.AppendLine(session.Location);
            sb.Append("Description: ");
            sb.AppendLine(session.Abstract);
            email.Body = sb.ToString();

            email.Show();


        }

        private void appbar_add_calendar_Click(object sender, EventArgs e)
        {
            var vm = (SessionViewModel)this.LayoutRoot.DataContext;
            var session = vm.Session;
            var saveAppointmentTask = new SaveAppointmentTask();
            saveAppointmentTask.StartTime = session.Begins;
            saveAppointmentTask.EndTime = session.Ends;
            saveAppointmentTask.Subject = session.Title;
            saveAppointmentTask.Location = session.Location;
            saveAppointmentTask.Details = session.Abstract;
            saveAppointmentTask.IsAllDayEvent = false;
            saveAppointmentTask.Reminder = Microsoft.Phone.Tasks.Reminder.FifteenMinutes;
            saveAppointmentTask.AppointmentStatus = Microsoft.Phone.UserData.AppointmentStatus.Busy;
            saveAppointmentTask.Show();
        }
    }
}