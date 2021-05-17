using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public List<Event> Events { get; private set; }

        private EventCatalogue eventCatalogue;

        public async Task OnGetMyEvents(int event_Id)
        {
            Events = await eventCatalogue.GetAllItems();
            //Event_Id = event_Id;
            //if (!String.IsNullOrEmpty(FilterCriteria))
            //{
            //    Events = await eventCatalogue.GetItemsWithKey(Event_Id);
            //}
            //else
            //    Events = await eventCatalogue.GetAllItems();
        }
    }
}
