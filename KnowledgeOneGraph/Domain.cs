using System.Text.Json.Serialization;

namespace Knowledge.Core.Graph;

/// <summary>
/// Represents a domain that can contain multiple related graphs
/// </summary>
public class Domain
{
    /// <summary>
    /// Unique identifier for the domain
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// Name of the domain
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Description of the domain
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Collection of graphs that belong to this domain
    /// </summary>
    public ICollection<Graph> Graphs { get; set; } = new List<Graph>();
    
    /// <summary>
    /// Collection of domains that this domain has influence on
    /// </summary>
    public ICollection<DomainConnection> ConnectedDomains { get; set; } = new List<DomainConnection>();
}

/// <summary>
/// Represents a connection between two domains and the nature of their relationship
/// </summary>
public class DomainConnection
{
    /// <summary>
    /// Unique identifier for the domain connection
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();
    
    /// <summary>
    /// The source domain of the connection
    /// </summary>
    [JsonIgnore]
    public Domain SourceDomain { get; set; } = null!;
    
    /// <summary>
    /// The target domain that is influenced
    /// </summary>
    [JsonIgnore]
    public Domain TargetDomain { get; set; } = null!;
    
    /// <summary>
    /// Description of how the source domain influences the target domain
    /// </summary>
    public string RelationshipDescription { get; set; } = string.Empty;
    
    /// <summary>
    /// Strength of influence (0.0 to 1.0)
    /// </summary>
    public double InfluenceStrength { get; set; }
}