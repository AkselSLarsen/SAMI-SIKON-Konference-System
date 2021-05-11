using SAMI_SIKON.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model {
    public class Booking {
        public int Id;
        public int Event_Id;
        public int Seat;
        public bool Locked;

        public Booking(int id, int eventNr, int seatNr) {
            Id = id;
            Event_Id = eventNr;
            Seat = seatNr;
            Locked = false;
        }

        public Booking(int id, int eventNr, int seatNr, bool locked) {
            Id = id;
            Event_Id = eventNr;
            Seat = seatNr;
            Locked = locked;
        }

        public async Task<Event> FindEvent() {
            return await new EventCatalogue().GetItem(new int[] { Event_Id });
        }
    }
}
