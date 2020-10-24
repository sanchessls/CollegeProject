using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScrumPokerAPI.Models;
using ToDelete.Data;

namespace ToDelete.Areas.Identity.Pages.CRUD
{
    public class CreateModel : PageModel
    {
        private readonly ToDelete.Data.ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ToDelete.Data.ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            //ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public TableOne TableOne { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            TableOne.UserId = _userManager.GetUserId(User);
            _context.TableOne.Add(TableOne);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
