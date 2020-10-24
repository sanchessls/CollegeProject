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
    public class IndexModel : PageModel
    {
        private readonly ToDelete.Data.ApplicationDbContext _context;

        public IndexModel(ToDelete.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<TableOne> TableOne { get;set; }

        public async Task OnGetAsync()
        {
            TableOne = await _context.TableOne
                .Include(t => t.Usuario).ToListAsync();
        }
    }
}
