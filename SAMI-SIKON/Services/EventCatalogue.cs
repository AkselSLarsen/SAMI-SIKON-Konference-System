using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;

namespace SAMI_SIKON.Services
{
    public class EventCatalogue : Catalogue<Event>
    {
        public EventCatalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) : base(relationalName, relationalKeys, relationalAttributes)
        {
        }
                public EventCatalogue() : base("Event", new string[] { "Event_Id" }, new string[] { "Description", "Name", "Booked_Seats", "StartTime", "Duration", "Room_Id" }) { }


        public override Task<List<Event>> GetAllItems()
        {
            throw new NotImplementedException();
        }

        public override Task<List<Event>> GetItemsWithKey(int keyNr, int key)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Event>> GetItemsWithAttribute(int attributeNr, object attribute)
        {
            throw new NotImplementedException();
        }

        public override Task<List<Event>> GetItemsWithAttributeLike(int attributeNr, string attribute)
        {
            throw new NotImplementedException();
        }

        public override Task<Event> GetItem(int[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> CreateItem(Event t)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateItem(Event t, int[] ids)
        {
            throw new NotImplementedException();
        }

        public override Task<Event> DeleteItem(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
