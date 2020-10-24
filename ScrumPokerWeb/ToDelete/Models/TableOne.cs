using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerAPI.Models
{
    public class TableOne
    {
        public int Id { get; set; }
        [Required]
        public string HowTo { get; set; }
        [Required]
        public string Line { get; set; }
        [Required]
        public string Plataform { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Usuario { get; internal set; }
    }

    public class ApplicationUser : IdentityUser
    {
    }
}
