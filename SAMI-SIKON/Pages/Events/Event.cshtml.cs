using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;
using System;
using System.Threading.Tasks;

namespace SAMI_SIKON.Pages.Events {
    public class EventModel : PageModel {

        public int EventNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Theme { get; set; }
        public string PictureSrc { get; set; }
        public string PictureAlt { get; set; }
        public int Seats { get; set; }

        public ICatalogue<Event> Events { get; set; }
        public ICatalogue<IUser> Users { get; set; }

        public EventModel(ICatalogue<Event> eventCatalogue, ICatalogue<IUser> userCatalogue) {
            Events = eventCatalogue;
            Users = userCatalogue;
        }

        public async Task OnGetAsync(int id) {
            Event evt = await Events.GetItem(new int[] { id });

            EventNumber = id;
            Name = evt.Name;
            Description = evt.Description;
            Theme = evt.Theme;
            PictureSrc = "../pictures/Auditorium.jpeg";
            PictureAlt = "Auditorium";
            Seats = evt.SeatsLeft;
        }

        public IActionResult OnPostBook() {
            if(UserCatalogue.CurrentUser == null) {
                return Redirect("~/Login/LoginPage");
            }

            Booking booking = new Booking(0, EventNumber, 0);

            UserCatalogue.CurrentUser.Bookings.Add(booking);
            Users.UpdateItem(UserCatalogue.CurrentUser, new int[] { UserCatalogue.CurrentUser.Id });
            return Page();
        }

        public IActionResult OnPostChooce() {
            return Redirect($"~/Rooms?id={EventNumber}");
        }
    }
}
