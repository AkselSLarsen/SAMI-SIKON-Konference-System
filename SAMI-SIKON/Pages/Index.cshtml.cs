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
            for(int i=0; i<100; i++) {
                events.Add(new Event(true));
            }

            EventTrackAssigner eta = new EventTrackAssigner(events);

            Tracks = eta.Tracks;
        }


        public string TrackWidth() {
            return (90 / NrOfTracks) + "vw";
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
    }
}
