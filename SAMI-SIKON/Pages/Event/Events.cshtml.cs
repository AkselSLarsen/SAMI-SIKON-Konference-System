using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using System.Collections.Generic;

namespace SAMI_SIKON.Pages.Event {
    public class EventsModel : PageModel {
        /
        [BindProperty]
        public int RoomNr { get; set; }
        [BindProperty]
        public string Criteria { get; set; }
        public List<Event> SortedEvents { get; set; }

        private ICatalogue<Room> Rooms { get; set; }
        private ICatalogue<Event> Events { get; set; }

        public EventsModel(ICatalogue<Room> rooms, ICatalogue<Event> events) {
            Rooms = rooms;
            Events = events;
        }

        public void OnGet() {
            List<Event> tmp = new List<Event>();
            
            if(Criteria == null || Criteria == "") {
                tmp = Events.ReadAll();
            } else {
                foreach(Event evt in Events.ReadAll()) {
                    bool add = false;

                    if(evt.Name.ToLower().Contains(Criteria.ToLower())) {
                        add = true;
                    } else if(evt.Theme.ToLower().Contains(Criteria.ToLower())) {
                        add = true;
                    }

                    if(evt.RoomNr != RoomNr) {
                        add = false;
                    }

                    if(add) {
                        SortedEvents.Add(evt);
                    }
                }
            }

            SortedEvents.Sort();
        }

        public IActionResult OnPostSort() {
            return Page();
        }
        */
    }
}
