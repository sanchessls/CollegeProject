﻿using Microsoft.AspNetCore.Identity;
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
        public string Description { get; set; }
        [Required]
        public EnumFeature Status { get; set; }
        [Required]
        public int SessionId { get; set; }
        [ForeignKey("SessionId")]
        public PlanningSession PlanningSession { get; set; }

        public virtual ICollection<FeatureUser> FeatureUser { get; set; }

        public float Average()
        {
            //If Feature Finished or fully voted
            if (Status == EnumFeature.Voted || Status == EnumFeature.Closed)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        return FeatureUser.Average(x => x.SelectedValue);
                    }
                }
            }

            return 0;
        }
    }

    public enum EnumFeature
    {
        Open,
        Voted,
        Closed
            
    }
}