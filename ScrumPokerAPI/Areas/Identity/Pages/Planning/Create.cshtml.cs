using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerAPI.Context;
using ScrumPokerAPI.Models;

namespace ScrumPokerAPI.Areas.Identity.Pages
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;       

        public CreateModel(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            Userid = _userManager.GetUserId(User);
            return Page();
        }

        [BindProperty]
        public PlanningSession PlanningSession { get; set; }
        public string Userid { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {           

            if (!ModelState.IsValid)
            {
                return Page();
            }

            PlanningSession.CreationDate = DateTime.Now;
            PlanningSession.Status = 1;
            _context.PlanningSession.Add(PlanningSession);
            await _context.SaveChangesAsync();

            return RedirectToPage("./PlanningSession");
        }
    }
}
