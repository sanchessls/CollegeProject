using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [Required]
        [MaxLength(15)]
        public string Identification { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public EnumFeature Status { get; set; }
        [Required]
        public int SessionId { get; set; }
        [ForeignKey("SessionId")]
        public PlanningSession PlanningSession { get; set; }

        [NotMapped]
        public bool FullVoted { get; set; }


        public virtual ICollection<FeatureUser> FeatureUser { get; set; }

        public float Average()
        {
            //If everyone voted then we can see
            if (Status != EnumFeature.Canceled)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        return FeatureUser.Where(x => x.SelectedValue > 0).Average(x => x.SelectedValue);
                    }
                }
            }
            return 0;
        }

        public string StatusColor()
        {
            switch (this.Status)
            {
                case EnumFeature.Open:
                    return "green";
                case EnumFeature.Canceled:
                    return "red";
                case EnumFeature.Closed:
                    return "brown";
                default:
                    return "yellow";
            }       
        }
    }

}
