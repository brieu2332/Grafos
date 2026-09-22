using System.Text;
using ED2Grafos.Dominio;

namespace ED2Grafos.Estruturas;

public sealed class Grafo : IGrafo
{
    private readonly Dictionary<string, Vertice> _vertices = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Aresta> _arestas = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Dictionary<string, Aresta>> _saidas = new(StringComparer.Ordinal);
    private readonly Dictionary<string, Dictionary<string, Aresta>> _entradas = new(StringComparer.Ordinal);

    public TipoGrafo Tipo { get; }

    public Grafo(TipoGrafo tipo)
    {
        Tipo = tipo;
    }

    public int GetOrdem()
    {
        return _vertices.Count;
    }

    public int GetTamanho()
    {
        return _arestas.Count;
    }

    public IReadOnlyList<Vertice> Vertices()
    {
        return _vertices.Values.OrderBy(vertice => vertice.Id, StringComparer.Ordinal).ToArray();
    }

    public IReadOnlyList<Aresta> Arestas()
    {
        return _arestas.Values.OrderBy(aresta => aresta.Id, StringComparer.Ordinal).ToArray();
    }

    public Vertice GetVertice(string id)
    {
        var idNormalizado = NormalizaId(id, "vértice");

        if (!_vertices.TryGetValue(idNormalizado, out var vertice))
        {
            throw new KeyNotFoundException($"O vértice '{idNormalizado}' não existe no grafo.");
        }

        return vertice;
    }

    public Vertice InsereV(string id)
    {
        var vertice = new Vertice(id);

        if (!_vertices.TryAdd(vertice.Id, vertice))
        {
            throw new InvalidOperationException($"O vértice '{vertice.Id}' já existe.");
        }

        _saidas[vertice.Id] = new Dictionary<string, Aresta>(StringComparer.Ordinal);
        _entradas[vertice.Id] = new Dictionary<string, Aresta>(StringComparer.Ordinal);

        return vertice;
    }

    public bool RemoveV(Vertice vertice)
    {
        ArgumentNullException.ThrowIfNull(vertice);

        if (!_vertices.TryGetValue(vertice.Id, out var verticeArmazenado) ||
            !ReferenceEquals(verticeArmazenado, vertice))
        {
            return false;
        }

        var arestasIncidentes = new HashSet<Aresta>(_saidas[vertice.Id].Values);
        arestasIncidentes.UnionWith(_entradas[vertice.Id].Values);

        foreach (var aresta in arestasIncidentes)
        {
            RemoveA(aresta);
        }

        _saidas.Remove(vertice.Id);
        _entradas.Remove(vertice.Id);
        _vertices.Remove(vertice.Id);

        return true;
    }

    public Aresta InsereA(string id, Vertice origem, Vertice destino)
    {
        var idNormalizado = NormalizaId(id, "aresta");
        ValidaVertice(origem);
        ValidaVertice(destino);

        if (_arestas.ContainsKey(idNormalizado))
        {
            throw new InvalidOperationException($"A aresta '{idNormalizado}' já existe.");
        }

        if (ReferenceEquals(origem, destino))
        {
            throw new InvalidOperationException("Um grafo simples não permite laços.");
        }

        if (_saidas[origem.Id].ContainsKey(destino.Id))
        {
            throw new InvalidOperationException($"Já existe uma aresta de '{origem.Id}' para '{destino.Id}'.");
        }

        var aresta = new Aresta(idNormalizado, origem, destino);
        _arestas.Add(aresta.Id, aresta);
        _saidas[origem.Id].Add(destino.Id, aresta);
        _entradas[destino.Id].Add(origem.Id, aresta);

        if (Tipo == TipoGrafo.NaoDirecionado)
        {
            _saidas[destino.Id].Add(origem.Id, aresta);
            _entradas[origem.Id].Add(destino.Id, aresta);
        }

        return aresta;
    }

    public bool RemoveA(Aresta aresta)
    {
        ArgumentNullException.ThrowIfNull(aresta);

        if (!_arestas.TryGetValue(aresta.Id, out var arestaArmazenada) ||
            !ReferenceEquals(arestaArmazenada, aresta))
        {
            return false;
        }

        _saidas[aresta.Origem.Id].Remove(aresta.Destino.Id);
        _entradas[aresta.Destino.Id].Remove(aresta.Origem.Id);

        if (Tipo == TipoGrafo.NaoDirecionado)
        {
            _saidas[aresta.Destino.Id].Remove(aresta.Origem.Id);
            _entradas[aresta.Origem.Id].Remove(aresta.Destino.Id);
        }

        _arestas.Remove(aresta.Id);
        return true;
    }

    public IReadOnlyList<Vertice> Adj(Vertice vertice)
    {
        ValidaVertice(vertice);

        return _saidas[vertice.Id]
            .Keys
            .Select(id => _vertices[id])
            .OrderBy(adjacente => adjacente.Id, StringComparer.Ordinal)
            .ToArray();
    }

    public Aresta? GetA(Vertice origem, Vertice destino)
    {
        ValidaVertice(origem);
        ValidaVertice(destino);

        return _saidas[origem.Id].GetValueOrDefault(destino.Id);
    }

    public int GrauE(Vertice vertice)
    {
        ValidaVertice(vertice);
        return _entradas[vertice.Id].Count;
    }

    public int GrauS(Vertice vertice)
    {
        ValidaVertice(vertice);
        return _saidas[vertice.Id].Count;
    }

    public int Grau(Vertice vertice)
    {
        ValidaVertice(vertice);

        if (Tipo == TipoGrafo.Direcionado)
        {
            throw new InvalidOperationException("Use GrauE e GrauS em grafos direcionados.");
        }

        return _saidas[vertice.Id].Count;
    }

    public (Vertice Origem, Vertice Destino) VerticesA(Aresta aresta)
    {
        ValidaAresta(aresta);
        return (aresta.Origem, aresta.Destino);
    }

    public Vertice Oposto(Vertice vertice, Aresta aresta)
    {
        ValidaVertice(vertice);
        ValidaAresta(aresta);

        if (ReferenceEquals(vertice, aresta.Origem))
        {
            return aresta.Destino;
        }

        if (ReferenceEquals(vertice, aresta.Destino))
        {
            return aresta.Origem;
        }

        throw new InvalidOperationException($"O vértice '{vertice.Id}' não é incidente à aresta '{aresta.Id}'.");
    }

    public IReadOnlyList<Aresta> ArestasE(Vertice vertice)
    {
        ValidaVertice(vertice);

        return _entradas[vertice.Id]
            .Values
            .OrderBy(aresta => aresta.Id, StringComparer.Ordinal)
            .ToArray();
    }

    public IReadOnlyList<Aresta> ArestasS(Vertice vertice)
    {
        ValidaVertice(vertice);

        return _saidas[vertice.Id]
            .Values
            .OrderBy(aresta => aresta.Id, StringComparer.Ordinal)
            .ToArray();
    }

    public string ParaTexto()
    {
        var separador = Tipo == TipoGrafo.Direcionado ? "->" : "--";
        var descricaoTipo = Tipo == TipoGrafo.Direcionado ? "direcionado" : "não direcionado";
        var texto = new StringBuilder();

        texto.AppendLine($"Grafo {descricaoTipo}");
        texto.AppendLine($"Ordem: {GetOrdem()}");
        texto.AppendLine($"Tamanho: {GetTamanho()}");
        texto.AppendLine();
        texto.AppendLine("Arestas:");

        foreach (var aresta in Arestas())
        {
            texto.AppendLine($"{aresta.Id}: {aresta.Origem.Id} {separador} {aresta.Destino.Id}");
        }

        texto.AppendLine();
        texto.AppendLine("Lista de adjacência:");

        foreach (var vertice in Vertices())
        {
            var adjacentes = Adj(vertice);
            var conteudo = adjacentes.Count == 0
                ? "nenhum"
                : string.Join(", ", adjacentes.Select(adjacente => adjacente.Id));

            texto.AppendLine($"{vertice.Id}: {conteudo}");
        }

        return texto.ToString().TrimEnd();
    }

    public override string ToString()
    {
        return ParaTexto();
    }

    private void ValidaVertice(Vertice vertice)
    {
        ArgumentNullException.ThrowIfNull(vertice);

        if (!_vertices.TryGetValue(vertice.Id, out var verticeArmazenado) ||
            !ReferenceEquals(verticeArmazenado, vertice))
        {
            throw new InvalidOperationException($"O vértice '{vertice.Id}' não pertence ao grafo.");
        }
    }

    private void ValidaAresta(Aresta aresta)
    {
        ArgumentNullException.ThrowIfNull(aresta);

        if (!_arestas.TryGetValue(aresta.Id, out var arestaArmazenada) ||
            !ReferenceEquals(arestaArmazenada, aresta))
        {
            throw new InvalidOperationException($"A aresta '{aresta.Id}' não pertence ao grafo.");
        }
    }

    private static string NormalizaId(string id, string elemento)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException($"O identificador de {elemento} é obrigatório.", nameof(id));
        }

        return id.Trim();
    }
}
