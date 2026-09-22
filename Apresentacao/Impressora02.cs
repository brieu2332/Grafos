using ED2Grafos.Algoritmos;
using ED2Grafos.Dominio;
using ED2Grafos.Resultados;

namespace ED2Grafos.Apresentacao;

public static class Impressora02
{
    public static void Exibir(
        ResultadoBuscaEmLargura largura,
        ResultadoBuscaEmProfundidade profundidade)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("segunda forma");
        Console.WriteLine(new string('=', 60));
        ExibirBuscaEmLargura(largura);
        Console.WriteLine();
        ExibirBuscaEmProfundidade(profundidade);
    }

    private static void ExibirBuscaEmLargura(ResultadoBuscaEmLargura resultado)
    {
        Console.WriteLine($"Busca em Largura a partir de {resultado.Origem.Id}");
        Console.WriteLine($"Ordem de visita: {FormataOrdem(resultado.OrdemVisita)}");
        Console.WriteLine();
        Console.WriteLine("| Vértice    | Distância   | Predecessor |");
        Console.WriteLine("|------------|-------------|-------------|");

        foreach (var vertice in OrdenaVertices(resultado.Predecessores.Keys))
        {
            var distancia = resultado.Distancias[vertice]?.ToString() ?? "infinito";
            var predecessor = resultado.Predecessores[vertice]?.Id ?? "-";
            Console.WriteLine($"| {vertice.Id,-10} | {distancia,-11} | {predecessor,-11} |");
        }

        ExibirCaminhos(resultado);
    }

    private static void ExibirBuscaEmProfundidade(ResultadoBuscaEmProfundidade resultado)
    {
        Console.WriteLine($"Busca em Profundidade iniciada por {resultado.Origem.Id}");
        Console.WriteLine($"Ordem de abertura: {FormataOrdem(resultado.OrdemVisita)}");
        Console.WriteLine();
        Console.WriteLine("| Vértice    | Predecessor | Abertura | Fechamento |");
        Console.WriteLine("|------------|-------------|----------|------------|");

        foreach (var vertice in OrdenaVertices(resultado.Predecessores.Keys))
        {
            var predecessor = resultado.Predecessores[vertice]?.Id ?? "-";
            Console.WriteLine(
                $"| {vertice.Id,-10} | {predecessor,-11} | {resultado.TemposAbertura[vertice],-8} | " +
                $"{resultado.TemposFechamento[vertice],-10} |");
        }

        ExibirCaminhos(resultado);
    }

    private static void ExibirCaminhos(IResultadoBusca resultado)
    {
        Console.WriteLine();
        Console.WriteLine($"Caminhos a partir de {resultado.Origem.Id}:");

        foreach (var destino in OrdenaVertices(resultado.Predecessores.Keys))
        {
            var caminho = Caminhos.Formatar(resultado.Origem, destino, resultado.Predecessores);
            Console.WriteLine($"{resultado.Origem.Id} até {destino.Id}: {caminho}");
        }
    }

    private static IEnumerable<Vertice> OrdenaVertices(IEnumerable<Vertice> vertices)
    {
        return vertices.OrderBy(vertice => vertice.Id, StringComparer.Ordinal);
    }

    private static string FormataOrdem(IReadOnlyList<Vertice> vertices)
    {
        return vertices.Count == 0
            ? "nenhum"
            : string.Join(" -> ", vertices.Select(vertice => vertice.Id));
    }
}
