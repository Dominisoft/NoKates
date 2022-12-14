using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoKates.Common.Infrastructure.Attributes
{

    [AttributeUsage(AttributeTargets.Property)]
    public class JsonColumnAttribute : ColumnAttribute
    {
        public JsonColumnAttribute(string name):base(name)
        {
        }

    }
}
