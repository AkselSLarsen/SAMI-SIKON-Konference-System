using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Pages.Login
{
    public class RegisterPageModel : PageModel
    {
        //Change this to individual properties
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public string Password { get; set; }



        public string CreationMessage;
        public RegisterPageModel()
        {
            CreationMessage = "";
        }

        public void OnGet() 
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                //The modelstate should almost always be valid since the only things to input are string, which means you would only get an error if they're too long i think.
                return Page();
            }

            IUser User = new Participant(3, Email, Password, "", PhoneNumber, UserName, new List<Booking>());
            
            bool userCreated = User.Register();
            if (userCreated)
            {
                CreationMessage = $"Success, User was created";
                return Page();
            }
            else
            {
                CreationMessage = $"Error, User was not created"; //formulering
                return Page();
            }
            
        }
    }
}
