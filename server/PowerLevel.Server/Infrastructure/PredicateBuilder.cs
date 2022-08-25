namespace PowerLevel.Server.Infrastructure;

using System;
using System.Linq.Expressions;

public static class PredicateBuilder
{
    public static Expression<Func<T, bool>> True<T>()
    {
        return x => true;
    }

    public static Expression<Func<T, bool>> False<T>()
    {
        return x => false;
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> x,
        Expression<Func<T, bool>> y)
    {
        return Expression.Lambda<Func<T, bool>>
            (Expression.OrElse(x.Body, Expression.Invoke(y, x.Parameters)), x.Parameters);
    }

    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> x,
        Expression<Func<T, bool>> y)
    {
        return Expression.Lambda<Func<T, bool>>
            (Expression.AndAlso(x.Body, Expression.Invoke(y, x.Parameters)), x.Parameters);
    }
}
