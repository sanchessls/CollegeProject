using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ScrumPokerPlanning.Context;

namespace ScrumPokerPlanning.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : BaseModelUser
    {
        public PersonalDataModel(UserManager<IdentityUser> userManager) : base(userManager)
        {
        }
    }
}