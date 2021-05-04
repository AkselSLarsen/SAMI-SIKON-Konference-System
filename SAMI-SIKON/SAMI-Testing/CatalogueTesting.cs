using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SAMI_Testing {
    [TestClass]
    public class CatalogueTesting {

        #region UserCatalogue Testing
        [TestMethod]
        public async Task UserCatalogueInsertTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();
            int preNr = users.Count;

            IUser user = new Participant();

            user.Email = "test@test.mail.com";
            user.Name = "Test Testersen";
            user.Password = "p455w02d";
            user.Salt = "5417";
            user.PhoneNumber = "+4512345678";

            await uc.CreateItem(user);

            users = await uc.GetAllItems();
            int postNr = users.Count;

            Assert.IsTrue(preNr == postNr - 1);
        }

        [TestMethod]
        public async Task UserCatalogueReadAllTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();

            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public async Task UserCatalogueUpdateTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetItemsWithAttribute(0, "test@test.mail.com");
            IUser preUser = users[0];

            IUser postUser = new Administrator(preUser.Id, "Test@Admin.org", "12345", "5417", "34343260", "Org.Slave Nr. ADM-217");

            bool success = await uc.UpdateItem(postUser, new int[] { preUser.Id });

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task UserCatalogueDeleteTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetItemsWithAttribute(0, "Test@Admin.org");
            IUser inputUser = users[0];

            IUser outputUser = await uc.DeleteItem(new int[] { inputUser.Id });

            Assert.AreEqual(inputUser.Id, outputUser.Id);
        }
        #endregion
        #region RoomCatalogue Testing
        [TestMethod]
        public async Task RoomCatalogueInsertTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();
            int preNr = rooms.Count;

            Room room = new Room(1, "S");

            await rc.CreateItem(room);

            rooms = await rc.GetAllItems();
            int postNr = rooms.Count;

            Assert.IsTrue(preNr == postNr - 1);
        }

        [TestMethod]
        public async Task RoomCatalogueReadAllTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();

            Assert.IsTrue(rooms.Count > 0);
        }

        [TestMethod]
        public async Task RoomCatalogueUpdateTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "S");
            Room preRoom = rooms[0];

            Room postRoom = new Room(2, "SS");

            bool success = await rc.UpdateItem(postRoom, new int[] { preRoom.Id });

            Assert.IsTrue(success);
        }

        [TestMethod]
        public async Task RoomCatalogueDeleteTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SS");
            Room inputRoom = rooms[0];

            Room outputRoom = await rc.DeleteItem(new int[] { inputRoom.Id });

            Assert.AreEqual(inputRoom.Id, outputRoom.Id);
        }
        #endregion
    }
}
        
