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
        public Participant User { get; set; }

        public UserCatalogue Users { get; }
        public string CreationMessage;
        public RegisterPageModel(UserCatalogue users)
        {
            Users = users;
            User=new Participant();
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

            IUser newUser = Users.RegisterUser(User.Email, User.Password, User.PhoneNumber, User.Name, false);
            bool userCreated = Users.CreateItem(newUser).Result;
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
