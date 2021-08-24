using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AuthSessionDemoAuthorization.Pages
{
    //No Authorization
    //[Authorize]

    //--------------------------
    // Role Based Authorization

    //[Authorize(Roles = "Admin")]

    // OR condition 
    //[Authorize(Roles = "Admin,Editor")]

    //// AND condition
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "Editor")]
    //--------------------------

    //-------------------------
    // Policy Based Authorization
    // Policy helps creating and maintaining rules from one place. 

    //[Authorize(Policy = "AdminsOnly")] // There can be many existing admin category roles, and new roles may be added or deleted at any point of time.

    //-------------------------

    //----------------------------
    //Claims based Authorization

    //[Authorize(Policy = "WithWriteClaimOnly")] // There can be many existing admin category roles, and new roles may be added or deleted at any point of time.

    //----------------------------
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}
