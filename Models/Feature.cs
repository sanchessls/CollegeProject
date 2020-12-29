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
        public string Link { get; set; }

        [Required]
        public int SessionId { get; set; }
        [ForeignKey("SessionId")]
        public PlanningSession PlanningSession { get; set; }

        [NotMapped]
        public int VotesTotal { get; set; }
        [NotMapped]
        public int VotesComplete { get; set; }


        public virtual ICollection<FeatureUser> FeatureUser { get; set; }
        public bool JiraCreated { get; internal set; }

        public float Average()
        {
            //If everyone voted then we can see
            if (Status != EnumFeature.Canceled)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        var query = FeatureUser.Where(x => x.Voted);
                        if (query.Any())
                        {
                            return query.Average(x => x.SelectedValue);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            return 0;
        }

        public float Lower()
        {
            if (Status != EnumFeature.Canceled)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        var query = FeatureUser.Where(x => x.Voted);
                        if (query.Any())
                        {
                            return query.Min(x => x.SelectedValue);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            return 0;
        }

        public float Higher()
        {
            if (Status != EnumFeature.Canceled)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        var query = FeatureUser.Where(x => x.Voted);
                        if (query.Any())
                        {
                            return query.Max(x => x.SelectedValue);
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
            }
            return 0;
        }

        public float Mode()
        {
            if (Status != EnumFeature.Canceled)
            {
                if (FeatureUser != null)
                {
                    if (FeatureUser.Any())
                    {
                        var query = FeatureUser.Where(x => x.Voted);
                        if (query.Any())
                        {
                            return query.Select(x => x.SelectedValue).GroupBy(i => i).OrderBy(g => g.Count()).Last().Key; 
                        }
                        else
                        {
                            return 0;
                        }
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
                    return "#4eebab45";
                case EnumFeature.Canceled:
                    return "#db3c3cba";
                case EnumFeature.Closed:
                    return "#be752aba";
                default:
                    return "#ebf64eba";
            }       
        }
    }

}
