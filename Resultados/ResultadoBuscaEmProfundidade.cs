using System.Collections.ObjectModel;
using ED2Grafos.Dominio;

namespace ED2Grafos.Resultados;

public sealed class ResultadoBuscaEmProfundidade : IResultadoBusca
{
    public Vertice Origem { get; }
    public IReadOnlyDictionary<Vertice, Vertice?> Predecessores { get; }
    public IReadOnlyDictionary<Vertice, int> TemposAbertura { get; }
    public IReadOnlyDictionary<Vertice, int> TemposFechamento { get; }
    public IReadOnlyList<Vertice> OrdemVisita { get; }

    internal ResultadoBuscaEmProfundidade(
        Vertice origem,
        Dictionary<Vertice, Vertice?> predecessores,
        Dictionary<Vertice, int> temposAbertura,
        Dictionary<Vertice, int> temposFechamento,
        List<Vertice> ordemVisita)
    {
        Origem = origem;
        Predecessores = new ReadOnlyDictionary<Vertice, Vertice?>(predecessores);
        TemposAbertura = new ReadOnlyDictionary<Vertice, int>(temposAbertura);
        TemposFechamento = new ReadOnlyDictionary<Vertice, int>(temposFechamento);
        OrdemVisita = ordemVisita.AsReadOnly();
    }
}
