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
        public async Task UserCatalogueTests() {
            Assert.IsTrue(await UserCatalogueInsertTest());
            Assert.IsTrue(await UserCatalogueReadTest());
            Assert.IsTrue(await UserCatalogueUpdateTest());
            Assert.IsTrue(await UserCatalogueFindTest());
            Assert.IsTrue(await UserCatalogueDeleteTest());
        }
        public async Task<bool> UserCatalogueInsertTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();
            int preNr = users.Count;

            IUser user = new Participant(0, "test@test.mail.com", "p455w02d", "5417", "+4512345678", "Test Testersen", new List<Booking>());

            await uc.CreateItem(user);

            users = await uc.GetAllItems();
            int postNr = users.Count;

            return preNr == postNr - 1;
        }
        public async Task<bool> UserCatalogueReadTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();

            int id = users[0].Id;
            IUser user = await uc.GetItem(new int[] { id });

            return user.Id == users[0].Id;
        }
        public async Task<bool> UserCatalogueUpdateTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetItemsWithAttribute(0, "test@test.mail.com");
            IUser preUser = users[0];

            IUser postUser = new Administrator(preUser.Id, "Test@Admin.org", "12345", "5417", "34343260", "Org.Slave Nr. ADM-217");

            bool success = await uc.UpdateItem(postUser, new int[] { preUser.Id });

            return success;
        }
        public async Task<bool> UserCatalogueFindTest() {
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
            return findsSameUser;
        }
        public async Task<bool> UserCatalogueDeleteTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetItemsWithAttribute(0, "Test@Admin.org");
            IUser inputUser = users[0];

            IUser outputUser = await uc.DeleteItem(new int[] { inputUser.Id });

            return inputUser.Id == outputUser.Id;
        }
        #endregion
        #region RoomCatalogue Testing
        [TestMethod]
        public async Task RoomCatalogueTests() {
            Assert.IsTrue(await RoomCatalogueInsertTest());
            Assert.IsTrue(await RoomCatalogueReadTest());
            Assert.IsTrue(await RoomCatalogueUpdateTest());
            Assert.IsTrue(await RoomCatalogueFindTest());
            Assert.IsTrue(await RoomCatalogueDeleteTest());
        }
        public async Task<bool> RoomCatalogueInsertTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();
            int preNr = rooms.Count;

            Room room = new Room(1, "S", "single");

            await rc.CreateItem(room);

            rooms = await rc.GetAllItems();
            int postNr = rooms.Count;

            return preNr == postNr - 1;
        }
        public async Task<bool> RoomCatalogueReadTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();
            int id = rooms[0].Id;
            Room room = await rc.GetItem(new int[] { id });

            return room.Id == rooms[0].Id;
        }
        public async Task<bool> RoomCatalogueUpdateTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "S");
            Room preRoom = rooms[0];

            Room postRoom = new Room(2, "SS", "double");

            bool success = await rc.UpdateItem(postRoom, new int[] { preRoom.Id });

            return success;
        }
        public async Task<bool> RoomCatalogueFindTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetAllItems();

            Room room = rooms[0];
            List<Room> roomsWithKey = await rc.GetItemsWithKey(0, room.Id);

            string roomLayout = room.LayoutAsString()[1..(room.LayoutAsString().Length - 1)];
            List<Room> roomsWithAttributeLike = await rc.GetItemsWithAttributeLike(0, roomLayout);

            bool findsSameRoom = false;
            foreach (Room rwk in roomsWithKey) {
                foreach (Room rwal in roomsWithAttributeLike) {
                    if (rwk.Id == rwal.Id) {
                        findsSameRoom = true;
                    }
                }
            }
            return findsSameRoom;
        }
        public async Task<bool> RoomCatalogueDeleteTest() {
            RoomCatalogue rc = new RoomCatalogue();

            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SS");
            Room inputRoom = rooms[0];

            Room outputRoom = await rc.DeleteItem(new int[] { inputRoom.Id });

            return inputRoom.Id == outputRoom.Id;
        }
        #endregion
        #region EventCatalogue Testing
        [TestMethod]
        public async Task EventCatalogueTestsAsync() {
            Assert.IsTrue(await EventCatalogueInsertTest());
            Assert.IsTrue(await EventCatalogueReadTest());
            Assert.IsTrue(await EventCatalogueUpdateTest());
            Assert.IsTrue(await EventCatalogueFindTest());
            Assert.IsTrue(await EventCatalogueDeleteTest());
        }
        public async Task<bool> EventCatalogueInsertTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetAllItems();
            int preNr = evts.Count;

            //Create a room to attach the event to.
            RoomCatalogue rc = new RoomCatalogue();
            bool successR = await rc.CreateItem(new Room(0, "SSS", "triple"));
            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SSS");
            Room room = rooms[0];
            //And a user to become a speaker
            UserCatalogue uc = new UserCatalogue();
            bool successU = await uc.CreateItem(new Participant(0, "Test@Testing.biz", "password", "salt", "0000", "Tester", new List<Booking>()));
            List<IUser> users = await uc.GetItemsWithAttribute(0, "Test@Testing.biz");
            IUser user = users[0];
            //And now we can continue

            List<int> uIds = new List<int>();
            uIds.Add(user.Id);
            Event evt = new Event(0, room.Id, uIds, DateTime.Now, "description", "name", 60, new int[] { 1 });
            bool success = await ec.CreateItem(evt);

            //Add a booking
            List<Event> es = await ec.GetItemsWithAttribute(0, "description");
            Event eve = es[0];

            List<Booking> bookings = new List<Booking>();
            bookings.Add(new Booking(0, eve.Id, 1));
            IUser u = new Participant(0, user.Email, user.Password, user.Salt, user.PhoneNumber, user.Name, bookings);
            await uc.UpdateItem(u, new int[] { (await uc.GetItemsWithAttribute(0, "Test@Testing.biz"))[0].Id });
            //and done

            evts = await ec.GetAllItems();
            int postNr = evts.Count;

            return preNr == postNr - 1;
        }
        public async Task<bool> EventCatalogueReadTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetAllItems();

            int id = evts[0].Id;
            Event evt = await ec.GetItem(new int[] { id });

            return evt.Id == evts[0].Id;
        }
        public async Task<bool> EventCatalogueUpdateTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetItemsWithAttribute(0, "description");
            Event preEvent = evts[0];

            Event postEvent = new Event(0, preEvent.RoomNr, preEvent.Speakers, DateTime.Now, "something", "name", 60, preEvent.SeatsTaken());

            bool success = await ec.UpdateItem(postEvent, new int[] { preEvent.Id });

            postEvent = await ec.GetItem(new int[] { preEvent.Id });
            if(postEvent.Speakers.Count > 0 && postEvent.SeatsTaken().Length > 0) {
                return success;
            } else {
                return false;
            }
        }
        public async Task<bool> EventCatalogueFindTest() {
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
            return findsSameEvent;
        }
        public async Task<bool> EventCatalogueDeleteTest() {
            EventCatalogue ec = new EventCatalogue();

            List<Event> evts = await ec.GetItemsWithAttribute(0, "something");
            Event inputEvent = evts[0];

            Event outputEvent = await ec.DeleteItem(new int[] { inputEvent.Id });

            //Clean up the room made in "EventCatalogueInsertTest()"
            RoomCatalogue rc = new RoomCatalogue();
            List<Room> rooms = await rc.GetItemsWithAttribute(0, "SSS");
            Room room = rooms[rooms.Count-1];
            _ = await rc.DeleteItem(new int[] { room.Id });
            //And the user that acted as speaker
            UserCatalogue uc = new UserCatalogue();
            List<IUser> users = await uc.GetItemsWithAttribute(0, "Test@Testing.biz");
            IUser user = users[users.Count - 1];
            _ = await uc.DeleteItem(new int[] { user.Id });
            //And now we can continue

            return inputEvent.Id == outputEvent.Id;
        }
        #endregion
    }
}