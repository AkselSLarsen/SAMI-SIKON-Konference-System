using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;

namespace SAMI_SIKON.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        private int _eventId = -1;
        private int _x = -1;
        private int _y = -1;


        [FromQuery(Name = "id")]
        public int EventId {
            get { return _eventId; }
            set { _eventId = value; }
        }
        [FromQuery(Name = "x")]
        public int ChosenSeatRow {
            get { return _x; }
            set { _x = value; }
        }
        [FromQuery(Name = "y")]
        public int ClickedTokenColumn {
            get { return _y; }
            set { _y = value; }
        }

        public ICatalogue<Room> Rooms { get; set; }
        public ICatalogue<Event> Events { get; set; }

        public IndexModel(ICatalogue<Room> rooms, ICatalogue<Event> events) {
            
        }

        public void OnGet() {

        }
    }
}
