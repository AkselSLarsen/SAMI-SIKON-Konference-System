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
        public string UserEmail { get; set; }
        [BindProperty]
        public string UserPassword { get; set; }
        public string errorMessage;
        
        public LoginPageModel()
        {
            errorMessage = "";
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            IUser UserLogin = new Participant(0, UserEmail, UserPassword, "", "","",new List<Booking>());
            bool loginCheck = UserLogin.Login();
            if (loginCheck)
            {
                return Redirect("~/");
            }
            else
            {
                errorMessage = "Incorrect e-mail or password";
                return Page();
            }
            
        }
    }
}
