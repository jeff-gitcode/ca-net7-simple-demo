
using System.Linq.Expressions;

using Application.SPI;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class Specification<T> : ISpecification<T>
{
    private readonly List<Expression<Func<T, object>>> _includeCollection = new List<Expression<Func<T, object>>>();

    public Specification() { }

    public Specification(Expression<Func<T, bool>> filterCondition)
    {
        this.FilterCondition = filterCondition;
    }

    public Expression<Func<T, bool>> FilterCondition { get; private set; }
    public Expression<Func<T, object>> OrderBy { get; private set; }
    public Expression<Func<T, object>> OrderByDescending { get; private set; }
    public List<Expression<Func<T, object>>> Includes
    {
        get
        {
            return _includeCollection;
        }
    }

    public Expression<Func<T, object>> GroupBy { get; private set; }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
    {
        OrderByDescending = orderByDescendingExpression;
    }

    protected void SetFilterCondition(Expression<Func<T, bool>> filterExpression)
    {
        FilterCondition = filterExpression;
    }

    protected void ApplyGroupBy(Expression<Func<T, object>> groupByExpression)
    {
        GroupBy = groupByExpression;
    }
}

public class SpecificationEvaluator<TEntity> where TEntity : class
{
    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> query, ISpecification<TEntity> specifications)
    {
        // Do not apply anything if specifications is null
        if (specifications == null)
        {
            return query;
        }

        // Modify the IQueryable
        // Apply filter conditions
        if (specifications.FilterCondition != null)
        {
            query = query.Where(specifications.FilterCondition);
        }

        // Includes
        query = specifications.Includes
                    .Aggregate(query, (current, include) => current.Include(include));

        // Apply ordering
        if (specifications.OrderBy != null)
        {
            query = query.OrderBy(specifications.OrderBy);
        }
        else if (specifications.OrderByDescending != null)
        {
            query = query.OrderByDescending(specifications.OrderByDescending);
        }

        // Apply GroupBy
        if (specifications.GroupBy != null)
        {
            query = query.GroupBy(specifications.GroupBy).SelectMany(x => x);
        }

        return query;
    }
}