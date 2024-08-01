using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eclipseworks.Infrastructure.Repositories
{
    public class Helpers
    {
        public static class StrSql
        {
            private static string GetTableName<T>()
            {
                var type = typeof(T);

                var attrTableDa = type.GetCustomAttributes(true).SingleOrDefault(x => x is TableAttribute) as TableAttribute;
                if (attrTableDa is not null) return attrTableDa.Name;

                return type.Name;
            }

            private static string GetKey<T>()
            {
                var type = typeof(T);

                var property = type.GetProperties().SingleOrDefault(x => x.GetCustomAttributes(true).Any(x => x is KeyAttribute));
                if (property is not null) return property.Name;

                return "Id";
            }

            private static List<string> GetPropertiesColumn<T>()
            {
                var key = GetKey<T>();

                return typeof(T)
                    .GetProperties()
                    .OrderBy(p => p.Name.ToLower())
                    .Where(x => x.Name.ToLower() != key.ToLower() && !x.GetCustomAttributes(true).Any(z => z is NotMappedAttribute))
                    .Select(x => ((ColumnAttribute)x.GetCustomAttributes(true).FirstOrDefault(z => z is ColumnAttribute))?.Name ?? x.Name)
                    .ToList();
            }

            public static string CreateSqlSelect<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var key = GetKey<T>();

                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    select *
                    from {tableName}
                    {internalFilter}
                ";
            }

            public static string CreateSqlInsert<T>()
            {
                var tableName = GetTableName<T>();
                var propertiesEntity = GetPropertiesColumn<T>();
                var key = GetKey<T>();
                var sqlInsert = $@"
                    insert into {tableName} ({string.Join(", ", propertiesEntity)})
                    values ({string.Join(", ", propertiesEntity.Select(x => $"@{x.Replace("\"", "")}"))}) returning {key}
                ";
                return sqlInsert;
            }

            public static string CreateSqlUpdate<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var propertiesEntity = GetPropertiesColumn<T>();
                var key = GetKey<T>();
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                var sqlUpdate = $@"
                    update {tableName}
                    set {string.Join(", ", propertiesEntity.Select(x => $"{x} = @{x.Replace("\"", "")}"))}
                    {internalFilter}
                ";
                return sqlUpdate;
            }

            public static string CreateSqlDelete<T>(string? sqlFilter = null)
            {
                var tableName = GetTableName<T>();
                var key = GetKey<T>();
                var internalFilter = "";

                if (string.IsNullOrEmpty(sqlFilter)) internalFilter = $" where {key} = @{key} ";

                if (!string.IsNullOrEmpty(sqlFilter) && !sqlFilter.Contains("where", StringComparison.OrdinalIgnoreCase)) internalFilter = " where " + sqlFilter;

                return $@"
                    delete from {tableName} 
                    {internalFilter}
                ";
            }
        }
    }
}
