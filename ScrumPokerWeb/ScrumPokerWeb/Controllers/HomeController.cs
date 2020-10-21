using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScrumPokerWeb.Helper;
using ScrumPokerWeb.Models;

namespace ScrumPokerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        TableOneApi api = new TableOneApi();
        public async Task<IActionResult> Index()
        {
            List<TableOne> tableone = new List<TableOne>();

            HttpClient client = api.initial();

            HttpResponseMessage res = await client.GetAsync("api/TableOne");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                tableone = JsonConvert.DeserializeObject<List<TableOne>>(results);
            }
            
            return View(tableone);
        }

        public async Task<IActionResult> Details(int Id)
        {
            TableOne tableone = new TableOne();

            HttpClient client = api.initial();

            HttpResponseMessage res = await client.GetAsync("api/TableOne/" + Id);

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;
                var list = JsonConvert.DeserializeObject<List<TableOne>>(results);
                if (list.Any())
                {
                    tableone = list.First();
                }
            }

            return View(tableone);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
