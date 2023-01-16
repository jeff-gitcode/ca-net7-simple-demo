using System.Linq.Expressions;

namespace Application.Interface.SPI;

public interface ISpecification<T>
{
    // Filter Conditions
    Expression<Func<T, bool>> FilterCondition { get; }

    // Order By Ascending
    Expression<Func<T, object>> OrderBy { get; }

    // Order By Descending
    Expression<Func<T, object>> OrderByDescending { get; }

    // Include collection to load related data
    List<Expression<Func<T, object>>> Includes { get; }

    // GroupBy expression
    Expression<Func<T, object>> GroupBy { get; }
}