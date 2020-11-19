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
            [Phone]
            [Display(Name = "Jira Web Site - COLOCAR EXEMPLO")]
            public string JiraWebSite { get; set; }
        }

        public override async Task LoadAsync()
        {
            var jiraWebSite = userIdentity().JiraWebSite;

            Input = new InputModel
            {
                JiraWebSite = jiraWebSite
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


            await _userManager.UpdateAsync(user);


            //if (Input.JiraWebSite != jiraWebSite)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.JiraWebSite);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
