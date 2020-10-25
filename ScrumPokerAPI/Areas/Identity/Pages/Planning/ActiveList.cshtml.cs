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
    public partial class ActiveList : BaseModelDatabaseUser
    {
        public ActiveList(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        public List<PlanningSession> PlanningSessionList;
        public string Username { get; set; }

        public override Task LoadAsync()
        {            
            PlanningSessionList = _appContext.PlanningSession.Where(x => x.UserId == userIdentity().Id && x.Status == 1).ToList();
            return base.LoadAsync();
        }
    }
}
