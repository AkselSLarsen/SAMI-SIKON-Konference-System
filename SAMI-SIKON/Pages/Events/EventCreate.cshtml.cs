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
    public class EventCreateModel : PageModel
    {
        [BindProperty]
        public Event evnt { get; set; }

        private ICatalogue<Event> eventCatalogue;
        private ICatalogue<Room> RoomCatalogue;
        private ICatalogue<Participant> userCatalogue;

        public List<Room> listRoom = new List<Room>();
        public List<Participant> listPart = new List<Participant>();


        public EventCreateModel(ICatalogue<Event> eventcat, ICatalogue<Room> roomcat/*, ICatalogue<Participant> particat*/)
        {
            this.eventCatalogue = eventcat;
            this.RoomCatalogue = roomcat;
            //this.userCatalogue = particat;
        }
        public async Task OnGetAsync()
        {
            listRoom = await RoomCatalogue.GetAllItems();
            //listPart = await userCatalogue.GetAllItems();
        }

        public async Task<IActionResult> OnPostAsync(Event evnt)
        {
            await eventCatalogue.CreateItem(evnt);
            return RedirectToPage("EventOverview");
        }
    }
}
