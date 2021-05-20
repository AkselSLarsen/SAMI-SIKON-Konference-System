using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SAMI_SIKON.Pages.Events {
    public class EventModel : PageModel {

        [BindProperty]
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

        public async Task<IActionResult> OnPostBook() {
            if(UserCatalogue.CurrentUser == null) {
                return Redirect("~/Login/LoginPage");
            }

            Booking booking = new Booking(0, EventNumber, null);

            UserCatalogue.CurrentUser.Bookings.Add(booking);
            await Users.UpdateItem(UserCatalogue.CurrentUser, new int[] { UserCatalogue.CurrentUser.Id });
            return Redirect("~/");
        }

        public IActionResult OnPostChooce() {
            return Redirect($"~/Rooms?id={EventNumber}");
        }

        public async Task<IActionResult> OnPostUnbook() {
            Booking toDelete = null;
            foreach(Booking booking in UserCatalogue.CurrentUser.Bookings) {
                if(booking.Event_Id == EventNumber) {
                    toDelete = booking;
                }
            }
            if(toDelete != null) { UserCatalogue.CurrentUser.Bookings.Remove(toDelete); }

            await Users.UpdateItem(UserCatalogue.CurrentUser, new int[] { UserCatalogue.CurrentUser.Id });
            return Redirect("~/");
        }

        public bool HasBooking() {
            if (UserCatalogue.CurrentUser != null) {
                List<Booking> bookings = UserCatalogue.CurrentUser.Bookings;
                foreach (Booking booking in bookings) {
                    if (booking.Event_Id == EventNumber) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool HasSeat() {
            if (UserCatalogue.CurrentUser != null) {
                List<Booking> bookings = UserCatalogue.CurrentUser.Bookings;
                foreach (Booking booking in bookings) {
                    if (booking.Event_Id == EventNumber) {
                        if (booking.Seat_Nr != null && booking.Seat_Nr != 0) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> CanBook() {
            if (UserCatalogue.CurrentUser != null) {
                if (Seats <= 0 || UserCatalogue.CurrentUser is Administrator) {
                    return false;
                }
                Event evt = await Events.GetItem(new int[] { EventNumber });
                bool overlaps = false;

                List<Booking> bookings = UserCatalogue.CurrentUser.Bookings;
                foreach (Booking booking in bookings) {
                    Event e = await booking.FindEvent();
                    if(e.Overlaps(evt)) {
                        overlaps = true;
                    }
                }
                return !overlaps;
            }
            return true;
        }
    }
}
