using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScrumPokerAPI.Models;
using ToDelete.Data;

namespace ToDelete.Areas.Identity.Pages.CRUD
{
    public class DetailsModel : PageModel
    {
        private readonly ToDelete.Data.ApplicationDbContext _context;

        public DetailsModel(ToDelete.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
