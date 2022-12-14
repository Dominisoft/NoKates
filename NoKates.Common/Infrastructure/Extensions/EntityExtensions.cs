using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using NoKates.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace NoKates.Common.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static ActionResult<bool> ToActionResult(this Entity e)
        {
            if (e?.Id > 0) return true;
            if (e != null) return false;
            return new NotFoundResult();
        }

        public static string GetTableName(this Entity entity)
        {
            var entityType = entity.GetType();
            return GetTableName(entityType);

        }

        public static string GetTableName(this Type entityType)
        {
            var attribute = (TableAttribute)entityType?.GetCustomAttribute(typeof(TableAttribute));
            var schema = attribute?.Schema?.Trim('[').Trim(']')??string.Empty;
            var name = attribute?.Name?.Trim('[').Trim(']')??string.Empty;

            if (string.IsNullOrWhiteSpace(name))
                return entityType?.Name + "s";

            if (string.IsNullOrWhiteSpace(schema))
                return $"[{name}]";

            return $"[{schema}].[{name}]";
        }
    }
}
