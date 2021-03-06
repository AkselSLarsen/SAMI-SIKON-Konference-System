using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Model
{
    public class Administrator : IUser
    {

        public Administrator() {

        }

        public Administrator(int id, string email, string password, string salt, string phoneNumber, string name) {
            Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            PhoneNumber = phoneNumber;
            Name = name;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Id { get; set; }
        public List<Booking> Bookings { get { return new List<Booking>(); } }



    }
}
