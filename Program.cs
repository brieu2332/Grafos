using System.Text;
using ED2Grafos.Algoritmos;
using ED2Grafos.Apresentacao;
using ED2Grafos.Dominio;
using ED2Grafos.Estruturas;

namespace ED2Grafos;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var grafo = CriarGrafo();
        var inicio = grafo.GetVertice("A");

        Impressora01.Exibir(grafo);

        var resultadoLargura = BuscaEmLargura.Executar(grafo, inicio);
        var resultadoProfundidade = BuscaEmProfundidade.Executar(grafo, inicio);

        Impressora02.Exibir(resultadoLargura, resultadoProfundidade);
    }

    private static Grafo CriarGrafo()
    {
        var grafo = new Grafo(TipoGrafo.NaoDirecionado);

        var a = grafo.InsereV("A");
        var b = grafo.InsereV("B");
        var c = grafo.InsereV("C");
        var d = grafo.InsereV("D");
        var e = grafo.InsereV("E");
        grafo.InsereV("F");

        grafo.InsereA("e1", a, b);
        grafo.InsereA("e2", a, c);
        grafo.InsereA("e3", b, d);
        grafo.InsereA("e4", c, e);
        grafo.InsereA("e5", d, e);

        return grafo;
    }
}
