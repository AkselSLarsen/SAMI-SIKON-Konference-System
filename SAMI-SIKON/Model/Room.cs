using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Room
    {
        public static char SeatSymbol = 'S';
        public static char MobileSeatSymbol = 'M';
        public static char SceneSymbol = 'C';
        public static char TableSymbol = 'T';
        public static char WallSymbol = 'W';
        public static char FloorSymbol = 'F';
        public static char EndLineSymbol = ';';

        public int Id;
        public string Name;
        private List<List<char>> _layout;
        private int _seats;

        public int Seats {
            get {
                return _seats;
            }
            private set {
                _seats = value;
            }
        }

        public Room(int id, string layout, string name) {
            Id = id;
            Layout = LayoutFromString(layout);
            Name = name;
        }

        public Room(int id, List<List<char>> layout, string name) {
            Id = id;
            Layout = layout;
            Name = name;
        }

        public List<List<char>> Layout {
            get { return _layout; }
            set {
                _layout = value;

                Seats = 0;
                foreach(List<char> cs in _layout) {
                    foreach(char c in cs) {
                        if(c == SeatSymbol || c == MobileSeatSymbol) {
                            Seats++;
                        }
                    }
                }
            }
        }

        public string LayoutAsString {
            get {
                string layout = "";

                if (Layout != null && Layout.Count > 0) {
                    foreach (List<char> cs in Layout) {
                        foreach (char c in cs) {
                            layout += c;
                        }
                        layout += EndLineSymbol;
                    }
                    layout = layout.Remove(layout.Length - 1); //remove last EndLineSymbol
                }

                return layout;
            }
        }

        public int Height { get { return Layout.Count; } }
        public int Width { get { return Layout[0].Count; } }

        public int FindSeat(int x, int y) {
            int seatNr = 0;
            for (int i=0; i <= x; i++) {
                for (int j=0; j < Layout[i].Count; j++) {
                    if (i < x || (i == x && j <= y)) {
                        if (Layout[i][j] == SeatSymbol) {
                            seatNr++;
                        }
                    }
                }
            }

            return seatNr;
        }

        public int[] GetSeatPlacement(int i) {
            int seatNr = 0;
            for (int x=0; x < Layout.Count; x++) {
                for (int y=0; y < Layout[x].Count; y++) {
                    if (Layout[x][y] == SeatSymbol) {
                        seatNr++;
                    }
                    if (seatNr == i) {
                        return new int[] { x, y };
                    }
                }
            }

            return null;
        }

        public static List<List<char>> LayoutFromString(string layout) {
            List<List<char>> llc = new List<List<char>>();
            llc.Add(new List<char>());

            if (layout != null) {
                int lineNr = 0;
                foreach (char c in layout) {
                    if (c == EndLineSymbol) {
                        lineNr++;
                        llc.Add(new List<char>());
                    } else {
                        llc[lineNr].Add(c);
                    }
                }
            }

            return llc;
        }


        public static string GetImageSource(char c) {
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

        public static string GetImageAltText(char c) {
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

    }
}
