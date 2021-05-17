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
        [BindProperty]
        public IUser User { get; set; }
        //Change this to individual properties
        
        public string CreationMessage;
        public RegisterPageModel()
        {
            User = new Participant(0, "", "", "", "", "", new List<Booking>());
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
