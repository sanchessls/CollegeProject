using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Models
{
    public class TableTwo
    {
        public int Id { get; set; }
        [Required]
        public string HowTo2 { get; set; }
        [Required]
        public string Line2 { get; set; }
        [Required]
        public string Plataform2 { get; set; }
    }
}
