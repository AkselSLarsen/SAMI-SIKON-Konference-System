using SAMI_SIKON.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Services {
    public abstract class Catalogue<T> : ICatalogue<T> {

        /// <summary>
        /// The connection address to the underlying relational database in string format.
        /// </summary>
        protected static string connectionString = "";
        /// <summary>
        /// The name of the table in the relational database in string format.
        /// </summary>
        protected string _relationalName;
        /// <summary>
        /// The name of the keys in the table in the relational database in string format.
        /// </summary>
        protected string[] _relationalKeys;
        /// <summary>
        /// The name of the non-prime attributes in the table in the relational database in string format.
        /// </summary>
        protected string[] _relationalAttributes;

        public Catalogue(string relationalName, string[] relationalKeys, string[] relationalAttributes) {
            _relationalName = relationalName;
            _relationalKeys = relationalKeys;
            _relationalAttributes = relationalAttributes;
        }

        string ICatalogue<T>.RelationalName {
            get { return _relationalName; }
            set { _relationalName = value; }
        }
        string[] ICatalogue<T>.RelationalKeys {
            get { return _relationalKeys; }
            set { _relationalKeys = value; }
        }
        string[] ICatalogue<T>.RelationalAttributes {
            get { return _relationalAttributes; }
            set { _relationalAttributes = value; }
        }

        /// <summary>
        /// Contains an SQL quary that retrieves all elements from the table with the name contained by RelationalName as a string.
        /// </summary>
        protected string SQLGetAll {
            get {
                string re = $"SELECT * FROM {_relationalName};";

                return re;
            }
        }
        /// <summary>
        /// Creates an SQL quary that retrieves all elements where the key of the given index number has the given value.
        /// </summary>
        /// <param name="keyNr">The index number of the key. Must be a non-negative integer less than the length of the array contained by RelationalKeys</param>
        /// <param name="value">The value the key should have for the element to be returned</param>
        /// <returns>An SQL statement in string format that retrieves all elements with the key of the given number equal to the given value</returns>
        protected string SQLGetFromKey(int keyNr, string value) {
            string re = $"SELECT * FROM {_relationalName} WHERE {_relationalKeys[keyNr]} = {value};";

            return re;
        }

        /// <summary>
        /// Creates an SQL quary that retrieves all elements where the attribute of the given index number has the given value.
        /// </summary>
        /// <param name="attributeNr">The index number of the attribute. Must be a non-negative integer less than the length of the array contained by RelationalAttributes</param>
        /// <param name="value">The value the attribute should have for the element to be returned</param>
        /// <returns>An SQL statement in string format that retrieves all elements with the attribute of the given number equal to the given value</returns>
        protected string SQLGetFromAtttribute(int attributeNr, string value) {
            string re = $"SELECT * FROM {_relationalName} WHERE {_relationalAttributes[attributeNr]} = {value};";

            return re;
        }
        /// <summary>
        /// Creates an SQL quary that retrieves all elements where the key of the given index number is like the given value.
        /// </summary>
        /// <param name="keyNr">The index number of the key. Must be a non-negative integer less than the length of the array contained by RelationalKeys</param>
        /// <param name="value">The value the key should be like for the element to be returned</param>
        /// <returns>An SQL statement in string format that retrieves all elements with a key of the given number that is like the given value</returns>
        protected string SQLGetLikeKey(int keyNr, string value) {
            string re = $"SELECT * FROM {_relationalName} WHERE {_relationalKeys[keyNr]} LIKE \'%{value}%\';";

            return re;
        }
        /// <summary>
        /// Creates an SQL quary that retrieves all elements where the attribute of the given index number is like the given value.
        /// </summary>
        /// <param name="attributeNr">The index number of the attribute. Must be a non-negative integer less than the length of the array contained by RelationalAttributes</param>
        /// <param name="value">The value the attribute should be like for the element to be returned</param>
        /// <returns>An SQL statement in string format that retrieves all elements with the attribute of the given number that is like the given value</returns>
        protected string SQLGetLikeAtttribute(int attributeNr, string value) {
            string re = $"SELECT * FROM {_relationalName} WHERE {_relationalAttributes[attributeNr]} LIKE \'%{value}%\';";

            return re;
        }
        /// <summary>
        /// Contains an SQL quary that retrieves the element with a specific set of keys from the table with the name contained by RelationalName as a string.
        /// </summary>
        protected string SQLGet {
            get {
                string re = $"SELECT * FROM {_relationalName} WHERE ";
                for (int i = 0; i < _relationalKeys.Length; i++) {
                    re += _relationalKeys[i] + " = @" + _relationalKeys[i];

                    if (i < _relationalKeys.Length - 1) {
                        re += " AND ";
                    }
                }
                re += ";";

                return re;
            }
        }
        /// <summary>
        /// Contains an SQL quary that inserts an element with a specific set of keys and attributes into the table with the name contained by RelationalName as a string.
        /// </summary>
        protected string SQLInsert {
            get {
                string re = $"INSERT INTO {_relationalName} (";
                foreach (string s in _relationalKeys) {
                    re += s + ",";
                }
                foreach (string s in _relationalAttributes) {
                    re += s + ",";
                }
                re = re.Remove(re.Length - 1);

                re += ") VALUES(";
                foreach (string s in _relationalKeys) {
                    re += "@" + s + ",";
                }
                foreach (string s in _relationalAttributes) {
                    re += "@" + s + ",";
                }
                re = re.Remove(re.Length - 1);

                re += ");";

                return re;
            }
        }
        /// <summary>
        /// Contains an SQL quary that deletes the element with a specific set of keys from the table with the name contained by RelationalName as a string.
        /// </summary>
        protected string SQLDelete {
            get {
                string re = $"DELETE FROM {_relationalName} WHERE ";
                for (int i = 0; i < _relationalKeys.Length; i++) {
                    re += _relationalKeys[i] + " = @" + _relationalKeys[i];

                    if (i < _relationalKeys.Length - 1) {
                        re += " AND ";
                    }
                }
                re += ";";

                return re;
            }
        }
        /// <summary>
        /// Contains an SQL quary that updates the element with a specific set of keys from the table with the name contained by RelationalName as a string.
        /// </summary>
        protected string SQLUpdate {
            get {
                string re = $"UPDATE {_relationalName} SET ";
                foreach (string s in _relationalKeys) {
                    re += s + " = @" + s + ",";
                }
                foreach (string s in _relationalAttributes) {
                    re += s + " = @" + s + ",";
                }
                re = re.Remove(re.Length - 1);

                re += " WHERE ";
                for (int i = 0; i < _relationalKeys.Length; i++) {
                    re += _relationalKeys[i] + " = @To_Update_" + i;

                    if (i < _relationalKeys.Length - 1) {
                        re += " AND ";
                    }
                }
                re += ";";

                return re;
            }
        }

        public abstract Task<List<T>> GetAllItems();
        public abstract Task<List<T>> GetItemsWithKey(int keyNr, int key);
        public abstract Task<List<T>> GetItemsWithAttribute(int attributeNr, object attribute);
        public abstract Task<List<T>> GetItemsWithAttributeLike(int attributeNr, string attribute);
        public abstract Task<T> GetItem(int[] ids);
        public abstract Task<bool> CreateItem(T t);
        public abstract Task<bool> UpdateItem(T t, int[] ids);
        public abstract Task<T> DeleteItem(int[] ids);
    }
}