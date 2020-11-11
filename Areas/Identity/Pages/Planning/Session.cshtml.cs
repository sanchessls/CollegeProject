using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.HashFunction.xxHash;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ScrumPokerPlanning.Context;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Repositories.Implementation;
using ScrumPokerPlanning.Repositories.Interface;

namespace ScrumPokerPlanning.Areas.Identity.Pages
{
    public partial class Session : BaseModelDatabaseUser
    {        
        public Session(ApplicationContext context, UserManager<IdentityUser> userManager) : base(context, userManager)
        {
        }

        [BindProperty]
        [Display(Name = "Feature description(*)")]
        public string FeatureDescription { get; set; }

        [BindProperty]
        [Display(Name = "Feature Identification(*)")]
        [MaxLength(15)]
        public string FeatureIdentification { get; set; }

        [BindProperty]
        public int PlanningSessionId { get; set; }
        public string DescriptionSession { get; set; }
        [BindProperty]
        public bool UserCreator { get; set; }
        [BindProperty]
        public string SessionCode { get; set; }
        //public List<Models.Feature> FeaturesList { get; set; }


        public async override Task<RedirectToPageResult> Validator()
        {
            string aSessionCode = Request.Query["code"];

            int idSession = 0;

            if (aSessionCode == null)
            {
                ModelState.AddModelError(string.Empty, "Null Session!");
                return RedirectToPage(".Join");
            }


            var query = _appContext.PlanningSession.Where(x => x.SessionCode == aSessionCode);

            if (query.Any())
            {
                idSession = query.FirstOrDefault().Id;
            }

            if (idSession <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session!");
                return RedirectToPage(".Join");
            }

            SessionCode = aSessionCode;

            return null;
        }

        public override Task LoadAsync()
        {
            string SessionCode = Request.Query["code"];

            int idSession = 0;

            var query = _appContext.PlanningSession.Where(x => x.SessionCode == SessionCode);
          
            if (query.Any())
            {
                idSession = query.FirstOrDefault().Id;
            }

            if (idSession <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid Session!");
            }

            PlanningSession SessionObject = _appContext.PlanningSession.Where(x => x.Id == idSession).Include(x => x.PlanningSessionUser).FirstOrDefault();
            PlanningSessionId = SessionObject.Id;
            DescriptionSession = SessionObject.Description;
            
            //FeaturesList = 

            //If the Creator is the one Logged
            //We will offer the option of create features
            UserCreator = SessionObject.PlanningSessionUser.Where(x => x.UserId == userIdentity().Id).FirstOrDefault().UserIsCreator;

            return base.LoadAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Model!");
                return Page();
            }


            if ((FeatureIdentification == null) || (FeatureIdentification.Trim() == ""))
            {
                ModelState.AddModelError("", "Invalid Identification!");
                return Page();
            }



            if ((FeatureDescription == null) || (FeatureDescription.Trim() == ""))
            {
                ModelState.AddModelError("", "Invalid Description!");
                return Page();
            }
            Models.Feature feature = new Models.Feature
            {
                SessionId = PlanningSessionId,
                Status = EnumFeature.Open,
                CreationDate = DateTime.Now,
                Description = FeatureDescription,
                Identification = FeatureIdentification
            };

            _appContext.Feature.Add(feature);
            await _appContext.SaveChangesAsync();
            

            Models.FeatureUser featureUser = new Models.FeatureUser
            {
                FeatureId = feature.Id,
                UserId = userIdentity().Id,                
            };

            _appContext.FeatureUser.Add(featureUser);
            await _appContext.SaveChangesAsync();

            return RedirectToPage("./session", new { code = SessionCode.ToUpper() });
        }

    }
}
