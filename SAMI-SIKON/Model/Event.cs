using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Event
    {
        private int room;
        private int duration;
        private int bookedSeats;
        public int SeatsLeft;
        public DateTime StartTime;
        public DateTime StopTime;
        public string Description;
        public string Name;
        public string Theme;
        private bool seatTaken; 
        public List<Participant> Speaker;


    }
}
