using System;

namespace NoKates.Common.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NoAuth : Attribute
    {
    }
}
