using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScrumPokerAPI.Models;
using ToDelete.Data;

namespace ToDelete.Areas.Identity.Pages.CRUD
{
    public class EditModel : PageModel
    {
        private readonly ToDelete.Data.ApplicationDbContext _context;

        public EditModel(ToDelete.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TableOne TableOne { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TableOne = await _context.TableOne
                .Include(t => t.Usuario).FirstOrDefaultAsync(m => m.Id == id);

            if (TableOne == null)
            {
                return NotFound();
            }
           ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TableOne).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableOneExists(TableOne.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TableOneExists(int id)
        {
            return _context.TableOne.Any(e => e.Id == id);
        }
    }
}
