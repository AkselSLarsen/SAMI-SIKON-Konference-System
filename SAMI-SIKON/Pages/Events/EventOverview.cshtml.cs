using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Pages.Events
{
    public class EventOverviewModel : PageModel
    {
        [BindProperty]
        public string FilterCriteria { get; set; }
        [BindProperty]
        public int Event_Id { get; set; }
        [BindProperty]
        public int RoomNr { get; set; }
        private ICatalogue<Room> Rooms { get; set; }

        public List<Event> Events { get; private set; }
        public List<Event> SortedEvents { get; set; }


        public EventOverviewModel(ICatalogue<Event> eventcat)
        {
            eventCatalogue = eventcat;
            SortedEvents = new List<Event>();

        }

        private ICatalogue<Event> eventCatalogue;

        public async Task OnGetAsync()
        {

            if (FilterCriteria == null || FilterCriteria == "")
            {
                SortedEvents = await eventCatalogue.GetAllItems();
            }
            else
            {
                foreach (Event evt in await eventCatalogue.GetAllItems())
                {
                    bool add = false;

                    if (evt.Name.ToLower().Contains(FilterCriteria.ToLower()))
                    {
                        add = true;
                    }
                    else if (evt.Theme.ToLower().Contains(FilterCriteria.ToLower()))
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

            SortedEvents.Sort(Comparer<Event>.Create((x, y) => x.Name.CompareTo(y.Name)));
        }

        public async Task OnPostDeleteAsync()
        {
            await eventCatalogue.DeleteItem(new int[] {Event_Id});
        }
        public IActionResult OnPostSort()
        {
            return Page();
        }
    }
}
