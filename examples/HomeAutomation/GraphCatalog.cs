using Kapa.Abstractions.Graphs;
using Kapa.Core.Graphs;

namespace HomeAutomation;

public static class GraphCatalog
{
    public static IGraph Full() => new Graph([.. NodeCatalog.GetNodes()]);
}
