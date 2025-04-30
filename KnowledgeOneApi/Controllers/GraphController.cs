using Knowledge.Core.Graph;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeOneApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GraphController : ControllerBase
{
    // Static mock data for demonstration
    private static readonly List<Domain> _mockDomains = InitializeMockData();
    
    private static List<Domain> InitializeMockData()
    {
        // Create company domains
        var companyDomain = new Domain
        {
            Name = "Acme Corporation",
            Description = "Global technology and consulting firm"
        };
        
        var financeDomain = new Domain
        {
            Name = "Finance Department",
            Description = "Handles all financial operations and reporting"
        };
        
        var salesDomain = new Domain
        {
            Name = "Sales Department",
            Description = "Manages customer relationships and sales processes"
        };
        
        var itDomain = new Domain
        {
            Name = "IT Department",
            Description = "Manages all technology infrastructure and systems"
        };
        
        var marketingDomain = new Domain
        {
            Name = "Marketing Department",
            Description = "Handles brand, marketing campaigns and customer acquisition"
        };
        
        // Create domain connections
        companyDomain.ConnectedDomains.Add(new DomainConnection
        {
            SourceDomain = companyDomain,
            TargetDomain = financeDomain,
            RelationshipDescription = "Provides financial oversight",
            InfluenceStrength = 0.9
        });
        
        companyDomain.ConnectedDomains.Add(new DomainConnection
        {
            SourceDomain = companyDomain,
            TargetDomain = salesDomain,
            RelationshipDescription = "Sets revenue targets",
            InfluenceStrength = 0.8
        });
        
        companyDomain.ConnectedDomains.Add(new DomainConnection
        {
            SourceDomain = companyDomain,
            TargetDomain = itDomain,
            RelationshipDescription = "Provides technical infrastructure",
            InfluenceStrength = 0.7
        });
        
        companyDomain.ConnectedDomains.Add(new DomainConnection
        {
            SourceDomain = companyDomain,
            TargetDomain = marketingDomain,
            RelationshipDescription = "Provides brand guidelines",
            InfluenceStrength = 0.6
        });
        
        // Create Finance domain graphs
        var financeProcessGraph = new Graph
        {
            Name = "Finance Processes",
            Description = "Key financial processes and workflows",
            Domain = financeDomain
        };
        financeDomain.Graphs.Add(financeProcessGraph);
        
        var financeDataGraph = new Graph
        {
            Name = "Finance Data Landscape",
            Description = "Financial data systems and relationships",
            Domain = financeDomain
        };
        financeDomain.Graphs.Add(financeDataGraph);
        
        // Create vertices for Finance Process Graph
        var accounting = financeProcessGraph.AddVertex("Accounting", "General ledger and accounting processes");
        var reconciliation = financeProcessGraph.AddVertex("Reconciliation", "Bank and account reconciliation processes");
        var invoicing = financeProcessGraph.AddVertex("Invoicing", "Customer billing and invoice management");
        var payables = financeProcessGraph.AddVertex("Accounts Payable", "Vendor payment processing");
        var reporting = financeProcessGraph.AddVertex("Financial Reporting", "Financial statements and regulatory reporting");
        var budgeting = financeProcessGraph.AddVertex("Budgeting", "Annual and quarterly budget planning");
        var businessControl = financeProcessGraph.AddVertex("Business Control", "Financial analysis and performance measurement");
        
        // Create edges for Finance Process Graph
        financeProcessGraph.AddEdge(invoicing, accounting, "feeds", "Invoice data is recorded in accounting", 1.0);
        financeProcessGraph.AddEdge(accounting, reconciliation, "requires", "Account data needs reconciliation", 0.9);
        financeProcessGraph.AddEdge(payables, accounting, "feeds", "Payment data is recorded in accounting", 1.0);
        financeProcessGraph.AddEdge(accounting, reporting, "provides data", "Accounting data used in financial reports", 1.0);
        financeProcessGraph.AddEdge(budgeting, businessControl, "enables", "Budgets are used to measure performance", 0.8);
        financeProcessGraph.AddEdge(reporting, businessControl, "informs", "Reports provide data for business control", 0.7);
        
        // Create vertices for Finance Data Graph
        var erp = financeDataGraph.AddVertex("ERP System", "Enterprise Resource Planning system for financial data");
        var crm = financeDataGraph.AddVertex("CRM System", "Customer Relationship Management system");
        var dataWarehouse = financeDataGraph.AddVertex("Data Warehouse", "Central repository for financial data");
        var biTool = financeDataGraph.AddVertex("BI Platform", "Business Intelligence reporting tools");
        var invoiceSystem = financeDataGraph.AddVertex("Invoice Management", "Specialized invoicing system");
        var bankingSystem = financeDataGraph.AddVertex("Banking System", "Banking transactions and reconciliation");
        
        // Create edges for Finance Data Graph
        financeDataGraph.AddEdge(crm, invoiceSystem, "sends", "Customer data for invoice creation", 0.8);
        financeDataGraph.AddEdge(invoiceSystem, erp, "integrates", "Invoice data flows to ERP", 1.0);
        financeDataGraph.AddEdge(erp, dataWarehouse, "exports", "Financial data loaded to data warehouse nightly", 1.0);
        financeDataGraph.AddEdge(bankingSystem, erp, "reconciles", "Banking transactions matched to ERP entries", 0.9);
        financeDataGraph.AddEdge(dataWarehouse, biTool, "feeds", "Data warehouse provides reporting data", 1.0);
        
        // Connect the two finance graphs
        financeProcessGraph.ConnectedGraphs.Add(new GraphConnection 
        {
            SourceGraph = financeProcessGraph,
            TargetGraph = financeDataGraph,
            Description = "Process to system mapping"
        });
        
        // Create Sales domain graphs
        var salesProcessGraph = new Graph
        {
            Name = "Sales Processes",
            Description = "Customer acquisition and management processes",
            Domain = salesDomain
        };
        salesDomain.Graphs.Add(salesProcessGraph);
        
        var businessUnitsGraph = new Graph
        {
            Name = "Business Units",
            Description = "Revenue-generating units with products and services",
            Domain = salesDomain
        };
        salesDomain.Graphs.Add(businessUnitsGraph);
        
        // Create vertices for Sales Process Graph
        var leadGeneration = salesProcessGraph.AddVertex("Lead Generation", "Identifying and acquiring potential customers");
        var qualification = salesProcessGraph.AddVertex("Lead Qualification", "Evaluating leads for sales potential");
        var opportunity = salesProcessGraph.AddVertex("Opportunity Management", "Converting qualified leads to opportunities");
        var quoting = salesProcessGraph.AddVertex("Quoting", "Preparing and sending customer quotes");
        var closing = salesProcessGraph.AddVertex("Deal Closing", "Finalizing sales and contracts");
        var accountManagement = salesProcessGraph.AddVertex("Account Management", "Ongoing customer relationship management");
        
        // Create edges for Sales Process Graph
        salesProcessGraph.AddEdge(leadGeneration, qualification, "forwards to", "Leads are qualified before proceeding", 1.0);
        salesProcessGraph.AddEdge(qualification, opportunity, "converts", "Qualified leads become opportunities", 0.7);
        salesProcessGraph.AddEdge(opportunity, quoting, "requests", "Opportunities lead to quote preparation", 0.8);
        salesProcessGraph.AddEdge(quoting, closing, "enables", "Quotes are needed to close deals", 0.9);
        salesProcessGraph.AddEdge(closing, accountManagement, "transitions to", "Closed deals move to account management", 1.0);
        
        // Create vertices for Business Units Graph
        var enterpriseSolutions = businessUnitsGraph.AddVertex("Enterprise Solutions", "High-value consulting and system integration");
        var cloudServices = businessUnitsGraph.AddVertex("Cloud Services", "SaaS and managed cloud offerings");
        var professionalServices = businessUnitsGraph.AddVertex("Professional Services", "Consulting and implementation services");
        var products = businessUnitsGraph.AddVertex("Software Products", "Licensed software products");
        var maintenance = businessUnitsGraph.AddVertex("Maintenance & Support", "Ongoing customer support services");
        
        // Create edges for Business Units Graph with financial properties
        var e1 = businessUnitsGraph.AddEdge(
            enterpriseSolutions, 
            professionalServices, 
            "utilizes", 
            "Enterprise projects rely on professional services", 
            0.9
        );
        e1.Properties["annualRevenue"] = 12500000;
        e1.Properties["costOfSales"] = 7500000;
        e1.Properties["opex"] = 2000000;
        
        var e2 = businessUnitsGraph.AddEdge(
            cloudServices, 
            maintenance, 
            "requires", 
            "Cloud services need ongoing maintenance", 
            0.8
        );
        e2.Properties["annualRevenue"] = 8700000;
        e2.Properties["costOfSales"] = 3500000;
        e2.Properties["opex"] = 1800000;
        
        var e3 = businessUnitsGraph.AddEdge(
            products, 
            maintenance, 
            "generates", 
            "Products create maintenance opportunities", 
            0.7
        );
        e3.Properties["annualRevenue"] = 5300000;
        e3.Properties["costOfSales"] = 1200000;
        e3.Properties["opex"] = 1100000;
        
        // Adding property data to Business Unit vertices
        enterpriseSolutions.Properties["annualRevenue"] = 18500000;
        enterpriseSolutions.Properties["targetGrowth"] = 0.15;
        enterpriseSolutions.Properties["profitMargin"] = 0.28;
        
        cloudServices.Properties["annualRevenue"] = 12300000;
        cloudServices.Properties["targetGrowth"] = 0.22;
        cloudServices.Properties["profitMargin"] = 0.32;
        
        products.Properties["annualRevenue"] = 7800000;
        products.Properties["targetGrowth"] = 0.1;
        products.Properties["profitMargin"] = 0.43;
        
        // Connect Sales graphs to Finance graphs for data flow
        var salesFinanceConnection = new GraphConnection
        {
            SourceGraph = salesProcessGraph,
            TargetGraph = financeDataGraph,
            Description = "Sales data for financial processing"
        };
        salesProcessGraph.ConnectedGraphs.Add(salesFinanceConnection);
        
        // Create IT domain graph
        var itSystemsGraph = new Graph
        {
            Name = "IT Systems Landscape",
            Description = "Enterprise technology systems and infrastructure",
            Domain = itDomain
        };
        itDomain.Graphs.Add(itSystemsGraph);
        
        // Create vertices for IT Systems Graph
        var coreInfrastructure = itSystemsGraph.AddVertex("Core Infrastructure", "Servers, networks and data centers");
        var erpSystem = itSystemsGraph.AddVertex("ERP System", "Enterprise Resource Planning platform");
        var crmSystem = itSystemsGraph.AddVertex("CRM System", "Customer Relationship Management platform");
        var dataLake = itSystemsGraph.AddVertex("Data Lake", "Big data storage and processing");
        var biPlatform = itSystemsGraph.AddVertex("BI Platform", "Business intelligence and analytics");
        var cloudPlatform = itSystemsGraph.AddVertex("Cloud Platform", "Public and private cloud resources");
        
        // Create edges for IT Systems Graph
        itSystemsGraph.AddEdge(coreInfrastructure, erpSystem, "hosts", "Infrastructure supports ERP system", 0.9);
        itSystemsGraph.AddEdge(coreInfrastructure, crmSystem, "hosts", "Infrastructure supports CRM system", 0.9);
        itSystemsGraph.AddEdge(crmSystem, dataLake, "feeds", "CRM data flows to data lake", 0.8);
        itSystemsGraph.AddEdge(erpSystem, dataLake, "feeds", "ERP data flows to data lake", 0.8);
        itSystemsGraph.AddEdge(dataLake, biPlatform, "provides", "Data lake supplies BI platform with data", 1.0);
        itSystemsGraph.AddEdge(cloudPlatform, dataLake, "hosts", "Cloud platform hosts data lake components", 0.7);
        
        // Connect IT systems to finance data systems
        var itFinanceConnection = new GraphConnection
        {
            SourceGraph = itSystemsGraph,
            TargetGraph = financeDataGraph,
            Description = "IT systems supporting financial operations"
        };
        itSystemsGraph.ConnectedGraphs.Add(itFinanceConnection);
        
        // Create Marketing domain graph
        var marketingGraph = new Graph
        {
            Name = "Marketing Operations",
            Description = "Marketing channels, campaigns and analytics",
            Domain = marketingDomain
        };
        marketingDomain.Graphs.Add(marketingGraph);
        
        // Create vertices for Marketing Graph
        var digitalMarketing = marketingGraph.AddVertex("Digital Marketing", "Online advertising and digital channels");
        var contentMarketing = marketingGraph.AddVertex("Content Marketing", "Content creation and distribution");
        var eventMarketing = marketingGraph.AddVertex("Event Marketing", "Conferences and corporate events");
        var brandManagement = marketingGraph.AddVertex("Brand Management", "Brand guidelines and corporate identity");
        var marketingAnalytics = marketingGraph.AddVertex("Marketing Analytics", "Campaign performance measurement");
        
        // Create edges for Marketing Graph
        marketingGraph.AddEdge(digitalMarketing, marketingAnalytics, "measured by", "Digital campaign performance", 0.9);
        marketingGraph.AddEdge(contentMarketing, marketingAnalytics, "measured by", "Content performance metrics", 0.8);
        marketingGraph.AddEdge(eventMarketing, marketingAnalytics, "measured by", "Event ROI analysis", 0.7);
        marketingGraph.AddEdge(brandManagement, digitalMarketing, "guides", "Brand guidelines direct digital marketing", 0.8);
        marketingGraph.AddEdge(brandManagement, contentMarketing, "guides", "Brand voice informs content marketing", 0.9);
        
        // Connect Marketing to Sales
        var marketingSalesConnection = new GraphConnection
        {
            SourceGraph = marketingGraph,
            TargetGraph = salesProcessGraph,
            Description = "Marketing leads to sales pipeline"
        };
        marketingGraph.ConnectedGraphs.Add(marketingSalesConnection);
        
        return new List<Domain> { 
            companyDomain, 
            financeDomain, 
            salesDomain, 
            itDomain, 
            marketingDomain 
        };
    }
    
    [HttpGet("domains")]
    public ActionResult<IEnumerable<Domain>> GetDomains()
    {
        return Ok(_mockDomains);
    }
    
    [HttpGet("domains/{domainId}")]
    public ActionResult<Domain> GetDomain(Guid domainId)
    {
        var domain = _mockDomains.FirstOrDefault(d => d.Id == domainId);
        if (domain == null)
            return NotFound();
        
        return Ok(domain);
    }
    
    [HttpGet("domains/{domainId}/graphs")]
    public ActionResult<IEnumerable<Graph>> GetGraphsInDomain(Guid domainId)
    {
        var domain = _mockDomains.FirstOrDefault(d => d.Id == domainId);
        if (domain == null)
            return NotFound();
        
        return Ok(domain.Graphs);
    }
    
    [HttpGet("graphs/{graphId}")]
    public ActionResult<Graph> GetGraph(Guid graphId)
    {
        var graph = _mockDomains
            .SelectMany(d => d.Graphs)
            .FirstOrDefault(g => g.Id == graphId);
            
        if (graph == null)
            return NotFound();
        
        return Ok(graph);
    }
    
    [HttpGet("graphs/{graphId}/vertices")]
    public ActionResult<IEnumerable<Vertex>> GetVertices(Guid graphId)
    {
        var graph = _mockDomains
            .SelectMany(d => d.Graphs)
            .FirstOrDefault(g => g.Id == graphId);
            
        if (graph == null)
            return NotFound();
        
        return Ok(graph.Vertices);
    }
    
    [HttpGet("graphs/{graphId}/edges")]
    public ActionResult<IEnumerable<Edge>> GetEdges(Guid graphId)
    {
        var graph = _mockDomains
            .SelectMany(d => d.Graphs)
            .FirstOrDefault(g => g.Id == graphId);
            
        if (graph == null)
            return NotFound();
        
        return Ok(graph.Edges);
    }
    
    [HttpGet("subdomains/{parentDomainId}")]
    public ActionResult<IEnumerable<Domain>> GetSubdomains(Guid parentDomainId)
    {
        var parentDomain = _mockDomains.FirstOrDefault(d => d.Id == parentDomainId);
        if (parentDomain == null)
            return NotFound();
        
        // Get domains that are connected to the parent domain (treated as subdomains)
        var subdomains = parentDomain.ConnectedDomains
            .Select(c => c.TargetDomain)
            .ToList();
            
        return Ok(subdomains);
    }
    
    [HttpGet("vertices/{vertexId}/relations")]
    public ActionResult<object> GetVertexRelations(Guid vertexId)
    {
        var vertex = _mockDomains
            .SelectMany(d => d.Graphs)
            .SelectMany(g => g.Vertices)
            .FirstOrDefault(v => v.Id == vertexId);
            
        if (vertex == null)
            return NotFound();
        
        var relations = new
        {
            Outgoing = vertex.OutgoingEdges.Select(e => new
            {
                e.Id,
                e.Relation,
                e.Description,
                e.Weight,
                Target = new { e.TargetVertex.Id, e.TargetVertex.Label }
            }),
            Incoming = vertex.IncomingEdges.Select(e => new
            {
                e.Id,
                e.Relation,
                e.Description,
                e.Weight,
                Source = new { e.SourceVertex.Id, e.SourceVertex.Label }
            })
        };
        
        return Ok(relations);
    }
    
    [HttpGet("domains/{domainId}/connected")]
    public ActionResult<object> GetConnectedDomains(Guid domainId)
    {
        var domain = _mockDomains.FirstOrDefault(d => d.Id == domainId);
        if (domain == null)
            return NotFound();
        
        var connectedInfo = domain.ConnectedDomains.Select(c => new
        {
            Connection = new { c.Id, c.RelationshipDescription, c.InfluenceStrength },
            TargetDomain = new { c.TargetDomain.Id, c.TargetDomain.Name, c.TargetDomain.Description }
        });
        
        return Ok(connectedInfo);
    }
    
    [HttpGet("graphs/{graphId}/connected")]
    public ActionResult<object> GetConnectedGraphs(Guid graphId)
    {
        var graph = _mockDomains
            .SelectMany(d => d.Graphs)
            .FirstOrDefault(g => g.Id == graphId);
            
        if (graph == null)
            return NotFound();
        
        var connectedInfo = graph.ConnectedGraphs.Select(c => new
        {
            Connection = new { c.Id, c.Description },
            TargetGraph = new { c.TargetGraph.Id, c.TargetGraph.Name, c.TargetGraph.Description }
        });
        
        return Ok(connectedInfo);
    }
}