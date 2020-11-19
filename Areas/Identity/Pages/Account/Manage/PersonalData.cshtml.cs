using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;

namespace ScrumPokerPlanning.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : BaseModelUser
    {
        public PersonalDataModel(UserManager<ApplicationUser> userManager) : base(userManager)
        {
        }
    }
}