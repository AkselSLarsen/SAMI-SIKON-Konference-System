using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Pages {
    public class IndexModel : PageModel {
        private List<List<Event>> _tracks;
        private double _viewStart = -1;
        private double _viewStop = -1;
        private int _year = -1;
        private int _month = -1;
        private int _day = -1;

        private double ViewStart {
            get {
                if(_viewStart == -1) {
                    double d = int.MaxValue;
                    foreach (List<Event> track in Tracks) {
                        foreach (Event evt in track) {
                            if (d > evt.StartTime.TimeOfDay.TotalMinutes) {
                                d = evt.StartTime.TimeOfDay.TotalMinutes;
                            }
                        }
                    }
                    if (d == int.MaxValue) {
                        _viewStart = 0;
                    } else {
                        _viewStart = d;
                    }
                }
                return _viewStart;
            }
        }
        private double ViewStop {
            get {
                if(_viewStop == -1) {
                    double d = int.MinValue;
                    foreach (List<Event> track in Tracks) {
                        foreach (Event evt in track) {
                            if (d < evt.StopTime.TimeOfDay.TotalMinutes) {
                                d = evt.StopTime.TimeOfDay.TotalMinutes;
                            }
                        }
                    }
                    if(d == int.MinValue) {
                        _viewStop = 0;
                    } else {
                        _viewStop = d;
                    }
                }
                return _viewStop;
            }
        }
        /// <summary>
        /// How many percent of the screen height should each minute corrospond to?
        /// 100/1440 would be 100 percent of the screen height for 24 hours,
        /// while 100/480 would be 100 percent of the screen height for 8 hours.
        /// Keep in mind that both the browser, the header and the footer will take some screen space.
        /// </summary>
        public double TimeScale {
            get {
                return 100.0 / (ViewStop - ViewStart);
            }
        }


        [FromQuery(Name = "year")]
        public int Year {
            get { return _year; }
            set { _year = value; }
        }
        [FromQuery(Name = "month")]
        public int Month {
            get { return _month; }
            set { _month = value; }
        }
        [FromQuery(Name = "day")]
        public int Day {
            get { return _day; }
            set { _day = value; }
        }
        [BindProperty]
        public DateTime Date { get; set; }

        [BindProperty]
        public List<List<Event>> Tracks {
            get { return _tracks; }
            set { _tracks = value; NrOfTracks = _tracks.Count; }
        }
        [BindProperty]
        public int NrOfTracks { get; set; }

        [BindProperty]
        public RoomCatalogue Rooms { get; set; }
        [BindProperty]
        public UserCatalogue Users { get; set; }
        [BindProperty]
        public List<Event> Events { get; set; }

        public IndexModel(ICatalogue<Room> rooms, ICatalogue<IUser> users, ICatalogue<Event> events) {
            Rooms = (RoomCatalogue)rooms;
            Users = (UserCatalogue)users;
            Events = events.GetAllItems().Result;
        }

        public void OnGetAsync() {
            if(Year == -1 || Month == -1 || Day == -1) {
                Date = GetClosestDate();
            } else {
                Date = new DateTime(Year, Month, Day);
            }
            List<Event> evts = GetEventsWithDate();
            EventTrackAssigner eta = new EventTrackAssigner(evts);

            Tracks = eta.Tracks;
        }

        public IActionResult OnPostFirst() {
            Date = getFirstDate();

            return Redirect($"~/?year={Date.Year}&month={Date.Month}&day={Date.Day}");
        }

        public IActionResult OnPostPrevious() {
            Date = Date.AddDays(-1);

            return Redirect($"~/?year={Date.Year}&month={Date.Month}&day={Date.Day}");
        }

        public IActionResult OnPostToday() {
            Date = DateTime.Today;
            return Redirect($"~/?year={Date.Year}&month={Date.Month}&day={Date.Day}");
        }

        public IActionResult OnPostNext() {
            Date = Date.AddDays(1);

            return Redirect($"~/?year={Date.Year}&month={Date.Month}&day={Date.Day}");
        }

        public IActionResult OnPostLast() {
            Date = getLastDate();

            return Redirect($"~/?year={Date.Year}&month={Date.Month}&day={Date.Day}");
        }

        public string TrackWidth() {
            return (94 / NrOfTracks) + "%";
        }

        public string GetColor(Event evt) {
            IUser user = UserCatalogue.CurrentUser;
            bool booked = false;

            if(user != null) {
                foreach (Booking b in user.Bookings) {
                    if (b.Event_Id == evt.Id) {
                        booked = true;
                    }
                }
            }

            if(booked) {
                return "beige";
            } else {
                return "azure";
            }
        }

        public async Task<string> GetEventTooltip(Event evt) {
            string re = evt.Name;
            re += "\nOplægsholdere:\n";

            foreach(Participant speaker in await evt.FindSpeakers()) {
                re += speaker.Name + "\n";
            }

            if(evt.Theme != null) {
                re += "Tema: " + evt.Theme;
            }

            return re;
        }

        public string GetDuration(Event evt) {
            double d = evt.StopTime.TimeOfDay.TotalMinutes - evt.StartTime.TimeOfDay.TotalMinutes;
            string s = string.Format("{0:N2}", d * TimeScale).Replace(',', '.') + "vh";
            return s;
        }

        public string GetOffset(Event evt) {
            double d = evt.StartTime.TimeOfDay.TotalMinutes - ViewStart;
            string s = string.Format("{0:N2}", d * TimeScale).Replace(',', '.') + "vh";
            return s;
        }

        public string GetStartTime(Event evt) {
            TimeSpan ts = evt.StartTime.TimeOfDay;
            string re = "";
            if (ts.Hours < 10) { re += "0"; }
            re += ts.Hours + ":";
            if (ts.Minutes < 10) { re += "0"; }
            re += ts.Minutes;
            return re;
        }

        public string GetStopTime(Event evt) {
            TimeSpan ts = evt.StopTime.TimeOfDay;
            string re = "";
            if (ts.Hours < 10) { re += "0"; }
            re += ts.Hours + ":";
            if (ts.Minutes < 10) { re += "0"; }
            re += ts.Minutes;
            return re;
        }

        public int[] GetHourIndicators() {
            int startHour = (int)(ViewStart / 60);
            int stopHour = (int)(ViewStop / 60);

            int[] re = new int[(stopHour-startHour)+1];
            for(int i=startHour; i<=stopHour; i++) {
                re[i - startHour] = i;
            }
            return re;
        }

        public string GetTimeIndicatorOffset(int i) {
            double offset = (i - (ViewStart / 60)) * 60;
            return string.Format("{0:N2}", offset * TimeScale).Replace(',', '.') + "vh";
        }

        private List<DateTime> GetDates() {
            List<Event> evts = Events;
            List<DateTime> dates = new List<DateTime>();

            foreach (Event evt in evts) {
                dates.Add(evt.StartTime.Date);
            }
            return dates;
        }

        private DateTime GetClosestDate() {
            List<DateTime> dates = GetDates();
            DateTime re = dates[0];
            DateTime now = DateTime.Now;
            foreach(DateTime date in dates) {
                if ((date - now).Duration().TotalMinutes < (re - now).Duration().TotalMinutes) {
                    re = date;
                }
            }
            return re;
        }

        private DateTime getFirstDate() {
            List<DateTime> dates = GetDates();
            DateTime re = dates[0];
            foreach (DateTime date in dates) {
                if (re.CompareTo(date) > 0) {
                    re = date;
                }
            }
            return re;
        }

        private DateTime getLastDate() {
            List<DateTime> dates =GetDates();
            DateTime re = dates[0];
            foreach (DateTime date in dates) {
                if (re.CompareTo(date) < 0) {
                    re = date;
                }
            }
            return re;
        }

        private List<Event> GetEventsWithDate() {
            List<Event> evts = Events;
            List<Event> re = new List<Event>();

            foreach (Event evt in evts) {
                if(evt.StartTime.Date.CompareTo(Date) == 0) {
                    re.Add(evt);
                }
            }
            return re;
        }
    }
}
