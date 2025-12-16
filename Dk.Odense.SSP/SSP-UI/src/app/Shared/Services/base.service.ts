import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable()
export class BaseService<T> {
  baseUrl = environment.baseUrl;
  endpoint: string = '';

  constructor(private http: HttpClient) {}

  // get methods
  public get(id: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + this.endpoint + '/get/' + id);
  }

  public list(): Observable<T[]> {
    return this.http.get<T[]>(this.baseUrl + this.endpoint + '/list');
  }

  public getFromEndpoint(endpoint: string): Observable<T> {
    return this.http.get<T>(
      this.baseUrl + this.endpoint + endpoint.toLowerCase()
    );
  }

  public listFromEndpoint(endpoint: string): Observable<T[]> {
    return this.http.get<T[]>(
      this.baseUrl + this.endpoint + '/' + endpoint.toLowerCase()
    );
  }

  // put methods
  public put(item: T): Observable<T> {
    return this.http.put<T>(this.baseUrl + this.endpoint + '/put', item);
  }

  // post methods
  public post(item: T): Observable<T> {
    return this.http.post<T>(this.baseUrl + this.endpoint + '/post', item);
  }

  public postToEndpoint<B>(item: B, endpoint: string): Observable<B> {
    return this.http.post<B>(
      this.baseUrl + this.endpoint + '/' + endpoint,
      item
    );
  }

  public create(): Observable<T> {
    return this.http.post<T>(this.baseUrl + this.endpoint + '/create', null);
  }

  // delete
  public delete(id: string): Observable<T> {
    return this.http.delete<T>(this.baseUrl + this.endpoint + '/delete/' + id);
  }

  // util
  public getHTTPService(): HttpClient {
    return this.http;
  }
}
