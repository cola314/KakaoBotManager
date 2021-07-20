using KakaoManagerBeta.Models;
using KakaoManagerBeta.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KakaoManagerBeta.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            Guid = Request.Cookies["guid"];
            Console.WriteLine(Guid);
            Console.WriteLine(Globals.Session);
            if (Guid != Globals.Session)
            {
                Console.WriteLine("Redirect");
                return Redirect("/login");
            }
            else
            {
                Domains = Globals.Domains.Select(x => x?.ConvertBase64ToText()).ToList();
            }
            return Page();
        }

        public string Guid { get; private set; }

        public List<string> Domains { get; private set; }
    }
}
