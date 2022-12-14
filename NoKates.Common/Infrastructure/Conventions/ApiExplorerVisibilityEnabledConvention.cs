using NoKates.Common.Infrastructure.Controllers;
using NoKates.Common.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace NoKates.Common.Infrastructure.Conventions
{
    public class ApiExplorerVisibilityEnabledConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            StatusController.EndpointGroups = application
                            .GetActions()
                           // .FilterOutWebCommon()
                            .GetEndpointGroups();  
                   
        }
    }
}
