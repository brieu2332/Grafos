using ED2Grafos.Dominio;
namespace ED2Grafos.Algoritmos;
public static class Caminhos
{
    public static IReadOnlyList<Vertice> Obter(
        Vertice origem,
        Vertice destino,
        IReadOnlyDictionary<Vertice, Vertice?> predecessores)
    {
        ArgumentNullException.ThrowIfNull(origem);
        ArgumentNullException.ThrowIfNull(destino);
        ArgumentNullException.ThrowIfNull(predecessores);
        var caminhoInvertido = new List<Vertice>();
        var visitados = new HashSet<Vertice>();
        Vertice? atual = destino;

        while (atual is not null)
        {
            caminhoInvertido.Add(atual);

            if (ReferenceEquals(atual, origem))
            {
                caminhoInvertido.Reverse();
                return caminhoInvertido;
            }
            atual = predecessores[atual];
        }
        return Array.Empty<Vertice>();
    }

    public static string Formatar(
        Vertice origem,
        Vertice destino,
        IReadOnlyDictionary<Vertice, Vertice?> predecessores)
    {
        var caminho = Obter(origem, destino, predecessores);

        return caminho.Count == 0
            ? "não existe caminho"
            : string.Join(" -> ", caminho.Select(vertice => vertice.Id));
    }
}
