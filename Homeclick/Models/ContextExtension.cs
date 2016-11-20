using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Homeclick.Models
{
    public static class ContextExtensions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName<T>();
        }

        public static string GetTableName<T>(this ObjectContext context) where T : class
        {
            string sql = context.CreateObjectSet<T>().ToTraceString();
            Regex regex = new Regex(@"FROM \[dbo\].\[(?<table>.*)\] AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
    public static class HomeclickExtensions
    {
        public static CollectionViewModel ToViewModel(this ProjectLayout_Collection source)
        {
            var newItem = new CollectionViewModel
            {
                id = source.Id,
                name = source.Name,
                image = source.Image,
            };

            return newItem;
        }
    }

}