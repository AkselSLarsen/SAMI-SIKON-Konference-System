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
        public void UserCatalogueTests() {
            _ = UserCatalogueInsertTest();
            _ = UserCatalogueReadTest();
            _ = UserCatalogueUpdateTest();
            _ = UserCatalogueFindTest();
            _ = UserCatalogueDeleteTest();
        }
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
        public async Task UserCatalogueReadTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();

            int id = users[0].Id;
            IUser user = await uc.GetItem(new int[] { id });

            Assert.IsTrue(user.Id == users[0].Id);
        }
        public async Task UserCatalogueUpdateTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetItemsWithAttribute(0, "test@test.mail.com");
            IUser preUser = users[0];

            IUser postUser = new Administrator(preUser.Id, "Test@Admin.org", "12345", "5417", "34343260", "Org.Slave Nr. ADM-217");

            bool success = await uc.UpdateItem(postUser, new int[] { preUser.Id });

            Assert.IsTrue(success);
        }
        public async Task UserCatalogueFindTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();

            IUser user = users[0];
            List<IUser> usersWithKey = await uc.GetItemsWithKey(0, user.Id);

            string userEmail = user.Email[1..(user.Email.Length-1)];
            List<IUser> usersWithAttributeLike = await uc.GetItemsWithAttributeLike(0, userEmail);

            bool findsSameUser = false;
            foreach(IUser uwk in usersWithKey) {
                foreach(IUser uwal in usersWithAttributeLike) {
                    if(uwk.Id == uwal.Id) {
                        findsSameUser = true;
                    }
                }
            }
            Assert.IsTrue(findsSameUser);
        }
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
        public void RoomCatalogueTests() {
            _ = RoomCatalogueInsertTest();
            _ = RoomCatalogueReadTest();
            _ = RoomCatalogueUpdateTest();
            _ = RoomCatalogueFindTest();
            _ = RoomCatalogueDeleteTest();
        }
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
        public async Task RoomCatalogueReadTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();
            int id = rooms[0].Id;
            Room room = await rc.GetItem(new int[] { id });

            Assert.IsTrue(room.Id == rooms[0].Id);
        }
        public async Task RoomCatalogueUpdateTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "S");
            Room preRoom = rooms[0];

            Room postRoom = new Room(2, "SS");

            bool success = await rc.UpdateItem(postRoom, new int[] { preRoom.Id });

            Assert.IsTrue(success);
        }
        public async Task RoomCatalogueFindTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();

            Room room = rooms[0];
            List<Room> roomsWithKey = await rc.GetItemsWithKey(0, room.Id);

            string roomLayout = room.GetLayoutAsString()[1..(room.GetLayoutAsString().Length - 1)];
            List<Room> roomsWithAttributeLike = await rc.GetItemsWithAttributeLike(0, roomLayout);

            bool findsSameRoom = false;
            foreach (Room rwk in roomsWithKey) {
                foreach (Room rwal in roomsWithAttributeLike) {
                    if (rwk.Id == rwal.Id) {
                        findsSameRoom = true;
                    }
                }
            }
            Assert.IsTrue(findsSameRoom);
        }
        public async Task RoomCatalogueDeleteTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SS");
            Room inputRoom = rooms[0];

            Room outputRoom = await rc.DeleteItem(new int[] { inputRoom.Id });

            Assert.AreEqual(inputRoom.Id, outputRoom.Id);
        }
        #endregion
        #region EventCatalogue Testing
        [TestMethod]
        public void EventCatalogueTests() {
            _ = EventCatalogueInsertTest();
            _ = EventCatalogueReadTest();
            _ = EventCatalogueUpdateTest();
            _ = EventCatalogueFindTest();
            _ = EventCatalogueDeleteTest();
        }
        public async Task EventCatalogueInsertTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetAllItems();
            int preNr = evts.Count;

            //Create a room to attach the event to.
            RoomCatalogue rc = new RoomCatalogue();
            bool succes = await rc.CreateItem(new Room(0, "SSS"));
            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SSS");
            Room room = rooms[0];
            //And now we can continue

            Event evt = new Event(0, room.Id, null, DateTime.Now, "description", "name", "theme", 60, new bool[] { false });

            await ec.CreateItem(evt);

            evts = await ec.GetAllItems();
            int postNr = evts.Count;

            Assert.IsTrue(preNr == postNr - 1);
        }
        public async Task EventCatalogueReadTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetAllItems();

            int id = evts[0].Id;
            Event evt = await ec.GetItem(new int[] { id });

            Assert.IsTrue(evt.Id == evts[0].Id);
        }
        public async Task EventCatalogueUpdateTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetItemsWithAttribute(0, "description");
            Event preEvent = evts[0];

            Event postEvent = new Event(0, preEvent.RoomNr, null, DateTime.Now, "something", "name", "theme", 60, new bool[] { false });

            bool success = await ec.UpdateItem(postEvent, new int[] { preEvent.Id });

            Assert.IsTrue(success);
        }
        public async Task EventCatalogueFindTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetAllItems();

            Event evt = evts[0];
            List<Event> evtsWithKey = await ec.GetItemsWithKey(0, evt.Id);

            string evtDescription = evt.Description[1..(evt.Description.Length - 1)];
            List<Event> evtsWithAttributeLike = await ec.GetItemsWithAttributeLike(0, evtDescription);

            bool findsSameEvent = false;
            foreach (Event ewk in evtsWithKey) {
                foreach (Event ewal in evtsWithAttributeLike) {
                    if (ewk.Id == ewal.Id) {
                        findsSameEvent = true;
                    }
                }
            }
            Assert.IsTrue(findsSameEvent);
        }
        public async Task EventCatalogueDeleteTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetItemsWithAttribute(0, "something");
            Event inputEvent = evts[0];

            Event outputEvent = await ec.DeleteItem(new int[] { inputEvent.Id });

            //Clean up the room made in "EventCatalogueInsertTest()"
            RoomCatalogue rc = new RoomCatalogue();
            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SSS");
            Room room = rooms[rooms.Count-1];
            _ = await rc.DeleteItem(new int[] { room.Id });
            //And now we can continue

            Assert.AreEqual(inputEvent.Id, outputEvent.Id);
        }
        #endregion
    }
}