using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NoKates.WebConsole.Pages.Shared;

namespace NoKates.WebConsole.Pages
{
    public class IndexModel : AuthenticatedPage
    {
        private readonly ILogger<IndexModel> _logger;
        public string dataUrl { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            dataUrl = "1234567";
        }


        public override void LoadData(string token)
        {
            
        }
    }
}
