﻿using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace SAMI_SIKON.Services
{
    public class UserCatalogue : Catalogue<IUser>
    {

        public UserCatalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) : base(relationalName, relationalKeys, relationalAttributes) { }

        public UserCatalogue() : base("_User", new string[] { "_User_Id" }, new string[] { "Email", "Password", "Salt", "Phone_Number", "_Name", "Administrator" }) { }
        public static IUser CurrentUser = new Participant();
        public override async Task<bool> CreateItem(IUser user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection))
                    {
                        //command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id); //not needed
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", user.Email);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", user.Password);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", user.Salt);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", user.PhoneNumber);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", user.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", user is Administrator);

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
            catch (Exception e)
            {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return false;
        }

        public IUser RegisterUser(string email, string password, string phoneNumber, string userName, bool isAdmin)
        {
            int saltSize = 16;
            IUser result = null;
            if (isAdmin)
            {
                result = new Administrator();
            }
            else
            {
                result = new Participant();
            }

            result.Email = email;
            result.Name = userName;
            result.PhoneNumber = phoneNumber;
            string salt = PasswordHasher.SaltMaker(saltSize);
            result.Salt = salt;
            result.Password = PasswordHasher.HashPasswordAndSalt(password, result.Salt, 1000, 48);
            return result;
        }



        public override async Task<IUser> DeleteItem(int[] ids)
        {
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
            catch (Exception e)
            {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return null;
        }

        public override async Task<List<IUser>> GetAllItems()
        {
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
                            int user_Id = reader.GetInt32(0);
                            string user_Email = reader.GetString(1);
                            string user_Password = reader.GetString(2);
                            string user_Salt = reader.GetString(3);
                            string user_PhoneNumber = reader.GetString(4);
                            string user_Name = reader.GetString(5);

                            IUser user = null;

                            if (reader.GetBoolean(6) == false)
                            {
                                user = new Participant(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }
                            else
                            {
                                user = new Administrator(user_Id, user_Email, user_Password, user_Salt, user_PhoneNumber, user_Name);
                            }

                            users.Add(user);
                        }

                        return users;
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

        public override async Task<IUser> GetItem(int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGet, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", ids[0]);


                        await command.Connection.OpenAsync();
                        IUser user = null;
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {

                            if (reader.GetBoolean(6) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Salt = reader.GetString(3);
                            user.PhoneNumber = reader.GetString(4);
                            user.Name = reader.GetString(5);
                        }

                        return user;
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

        public override async Task<List<IUser>> GetItemsWithAttribute(int attributeNr, object attribute)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetFromAttribute(attributeNr, attribute), connection))
                    {

                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            IUser user = null;
                            if (reader.GetBoolean(6) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Salt = reader.GetString(3);
                            user.PhoneNumber = reader.GetString(4);
                            user.Name = reader.GetString(5);
                            users.Add(user);
                        }

                        return users;
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

        public override async Task<List<IUser>> GetItemsWithAttributeLike(int attributeNr, string attribute)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLGetLikeAtttribute(attributeNr, attribute), connection))
                    {

                        await command.Connection.OpenAsync();
                        List<IUser> users = new List<IUser>();
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        while (reader.Read())
                        {
                            IUser user = null;
                            if (reader.GetBoolean(6) == false)
                            {
                                user = new Participant();
                            }
                            else
                            {
                                user = new Administrator();
                            }

                            user.Id = reader.GetInt32(0);
                            user.Email = reader.GetString(1);
                            user.Password = reader.GetString(2);
                            user.Salt = reader.GetString(3);
                            user.PhoneNumber = reader.GetString(4);
                            user.Name = reader.GetString(5);
                            users.Add(user);
                        }

                        return users;
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

        public override async Task<List<IUser>> GetItemsWithKey(int keyNr, int key)
        {
            List<IUser> result = new List<IUser>();
            result.Add(await GetItem(new int[] { key }));
            return result;
        }

        public override async Task<bool> UpdateItem(IUser user, int[] ids)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(SQLUpdate, connection))
                    {

                        command.Parameters.AddWithValue($"@{_relationalKeys[0]}", user.Id);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[0]}", user.Email);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[1]}", user.Password);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[2]}", user.Salt);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[3]}", user.PhoneNumber);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[4]}", user.Name);
                        command.Parameters.AddWithValue($"@{_relationalAttributes[5]}", (user is Administrator).ToString());
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
            catch (Exception e)
            {
                string s = e.StackTrace;
                Console.WriteLine(s);
                Console.Beep();
            }
            return false;
        }

        public bool ValidateUser(string email, string password)
        {
            IUser user = GetItemsWithAttribute(0, email).Result[0];
            string prePwd = PasswordHasher.HashPasswordAndSalt(password, user.Salt, 1000, 48);
            return prePwd.SequenceEqual(user.Password);
        }

        public bool Login(string email, string password)
        {
            if (GetItemsWithAttribute(0, email).Result.Count != 0)
            {
                if (ValidateUser(email, password))
                {
                    CurrentUser = GetItemsWithAttribute(0, email).Result[0];
                    return true;
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}

