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
        private int _roomId = -1;
        private string _roomLayout = "";
        private string _roomName = "";
        private int _x = -1;
        private int _y = -1;

        public int GridHeight {
            get {
                if (Room != null && RoomLayout != null) {
                    return Room.Height;
                }
                return 0;
            }
        }
        public int GridWidth {
            get {
                if (Room != null && RoomLayout != null) {
                    return Room.Width;
                }
                return 0;
            }
        }

        public double SquareSize {
            get {
                double maxWidth = 80.0 / GridWidth;
                double maxHeight = 80.0 / GridHeight;
                return maxHeight < maxWidth ? maxHeight : maxWidth;
            }
        }



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
        public int ClickedSeatColumn {
            get { return _y; }
            set { _y = value; }
        }

        public int RoomId {
            get { return _roomId; }
            set { _roomId = value; }
        }

        public string RoomLayout {
            get { return _roomLayout; }
            set { _roomLayout = value; }
        }

        public string RoomName {
            get { return _roomName; }
            set { _roomName = value; }
        }

        public Room Room {
            get {
                return new Room(RoomId, RoomLayout, RoomName);
            }
            set {
                RoomId = value.Id;
                RoomLayout = value.LayoutAsString;
                RoomName = value.Name;
            }
        }

        public ICatalogue<Room> Rooms { get; set; }
        public ICatalogue<Event> Events { get; set; }

        public IndexModel(ICatalogue<Room> rooms, ICatalogue<Event> events) {
            Rooms = rooms;
            Events = events;
        }

        public void OnGet() {

        }





        public string GetOffset(int i) {
            string s = string.Format("{0:N2}", i * SquareSize).Replace(',', '.') + "vh";
            return s;
        }

        public string GetSymbolSize() {
            string s = string.Format("{0:N2}", SquareSize).Replace(',', '.') + "vh";
            return s;
        }


    }
}
