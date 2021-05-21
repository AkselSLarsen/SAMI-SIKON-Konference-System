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
    public class DesignerModel : PageModel
    {
        private int _id = -1;


        /*public int GridHeight {
            get {
                //return 
            }
        }*/

        [FromQuery(Name = "id")]
        public int Id {
            get { return _id; }
            set { _id = value; }
        }
        public Room Room { get; set; }
        public ICatalogue<Room> Rooms { get; set; }

        public DesignerModel(ICatalogue<Room> rooms) {
            Rooms = rooms;
        }

        public void OnGet() {
            if(Id < 0) {
                Room = new Room(0, "", "");
            } else {
                //Room = Rooms.GetItem
            }
        }
    }
}
