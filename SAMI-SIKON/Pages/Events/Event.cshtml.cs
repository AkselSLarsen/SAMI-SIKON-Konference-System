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
        public int EventId { get; set; }
        public int RoomNr { get; set; }
        public List<int> Speakers { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
        public int[] SeatsTaken { get; set; }
        public string Theme { get; set; }

        public string Tooltip { get; set; }


        public Event Event {
            get {
                return new Event(EventId, RoomNr, Speakers, StartTime, Description, Name, Duration, SeatsTaken, Theme);
            }
            set {
                EventId = value.Id;
                RoomNr = value.RoomNr;
                Speakers = value.Speakers;
                StartTime = value.StartTime;
                Description = value.Description;
                Name = value.Name;
                Duration = (int)(value.StartTime.TimeOfDay.TotalMinutes - value.StopTime.TimeOfDay.TotalMinutes);
                SeatsTaken = value.SeatsTaken();
                Theme = value.Theme;
            }
        }

        public ICatalogue<Event> Events { get; set; }
        public ICatalogue<IUser> Users { get; set; }

        public EventModel(ICatalogue<Event> eventCatalogue, ICatalogue<IUser> userCatalogue) {
            Events = eventCatalogue;
            Users = userCatalogue;
        }

        public async Task OnGetAsync(int id) {
            Event = await Events.GetItem(new int[] { id });
        }

        public async Task<IActionResult> OnPostBook() {
            if(UserCatalogue.CurrentUser == null) {
                return Redirect("~/Login/LoginPage");
            }

            Booking booking = new Booking(0, EventId, null);

            UserCatalogue.CurrentUser.Bookings.Add(booking);
            await Users.UpdateItem(UserCatalogue.CurrentUser, new int[] { UserCatalogue.CurrentUser.Id });
            return Redirect("~/");
        }

        public IActionResult OnPostChooce() {
            return Redirect($"~/Rooms?id={EventId}");
        }

        public async Task<IActionResult> OnPostUnbook() {
            Booking toDelete = null;
            foreach(Booking booking in UserCatalogue.CurrentUser.Bookings) {
                if(booking.Event_Id == EventId) {
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
                    if (booking.Event_Id == EventId) {
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
                    if (booking.Event_Id == EventId) {
                        if (booking.Seat_Nr != null && booking.Seat_Nr != 0) {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> CanBook() {
            Tooltip = "";
            if (UserCatalogue.CurrentUser != null) {
                if (Event.SeatsLeft <= 0) {
                    Tooltip = "Der er ikke nogen pladser tilbage.";
                    return false;
                } else if(UserCatalogue.CurrentUser is Administrator) {
                    Tooltip = "Administratorer kan ikke booke sig ind på et oplæg.";
                    return false;
                }
                Event evt = await Events.GetItem(new int[] { EventId });
                bool overlaps = false;

                List<Booking> bookings = UserCatalogue.CurrentUser.Bookings;
                foreach (Booking booking in bookings) {
                    Event e = await booking.FindEvent();
                    if(e.Overlaps(evt)) {
                        overlaps = true;
                    }
                }
                Tooltip = "Du er allerede booket til et oplæg der forgår på samme tid.";
                return !overlaps;
            }
            Tooltip = "Du skal være logget ind for at booke en plads.";
            return true;
        }

        public async Task<string> GetSpeakerNames() {
            string re = "Holdt af ";

            List<Participant> speakers = await Event.FindSpeakers();
            for(int i=0; i<speakers.Count; i++) {
                re += speakers[i].Name;

                if(speakers.Count > 1) {
                    if (speakers.Count - i > 2) {
                        re += ", ";
                    } else if (speakers.Count - i == 2) {
                        re += " og ";
                    }
                }
            }

            return re;
        }
    }
}
