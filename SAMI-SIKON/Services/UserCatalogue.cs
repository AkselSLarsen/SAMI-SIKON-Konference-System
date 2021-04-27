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
                    using (SqlCommand command = new SqlCommand(SQLInsert, connection)) {

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

        public override Task<IUser> DeleteItem(int[] ids) {
            throw new NotImplementedException();
        }

        public override Task<List<IUser>> GetAllItems() {
            throw new NotImplementedException();
        }

        public override Task<IUser> GetItem(int[] ids) {
            throw new NotImplementedException();
        }

        public override Task<List<IUser>> GetItemsWithAttribute(int attributeNr, object attribute) {
            throw new NotImplementedException();
        }

        public override Task<List<IUser>> GetItemsWithAttributeLike(int attributeNr, string attribute) {
            throw new NotImplementedException();
        }

        public override Task<List<IUser>> GetItemsWithKey(int keyNr, object key) {
            throw new NotImplementedException();
        }

        public override Task<List<IUser>> GetItemsWithKeyLike(int keyNr, string key) {
            throw new NotImplementedException();
        }

        public override Task<bool> UpdateItem(IUser t, int[] ids) {
            throw new NotImplementedException();
        }
    }
}
