using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace NoKates.WebConsole.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string dataUrl { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            dataUrl = "1234567";
        }

        public void OnGet()
        {

        }
    }
}
