using Microsoft.EntityFrameworkCore;
using ScrumPokerPlanning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { 
        
        }

        public DbSet<TableOne> TableOne { get; set; }
        public DbSet<TableOne> TableTwo { get; set; }

    }
}
