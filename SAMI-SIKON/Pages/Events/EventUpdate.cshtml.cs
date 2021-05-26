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
    public class EventUpdateModel : PageModel
    {
        [BindProperty]
        public Event evnt { get; set; }

        ICatalogue<Event> eventService;

        public EventUpdateModel(ICatalogue<Event> service)
        {
            this.eventService = service;
        }

        public async Task OnGetAsync(int id)
        {
            evnt = await eventService.GetItem(new int[id]);
        }

        public async Task<IActionResult> OnPostUpdateAsync(Event evnt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await eventService.UpdateItem(evnt, new int[evnt.Id]);
            return RedirectToPage("GetAllHotels");
        }
    }
}
