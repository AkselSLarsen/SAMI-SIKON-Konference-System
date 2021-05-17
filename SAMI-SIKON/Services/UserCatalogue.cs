using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Services {
    public class UserCatalogue : Catalogue<IUser> {

        public UserCatalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) : base(relationalName, relationalKeys, relationalAttributes) { }

        public UserCatalogue() : base("_User", new string[] { "_User_Id" }, new string[] { "Email", "_Password", "Salt", "Phone_Number", "_Name", "Administrator" }) { }
        public static IUser CurrentUser = null;
        public override async Task<bool> CreateItem(IUser user) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection)) {
                        //command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id); //not needed
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", user.Email);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", user.Password);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", user.Salt);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", user.PhoneNumber);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", user.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", user is Administrator);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i != 1) {
                            return false;
                        } else {
                            int user_Id = await GetHighstId();
                            bool re = true;
                            foreach (Booking b in user.Bookings) {
                                if (!await CreateBooking(b.Id, b.Seat, b.Event_Id, user_Id, b.Locked)) {
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

        public async Task<bool> RegisterUser(string email, string password, string phoneNumber, string userName, bool isAdmin) {
            int saltSize = 16;
            IUser result = null;
            if (isAdmin) {
                result = new Administrator();
            } else {
                result = new Participant(0, email, "", "", phoneNumber, userName, new List<Booking>());
            }

            if (GetItemsWithAttribute(0,result.Email).Result.Count!=0)
            {
                return false;
            }
            string salt = PasswordHasher.SaltMaker(saltSize);
            result.Salt = salt;
            result.Password = PasswordHasher.HashPasswordAndSalt(password, result.Salt, 1000, 48);
            return await CreateItem(result);
        }



        public override async Task<IUser> DeleteItem(int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLDelete, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        await command.Connection.OpenAsync();
                        IUser result = await GetItem(ids);
                        int i = await command.ExecuteNonQueryAsync();

                        //Deletion of bookings is automatic as the table has cascade deletion for _User_Id.

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

        public override async Task<List<IUser>> GetAllItems() {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetAll, connection)) {
                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int user_Id = reader.GetInt32(0);
                            string user_Email = reader.GetString(1);
                            string user_Password = reader.GetString(2);
                            string user_Salt = reader.GetString(3);
                            string user_PhoneNumber = reader.GetString(4);
                            string user_Name = reader.GetString(5);

                            IUser user = null;

                            if (reader.GetBoolean(6) == false) {
                                user = new Participant(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name, await GetBookingsForUser(user_Id));
                            } else {
                                user = new Administrator(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }

                            users.Add(user);
                        }

                        return users;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<IUser> GetItem(int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGet, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);


                        await command.Connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int user_Id = reader.GetInt32(0);
                            string user_Email = reader.GetString(1);
                            string user_Password = reader.GetString(2);
                            string user_Salt = reader.GetString(3);
                            string user_PhoneNumber = reader.GetString(4);
                            string user_Name = reader.GetString(5);

                            IUser user = null;

                            if (reader.GetBoolean(6) == false) {
                                user = new Participant(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name, await GetBookingsForUser(user_Id));
                            } else {
                                user = new Administrator(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }

                            return user;
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

        public override async Task<List<IUser>> GetItemsWithAttribute(int attributeNr, object attribute) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetFromAttribute(attributeNr, attribute), connection)) {

                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int user_Id = reader.GetInt32(0);
                            string user_Email = reader.GetString(1);
                            string user_Password = reader.GetString(2);
                            string user_Salt = reader.GetString(3);
                            string user_PhoneNumber = reader.GetString(4);
                            string user_Name = reader.GetString(5);

                            IUser user = null;

                            if (reader.GetBoolean(6) == false) {
                                user = new Participant(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name, await GetBookingsForUser(user_Id));
                            } else {
                                user = new Administrator(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }
                            users.Add(user);
                        }

                        return users;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<IUser>> GetItemsWithAttributeLike(int attributeNr, string attribute) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetLikeAtttribute(attributeNr, attribute), connection)) {

                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {
                            int user_Id = reader.GetInt32(0);
                            string user_Email = reader.GetString(1);
                            string user_Password = reader.GetString(2);
                            string user_Salt = reader.GetString(3);
                            string user_PhoneNumber = reader.GetString(4);
                            string user_Name = reader.GetString(5);

                            IUser user = null;

                            if (reader.GetBoolean(6) == false) {
                                user = new Participant(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name, await GetBookingsForUser(user_Id));
                            } else {
                                user = new Administrator(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }
                            users.Add(user);
                        }

                        return users;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<IUser>> GetItemsWithKey(int keyNr, int key) {
            List<IUser> result = new List<IUser>();
            result.Add(await GetItem(new int[] { key }));
            return result;
        }

        public override async Task<bool> UpdateItem(IUser user, int[] ids) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection)) {

                        //command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id); // Not used
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", user.Email);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", user.Password);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", user.Salt);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", user.PhoneNumber);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", user.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", (user is Administrator).ToString());
                        command.Parameters.AddWithValue($"@To_Update_0", ids[0]);

                        await command.Connection.OpenAsync();

                        int i = await command.ExecuteNonQueryAsync();
                        if (i == 0) {
                            return false;
                        } else {
                            bool re = true;
                            await DeleteBookings(ids[0]);
                            foreach (Booking b in user.Bookings) {
                                if (!await CreateBooking(b.Id, b.Seat, b.Event_Id, ids[0], b.Locked)) {
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

        public async Task<bool> UpdatePassword(IUser user, string password)
        {
            int saltSize = 16;
            

            string salt = PasswordHasher.SaltMaker(saltSize);
            user.Salt = salt;
            user.Password = PasswordHasher.HashPasswordAndSalt(password, user.Salt, 1000, 48);
            return await UpdateItem(user, new int[] {user.Id});
        }

        public bool ValidateUser(string email, string password) {
            IUser user = GetItemsWithAttribute(0, email).Result[0];
            string prePwd = PasswordHasher.HashPasswordAndSalt(password, user.Salt, 1000, 48);
            return prePwd.SequenceEqual(user.Password);
        }

        public bool Login(string email, string password) {
            if (GetItemsWithAttribute(0, email).Result.Count != 0) {
                if (ValidateUser(email, password)) {
                    CurrentUser = GetItemsWithAttribute(0, email).Result[0];
                    return true;
                }
            } else {
                return false;
            }

            return false;
        }

        public static void Logout() {
            CurrentUser = null;
        }



        #region private methods
        private static string SQLGetBookingsForUser = "SELECT * FROM Booking WHERE _User_Id = @_User_Id";

        private async Task<List<Booking>> GetBookingsForUser(int _User_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLGetBookingsForUser, connection)) {

                        command.Parameters.AddWithValue("@_User_Id", _User_Id);

                        await command.Connection.OpenAsync();
                        List<Booking> bookings = new List<Booking>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read()) {

                            int booking_Id = reader.GetInt32(0);
                            int seat_Id = reader.GetInt32(1);
                            int event_Id = reader.GetInt32(2);
                            bool locked = reader.GetBoolean(4);

                            bookings.Add(new Booking(booking_Id, event_Id, seat_Id, locked));
                        }

                        return bookings;
                    }
                }
            } catch (Exception e) {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return new List<Booking>();
        }


        private static string SQLInsertBooking = "INSERT INTO Booking (Seat_Id, Event_Id, _User_Id, Locked) VALUES (@Seat_Id, @Event_Id, @_User_Id, @Locked);";

        private async Task<bool> CreateBooking(int Booking_Id, int Seat_Id, int Event_Id, int _User_Id, bool Locked) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsertBooking, connection)) {

                        //command.Parameters.AddWithValue("@Booking_Id", Booking_Id); // not used
                        command.Parameters.AddWithValue("@Seat_Id", Seat_Id);
                        command.Parameters.AddWithValue("@Event_Id", Event_Id);
                        command.Parameters.AddWithValue("@_User_Id", _User_Id);
                        command.Parameters.AddWithValue("@Locked", Locked);

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


        private static string SQLDeleteBookings = "DELETE FROM Booking WHERE _User_Id = @_User_Id;";

        private async Task DeleteBookings(int _User_Id) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLDeleteBookings, connection)) {

                        command.Parameters.AddWithValue("@_User_Id", _User_Id);

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
            List<IUser> users = await GetAllItems();
            int maxId = 0;
            foreach (IUser user in users) {
                if (user.Id > maxId) {
                    maxId = user.Id;
                }
            }
            return maxId;
        }
        #endregion
    }
}

