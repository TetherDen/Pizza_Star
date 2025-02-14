using System.Linq.Expressions;

namespace Pizza_Star.Models.Pages
{

    public class PagedList<T> : List<T>
    {
        public PagedList(IQueryable<T> query, QueryOptions? options = null)
        {
            CurrentPage = options.CurrentPage;
            PageSize = options.PageSize;
            Options = options;


            if (options != null)
            {
                if (!string.IsNullOrEmpty(options.OrderPropertyName))
                {
                    query = Order(query, options.OrderPropertyName, options.DescendingOrder);
                }
                if (!string.IsNullOrEmpty(options.SearchPropertyName) && !string.IsNullOrEmpty(options.SearchTerm))
                {
                    query = Search(query, options.SearchPropertyName, options.SearchTerm);
                }
            }


            int queryCount = query.Count();


            TotalPages = queryCount / PageSize;
            if (queryCount % PageSize > 0)
            {
                TotalPages += 1;
            }


            AddRange(query.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
        }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public QueryOptions Options { get; set; }


        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;


        //private static IQueryable<T> Search(IQueryable<T> query, string propertyName, string searchTerm)
        //{
        //    var parameter = Expression.Parameter(typeof(T), "x");
        //    var source = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
        //    var body = Expression.Call(source, "Contains", Type.EmptyTypes, Expression.Constant(searchTerm, typeof(string)));
        //    var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
        //    return query.Where(lambda);
        //}

        private static IQueryable<T> Search(IQueryable<T> query, string propertyName, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return query;

            // Удаляем пробелы в начале и конце поисковой строки
            searchTerm = searchTerm.Trim();

            if (string.IsNullOrEmpty(searchTerm))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var source = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);

            // Проверяем, является ли свойство Enum
            if (source.Type.IsEnum)
            {
                // Получаем все возможные значения enum
                var enumValues = Enum.GetValues(source.Type);
                var enumNames = Enum.GetNames(source.Type);

                // Ищем подходящие значения enum
                List<Expression> matchingValues = new List<Expression>();

                for (int i = 0; i < enumNames.Length; i++)
                {
                    if (enumNames[i].Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    {
                        // Создаем выражение для сравнения с этим значением enum
                        var enumValue = Expression.Constant(enumValues.GetValue(i));
                        var equality = Expression.Equal(source, enumValue);
                        matchingValues.Add(equality);
                    }
                }

                // Если найдены подходящие значения, объединяем их с OR
                if (matchingValues.Count > 0)
                {
                    Expression orExpression = matchingValues[0];
                    for (int i = 1; i < matchingValues.Count; i++)
                    {
                        orExpression = Expression.Or(orExpression, matchingValues[i]);
                    }

                    var lambda = Expression.Lambda<Func<T, bool>>(orExpression, parameter);
                    return query.Where(lambda);
                }

                // Если подходящих значений нет, возвращаем пустой результат
                return query.Where(_ => false);
            }
            else
            {
                // Стандартная обработка для строковых полей
                var body = Expression.Call(source, "Contains", Type.EmptyTypes, Expression.Constant(searchTerm, typeof(string)));
                var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);
                return query.Where(lambda);
            }
        }


        private static IQueryable<T> Order(IQueryable<T> query, string propertyName, bool desc)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var source = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var lambda = Expression.Lambda(typeof(Func<,>).MakeGenericType(typeof(T), source.Type), source, parameter);
            return typeof(Queryable).GetMethods().Single(e => e.Name == (desc ? "OrderByDescending" : "OrderBy") &&
            e.IsGenericMethodDefinition &&
            e.GetGenericArguments().Length == 2 &&
            e.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), source.Type)
            .Invoke(null, new object[] { query, lambda }) as IQueryable<T>;
        }
    }




}
