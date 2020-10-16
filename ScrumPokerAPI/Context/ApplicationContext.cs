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

        //Here we set the Objects that are a table in our Database Context
        public DbSet<TableOne> TableOne { get; set; }
        public DbSet<TableTwo> TableTwo { get; set; }

    }
}
