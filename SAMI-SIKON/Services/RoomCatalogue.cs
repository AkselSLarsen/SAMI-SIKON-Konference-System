using SAMI_SIKON.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Services {
    public class RoomCatalogue : Catalogue<Room> {

        public RoomCatalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) : base(relationalName, relationalKeys, relationalAttributes) { }

        public RoomCatalogue() : base("Room", new string[] { "Room_Id" }, new string[] { "Layout" }) { }

        public override async Task<List<Room>> GetAllItems() {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetAll, connection)) {
                        await command.Connection.OpenAsync();
                        List<Room> rooms = new List<Room>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int room_Id = reader.GetInt32(0);
                            string room_layout = reader.GetString(1);

                            Room room = new Room(room_Id, room_layout);

                            rooms.Add(room);
                        }

                        return rooms;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<Room>> GetItemsWithKey(int keyNr, int key) {
            List<Room> result = new List<Room>();
            result.Add(await GetItem(new int[] { key }));
            return result;
        }

        public override async Task<List<Room>> GetItemsWithAttribute(int attributeNr, object attribute) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetFromAttribute(attributeNr, attribute), connection)) {

                        await command.Connection.OpenAsync();
                        List<Room> rooms = new List<Room>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int room_Id = reader.GetInt32(0);
                            string room_Layout = reader.GetString(1);

                            Room room = new Room(room_Id, room_Layout);
                            rooms.Add(room);
                        }

                        return rooms;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<Room>> GetItemsWithAttributeLike(int attributeNr, string attribute) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetLikeAtttribute(attributeNr, attribute), connection)) {

                        await command.Connection.OpenAsync();
                        List<Room> rooms = new List<Room>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int room_Id = reader.GetInt32(0);
                            string room_Layout = reader.GetString(1);

                            Room room = new Room(room_Id, room_Layout);
                            rooms.Add(room);
                        }

                        return rooms;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<Room> GetItem(int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGet, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);


                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int room_Id = reader.GetInt32(0);
                            string room_Layout = reader.GetString(1);
                            
                            return new Room(room_Id, room_Layout);
                        }
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<bool> CreateItem(Room room) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection)) {
                        //command.Parameters.AddWithValue($"@{_relationalKeys[0]}", room.Id); //not needed
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", room.GetLayoutAsString());

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

        public override async Task<bool> UpdateItem(Room room, int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", room.Id);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", room.GetLayoutAsString());
                        command.Parameters.AddWithValue($"@To_Update_0", ids[0]);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i == 0) {
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

        public override async Task<Room> DeleteItem(int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLDelete, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        await command.Connection.OpenAsync();
                        Room result = await GetItem(ids);
                        int i = await command.ExecuteNonQueryAsync();
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
    }
}
