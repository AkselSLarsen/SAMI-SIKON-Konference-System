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
                //change errorMessage to something depending on what is wrong with the model
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
