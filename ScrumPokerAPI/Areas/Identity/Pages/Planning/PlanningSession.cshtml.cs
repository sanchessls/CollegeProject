using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;
using ScrumPokerAPI.Repositories.Implementation;
using ScrumPokerAPI.Repositories.Interface;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public partial class PlanningSessionInfoModel : PageModel
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepositoryPlanningSession _appContext;


        public List<PlanningSession> PlanningSessionList;
        public string Username { get; set; }

        public PlanningSessionInfoModel(IRepositoryPlanningSession appContext, UserManager<IdentityUser> userManager)
        {
            _appContext = appContext;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();            
        }

        private async Task LoadAsync(IdentityUser user)
        {            
            Username = user.UserName;
            PlanningSessionList = _appContext.GetWhere(x => x.UserId == user.Id).ToList();
        }
    }
}
