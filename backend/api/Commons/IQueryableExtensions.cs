using System.Linq.Expressions;
using api.Models;
namespace api.Commons
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, bool ascending)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
                return query;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression property = parameter;

                // Support nested properties (e.g., "CategoryTransactionsDto.NamaKategoriTransact")
                foreach (var member in sortColumn.Split('.'))
                {
                    property = Expression.PropertyOrField(property, member);
                }

                var lambda = Expression.Lambda(property, parameter);

                string methodName = !ascending
                    ? "OrderByDescending"
                    : "OrderBy";

                var result = typeof(Queryable)
                    .GetMethods()
                    .Single(
                        method => method.Name == methodName
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), property.Type)
                    .Invoke(null, new object[] { query, lambda });

                return (IQueryable<T>)result!;
            }
            catch (Exception ex)
            {
                // 🧩 Optional: Log error or fallback to default ordering
                Console.WriteLine($"OrderByDynamic failed: {ex.Message}");

                // Return original query without ordering
                return query;
            }
        }
        public static IQueryable<T> WhereDynamicSearch<T>(
            this IQueryable<T> query,
            string? searchValue,
            string[] searchableColumns)
        {
            if (string.IsNullOrWhiteSpace(searchValue) || searchableColumns.Length == 0)
                return query;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? orExpression = null;

                string input = searchValue.Trim();

                foreach (var column in searchableColumns)
                {
                    Expression property = parameter;

                    // Support nested property like "UserDto.Name"
                    foreach (var member in column.Split('.'))
                    {
                        property = Expression.PropertyOrField(property, member);
                    }

                    // ===========================================================
                    // DATE HANDLING ONLY FOR COLUMN "CreatedAt" or "created_at"
                    // ===========================================================
                    if (column.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) ||
                        column.Equals("created_at", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = input.Split(',');

                        // ---------- CASE 1: ONLY ONE DATE ----------
                        if (parts.Length == 1 &&
                            DateTime.TryParse(parts[0], out var exactDate))
                        {
                            exactDate = exactDate.Date;

                            var dateProperty = Expression.Property(property, "Date");

                            var eq = Expression.Equal(
                                dateProperty,
                                Expression.Constant(exactDate)
                            );

                            orExpression = orExpression == null ? eq : Expression.OrElse(orExpression, eq);
                            continue; // skip string search
                        }

                        // ---------- CASE 2: RANGE DATE ----------
                        if (parts.Length == 2 &&
                            DateTime.TryParse(parts[0], out var startDate) &&
                            DateTime.TryParse(parts[1], out var endDate))
                        {
                            startDate = startDate.Date;
                            endDate = endDate.Date;

                            var dateProperty = Expression.Property(property, "Date");

                            var ge = Expression.GreaterThanOrEqual(dateProperty, Expression.Constant(startDate));
                            var le = Expression.LessThanOrEqual(dateProperty, Expression.Constant(endDate));

                            var between = Expression.AndAlso(ge, le);

                            orExpression = orExpression == null ? between : Expression.OrElse(orExpression, between);
                            continue;
                        }
                    }

                    // ===========================================================
                    // DEFAULT STRING SEARCH MODE
                    // ===========================================================
                    var keyword = Expression.Constant(input.ToLower());

                    Expression toStringExpr = Expression.Call(property, "ToString", Type.EmptyTypes);
                    var toLowerExpr = Expression.Call(toStringExpr, typeof(string).GetMethod("ToLower", Type.EmptyTypes)!);

                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    var containsExpr = Expression.Call(toLowerExpr, containsMethod, keyword);

                    orExpression = orExpression == null
                        ? containsExpr
                        : Expression.OrElse(orExpression, containsExpr);
                }

                if (orExpression == null)
                    return query;

                var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
                return query.Where(lambda);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DynamicSearch failed: {ex.Message}");
                return query;
            }
        }

        public static IQueryable<T> WhereDynamicColumnFilter<T>(
            this IQueryable<T> query,
            DataTableRequest request)
        {
            if (request?.Columns == null || request.Columns.Count == 0)
                return query;

            try
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var col in request.Columns)
                {
                    if (!col.Searchable || string.IsNullOrWhiteSpace(col.Search?.Value))
                        continue;

                    string searchValue = col.Search.Value.Trim();
                    Expression property = parameter;

                    // Support nested property
                    foreach (var member in col.Data.Split('.'))
                    {
                        try
                        {
                            property = Expression.PropertyOrField(property, member);
                        }
                        catch
                        {
                            property = null!;
                            break;
                        }
                    }

                    if (property == null)
                        continue;

                    // --------------------------------------------
                    // DATE FILTER (CreatedAt)
                    // --------------------------------------------
                    if (col.Data.Equals("CreatedAt", StringComparison.OrdinalIgnoreCase) ||
                        col.Data.Equals("created_at", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = searchValue.Split(',');

                        // ============================
                        // Case 1: ONLY ONE DATE
                        // ============================
                        if (parts.Length == 1 &&
                            DateTime.TryParse(parts[0], out var exactDate))
                        {
                            exactDate = exactDate.Date;

                            // x.CreatedAt.Date
                            var dateProperty = Expression.Property(property, "Date");

                            // x.CreatedAt.Date == exactDate
                            var eq = Expression.Equal(
                                dateProperty,
                                Expression.Constant(exactDate)
                            );

                            combined = combined == null
                                ? eq
                                : Expression.AndAlso(combined, eq);

                            continue; // skip default string search
                        }

                        // ============================
                        // Case 2: DATE RANGE (start,end)
                        // ============================
                        if (parts.Length == 2 &&
                            DateTime.TryParse(parts[0], out var startDate) &&
                            DateTime.TryParse(parts[1], out var endDate))
                        {
                            startDate = startDate.Date;
                            endDate = endDate.Date;

                            // x.CreatedAt.Date
                            var dateProperty = Expression.Property(property, "Date");

                            var ge = Expression.GreaterThanOrEqual(
                                dateProperty,
                                Expression.Constant(startDate)
                            );

                            var le = Expression.LessThanOrEqual(
                                dateProperty,
                                Expression.Constant(endDate)
                            );

                            var betweenExpr = Expression.AndAlso(ge, le);

                            combined = combined == null
                                ? betweenExpr
                                : Expression.AndAlso(combined, betweenExpr);

                            continue;
                        }
                    }

                    // --------------------------------------------
                    // DEFAULT STRING SEARCH MODE
                    // --------------------------------------------
                    string lowered = searchValue.ToLower();

                    Expression toStringExpr = property.Type == typeof(string)
                        ? property
                        : Expression.Call(property, "ToString", Type.EmptyTypes);

                    var toLowerExpr = Expression.Call(
                        toStringExpr,
                        typeof(string).GetMethod("ToLower", Type.EmptyTypes)!
                    );

                    var keyword = Expression.Constant(lowered);
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    var containsExpr = Expression.Call(toLowerExpr, containsMethod, keyword);

                    combined = combined == null ? containsExpr : Expression.AndAlso(combined, containsExpr);
                }

                if (combined == null)
                    return query;

                var lambda = Expression.Lambda<Func<T, bool>>(combined, parameter);
                return query.Where(lambda);
            }
            catch
            {
                return query;
            }
        }


        public static List<string> GetSearchableColumns(DataTableRequest request)
        {
            var searchableColumns = new List<string>();

            if (request.Columns != null)
            {
                foreach (var col in request.Columns)
                {
                    if (col.Searchable && !string.IsNullOrWhiteSpace(col.Data))
                    {
                        searchableColumns.Add(col.Data);
                    }
                }
            }

            return searchableColumns;
        }
    }

    
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() => x => true;
        public static Expression<Func<T, bool>> False<T>() => x => false;

        public static Expression<Func<T, bool>> Or<T>(
            this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(
                Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
    
}