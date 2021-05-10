using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Model;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Pages.Login
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public Participant UserLogin { get; set; }

        public UserCatalogue Users { get; set; }
        public string errorMessage;
        //Har addet UserCatalogue som transient fordi den har nogen metoder som Catalogue og ICatalogue ikke har, kan godt vaere at de metoder skal flyttes et andet sted hen
        public LoginPageModel(UserCatalogue users)
        {
            Users = users;
            errorMessage = "";
            UserLogin=new Participant();
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            bool loginCheck = Users.Login(UserLogin.Email, UserLogin.Password);
            if (loginCheck)
            {
                return RedirectToPage("Index");
            }
            else
            {
                errorMessage = "Incorrect e-mail or password";
                return Page();
            }
            
        }
    }
}
