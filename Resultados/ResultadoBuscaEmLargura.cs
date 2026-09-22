using System.Collections.ObjectModel;
using ED2Grafos.Dominio;

namespace ED2Grafos.Resultados;

public sealed class ResultadoBuscaEmLargura : IResultadoBusca
{
    public Vertice Origem { get; }
    public IReadOnlyDictionary<Vertice, Vertice?> Predecessores { get; }
    public IReadOnlyDictionary<Vertice, int?> Distancias { get; }
    public IReadOnlyList<Vertice> OrdemVisita { get; }

    internal ResultadoBuscaEmLargura(
        Vertice origem,
        Dictionary<Vertice, Vertice?> predecessores,
        Dictionary<Vertice, int?> distancias,
        List<Vertice> ordemVisita)
    {
        Origem = origem;
        Predecessores = new ReadOnlyDictionary<Vertice, Vertice?>(predecessores);
        Distancias = new ReadOnlyDictionary<Vertice, int?>(distancias);
        OrdemVisita = ordemVisita.AsReadOnly();
    }
}
