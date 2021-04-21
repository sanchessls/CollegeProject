using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ScrumPokerPlanning.Models;
using ScrumPokerPlanning.Services;

namespace ScrumPokerPlanning.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IReportingService _ReportingService;
        public HomeController(IReportingService reportingService)
        {
            _ReportingService = reportingService;
        }

      
        public IActionResult Index()
        {
            return LocalRedirect(Url.Content("~/Identity/Planning/Create"));
        }

        public async Task<IActionResult> PrintPDF(int sessionId,string fileName)
        {
            return _ReportingService.CreatePDF();
        }

    }
}
