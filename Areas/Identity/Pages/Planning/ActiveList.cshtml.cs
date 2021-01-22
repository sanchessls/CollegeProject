using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Implementation;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class ActiveList : BaseModelDatabaseUser
    {
        public ActiveList(ApplicationContext context, UserManager<ApplicationUser> userManager) : base(context, userManager)
        {
        }

        public List<PlanningSession> PlanningSessionList;
        public string Username { get; set; }

        public override Task LoadAsync()
        {            
            PlanningSessionList = _appContext.PlanningSessionUser.Include(a => a.PlanningSession).Where(x => x.UserId == userIdentity().Id && x.PlanningSession.Status == EnumPlanningSession.Open).Select(g => g.PlanningSession).OrderByDescending(x => x.CreationDate).ToList();
            return base.LoadAsync();
        }
    }
}
