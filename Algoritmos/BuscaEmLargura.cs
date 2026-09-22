using ED2Grafos.Dominio;
using ED2Grafos.Estruturas;
using ED2Grafos.Resultados;

namespace ED2Grafos.Algoritmos;

public static class BuscaEmLargura
{
    public static ResultadoBuscaEmLargura Executar(IGrafo grafo, Vertice inicio)
    {
        ValidaInicio(grafo, inicio);

        var vertices = grafo.Vertices();
        var predecessores = vertices.ToDictionary(vertice => vertice, _ => (Vertice?)null);
        var distancias = vertices.ToDictionary(vertice => vertice, _ => (int?)null);
        var ordemVisita = new List<Vertice>();
        var fila = new Queue<Vertice>();

        distancias[inicio] = 0;
        fila.Enqueue(inicio);

        while (fila.Count > 0)
        {
            var atual = fila.Dequeue();
            ordemVisita.Add(atual);

            foreach (var adjacente in grafo.Adj(atual))
            {
                if (distancias[adjacente].HasValue)
                {
                    continue;
                }

                var distanciaAtual = distancias[atual]
                    ?? throw new InvalidOperationException("O vértice atual não possui distância.");

                distancias[adjacente] = distanciaAtual + 1;
                predecessores[adjacente] = atual;
                fila.Enqueue(adjacente);
            }
        }

        return new ResultadoBuscaEmLargura(inicio, predecessores, distancias, ordemVisita);
    }

    private static void ValidaInicio(IGrafo grafo, Vertice inicio)
    {
        ArgumentNullException.ThrowIfNull(grafo);
        ArgumentNullException.ThrowIfNull(inicio);

        if (!ReferenceEquals(grafo.GetVertice(inicio.Id), inicio))
        {
            throw new InvalidOperationException("O vértice inicial não pertence ao grafo.");
        }
    }
}
