using System.Linq.Expressions;

namespace HieLie.Domain.Core.Specifications
{
    public interface IBaseSpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>>? Includes { get; }
        Expression<Func<T, object>>? GroupBy { get; }
    }
}