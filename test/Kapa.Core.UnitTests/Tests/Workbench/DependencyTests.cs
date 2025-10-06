using Kapa.Abstractions.Prototypes;
using Kapa.Core.Prototypes;
using Kapa.Core.Prototypes.Graphs;

namespace Kapa.Core.UnitTests.Tests.Workbench;

public class DependencyTests
{
    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303")]
    public void MyTestMethod()
    {
        // Arrange - Create nodes from relations
        var _ = new PrototypeA();
        var relations1 = new Relations1();
        var relations2 = new Relations2();
        var relations3 = new Relations3();

        var node1 = new Node(
            typeof(Relations1),
            [.. relations1.Mutations],
            [.. relations1.Requirements]
        );

        var node2 = new Node(typeof(Relations2), relations2.Mutations, relations2.Requirements);

        var node3 = new Node(typeof(Relations3), relations3.Mutations, relations3.Requirements);

        // Debug: Check what properties are being extracted
        var req3 = node3.Requirements.First();
        var reqProps = string.Join(", ", req3.ReferencedProperties);

        // Check mutation properties
        var mut1Props = string.Join(", ", ExtractPropsFromMutation(node1.Mutations.First()));
        var mut2Props = string.Join(", ", ExtractPropsFromMutation(node2.Mutations.First()));

        // Debug: check what each node provides for the requirement
        var req = node3.Requirements.First();
        var reqPropsSet = req.ReferencedProperties;
        
        var node1Provides = ExtractPropsFromMutation(node1.Mutations.First());
        var node2Provides = ExtractPropsFromMutation(node2.Mutations.First());
        
        var node1Overlap = reqPropsSet.Intersect(node1Provides).ToList();
        var node2Overlap = reqPropsSet.Intersect(node2Provides).ToList();

        // Create graph
        var graph = new Graph([node1, node2, node3]);

        // Act - Resolve routes
        var routes = graph.Resolve([node3]);

        // Better error message
        if (routes.Count == 0)
        {
            Assert.Fail(
                $"No routes found. Req props: [{reqProps}], Mut1: [{mut1Props}] (overlap: {string.Join(",", node1Overlap)}), Mut2: [{mut2Props}] (overlap: {string.Join(",", node2Overlap)})"
            );
        }

        // Assert - Should find 2 valid dependency routes (different orderings)
        if (routes.Count != 2)
        {
            var routeInfo = string.Join("; ", routes.Select((r, i) => 
                $"Route{i+1}: " + string.Join("→", r.Edges.Select(e => e.FromNode.Type.Name))
            ));
            Assert.Fail($"Expected 2 routes but got {routes.Count}. Routes: {routeInfo}");
        }

        // Check that each route contains all three nodes
        for (int i = 0; i < routes.Count; i++)
        {
            var route = routes.ElementAt(i);
            var nodesInRoute = new HashSet<Node>();

            // Collect all unique nodes from edges
            foreach (var edge in route.Edges)
            {
                nodesInRoute.Add(edge.FromNode);
                nodesInRoute.Add(edge.ToNode);
            }

            // Verify all three nodes are in each route
            if (!nodesInRoute.Contains(node1) || !nodesInRoute.Contains(node2) || !nodesInRoute.Contains(node3))
            {
                var nodeNames = string.Join(", ", nodesInRoute.Select(n => n.Type.Name));
                Assert.Fail($"Route {i+1} missing nodes. Has: {nodeNames}");
            }
        }

        // Example: Relations3 requires IsTraitTrue and TraitAsInt
        // Route 1: Relations1 → Relations2 → Relations3
        // Route 2: Relations2 → Relations1 → Relations3
    }

    private static HashSet<string> ExtractPropsFromMutation(IMutation<IPrototype> mutation)
    {
        var visitor = new PropertyVisitor();
        visitor.Visit(mutation.MutationExpression);
        return visitor.Properties;
    }

    private sealed class PropertyVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        public HashSet<string> Properties { get; } = [];

        protected override System.Linq.Expressions.Expression VisitMember(
            System.Linq.Expressions.MemberExpression node
        )
        {
            if (
                node.Member.DeclaringType != null
                && node.Expression?.NodeType == System.Linq.Expressions.ExpressionType.Parameter
            )
            {
                Properties.Add(node.Member.Name);
            }
            return base.VisitMember(node);
        }
    }
}

internal sealed class PrototypeA : IPrototype
{
    public string Name => nameof(PrototypeA);

    public string Description => "Description of nameof(PrototypeA)";

    public IReadOnlyCollection<ITrait> Traits =>
        [
            new Trait(nameof(IsTraitTrue), $"description {nameof(IsTraitTrue)}", []),
            new Trait(nameof(TraitAsString), $"description {nameof(TraitAsString)}", []),
            new Trait(nameof(TraitAsInt), $"description {nameof(TraitAsInt)}", []),
            new Trait(nameof(TraitAsDouble), $"description {nameof(TraitAsDouble)}", []),
            new Trait(nameof(TraitAsLong), $"description {nameof(TraitAsLong)}", []),
        ];

    public bool IsTraitTrue { get; set; }
    public string? TraitAsString { get; set; }

    public int TraitAsInt { get; set; }
    public double TraitAsDouble { get; set; }
    public long TraitAsLong { get; set; }
}

internal sealed class Relations1 : IPrototypeRelations<IPrototype>
{
    public ICollection<IMutation<IPrototype>> Mutations =>
        [new Mutation<IPrototype>(p => ((PrototypeA)p).IsTraitTrue == true)];

    public ICollection<IRequirement<IPrototype>> Requirements => [];
}

internal sealed class Relations2 : IPrototypeRelations<IPrototype>
{
    public ICollection<IMutation<IPrototype>> Mutations =>
        [new Mutation<IPrototype>(p => ((PrototypeA)p).TraitAsInt > 2)];

    public ICollection<IRequirement<IPrototype>> Requirements => [];
}

internal sealed class Relations3 : IPrototypeRelations<IPrototype>
{
    public ICollection<IMutation<IPrototype>> Mutations => [];

    public ICollection<IRequirement<IPrototype>> Requirements =>
        [
            new Requirement<IPrototype>(p =>
                ((PrototypeA)p).IsTraitTrue == true && ((PrototypeA)p).TraitAsInt > 2
            ),
        ];
}
