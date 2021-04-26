using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SAMI_SIKON.Interfaces;

namespace SAMI_SIKON.Model
{
    public class Administrator : IUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int Id { get; set; }

        public void Login()
        {
            throw new NotImplementedException();
        }
    }
}
