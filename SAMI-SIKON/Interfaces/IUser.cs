using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Interfaces
{
    public interface IUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Id { get; set; }


        public virtual bool Login()
        {
            UserCatalogue users = new UserCatalogue();
            return users.Login(Email, Password);
        }

        public virtual bool Register()
        {
            UserCatalogue users = new UserCatalogue();
            return users.RegisterUser(Email, Password, PhoneNumber, Name, this is Administrator).Result;
        }
    }
}
