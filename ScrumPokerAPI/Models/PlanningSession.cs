using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerAPI.Models
{
    public class PlanningSession
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser UserCreator { get; internal set; }
    }

    public class ApplicationUser : IdentityUser
    {
    }
}
