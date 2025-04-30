using System.Text.Json.Serialization;

namespace Knowledge.Core.Graph;

/// <summary>
/// Represents a vertex (node) in a graph
/// </summary>
public class Vertex
{
    /// <summary>
    /// Unique identifier for the vertex
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Label or name of the vertex
    /// </summary>
    public string Label { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the vertex
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Collection of tags associated with this vertex for categorization and search
    /// </summary>
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    /// <summary>
    /// The graph that this vertex belongs to
    /// </summary>
    [JsonIgnore]
    public Graph ParentGraph { get; set; } = null!;
    
    /// <summary>
    /// Collection of outgoing edges from this vertex
    /// </summary>
    public ICollection<Edge> OutgoingEdges { get; set; } = new List<Edge>();
    
    /// <summary>
    /// Collection of incoming edges to this vertex
    /// </summary>
    public ICollection<Edge> IncomingEdges { get; set; } = new List<Edge>();
    
    /// <summary>
    /// Additional properties stored as key-value pairs
    /// </summary>
    public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
}

/// <summary>
/// Represents a tag that can be assigned to vertices and edges for categorization
/// </summary>
public class Tag
{
    /// <summary>
    /// Unique identifier for the tag
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Name of the tag
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Optional group or category for organizing tags
    /// </summary>
    public string Category { get; set; } = string.Empty;
}