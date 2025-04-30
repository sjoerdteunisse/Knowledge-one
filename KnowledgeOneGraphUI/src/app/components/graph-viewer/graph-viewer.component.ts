import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Network, DataSet, Node, Edge } from 'vis-network/standalone';
import { GraphDataService, Domain, Graph, Vertex } from '../../services/graph-data.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-graph-viewer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './graph-viewer.component.html',
  styleUrl: './graph-viewer.component.scss'
})
export class GraphViewerComponent implements OnInit {
  @ViewChild('graphContainer', { static: true }) graphContainer!: ElementRef;
  
  network: Network | null = null;
  nodes = new DataSet<Node>([]);
  edges = new DataSet<Edge>([]);
  searchQuery: string = '';
  isLoading: boolean = false;
  selectedDomain: Domain | null = null;
  selectedGraph: Graph | null = null;
  domains: Domain[] = [];

  constructor(private graphDataService: GraphDataService) { }

  ngOnInit(): void {
    this.initNetwork();
    this.loadDomains();
  }

  initNetwork(): void {
    const container = this.graphContainer.nativeElement;
    
    const data = {
      nodes: this.nodes,
      edges: this.edges
    };
    
    const options = {
      nodes: {
        shape: 'dot',
        size: 16,
        font: {
          size: 14,
          face: 'Roboto'
        },
        borderWidth: 2,
        shadow: true
      },
      edges: {
        width: 2,
        shadow: true,
        arrows: {
          to: { enabled: true, scaleFactor: 0.5 }
        }
      },
      physics: {
        enabled: true,
        barnesHut: {
          gravitationalConstant: -2000,
          centralGravity: 0.3,
          springLength: 95,
          springConstant: 0.04,
          damping: 0.09
        }
      },
      interaction: {
        navigationButtons: true,
        keyboard: true,
        tooltipDelay: 200,
        hover: true
      }
    };
    
    this.network = new Network(container, data, options);
    this.network.on('click', (params) => {
      if (params.nodes.length > 0) {
        const nodeId = params.nodes[0];
        console.log('Selected node:', nodeId);
        // Handle node selection logic here
      }
    });
  }

  loadDomains(): void {
    this.isLoading = true;
    this.graphDataService.getDomains().subscribe({
      next: (domains) => {
        this.domains = domains;
        this.isLoading = false;
        if (domains.length > 0) {
          this.loadDomainGraph(domains[0]);
        }
      },
      error: (error) => {
        console.error('Error loading domains:', error);
        this.isLoading = false;
      }
    });
  }

  loadDomainGraph(domain: Domain): void {
    this.isLoading = true;
    this.selectedDomain = domain;
    this.graphDataService.getDomainById(domain.id).subscribe({
      next: (fullDomain) => {
        this.selectedDomain = fullDomain;
        this.visualizeDomain(fullDomain);
        this.isLoading = false;
      },
      error: (error) => {
        console.error(`Error loading domain ${domain.id}:`, error);
        this.isLoading = false;
      }
    });
  }

  loadGraph(graph: Graph): void {
    this.isLoading = true;
    this.selectedGraph = graph;
    this.graphDataService.getGraphById(graph.id).subscribe({
      next: (fullGraph) => {
        this.selectedGraph = fullGraph;
        this.visualizeGraph(fullGraph);
        this.isLoading = false;
      },
      error: (error) => {
        console.error(`Error loading graph ${graph.id}:`, error);
        this.isLoading = false;
      }
    });
  }

  search(): void {
    if (!this.searchQuery.trim()) return;
    
    this.isLoading = true;
    this.graphDataService.searchByTerm(this.searchQuery).subscribe({
      next: (results) => {
        this.visualizeSearchResults(results);
        this.isLoading = false;
      },
      error: (error) => {
        console.error(`Error searching for ${this.searchQuery}:`, error);
        this.isLoading = false;
      }
    });
  }

  visualizeDomain(domain: Domain): void {
    this.nodes.clear();
    this.edges.clear();
    
    // Add the domain node
    this.nodes.add({
      id: domain.id,
      label: domain.name,
      group: 'domains',
      title: domain.description || domain.name,
      color: { background: '#E91E63', border: '#C2185B' }
    });
    
    // Add graphs related to this domain
    if (domain.graphs && domain.graphs.length > 0) {
      domain.graphs.forEach(graph => {
        this.nodes.add({
          id: graph.id,
          label: graph.name,
          group: 'graphs',
          title: graph.description || graph.name,
          color: { background: '#2196F3', border: '#1976D2' }
        });
        
        this.edges.add({
          from: domain.id,
          to: graph.id,
          label: 'contains'
        });
      });
    }
    
    this.network?.fit();
  }

  visualizeGraph(graph: Graph): void {
    this.nodes.clear();
    this.edges.clear();
    
    // Add the graph node
    this.nodes.add({
      id: graph.id,
      label: graph.name,
      group: 'graphs',
      title: graph.description || graph.name,
      color: { background: '#2196F3', border: '#1976D2' },
      size: 30
    });
    
    // Add vertices
    if (graph.vertices && graph.vertices.length > 0) {
      graph.vertices.forEach(vertex => {
        this.nodes.add({
          id: vertex.id,
          label: vertex.label,
          group: 'vertices',
          title: this.formatProperties(vertex.properties),
          color: { background: '#4CAF50', border: '#388E3C' }
        });
      });
    }
    
    // Add edges
    if (graph.edges && graph.edges.length > 0) {
      graph.edges.forEach(edge => {
        // Handle the new edge data format where we need to extract source and target vertices
        // If edge has direct from/to properties, use them directly
        // Otherwise try to determine them from the relationships
        let fromId = edge.from;
        let toId = edge.to;
        
        if (!fromId && edge.hasOwnProperty('relation')) {
          // Find the vertex with this edge in its outgoingEdges
          const sourceVertex = graph.vertices?.find(v => 
            v.outgoingEdges?.some(e => e.id === edge.id)
          );
          if (sourceVertex) fromId = sourceVertex.id;
        }
        
        if (!toId && edge.hasOwnProperty('relation')) {
          // Find the vertex with this edge in its incomingEdges
          const targetVertex = graph.vertices?.find(v => 
            v.incomingEdges?.some(e => e.id === edge.id)
          );
          if (targetVertex) toId = targetVertex.id;
        }
        
        if (fromId && toId) {
          this.edges.add({
            id: edge.id,
            from: fromId,
            to: toId,
            label: edge.relation || edge.label,
            title: edge.description || this.formatProperties(edge.properties),
            arrows: edge.isDirected !== false ? { to: true } : { to: false }
          });
        }
      });
    }
    
    this.network?.fit();
  }

  visualizeSearchResults(results: { domains: Domain[], graphs: Graph[], vertices: Vertex[] }): void {
    this.nodes.clear();
    this.edges.clear();
    
    const addedNodeIds = new Set<string>();
    
    // Add domains
    if (results.domains && results.domains.length > 0) {
      results.domains.forEach(domain => {
        this.nodes.add({
          id: domain.id,
          label: domain.name,
          group: 'domains',
          title: domain.description || domain.name,
          color: { background: '#E91E63', border: '#C2185B' }
        });
        addedNodeIds.add(domain.id);
      });
    }
    
    // Add graphs
    if (results.graphs && results.graphs.length > 0) {
      results.graphs.forEach(graph => {
        this.nodes.add({
          id: graph.id,
          label: graph.name,
          group: 'graphs',
          title: graph.description || graph.name,
          color: { background: '#2196F3', border: '#1976D2' }
        });
        addedNodeIds.add(graph.id);
        
        // If the domain of this graph is also in results, add an edge
        if (graph.id.includes(':') && addedNodeIds.has(graph.id.split(':')[0])) {
          this.edges.add({
            from: graph.id.split(':')[0],
            to: graph.id,
            label: 'contains'
          });
        }
      });
    }
    
    // Add vertices
    if (results.vertices && results.vertices.length > 0) {
      results.vertices.forEach(vertex => {
        this.nodes.add({
          id: vertex.id,
          label: vertex.label,
          group: 'vertices',
          title: this.formatProperties(vertex.properties),
          color: { background: '#4CAF50', border: '#388E3C' }
        });
        addedNodeIds.add(vertex.id);
        
        // If the graph of this vertex is also in results, add an edge
        if (vertex.id.includes(':')) {
          const parts = vertex.id.split(':');
          if (parts.length > 1) {
            const possibleGraphId = parts.slice(0, -1).join(':');
            if (addedNodeIds.has(possibleGraphId)) {
              this.edges.add({
                from: possibleGraphId,
                to: vertex.id,
                label: 'contains'
              });
            }
          }
        }
      });
    }
    
    this.network?.fit();
  }

  formatProperties(properties: any | undefined): string {
    if (!properties) return '';
    return Object.entries(properties)
      .map(([key, value]) => `${key}: ${value}`)
      .join('\n');
  }
}
