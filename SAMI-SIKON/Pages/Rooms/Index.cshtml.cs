using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;

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
        public int SelectedSeatRow {
            get { return _x; }
            set { _x = value; }
        }
        [FromQuery(Name = "y")]
        public int SelectedSeatColumn {
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
        public ICatalogue<IUser> Users { get; set; }

        public IndexModel(ICatalogue<Room> rooms, ICatalogue<Event> events, ICatalogue<IUser> users) {
            Rooms = rooms;
            Events = events;
            Users = users;
        }

        public async Task OnGet() {
            await OnLoad();
        }

        public async Task OnPostSelect() {
            await OnLoad();
        }

        public async Task<IActionResult> OnPostChooce() {
            await OnLoad();

            Booking booking = new Booking(-1, EventId, GetSelectedSeat());
            UserCatalogue.CurrentUser.Bookings.Add(booking);
            await Users.UpdateItem(UserCatalogue.CurrentUser, new int[] { UserCatalogue.CurrentUser.Id });

            return Redirect("~/");
        }

        public int GetSelectedSeat() {
            return Room.FindSeat(SelectedSeatRow, SelectedSeatColumn);
        }

        public async Task<bool> SeatTaken(int x, int y) {
            int seatNr = Room.FindSeat(x, y);

            Event evt = await Events.GetItem(new int[] { EventId });
            return evt.SeatTaken(seatNr);
        }

        public bool SeatSelected(int x, int y) {
            if(x == SelectedSeatRow && y == SelectedSeatColumn) {
                return true;
            } else {
                return false;
            }
        }

        public string GetOffset(int i) {
            string s = string.Format("{0:N2}", i * SquareSize).Replace(',', '.') + "vh";
            return s;
        }

        public string GetSymbolSize() {
            string s = string.Format("{0:N2}", SquareSize).Replace(',', '.') + "vh";
            return s;
        }

        private async Task OnLoad() {
            Event evt = await Events.GetItem(new int[] { EventId });
            Room = await evt.FindRoom();
        } 


    }
}
