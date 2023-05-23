using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoKates.Admin.Clients;

namespace NoKates.Admin.Pages.Configuration
{
    public class EditorModel : PageModel
    {
        #region Local Properties


        private string[] _blacklist = new[] { "Master", "defaults", "RoutingDefinitions" };
        public string TemplateJson { get; set; }
        public string MasterJson { get; set; }
        public string DefaultJson { get; set; }
        public string ConfigJson { get; set; }
        public string SelectedFileName { get; set; }
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
                .Where(name => !_blacklist.Contains(name))
                .Select(name => new SelectListItem(name, name))
                .ToList();

        }
        #endregion
        public void OnGet()
        {
            if (Request.Query.ContainsKey("templateName"))
                SelectedFileName = Request.Query["templateName"].ToString();
            else SelectedFileName = FileNames.First().Value;


            TemplateJson = Client.GetConfigRawByApplicationName(SelectedFileName, string.Empty);
            MasterJson = Client.GetConfigRawByApplicationName("Master", string.Empty);
            DefaultJson = Client.GetConfigRawByApplicationName("defaults", string.Empty);
            ConfigJson = Client.GetConfigWithDefaultsByApplicationName(SelectedFileName, string.Empty);
        }
    }
}
