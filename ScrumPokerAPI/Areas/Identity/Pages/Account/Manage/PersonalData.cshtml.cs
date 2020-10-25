using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ScrumPokerAPI.Context;

namespace ScrumPokerAPI.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : BaseModelUser
    {
        public PersonalDataModel(UserManager<IdentityUser> userManager) : base(userManager)
        {
        }
    }
}