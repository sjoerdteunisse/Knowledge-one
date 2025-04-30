using System.Text.Json.Serialization;

namespace Knowledge.Core.Graph;

/// <summary>
/// Represents an edge that connects two vertices in a graph with a defined relationship
/// </summary>
public class Edge
{
    /// <summary>
    /// Unique identifier for the edge
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// The source vertex where the edge begins
    /// </summary>
    [JsonIgnore]
    public Vertex SourceVertex { get; set; } = null!;
    
    /// <summary>
    /// The target vertex where the edge ends
    /// </summary>
    [JsonIgnore]
    public Vertex TargetVertex { get; set; } = null!;
    
    /// <summary>
    /// Type of relationship that this edge represents
    /// </summary>
    public string Relation { get; set; } = string.Empty;
    
    /// <summary>
    /// Detailed description of the relationship
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Weight/strength of the relationship (can be used for various calculations)
    /// </summary>
    public double Weight { get; set; } = 1.0;
    
    /// <summary>
    /// Collection of tags associated with this edge for categorization and search
    /// </summary>
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    
    /// <summary>
    /// The graph that this edge belongs to
    /// </summary>
    [JsonIgnore]
    public Graph ParentGraph { get; set; } = null!;
    
    /// <summary>
    /// Whether the relationship is directed (one-way) or undirected (two-way)
    /// </summary>
    public bool IsDirected { get; set; } = true;
    
    /// <summary>
    /// Additional properties stored as key-value pairs
    /// </summary>
    public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();
}