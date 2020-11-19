using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ScrumPokerPlanning.Models;

namespace ScrumPokerPlanning.Areas.Identity.Pages.Account.Manage
{
    public partial class JiraIntegrationModel : BaseModelUser
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        public JiraIntegrationModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : base(userManager)
        {
            _signInManager = signInManager;
        }


        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Jira Web Site")]
            public string JiraWebSite { get; set; }

            [EmailAddress]
            [Display(Name = "Jira Email")]
            public string JiraEmail { get; set; }


            [Display(Name = "Jira Key")]
            public string JiraKey { get; set; }

        }


        public override async Task LoadAsync()
        {            
            Input = new InputModel
            {
                JiraWebSite = userIdentity().JiraWebSite,
                JiraEmail = userIdentity().JiraEmail,
                JiraKey = userIdentity().JiraKey
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage(".Login");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync();
                return Page();
            }

            user.JiraWebSite = Input.JiraWebSite;
            user.JiraEmail = Input.JiraEmail;
            user.JiraKey = Input.JiraKey;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your jira integration information has been updated";
            return RedirectToPage();
        }
    }
}
