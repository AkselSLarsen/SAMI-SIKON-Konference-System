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
        private int _x = -1;
        private int _y = -1;

        [BindProperty]
        public int GridHeight {
            get {
                if (Room != null && Room.Layout != null) {
                    return Room.Height;
                }
                return 0;
            }
        }
        [BindProperty]
        public int GridWidth {
            get {
                if(Room != null && Room.Layout != null) {
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
        public List<List<char>> RoomLayout { get; set; }
        public string RoomName { get; set; }
        public Room Room {
            get {
                return new Room(RoomId, RoomLayout, RoomName);
            }
            set {
                RoomId = value.Id;
                RoomLayout = value.Layout;
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
            await Load();
        }

        public async Task OnPostWidthIncrease(string layout) {
            await Load();
            RoomLayout = Room.LayoutFromString(layout);

            foreach (List<char> cs in RoomLayout) {
                cs.Add(Room.FloorSymbol);
            }
        }

        public async Task OnPostWidthDecrease(string layout) {
            await Load();
            RoomLayout = Room.LayoutFromString(layout);

            foreach (List<char> cs in RoomLayout) {
                cs.RemoveAt(cs.Count-1);
            }
        }

        public async Task OnPostHeightIncrease(string layout) {
            await Load();
            RoomLayout = Room.LayoutFromString(layout);

            List<char> cs = new List<char>();

            for(int i=0; i<GridWidth-1; i++) {
                cs.Add(Room.FloorSymbol);
            }
            RoomLayout.Add(cs);
        }

        public async Task OnPostHeightDecrease(string layout) {
            await Load();
            RoomLayout = Room.LayoutFromString(layout);

            RoomLayout.RemoveAt(GridHeight-1);
        }

        public async Task OnPostCycle(string layout) {
            await Load();
            RoomLayout = Room.LayoutFromString(layout);

            if (ClickedTokenRow >= 0 && ClickedTokenColumn >= 0) {
                int x = ClickedTokenRow;
                int y = ClickedTokenColumn;

                char pre = RoomLayout[x][y];

                if (pre == Room.SeatSymbol) {
                    RoomLayout[x][y] = Room.MobileSeatSymbol;
                } else if (pre == Room.MobileSeatSymbol) {
                    RoomLayout[x][y] = Room.SceneSymbol;
                } else if (pre == Room.SceneSymbol) {
                    RoomLayout[x][y] = Room.TableSymbol;
                } else if (pre == Room.TableSymbol) {
                    RoomLayout[x][y] = Room.WallSymbol;
                } else if (pre == Room.WallSymbol) {
                    RoomLayout[x][y] = Room.FloorSymbol;
                } else if (pre == Room.FloorSymbol) {
                    RoomLayout[x][y] = Room.SeatSymbol;
                }


            }
        }


        public string GetImageSource(char c) {
            if (c == Room.SeatSymbol) {
                return "../pictures/Seat_Open.png";
            } else if (c == Room.MobileSeatSymbol) {
                return "../pictures/Seat_Mobile.png";
            } else if (c == Room.SceneSymbol) {
                return "../pictures/Scene.png";
            } else if (c == Room.TableSymbol) {
                return "../pictures/Table.png";
            } else if (c == Room.WallSymbol) {
                return "../pictures/Wall.png";
            } else if (c == Room.FloorSymbol) {
                return "../pictures/Floor.png";
            }
            return "";
        }

        public string GetImageAltText(char c) {
                if (c == Room.SeatSymbol) {
                return "Open Plads";
            } else if (c == Room.MobileSeatSymbol) {
                return "Flytbar Plads";
            } else if (c == Room.SceneSymbol) {
                return "Scene";
            } else if (c == Room.TableSymbol) {
                return "Bord eller lign.";
            } else if (c == Room.WallSymbol) {
                return "Væg";
            } else if (c == Room.FloorSymbol) {
                return "Gulv";
            }
            return "";
        }

        public string GetOffset(int i) {
            string s = string.Format("{0:N2}", i * SquareSize).Replace(',', '.') + "vh";
            return s;
        }

        public string GetSymbolSize() {
            string s = string.Format("{0:N2}", SquareSize).Replace(',', '.') + "vh";
            return s;
        }

        private async Task Load() {
            if (!(UserCatalogue.CurrentUser is Administrator)) { Redirect("~/"); }

            if (RoomId <= 0) {
                Room = new Room(0, "", "");
            } else {
                Room = await Rooms.GetItem(new int[] { RoomId });
            }
        }


    }
}
