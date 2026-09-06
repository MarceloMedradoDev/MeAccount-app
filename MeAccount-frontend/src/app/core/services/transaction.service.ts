import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Transaction, CreateTransactionRequest, UpdateTransactionRequest } from '../models/transaction.model';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private readonly API_URL = '/api/transactions';

  constructor(private http: HttpClient) {}

  getAll(filters?: {
    startDate?: Date;
    endDate?: Date;
    accountId?: string;
    categoryId?: string;
    type?: string;
  }): Observable<Transaction[]> {
    let params = new HttpParams();

    if (filters?.startDate) {
      params = params.set('startDate', filters.startDate.toISOString());
    }
    if (filters?.endDate) {
      params = params.set('endDate', filters.endDate.toISOString());
    }
    if (filters?.accountId) {
      params = params.set('accountId', filters.accountId);
    }
    if (filters?.categoryId) {
      params = params.set('categoryId', filters.categoryId);
    }
    if (filters?.type) {
      params = params.set('type', filters.type);
    }

    return this.http.get<Transaction[]>(this.API_URL, { params });
  }

  getById(id: string): Observable<Transaction> {
    return this.http.get<Transaction>(`${this.API_URL}/${id}`);
  }

  create(request: CreateTransactionRequest): Observable<Transaction> {
    return this.http.post<Transaction>(this.API_URL, request);
  }

  update(id: string, request: UpdateTransactionRequest): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.API_URL}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
