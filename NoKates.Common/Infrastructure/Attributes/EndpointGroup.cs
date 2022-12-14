using System;
using System.Collections.Generic;
using System.Linq;

namespace NoKates.Common.Infrastructure.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class EndpointGroup : Attribute
    {
        public EndpointGroup(params string[] groups)
        {
            Groups = groups.ToList();
        }

        public List<string> Groups { get; }
    }
}
