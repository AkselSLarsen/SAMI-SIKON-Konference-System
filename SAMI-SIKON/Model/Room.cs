using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Room
    {
        private static char SeatSymbol = 'S'; //should maybe be public, if you need it in some other class feel free to make it so.


        public int Seats;
        private List<List<char>> _layout;

        public Room(List<List<char>> layout)
        {
            Layout = layout;
        }

        public List<List<char>> Layout {
            get { return _layout; }
            set {
                _layout = value;

                Seats = 0;
                foreach(List<char> cs in _layout) {
                    foreach(char c in cs) {
                        if(c == SeatSymbol) {
                            Seats++;
                        }
                    }
                }
            }
        }

        public int Height { get { return Layout.Count; } }
        public int Width { get { return Layout[0].Count; } }

        public int FindSeat(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
