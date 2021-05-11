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
        private int duration;
        private int bookedSeats;
        public DateTime StartTime;
        public string Description;
        public string Name;
        public string Theme;
        private bool seatTaken; 
        public List<Participant> Speaker;

        public DateTime StopTime { get; private set; }
        public int SeatsLeft { get; private set; }

        public Event(int id, int roomNr, int _duration, DateTime startTime, string description, string name, int seatsTaken)
        {
            Id = id;
            RoomNr = roomNr;
            duration = _duration;
            StartTime = startTime;
            Description = description;
            Name = name;
            bookedSeats = seatsTaken;
        }
    }
}
