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
        public List<Booking> Bookings { get; }


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
        /// <summary>
        /// Even though it's called NewPassword it also changes the salt since that is commonly done when changing a password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool NewPassword(string password)
        {
            UserCatalogue users = new UserCatalogue();
            return users.UpdatePassword(this, password).Result;
        }

        //There is a difference between how the NewPassword and Login methods handle the password, in login it is contained as plain text in the IUser object,
        //in the NewPassword it is taken as a parameter, its worth discussing which is better since normally the Password property in a user is a hashed password, and not plain text.
    }
}
