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


        public int GridHeight {
            get {
                if (Room != null && Room.Layout != null) {
                    return Room.Layout.Count;
                }
                return 0;
            }
        }

        public int GridWidth {
            get {
                if(Room != null && Room.Layout != null) {
                    return Room.Layout[0].Count;
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
        public Room Room { get; set; }
        public ICatalogue<Room> Rooms { get; set; }

        public DesignerModel(ICatalogue<Room> rooms) {
            Rooms = rooms;
        }

        public async Task OnGet() {
            if(!(UserCatalogue.CurrentUser is Administrator)) { Redirect("~/"); }

            if(RoomId < 0) {
                Room = new Room(0, "", "");
            } else {
                Room = await Rooms.GetItem(new int[] { RoomId });
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
                return "V�g";
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


    }
}
