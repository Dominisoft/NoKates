using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NoKates.WebAdmin.Pages
{
    public class ServicesModel : PageModel
    {

        public class RequestMetricSummary
        {
            public string Name { get; set; }
            public int RequestCount { get; set; }
            public DateTime FirstRequest { get; set; }
            public DateTime LastRequest { get; set; }
            public int Index { get; set; }
            public int AverageResponseTime { get; set; }
            public int Errors { get; set; }
        }

    }
}
