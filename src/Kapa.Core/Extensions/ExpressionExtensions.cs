using System.Linq.Expressions;
using Kapa.Abstractions.Actors;
using Kapa.Core.Actors;
using Kapa.Core.Factories;

namespace Kapa.Core.Extensions;

public static class ExpressionExtensions
{
    public static IRequirement<IGeneratedActor> ToRequirement<TGeneratedActor>(
        this Expression<Func<TGeneratedActor, bool>> expression
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(expression);

        return RequirementFactory.Create(expression);
    }

    public static Mutation<IGeneratedActor> ToMutation<TGeneratedActor>(
        this Expression<Func<TGeneratedActor, bool>> expression
    )
        where TGeneratedActor : IGeneratedActor
    {
        ArgumentNullException.ThrowIfNull(expression);

        return MutationFactory.Create(expression);
    }
}
