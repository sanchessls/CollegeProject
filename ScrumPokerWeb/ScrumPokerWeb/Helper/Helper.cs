using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScrumPokerWeb.Helper
{
    public class TableOneApi
    {
        public HttpClient initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44368/");
            return client;
                 
        }
    }
}
