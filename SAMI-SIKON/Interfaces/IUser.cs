using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

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
        public bool Admis

        public void Login();
    }
}
