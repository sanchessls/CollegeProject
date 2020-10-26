using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Context;

namespace ScrumPokerPlanning.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : BaseModelUser
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public IndexModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : base(userManager)
        {
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public override async Task LoadAsync()
        {
            var userName = await _userManager.GetUserNameAsync(userIdentity());
            var phoneNumber = await _userManager.GetPhoneNumberAsync(userIdentity());

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
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

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
