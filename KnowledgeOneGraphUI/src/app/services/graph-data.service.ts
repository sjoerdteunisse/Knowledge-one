import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface Domain {
  id: string;
  name: string;
  description?: string;
  graphs?: Graph[];
  connectedDomains?: DomainConnection[];
}

export interface DomainConnection {
  id: string;
  relationshipDescription?: string;
  influenceStrength?: number;
}

export interface Graph {
  id: string;
  name: string;
  description?: string;
  vertices?: Vertex[];
  edges?: Edge[];
  connectedGraphs?: GraphConnection[];
  tags?: string[];
}

export interface GraphConnection {
  id: string;
  description?: string;
  connectedVertices?: any[];
}

export interface Vertex {
  id: string;
  label: string;
  description?: string;
  properties?: { [key: string]: any };
  tags?: string[];
  outgoingEdges?: Edge[];
  incomingEdges?: Edge[];
}

export interface Edge {
  id: string;
  from?: string;
  to?: string;
  relation?: string;
  description?: string;
  label?: string;
  weight?: number;
  properties?: { [key: string]: any };
  tags?: string[];
  isDirected?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class GraphDataService {
  private apiUrl = 'https://localhost:7233/api'; // Update with your actual API URL

  constructor(private http: HttpClient) { }

  getDomains(): Observable<Domain[]> {
    return this.http.get<Domain[]>(`${this.apiUrl}/graph/domains`)
      .pipe(
        catchError(this.handleError<Domain[]>('getDomains', []))
      );
  }

  getDomainById(id: string): Observable<Domain> {
    return this.http.get<Domain>(`${this.apiUrl}/graph/domains/${id}`)
      .pipe(
        catchError(this.handleError<Domain>('getDomainById'))
      );
  }

  getGraphById(id: string): Observable<Graph> {
    return this.http.get<Graph>(`${this.apiUrl}/graphs/${id}`)
      .pipe(
        catchError(this.handleError<Graph>('getGraphById'))
      );
  }

  searchByTerm(term: string): Observable<{ domains: Domain[], graphs: Graph[], vertices: Vertex[] }> {
    return this.http.get<{ domains: Domain[], graphs: Graph[], vertices: Vertex[] }>(`${this.apiUrl}/search?term=${term}`)
      .pipe(
        catchError(this.handleError<{ domains: Domain[], graphs: Graph[], vertices: Vertex[] }>(
          'searchByTerm', 
          { domains: [], graphs: [], vertices: [] }
        ))
      );
  }

  // Helper method to handle errors
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(`${operation} failed: ${error.message}`);
      // Return an empty result to keep the app running
      return of(result as T);
    };
  }
}
