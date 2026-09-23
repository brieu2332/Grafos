using ED2Grafos.Dominio;
using ED2Grafos.Estruturas;
using ED2Grafos.Resultados;
namespace ED2Grafos.Algoritmos;

public static class BuscaEmProfundidade
{
    public static ResultadoBuscaEmProfundidade Executar(IGrafo grafo, Vertice inicio)
    {
        ValidaInicio(grafo, inicio);
        var vertices = grafo.Vertices();
        var estados = vertices.ToDictionary(vertice => vertice, _ => EstadoVisita.NaoVisitado);
        var predecessores = vertices.ToDictionary(vertice => vertice, _ => (Vertice?)null);
        var temposAbertura = new Dictionary<Vertice, int>();
        var temposFechamento = new Dictionary<Vertice, int>();
        var ordemVisita = new List<Vertice>();
        var tempo = 0;
        Visitar(inicio);

        foreach (var vertice in vertices)
        {
            if (estados[vertice] == EstadoVisita.NaoVisitado)
            {
                Visitar(vertice);
            }
        }
        return new ResultadoBuscaEmProfundidade(inicio,predecessores,temposAbertura,temposFechamento,ordemVisita);

        void Visitar(Vertice atual)
        {
            estados[atual] = EstadoVisita.EmVisita;
            temposAbertura[atual] = ++tempo;
            ordemVisita.Add(atual);

            foreach (var adjacente in grafo.Adj(atual))
            {
                if (estados[adjacente] != EstadoVisita.NaoVisitado)
                {
                    continue;
                }
                predecessores[adjacente] = atual;
                Visitar(adjacente);
            }
            estados[atual] = EstadoVisita.Finalizado;
            temposFechamento[atual] = ++tempo;
        }
    }
    private static void ValidaInicio(IGrafo grafo, Vertice inicio)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        ArgumentNullException.ThrowIfNull(inicio);
    }
}
