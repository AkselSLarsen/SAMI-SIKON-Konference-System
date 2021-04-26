using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model {
    public class Booking {
        private int _event;
        public int Seat;
        public bool Locked;

        public Booking(int eventNr, int seatNr) {
            _event = eventNr;
            Seat = seatNr;
            Locked = false;
        }

        public Booking(int eventNr, int seatNr, bool locked) {
            _event = eventNr;
            Seat = seatNr;
            Locked = locked;
        }

        //public Event FindEvent(EventCatalogue evtctlg) {

        //}
    }
}
