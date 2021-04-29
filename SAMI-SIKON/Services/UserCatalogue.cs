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

        public UserCatalogue() : base("_User", new string[] { "_User_Id" }, new string[] { "Email", "Password", "Salt", "Phone_Number", "Name", "Administrator" }) { }

        public override async Task<bool> CreateItem(IUser user) {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {


                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id);
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
                            return true;
                        }
                    }
                }
            } catch (Exception) {

            }
            return false;
        }

        public override async Task<IUser> DeleteItem(int[] ids) {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLDelete, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);

                        await command.Connection.OpenAsync();
                        IUser result = await GetItem(ids);
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

        public override async Task<List<IUser>> GetAllItems() {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetAll, connection))
                    {



                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            IUser user = null;
                            if (reader.GetBoolean(0) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(0);
                            user.Name = reader.GetString(4);
                            user.Password = reader.GetString(1);
                            user.Salt = reader.GetString(2);
                            user.PhoneNumber = reader.GetString(3);
                            users.Add(user);
                        }

                        return users;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<IUser> GetItem(int[] ids) {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);
                        

                        await command.Connection.OpenAsync();
                        IUser user=null;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            
                            if (reader.GetBoolean(0) == false)
                            {
                                user=new Participant();
                            }
                            else
                            {
                                user=new Administrator();
                            }

                            user.Id = ids[0];
                            user.Email = reader.GetString(0);
                            user.Name = reader.GetString(4);
                            user.Password = reader.GetString(1);
                            user.Salt = reader.GetString(2);
                            user.PhoneNumber = reader.GetString(3);
                        }

                        return user;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<List<IUser>> GetItemsWithAttribute(int attributeNr, object attribute) {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetFromAtttribute(attributeNr,attribute.ToString()), connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalAttributes[attributeNr]}", attribute);


                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            IUser user = null;
                            if (reader.GetBoolean(0) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(0);
                            user.Name = reader.GetString(4);
                            user.Password = reader.GetString(1);
                            user.Salt = reader.GetString(2);
                            user.PhoneNumber = reader.GetString(3);
                            users.Add(user);
                        }

                        return users;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<List<IUser>> GetItemsWithAttributeLike(int attributeNr, string attribute) {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetLikeAtttribute(attributeNr, attribute), connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalAttributes[attributeNr]}", attribute);


                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            IUser user = null;
                            if (reader.GetBoolean(0) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(0);
                            user.Name = reader.GetString(4);
                            user.Password = reader.GetString(1);
                            user.Salt = reader.GetString(2);
                            user.PhoneNumber = reader.GetString(3);
                            users.Add(user);
                        }

                        return users;
                    }
                }
            }
            catch (Exception)
            {

            }
            return null;
        }

        public override async Task<List<IUser>> GetItemsWithKey(int keyNr, int key)
        {
            List<IUser> result = new List<IUser>();
            result.Add(await GetItem(new int[] { key }));
            return result;
        }

        

        public override async Task<bool> UpdateItem(IUser user, int[] ids)
        {
            try {
                using (SqlConnection connection = new SqlConnection(connectionString)) {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection)) {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", user.Email);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", user.Password);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", user.Salt);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", user.PhoneNumber);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", user.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", user is Administrator);
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
            } catch (Exception) {

            }
            return false;
        }

        public void Login()
        {

        }
    }
    }
}
