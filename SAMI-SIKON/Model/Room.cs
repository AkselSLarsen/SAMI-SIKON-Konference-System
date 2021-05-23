﻿using System;
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

        public static List<List<char>> LayoutFromString(string layout) {
            List<List<char>> llc = new List<List<char>>();
            llc.Add(new List<char>());

            int lineNr = 0;
            foreach(char c in layout) {
                if(c == EndLineSymbol) {
                    lineNr++;
                    llc.Add(new List<char>());
                } else {
                    llc[lineNr].Add(c);
                }
            }

            return llc;
        }

        public static string LayoutAsString(List<List<char>> llc) {
            string layout = "";

            if(llc != null && llc.Count > 0) {
                foreach (List<char> cs in llc) {
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
}
