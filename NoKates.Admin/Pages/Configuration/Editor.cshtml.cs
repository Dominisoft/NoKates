using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoKates.Admin.Clients;

namespace NoKates.Admin.Pages.Configuration
{
    public class EditorModel : PageModel
    {
        #region Local Properties


        private string[] blacklist = new[] { "Master", "defaults", "RoutingDefinitions" };
        public string templateJson { get; set; }
        public string masterJson { get; set; }
        public string defaultJson { get; set; }
        public string configJson { get; set; }
        public string selectedFileName { get; set; }
        public List<SelectListItem> FileNames { get; set; }
        public IConfigurationClient Client;


        public EditorModel(IConfigurationClient client)
        {
            Client = client;
            LoadFileNames();

        }

        public void LoadFileNames()
        {
            FileNames = Client
                .GetFiles(string.Empty)
                .Where(name => !blacklist.Contains(name))
                .Select(name => new SelectListItem(name, name))
                .ToList();

        }
        #endregion
        public void OnGet()
        {
            if (Request.Query.ContainsKey("templateName"))
                selectedFileName = Request.Query["templateName"].ToString();
            else selectedFileName = FileNames.First().Value;


            templateJson = Client.GetConfigRawByApplicationName(selectedFileName, string.Empty);
            masterJson = Client.GetConfigRawByApplicationName("Master", string.Empty);
            defaultJson = Client.GetConfigRawByApplicationName("defaults", string.Empty);
            configJson = Client.GetConfigWithDefaultsByApplicationName(selectedFileName, string.Empty);
        }
    }
}
