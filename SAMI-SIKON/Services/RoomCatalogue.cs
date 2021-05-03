using SAMI_SIKON.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Services {
    public class RoomCatalogue : Catalogue<Room> {

        public RoomCatalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) : base(relationalName, relationalKeys, relationalAttributes) { }

        public RoomCatalogue() : base("Room", new string[] { "Room_Id" }, new string[] { "Layout" }) { }

        
    }
}
