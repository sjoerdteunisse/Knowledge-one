using System.Text.Json.Serialization;
using System.Linq;

namespace Knowledge.Core.Graph;

/// <summary>
/// Represents a graph structure with vertices and edges
/// </summary>
public class Graph
{
    /// <summary>
    /// Unique identifier for the graph
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Name of the graph
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the graph
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// The domain this graph belongs to
    /// </summary>
    [JsonIgnore]
    public Domain Domain { get; set; } = null!;
    
    /// <summary>
    /// Collection of all vertices in the graph
    /// </summary>
    public ICollection<Vertex> Vertices { get; set; } = new List<Vertex>();
    
    /// <summary>
    /// Collection of all edges in the graph
    /// </summary>
    public ICollection<Edge> Edges { get; set; } = new List<Edge>();
    
    /// <summary>
    /// Collection of graphs that this graph has connections with
    /// </summary>
    public ICollection<GraphConnection> ConnectedGraphs { get; set; } = new List<GraphConnection>();
    
    /// <summary>
    /// Set of tags associated with this graph
    /// </summary>
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    /// <summary>
    /// Creates a new vertex and adds it to the graph
    /// </summary>
    /// <param name="label">Label for the vertex</param>
    /// <param name="description">Description for the vertex</param>
    /// <returns>The created vertex</returns>
    public Vertex AddVertex(string label, string description = "")
    {
        var vertex = new Vertex
        {
            Label = label,
            Description = description,
            ParentGraph = this
        };
        
        Vertices.Add(vertex);
        return vertex;
    }
    
    /// <summary>
    /// Creates an edge between two vertices with the specified relationship
    /// </summary>
    /// <param name="source">Source vertex</param>
    /// <param name="target">Target vertex</param>
    /// <param name="relation">Relationship type</param>
    /// <param name="description">Description of the relationship</param>
    /// <param name="weight">Weight/strength of the relationship</param>
    /// <param name="isDirected">Whether the relationship is directed or undirected</param>
    /// <returns>The created edge</returns>
    public Edge AddEdge(Vertex source, Vertex target, string relation, string description = "", double weight = 1.0, bool isDirected = true)
    {
        var edge = new Edge
        {
            SourceVertex = source,
            TargetVertex = target,
            Relation = relation,
            Description = description,
            Weight = weight,
            IsDirected = isDirected,
            ParentGraph = this
        };
        
        Edges.Add(edge);
        source.OutgoingEdges.Add(edge);
        target.IncomingEdges.Add(edge);
        
        return edge;
    }
    
    /// <summary>
    /// Removes a vertex and all its connected edges from the graph
    /// </summary>
    /// <param name="vertex">The vertex to remove</param>
    public void RemoveVertex(Vertex vertex)
    {
        // Remove all edges connected to this vertex
        var edgesToRemove = vertex.OutgoingEdges.Concat(vertex.IncomingEdges).ToList();
        foreach (var edge in edgesToRemove)
        {
            RemoveEdge(edge);
        }
        
        Vertices.Remove(vertex);
    }
    
    /// <summary>
    /// Removes an edge from the graph
    /// </summary>
    /// <param name="edge">The edge to remove</param>
    public void RemoveEdge(Edge edge)
    {
        edge.SourceVertex.OutgoingEdges.Remove(edge);
        edge.TargetVertex.IncomingEdges.Remove(edge);
        Edges.Remove(edge);
    }
}

/// <summary>
/// Represents a connection between two graphs
/// </summary>
public class GraphConnection
{
    /// <summary>
    /// Unique identifier for the graph connection
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// The source graph
    /// </summary>
    [JsonIgnore]
    public Graph SourceGraph { get; set; } = null!;
    
    /// <summary>
    /// The target graph
    /// </summary>
    [JsonIgnore]
    public Graph TargetGraph { get; set; } = null!;
    
    /// <summary>
    /// Description of the relationship between the graphs
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// A collection of paired vertices that connect the two graphs
    /// </summary>
    public ICollection<(Vertex SourceVertex, Vertex TargetVertex)> ConnectedVertices { get; set; } = 
        new List<(Vertex SourceVertex, Vertex TargetVertex)>();
}