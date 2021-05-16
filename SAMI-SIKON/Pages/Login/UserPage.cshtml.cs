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
    public class UserPageModel : PageModel
    {
        [BindProperty]
        public string UserPassword { get; set; }
        [BindProperty]
        public string NewPassword1 { get; set; }
        [BindProperty]
        public string NewPassword2 { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }



        public void OnGet()
        {
        }
        //Could maybe also allow updating the phone number
        public IActionResult OnPost()
        {
            IUser UserLogin = UserCatalogue.CurrentUser;
            
            bool loginCheck = UserLogin.Login();
            if (!loginCheck)
            {
                ErrorMessage = "Incorrect Current password";
                return Page();
            }
            if (NewPassword1 != NewPassword2)
            {
                ErrorMessage = "New Password was not the same in both boxes"; //underlig formulering
                return Page();
            }
            bool PasswordUpdated = UserLogin.NewPassword(NewPassword1);
            if (!PasswordUpdated)
            {
                ErrorMessage = "Password not updated should not happen"; // I don't know if this will happen, but i don't think it should
                return Page();
            }
            else
            {
                SuccessMessage = "Password updated";
                return Page();
            }
        }

    }
}
