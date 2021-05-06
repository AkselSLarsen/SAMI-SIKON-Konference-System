using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using System;
using System.Threading.Tasks;

namespace SAMI_SIKON.Pages.Events {
    public class EventModel : PageModel {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Theme { get; set; }
        public string PictureSrc { get; set; }
        public string PictureAlt { get; set; }
        public int Seats { get; set; }

        public ICatalogue<Event> EventCatalogue { get; set; }

        public EventModel(ICatalogue<Event> eventCatalogue) {
            EventCatalogue = eventCatalogue;
        }

        public async Task OnGetAsync(int id) {
            //Event evt = await EventCatalogue.GetItem(new int[] { id });
            Event evt = new Event(1, 1, new System.Collections.Generic.List<Participant>(), DateTime.Now, "Lorem Ipsum", "Event Name Here", "Theme", 30, new bool[] { false, false, true});

            Name = evt.Name;
            Description = evt.Description;
            Theme = evt.Theme;
            PictureSrc = "../pictures/Auditorium.jpeg";
            PictureAlt = "Auditorium";
            //Seats = evt.SeatsLeft;
            Seats = 3;
        }
    }
}
