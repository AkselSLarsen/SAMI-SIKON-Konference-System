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

            Assert.IsTrue(preNr == postNr-1);
        }

        [TestMethod]
        public async Task UserCatalogueReadAllTest() {
            UserCatalogue uc = new UserCatalogue();

            List<IUser> users = await uc.GetAllItems();

            Assert.IsTrue(users.Count > 0);
        }

        /*
        [TestMethod]
        public async Task HotelReadTestAsync() {
            HotelService hs = new HotelService();

            List<Hotel> hotels = await hs.GetAllItems();

            Assert.IsTrue(hotels.Count > 0);
        }

        [TestMethod]
        public async Task RoomReadTestAsync() {
            RoomService rs = new RoomService();

            List<Room> rooms = await rs.GetAllItems();

            Assert.IsTrue(rooms.Count > 0);
        }

        [TestMethod]
        public async Task HotelCreateDeleteTestAsync() {
            HotelService hs = new HotelService();

            List<Hotel> hotels = await hs.GetAllItems();

            int id = getUniqueHotelId(hotels);

            Hotel testHotel = new Hotel(id, "test_name", "test_address");

            bool createSuccess = await hs.CreateItem(testHotel);
            Hotel deletedHotel = await hs.DeleteItem(new int[] { id });

            Assert.IsTrue(createSuccess && deletedHotel.Equals(testHotel));
        }

        [TestMethod]
        public async Task RoomCreateDeleteTestAsync() {
            HotelService hs = new HotelService();
            RoomService rs = new RoomService();

            //create hotel
            List<Hotel> hotels = await hs.GetAllItems();

            int hotelId = getUniqueHotelId(hotels);

            Hotel testHotel = new Hotel(hotelId, "test_name", "test_address");
            bool createHotelSuccess = await hs.CreateItem(testHotel);

            //create room
            List<Room> rooms = await rs.GetItemsWithKey(0, hotelId);

            int roomId = getUniqueRoomId(rooms);

            Room testRoom = new Room(roomId, hotelId, RoomTypes.S, 0.0D);
            bool createRoomSuccess = await rs.CreateItem(testRoom);

            //delete
            Room deletedRoom = await rs.DeleteItem(new int[] { roomId, hotelId });
            Hotel deletedHotel = await hs.DeleteItem(new int[] { hotelId });

            Assert.IsTrue(createHotelSuccess && createRoomSuccess && deletedHotel.Equals(testHotel) && deletedRoom.Equals(testRoom));
        }

        private int getUniqueHotelId(List<Hotel> hotels) {
            int id = -1;
            for (int i = 0; i < int.MaxValue; i++) {
                bool idTaken = false;
                foreach (Hotel hotel in hotels) {
                    if (hotel.Hotel_No == i) {
                        idTaken = true;
                    }
                }

                if (!idTaken) {
                    id = i;
                    break;
                }
            }

            return id;
        }

        private int getUniqueRoomId(List<Room> rooms) {
            int id = -1;
            for (int i = 0; i < int.MaxValue; i++) {
                bool idTaken = false;
                foreach (Room room in rooms) {
                    if (room.Hotel_No == i) {
                        idTaken = true;
                    }
                }

                if (!idTaken) {
                    id = i;
                    break;
                }
            }

            return id;
        }
        */
    }
}
        
