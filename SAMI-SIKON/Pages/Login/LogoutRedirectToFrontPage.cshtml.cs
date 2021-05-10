using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SAMI_SIKON.Services;

namespace SAMI_SIKON.Pages.Login
{
    public class LogoutRedirectToFrontPageModel : PageModel
    {
        public IActionResult OnGet()
        {
            UserCatalogue.Logout();
            return RedirectToPage("/Index");
        }


        //Would like to make an alert appear on Index when you get redirected that tells you that you are logged out, it is not very clear at the moment
        //<div class="alert">
        //  <span class="closebtn" onclick="this.parentElement.style.display='none';">&times;</span>
        //  This is an alert box.
        //</div>
        //Something like this that shows up if you get redirected from here maybe
    }
}
