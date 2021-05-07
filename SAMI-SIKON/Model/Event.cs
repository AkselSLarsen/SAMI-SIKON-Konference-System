using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Event
    {
        public int Id;
        public int RoomNr;
        public List<Participant> Speakers;
        public DateTime StartTime;
        public string Description;
        public string Name;
        public string Theme;

        private int _duration;
        private bool[] _seatsTaken;

        private int bookedSeats { get { return SeatsTaken().Length; } }

        public DateTime StopTime {
            get {
                return StartTime.AddMinutes(_duration);
            }
        }
        public int SeatsLeft {
            get {
                return FindRoom().Result.Seats - bookedSeats;
            }
        }

        public Event(int id, int roomNr, List<Participant> speakers, DateTime startTime, string description, string name, string theme, int duration, bool[] seatsTaken)
        {
            Id = id;
            RoomNr = roomNr;
            Speakers = speakers;
            StartTime = startTime;
            Description = description;
            Name = name;
            Theme = theme;
            _duration = duration;
            _seatsTaken = seatsTaken;
        }

        [Obsolete]
        /// <summary>
        /// This is a testing constructor, please delete it after explaining all about what it was for in the report.
        /// </summary>
        /// <param name="testing">Doesn't do anything, just there to make it not interfere with the rest of the class</param>
        public Event(bool testing) {
            Random r = new Random();
            Id = r.Next(1, 10000);
            RoomNr = r.Next(1, 100);
            _duration = r.Next(1, 4) * 30;
            StartTime = DateTime.Today.AddMinutes(r.Next(16, 28) * 30);
            Description = "";
            Name = "Name Here";
            Theme = r.Next(0, 2) < 1 ? "a" : r.Next(0, 2) < 1 ? "b" : "c";
            Speakers = null;
        }

        public bool SeatTaken(int i) {
            return _seatsTaken[i];
        }

        public int[] SeatsTaken() {
            List<int> ints = new List<int>();
            for(int i=0; i<_seatsTaken.Length; i++) {
                if(_seatsTaken[i]) {
                    ints.Add(i);
                }
            }
            return ints.ToArray();
        }

        public async Task<Room> FindRoom() {
            ICatalogue<Room> catalogue = new RoomCatalogue();
            return await catalogue.GetItem(new int[] { RoomNr });
        }

    }
}
