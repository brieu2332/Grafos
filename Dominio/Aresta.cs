namespace ED2Grafos.Dominio;

public sealed class Aresta
{
    public string Id { get; }
    public Vertice Origem { get; }
    public Vertice Destino { get; }

    internal Aresta(string id, Vertice origem, Vertice destino)
    {
        Id = id;
        Origem = origem;
        Destino = destino;
    }

    public override string ToString()
    {
        return Id;
    }
}
