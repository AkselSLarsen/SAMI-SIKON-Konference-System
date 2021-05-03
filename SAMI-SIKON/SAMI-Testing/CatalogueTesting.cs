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
    }
}
        
