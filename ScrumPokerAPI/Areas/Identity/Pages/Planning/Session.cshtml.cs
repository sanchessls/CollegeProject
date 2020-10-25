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
    public partial class Session : BaseModelDatabaseUser
    {        
        public Session(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        public PlanningSession SessionObject;
     
        public override Task LoadAsync()
        {
            int idSession = Convert.ToInt32(Request.Query["id"]);
            SessionObject = _appContext.PlanningSession.Where(x => x.Id == idSession).FirstOrDefault();
            return base.LoadAsync();
        }

    }
}
