using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Factories;

public static class MutationFactory
{
    public static Mutation<IHasTrait> Create<T>(this Expression<Func<T, object?>> expr)
        where T : IHasTrait => new(p => expr.Compile()((T)p));
}
