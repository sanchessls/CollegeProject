using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public class BaseModelDatabaseUser : BaseModelUser
    {
        protected readonly ApplicationContext _appContext;

        public BaseModelDatabaseUser(ApplicationContext appContext, UserManager<ApplicationUser> userManager) :base(userManager)
        {
            _appContext = appContext;
        }
        
    }



}
