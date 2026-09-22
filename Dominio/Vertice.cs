namespace ED2Grafos.Dominio;

public sealed class Vertice
{
    public string Id { get; }

    public Vertice(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("O identificador do vértice é obrigatório.", nameof(id));
        }

        Id = id.Trim();
    }

    public override string ToString()
    {
        return Id;
    }
}
