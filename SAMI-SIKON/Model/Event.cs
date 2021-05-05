using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Event
    {
        public int Event_Id;
        public int RoomNr;
        private int duration;
        private int bookedSeats;
        public DateTime StartTime;
        public string Description;
        public string Name;
        public string Theme;
        private bool seatTaken; 
        public List<Participant> Speaker;

        public DateTime StopTime {
            get {
                return StartTime.AddMinutes(duration);
            }
        }
        public int SeatsLeft { get; private set; }

        public Event(int id, int roomNr, int _duration, DateTime startTime, string description, string name, int seatsTaken)
        {
            Event_Id = id;
            RoomNr = roomNr;
            duration = _duration;
            StartTime = startTime;
            Description = description;
            Name = name;
            bookedSeats = seatsTaken;
        }
        /// <summary>
        /// This is a testing constructor, please delete.
        /// </summary>
        /// <param name="testing"></param>
        public Event(bool testing) {
            Random r = new Random();
            Event_Id = r.Next(1, 10000);
            RoomNr = r.Next(1, 100);
            duration = r.Next(30, 120);
            bookedSeats = r.Next(0, 100);
            StartTime = DateTime.Today.AddHours(r.Next(8, 14));
            Description = "";
            Name = "";
            Theme = r.Next(0, 2) < 1 ? "a" : r.Next(0, 2) < 1 ? "b" : "c";
            Speaker = null;
        }


    }
}
