using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerAPI.Models
{
    public class PlanningSessionUser
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int? PlanningSessionId { get; set; }
        [ForeignKey("PlanningSessionId")]
        public PlanningSession PlanningSession { get; set; }
    }  
}
