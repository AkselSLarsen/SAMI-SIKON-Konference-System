using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SAMI_SIKON.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Pages {
    public class IndexModel : PageModel {
        private List<List<Event>> _tracks;
        private double _viewStart = -1;
        private double _viewStop = -1;

        private double ViewStart {
            get {
                if(_viewStart == -1) {
                    double d = int.MaxValue;
                    foreach (List<Event> track in Tracks) {
                        foreach (Event evt in track) {
                            if (d > evt.StartTime.TimeOfDay.TotalMinutes) {
                                d = (int)evt.StartTime.TimeOfDay.TotalMinutes;
                            }
                        }
                    }
                    _viewStart = d;
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
                                d = (int)evt.StopTime.TimeOfDay.TotalMinutes;
                            }
                        }
                    }
                    _viewStop = d;
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

        [BindProperty]
        public List<List<Event>> Tracks {
            get { return _tracks; }
            set { _tracks = value; NrOfTracks = _tracks.Count; }
        }
        [BindProperty]
        public int NrOfTracks { get; set; }

        public IndexModel() {

        }

        public void OnGet() {
            List<Event> events = new List<Event>();
            for(int i=0; i<10; i++) {
                events.Add(new Event(true));
            }

            EventTrackAssigner eta = new EventTrackAssigner(events);

            Tracks = eta.Tracks;
        }

        public string TrackWidth() {
            return (90 / NrOfTracks) + "%";
        }

        public string ThemeToColor(Event evt) {

            if(evt.Theme == "a") {
                return "antiquewhite";
            }

            if (evt.Theme == "b") {
                return "gainsboro";
            }

            return "lightblue";
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
    }
}
