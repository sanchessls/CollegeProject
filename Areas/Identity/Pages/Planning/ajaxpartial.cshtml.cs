using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScrumPokerPlanning.Areas.Identity.Pages;
using ScrumPokerPlanning.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.PartialCar
{
    public class AjaxPartialModel : PageModel
    {
        private ICarService _carService;
        private ApplicationContext _appContext;
        public AjaxPartialModel(ICarService carService, ApplicationContext appContext)
        {
            _carService = carService;
            _appContext = appContext;
        }
        public List<UsersVoting> Cars { get; set; }
        public void OnGet()
        {
        }
        public PartialViewResult OnGetCarPartial(int sessionid,int featureid)
        {
            Cars = _carService.GetAll(_appContext, sessionid, featureid);
            return Partial("_CarPartial", Cars);
        }

        public PartialViewResult OnGetCarPartial2()
        {
            Cars = _carService.GetAll2();
            return Partial("_CarPartial", Cars);
        }
    }

}
