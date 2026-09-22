using ED2Grafos.Dominio;

namespace ED2Grafos.Estruturas;

public interface IGrafo
{
    TipoGrafo Tipo { get; }
    int GetOrdem();
    int GetTamanho();
    IReadOnlyList<Vertice> Vertices();
    IReadOnlyList<Aresta> Arestas();
    Vertice GetVertice(string id);
    Vertice InsereV(string id);
    bool RemoveV(Vertice vertice);
    Aresta InsereA(string id, Vertice origem, Vertice destino);
    bool RemoveA(Aresta aresta);
    IReadOnlyList<Vertice> Adj(Vertice vertice);
    Aresta? GetA(Vertice origem, Vertice destino);
    int GrauE(Vertice vertice);
    int GrauS(Vertice vertice);
    int Grau(Vertice vertice);
    (Vertice Origem, Vertice Destino) VerticesA(Aresta aresta);
    Vertice Oposto(Vertice vertice, Aresta aresta);
    IReadOnlyList<Aresta> ArestasE(Vertice vertice);
    IReadOnlyList<Aresta> ArestasS(Vertice vertice);
    string ParaTexto();
}
