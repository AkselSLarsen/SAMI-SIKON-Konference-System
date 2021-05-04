using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Interfaces;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Pages.Login
{
    public class LoginPageModel : PageModel
    {
        [BindProperty]
        public IUser UserLogin { get; set; }

        public UserCatalogue Users { get; set; }
        public string errorMessage;

        public LoginPageModel(UserCatalogue users)
        {
            Users = users;
            errorMessage = "";
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
