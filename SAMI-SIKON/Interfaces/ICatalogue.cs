using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SAMI_SIKON.Interfaces {
    public interface ICatalogue<T> {

        /// <summary>
        /// Should contain the name of the database table.
        /// </summary>
        string RelationalName { get; protected set; }
        /// <summary>
        /// Should contain the name(s) of the key(s) of the database table.
        /// </summary>
        string[] RelationalKeys { get; protected set; }
        /// <summary>
        /// Should contain the name(s) of the non-prime attribute(s) of the database table. 
        /// </summary>
        string[] RelationalAttributes { get; protected set; }

        /// <summary>
        /// Retrieves all elements of the table with the name contained by the RelationalName property.
        /// Elements possess all keys and attributes contained by the RelationalKeys and RelationalAttributes properties.
        /// </summary>
        /// <returns>A list of all elements in the table with the name contained in RelationalName</returns>
        Task<List<T>> GetAllItems();
        /// <summary>
        /// Retrieves all elements of the table where the key with the given number is equal to the given key.
        /// Elements possess all keys and attributes contained by the RelationalKeys and RelationalAttributes properties.
        /// </summary>
        /// <param name="keyNr">The placement of the keyname in the array returned by RelationalKeys</param>
        /// <param name="key">The value the key should have for the element to be returned</param>
        /// <returns>A list of all elements with the specified key</returns>
        Task<List<T>> GetItemsWithKey(int keyNr, int key);
        /// <summary>
        /// Retrieves all elements of the table where the attribute with the given number is equal to the given attribute.
        /// Elements possess all keys and attributes contained by the RelationalKeys and RelationalAttributes properties.
        /// </summary>
        /// <param name="attributeNr">The placement of the attributename in the array returned by RelationalAttributes</param>
        /// <param name="attribute">The value the attribute should have for the element to be returned</param>
        /// <returns>A list of all elements with the specified attribute</returns>
        Task<List<T>> GetItemsWithAttribute(int attributeNr, object attribute);
        /// <summary>
        /// Retrieves all elements of the table where the attribute with the given number contains the given attribute.
        /// Elements possess all keys and attributes contained by the RelationalKeys and RelationalAttributes properties.
        /// </summary>
        /// <param name="attributeNr">The placement of the attributename in the array returned by RelationalAttributes</param>
        /// <param name="attribute">The value the attribute should contain for the element to be returned</param>
        /// <returns>A list of all elements with the attribute containing the specified attribute</returns>
        Task<List<T>> GetItemsWithAttributeLike(int attributeNr, string attribute);
        /// <summary>
        /// Retrieves the element, of the table with the name returned by the RelationalName property, with the given ids.
        /// the element possess all keys and attributes contained by the RelationalKeys and RelationalAttributes properties.
        /// </summary>
        /// <param name="ids">The key values of the desired elements. This array should have the same length as the one contained by RelationalKeys</param>
        /// <returns>The element with the keys equal to the given ids. If no element has the given key combination, then it returns null</returns>
        Task<T> GetItem(int[] ids);
        /// <summary>
        /// Inserts a new element with the properties of the item into the table with the name contained by the RelationalName property.
        /// </summary>
        /// <param name="t">The element to insert</param>
        /// <returns>Returns if the opperation successfully inserted the element</returns>
        Task<bool> CreateItem(T t);
        /// <summary>
        /// Updates the element with keys equal to the given ids, from the table with the name contained by the RelationalName property.
        /// It is possible to change the keys of the element, by having the item contain different keys than the given ids.
        /// </summary>
        /// <param name="t">The element to insert</param>
        /// <param name="ids">The key combination of the element to delete. This array should have the same length as the one contained by RelationalKeys</param>
        /// <returns>Returns if the opperation successfully updated the element</returns>
        Task<bool> UpdateItem(T t, int[] ids);
        /// <summary>
        /// Deletes the element with keys equal to the given ids.
        /// </summary>
        /// <param name="ids">The key combination of the element to delete. This array should have the same length as the one contained by RelationalKeys</param>
        /// <returns>The deleted element if any, otherwise null</returns>
        Task<T> DeleteItem(int[] ids);

    }
}
