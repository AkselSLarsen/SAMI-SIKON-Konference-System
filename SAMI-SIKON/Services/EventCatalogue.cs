using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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


        public override async Task<List<Event>> GetAllItems()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetAll, connection))
                    {
                        await command.Connection.OpenAsync();
                        List <Event> events = new List<Event>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            int seats_Taken = reader.GetInt32(3);
                            DateTime startTime = reader.GetDateTime(4);
                            int duration = reader.GetInt32(5);
                            int room_Id = reader.GetInt32(6);

                            Event evnt = null;
                            
                            evnt = new Event(event_Id, room_Id, duration, startTime, description, name, seats_Taken);


                            events.Add(evnt);
                        }

                        return events;
                    }
                }
            }
            catch (Exception e)
            {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<Event>> GetItemsWithKey(int keyNr, int key)
        {
            List<Event> result = new List<Event>();
            result.Add(await GetItem(new int[] { key }));
            return result;
        }

        public override async Task<List<Event>> GetItemsWithAttribute(int attributeNr, object attribute)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetFromAtttribute(attributeNr, attribute.ToString()), connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalAttributes[attributeNr]}", attribute);


                        await command.Connection.OpenAsync();
                        List<Event> events = new List<Event>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Event evnt = null;

                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            int seats_Taken = reader.GetInt32(3);
                            DateTime startTime = reader.GetDateTime(4);
                            int duration = reader.GetInt32(5);
                            int room_Id = reader.GetInt32(6);


                            evnt = new Event(event_Id, room_Id, duration, startTime, description, name, seats_Taken);


                            events.Add(evnt);
                        }

                        return events;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<List<Event>> GetItemsWithAttributeLike(int attributeNr, string attribute)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetLikeAtttribute(attributeNr, attribute), connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalAttributes[attributeNr]}", attribute);


                        await command.Connection.OpenAsync();
                        List<Event> events = new List<Event>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            Event evnt = null;

                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            int seats_Taken = reader.GetInt32(3);
                            DateTime startTime = reader.GetDateTime(4);
                            int duration = reader.GetInt32(5);
                            int room_Id = reader.GetInt32(6);


                            evnt = new Event(event_Id, room_Id, duration, startTime, description, name, seats_Taken);


                            events.Add(evnt);
                        }

                        return events;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<Event> GetItem(int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        Event evnt = null;

                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            int seats_Taken = reader.GetInt32(3);
                            DateTime startTime = reader.GetDateTime(4);
                            int duration = reader.GetInt32(5);
                            int room_Id = reader.GetInt32(6);

                            evnt = new Event(event_Id, room_Id, duration, startTime, description, name, seats_Taken);
                        }

                        return evnt;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<bool> CreateItem(Event t)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {


                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", t.Event_Id);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", t.Description);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", t.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", t.StartTime);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", t.RoomNr);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i != 1)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public override async Task<bool> UpdateItem(Event t, int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", t.Event_Id);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", t.Description);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", t.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", t.SeatsLeft);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", t.StartTime);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", t.RoomNr);
                        command.Parameters.AddWithValue($"@To_Update_0", ids[0]);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i == 0)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
            return false;
        }

        public override async Task<Event> DeleteItem(int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLDelete, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        await command.Connection.OpenAsync();
                        Event result = await GetItem(ids);
                        int i = await command.ExecuteNonQueryAsync();
                        return result;
                    }
                }

            }
            catch (Exception)
            {

            }
            return null;
        }
    }
}
