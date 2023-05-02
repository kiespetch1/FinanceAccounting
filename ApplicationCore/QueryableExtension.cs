using System.Linq.Expressions;
using PublicApi.Exceptions;

namespace PublicApi;

public static class QueryableExtension
{
    public static TSource? SingleOrNotFound<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
    {
        if (source == null)
            throw new NotFoundException();
        if (predicate == null)
            throw new NotFoundException();
        
        var result = source.SingleOrDefault(predicate); 
        
        if (result == null)
            throw new NotFoundException();
        return result;
    }
}