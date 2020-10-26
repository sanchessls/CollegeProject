using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.APIViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6)]
        public string ConfirmPassword { get; set; }
    }
}
