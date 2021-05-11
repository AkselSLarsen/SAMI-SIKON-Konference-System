using SAMI_SIKON.Interfaces;
using System.Collections.Generic;

namespace SAMI_SIKON.Model {
    public class Participant : IUser {

        public Participant(int id, string email, string password, string salt, string phoneNumber, string name, List<Booking> bookings) {
            Id = id;
            Email = email;
            Password = password;
            Salt = salt;
            PhoneNumber = phoneNumber;
            Name = name;
            Bookings = bookings;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Id { get; set; }
        public List<Booking> Bookings { get; private set; }


    }
}
