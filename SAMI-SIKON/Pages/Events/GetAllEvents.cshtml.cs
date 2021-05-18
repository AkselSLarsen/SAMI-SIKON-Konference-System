using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;

namespace SAMI_SIKON.Pages.Events
{
    public class GetAllEventsModel : PageModel
    {
        [BindProperty]
        public int RoomNr { get; set; }
        [BindProperty]
        public string Criteria { get; set; }
        public List<Event> SortedEvents { get; set; }

        private ICatalogue<Room> Rooms { get; set; }
        private ICatalogue<Event> Events { get; set; }

        public GetAllEventsModel(ICatalogue<Room> rooms, ICatalogue<Event> events)
        {
            Rooms = rooms;
            Events = events;
            SortedEvents = new List<Event>();
        }

        public async Task OnGetAsync()
        {
            List<Event> tmp = new List<Event>();

            if (Criteria == null || Criteria == "")
            {
                tmp = await Events.GetAllItems();
            }
            else
            {
                foreach (Event evt in await Events.GetAllItems())
                {
                    bool add = false;

                    if (evt.Name.ToLower().Contains(Criteria.ToLower()))
                    {
                        add = true;
                    }
                    else if (evt.Theme.ToLower().Contains(Criteria.ToLower()))
                    {
                        add = true;
                    }

                    if (evt.RoomNr != RoomNr)
                    {
                        add = false;
                    }

                    if (add)
                    {
                        SortedEvents.Add(evt);
                    }
                }
            }

            SortedEvents.Sort();
        }

        public IActionResult OnPostSort()
        {
            return Page();
        }
    }
}
