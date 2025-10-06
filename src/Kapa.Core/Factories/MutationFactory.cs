using System.Linq.Expressions;
using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;

namespace Kapa.Core.Factories;

public static class MutationFactory
{
    public static Mutation<IGeneratedPrototype> Create<T>(this Expression<Func<T, bool>> expr)
        where T : IGeneratedPrototype => new(p => expr.Compile()((T)p));
}
