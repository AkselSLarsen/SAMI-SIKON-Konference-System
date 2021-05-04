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

        /// <summary>
        /// How many percent of the screen height should each minute corrospond to?
        /// 100/1440 would be 100 percent of the screen height for 24 hours,
        /// while 100/480 would be 100 percent of the screen height for 8 hours.
        /// Keep in mind that both the browser, the header and the footer will take some screen space.
        /// </summary>
        public static double TimeScale = 0.2;
        /// <summary>
        /// 
        /// </summary>
        public static double ViewStart = 480;
        /// <summary>
        /// 
        /// </summary>
        public static double ViewStop = 960;


        private List<List<Event>> _tracks;

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
            for(int i=0; i<25; i++) {
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
    }
}
