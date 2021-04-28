using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Model
{
    public class Room
    {
        public int Seats;
        public List<List<Char>> Layout;
        public int Height;
        public int Width;

        public Room(int seats, int height, int width, List<List<char>> layout)
        {
            Seats = seats;
            Layout = layout;
            Height = height;
            Width = width;
        }

        public int FindSeat(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
