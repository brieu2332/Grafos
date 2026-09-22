using ED2Grafos.Dominio;

namespace ED2Grafos.Resultados;

public interface IResultadoBusca
{
    Vertice Origem { get; }
    IReadOnlyDictionary<Vertice, Vertice?> Predecessores { get; }
    IReadOnlyList<Vertice> OrdemVisita { get; }
}
