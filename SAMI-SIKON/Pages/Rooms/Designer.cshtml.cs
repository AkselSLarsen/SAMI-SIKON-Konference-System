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
    public class DesignerModel : PageModel
    {
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
                if(Room != null && RoomLayout != null) {
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
        public int RoomId {
            get { return _roomId; }
            set { _roomId = value; }
        }

        [FromQuery(Name = "layout")]
        public string RoomLayout {
            get { return _roomLayout; }
            set { _roomLayout = value; }
        }


        [BindProperty]
        public string Name { get; set; }
        [FromQuery(Name = "name")]
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

        [FromQuery(Name = "x")]
        public int ClickedTokenRow {
            get { return _x; }
            set { _x = value; }
        }
        [FromQuery(Name = "y")]
        public int ClickedTokenColumn {
            get { return _y; }
            set { _y = value; }
        }

        public ICatalogue<Room> Rooms { get; set; }

        public DesignerModel(ICatalogue<Room> rooms) {
            Rooms = rooms;
        }

        public async Task OnGetAsync() {
            if (!(UserCatalogue.CurrentUser is Administrator)) { Redirect("~/"); }

            if (RoomId <= 0) {
                Room = new Room(0, "", "");
            } else {
                Room = await Rooms.GetItem(new int[] { RoomId });
                Name = RoomName;
            }
        }

        public void OnPostNameChange() {
            RoomName = Name;
        }

        public void OnPostWidthIncrease() {
            List<List<char>> llc = Room.Layout;
            foreach (List<char> cs in llc) {
                cs.Add(Room.FloorSymbol);
            }
            Room = new Room(RoomId, llc, RoomName);
        }

        public void OnPostWidthDecrease() {
            List<List<char>> llc = Room.Layout;
            foreach (List<char> cs in llc) {
                cs.RemoveAt(cs.Count-1);
            }
            Room = new Room(RoomId, llc, RoomName);
        }

        public void OnPostHeightIncrease() {
            List<List<char>> llc = Room.Layout;
            List<char> cs = new List<char>();

            for(int i=0; i<GridWidth; i++) {
                cs.Add(Room.FloorSymbol);
            }
            llc.Add(cs);
            Room = new Room(RoomId, llc, RoomName);
        }

        public void OnPostHeightDecrease() {
            List<List<char>> llc = Room.Layout;
            llc.RemoveAt(GridHeight-1);
            Room = new Room(RoomId, llc, RoomName);
        }

        public void OnPostCycle() {
            if (ClickedTokenRow >= 0 && ClickedTokenColumn >= 0) {
                int x = ClickedTokenRow;
                int y = ClickedTokenColumn;

                List<List<char>> llc = Room.Layout;
                char pre = llc[x][y];

                if (pre == Room.SeatSymbol) {
                    llc[x][y] = Room.MobileSeatSymbol;
                } else if (pre == Room.MobileSeatSymbol) {
                    llc[x][y] = Room.SceneSymbol;
                } else if (pre == Room.SceneSymbol) {
                    llc[x][y] = Room.TableSymbol;
                } else if (pre == Room.TableSymbol) {
                    llc[x][y] = Room.WallSymbol;
                } else if (pre == Room.WallSymbol) {
                    llc[x][y] = Room.FloorSymbol;
                } else if (pre == Room.FloorSymbol) {
                    llc[x][y] = Room.SeatSymbol;
                }
                Room = new Room(RoomId, llc, RoomName);
            }
        }

        public async Task OnPostCreate() {
            await Rooms.CreateItem(Room);
            Redirect("~/");
        }

        public async Task OnPostUpdate() {
            await Rooms.UpdateItem(Room, new int[] { RoomId });
            Redirect("~/");
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
