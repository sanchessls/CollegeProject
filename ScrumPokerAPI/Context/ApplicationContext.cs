using Microsoft.EntityFrameworkCore;
using ScrumPokerAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerAPI.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { 
        
        }

        public DbSet<TableOne> TableOne { get; set; }
        public DbSet<TableTwo> TableTwo { get; set; }

    }
}
