using ED2Grafos.Dominio;
using ED2Grafos.Estruturas;

namespace ED2Grafos.Apresentacao;

public static class Impressora01
{
    public static void Exibir(IGrafo grafo)
    {
        Console.WriteLine("primeira forma");
        Console.WriteLine(new string('=', 60));
        Console.WriteLine(grafo.ParaTexto());
        Console.WriteLine();
        Console.WriteLine("Consultas por vértice:");

        foreach (var vertice in grafo.Vertices())
        {
            ExibirVertice(grafo, vertice);
        }

        var primeiraAresta = grafo.Arestas().FirstOrDefault();

        if (primeiraAresta is not null)
        {
            var (origem, destino) = grafo.VerticesA(primeiraAresta);
            var arestaEncontrada = grafo.GetA(origem, destino);

            Console.WriteLine();
            Console.WriteLine("Consultas da primeira aresta:");
            Console.WriteLine($"getA({origem.Id}, {destino.Id}): {arestaEncontrada?.Id ?? "null"}");
            Console.WriteLine($"verticesA({primeiraAresta.Id}): ({origem.Id}, {destino.Id})");
            Console.WriteLine($"oposto({origem.Id}, {primeiraAresta.Id}): {grafo.Oposto(origem, primeiraAresta).Id}");
        }
    }

    private static void ExibirVertice(IGrafo grafo, Vertice vertice)
    {
        var adjacentes = grafo.Adj(vertice);
        var textoAdjacentes = adjacentes.Count == 0
            ? "nenhum"
            : string.Join(", ", adjacentes.Select(adjacente => adjacente.Id));

        if (grafo.Tipo == TipoGrafo.Direcionado)
        {
            var entradas = FormataArestas(grafo.ArestasE(vertice));
            var saidas = FormataArestas(grafo.ArestasS(vertice));
            Console.WriteLine(
                $"{vertice.Id}: adjacentes = [{textoAdjacentes}], grauE = {grafo.GrauE(vertice)}, " +
                $"grauS = {grafo.GrauS(vertice)}, entradas = [{entradas}], saídas = [{saidas}]");
            return;
        }

        var incidentes = FormataArestas(grafo.ArestasS(vertice));
        Console.WriteLine(
            $"{vertice.Id}: adjacentes = [{textoAdjacentes}], grau = {grafo.Grau(vertice)}, " +
            $"arestas incidentes = [{incidentes}]");
    }

    private static string FormataArestas(IReadOnlyList<Aresta> arestas)
    {
        return arestas.Count == 0
            ? "nenhuma"
            : string.Join(", ", arestas.Select(aresta => aresta.Id));
    }
}
