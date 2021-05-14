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
        public EventCatalogue() : base("_Event", new string[] { "Event_Id" }, new string[] { "_Description", "_Name", "StartTime", "Duration", "Room_Id" }) { }

        #region Catalogue methods
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
                            DateTime startTime = reader.GetDateTime(3);
                            int duration = reader.GetInt32(4);
                            int room_Id = reader.GetInt32(5);

                            Event evt = new Event(event_Id, room_Id, await GetSpeakersForEvent(event_Id), startTime, description, name, duration, await GetSeatsReserved(event_Id));

                            events.Add(evt);
                        }

                        return events;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return new List<Event>();
        }

        public override async Task<List<Event>> GetItemsWithKey(int keyNr, int key) {
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
                    using (SqlCommand command = new SqlCommand(SQLGetFromAttribute(attributeNr, attribute.ToString()), connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalAttributes[attributeNr]}", attribute);


                        await command.Connection.OpenAsync();
                        List<Event> events = new List<Event>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            DateTime startTime = reader.GetDateTime(3);
                            int duration = reader.GetInt32(4);
                            int room_Id = reader.GetInt32(5);

                            Event evt = new Event(event_Id, room_Id, await GetSpeakersForEvent(event_Id), startTime, description, name, duration, await GetSeatsReserved(event_Id));


                            events.Add(evt);
                        }

                        return events;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
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
                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            DateTime startTime = reader.GetDateTime(3);
                            int duration = reader.GetInt32(4);
                            int room_Id = reader.GetInt32(5);

                            Event evt = new Event(event_Id, room_Id, await GetSpeakersForEvent(event_Id), startTime, description, name, duration, await GetSeatsReserved(event_Id));


                            events.Add(evt);
                        }

                        return events;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<Event> GetItem(int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGet, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        Event evt = null;

                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            int event_Id = reader.GetInt32(0);
                            string description = reader.GetString(1);
                            string name = reader.GetString(2);
                            DateTime startTime = reader.GetDateTime(3);
                            int duration = reader.GetInt32(4);
                            int room_Id = reader.GetInt32(5);

                            evt = new Event(event_Id, room_Id, await GetSpeakersForEvent(event_Id), startTime, description, name, duration, await GetSeatsReserved(event_Id));
                        }

                        return evt;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<bool> CreateItem(Event evt)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {
                        //command.Parameters.AddWithValue($"@{_relationalKeys[0]}", evt.Id); //not needed for tables that are auto indexed
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", evt.Description);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", evt.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", evt.StartTime);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", evt.StopTime.TimeOfDay.TotalMinutes - evt.StartTime.TimeOfDay.TotalMinutes);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", evt.RoomNr);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i != 1)
                        {
                            return false;
                        }
                        else
                        {
                            int evt_Id = await GetHighstId();
                            bool re = true;
                            foreach(int participant in evt.Speakers) {
                                if (!await CreateSpeaker(evt_Id, participant)) {
                                    re = false;
                                }
                            }
                            for(int j=0; j<evt.FindRoom().Result.Seats; j++) {
                                if (!await CreateSeat(j, evt_Id, false)) {
                                    re = false;
                                }
                            }
                            return re;
                        }
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return false;
        }

        #warning Currently updating an event with bookings will cause problems, please fix!!!
        public override async Task<bool> UpdateItem(Event evt, int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection))
                    {
                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", evt.Id); //not used
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", evt.Description);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", evt.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", evt.StartTime);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", evt.StopTime.TimeOfDay.Minutes - evt.StartTime.TimeOfDay.Minutes);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", evt.RoomNr);
                        command.Parameters.AddWithValue($"@To_Update_0", ids[0]);

                        //check if RoomNr has changed so we can delete all the seats and add new ones.
                        Event oldEvent = await GetItem(new int[] { ids[0] });
                        bool RoomNrChanged = evt.RoomNr != oldEvent.RoomNr;

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i == 0)
                        {
                            return false;
                        }
                        else
                        {
                            int evt_Id = await GetHighstId();
                            bool re = true;
                            await DeleteSpeakers(evt_Id);
                            foreach (int participant in evt.Speakers) {
                                if (!await CreateSpeaker(evt_Id, participant)) {
                                    re = false;
                                }
                            }
                            if(RoomNrChanged) {
                                await DeleteSeats(evt_Id);
                                for (int j = 0; j < evt.FindRoom().Result.Seats; j++) {
                                    if (!await CreateSeat(j, evt_Id, false)) {
                                        re = false;
                                    }
                                }
                            }
                            return re;
                        }
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
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

                        //Deletion of seats and speakers is automatic as the database has cascade deletion for Event_Id on both.

                        return result;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }
        #endregion

        #region private methods
        private static string SQLGetSpeakersForEvent = "SELECT * FROM Speaker WHERE Event_Id = @Event_Id";

        private async Task<List<int>> GetSpeakersForEvent(int Event_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetSpeakersForEvent, connection)) {

                        command.Parameters.AddWithValue("@Event_Id", Event_Id);

                        await command.Connection.OpenAsync();
                        List<int> speakers = new List<int>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {

                            int speaker_Id = reader.GetInt32(1);

                            speakers.Add(speaker_Id);
                        }

                        return speakers;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return new List<int>();
        }


        private static string SQLInsertSpeaker = "INSERT INTO Speaker (Event_Id, _User_Id) VALUES (@Event_Id, @_User_Id);";

        private async Task<bool> CreateSpeaker(int Event_Id, int _User_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsertSpeaker, connection)) {

                        command.Parameters.AddWithValue("@Event_Id", Event_Id);
                        command.Parameters.AddWithValue("@_User_Id", _User_Id);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i != 1) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return false;
        }


        private static string SQLDeleteSpeakers = "DELETE FROM Speaker WHERE Event_Id = @Event_Id;";

        private async Task DeleteSpeakers(int Event_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLDeleteSpeakers, connection)) {

                        command.Parameters.AddWithValue("@Event_Id", Event_Id);

                        await command.Connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }

            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
        }


        private static string SQLGetSeatsReserved = "SELECT * FROM Seat WHERE Event_Id = @Event_Id";

        private async Task<bool[]> GetSeatsReserved(int Event_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetSeatsReserved, connection)) {

                        command.Parameters.AddWithValue("@Event_Id", Event_Id);

                        await command.Connection.OpenAsync();
                        List<bool> seatsTaken = new List<bool>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            bool b = reader.GetBoolean(2);
                            seatsTaken.Add(b);
                        }

                        return seatsTaken.ToArray();

                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return new bool[0];
        }


        public static string SQLInsertSeat = "INSERT INTO Seat (Seat_Id, Event_Id, Reserved) VALUES (@Seat_Id, @Event_Id, @Reserved);";

        public async Task<bool> CreateSeat(int Seat_Id, int Event_Id, bool Reserved) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsertSeat, connection)) {

                        command.Parameters.AddWithValue("@Seat_Id", Seat_Id);
                        command.Parameters.AddWithValue("@Event_Id", Event_Id);
                        command.Parameters.AddWithValue("@Reserved", Reserved);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i != 1) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return false;
        }


        private static string SQLDeleteSeats = "DELETE FROM Seat WHERE Event_Id = @Event_Id;";

        private async Task DeleteSeats(int Event_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLDeleteSeats, connection)) {

                        command.Parameters.AddWithValue("@Event_Id", Event_Id);

                        await command.Connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }

            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
        }

        private async Task<int> GetHighstId() {
            List<Event> events = await GetAllItems();
            int maxId = 0;
            foreach(Event evt in events) {
                if(evt.Id > maxId) {
                    maxId = evt.Id;
                }
            }
            return maxId;
        }
        #endregion
    }
}
