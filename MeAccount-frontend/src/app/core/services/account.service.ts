import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Account, CreateAccountRequest, UpdateAccountRequest } from '../models/account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly API_URL = '/api/accounts';

  constructor(private http: HttpClient) {}

  getAll(): Observable<Account[]> {
    return this.http.get<Account[]>(this.API_URL);
  }

  getById(id: string): Observable<Account> {
    return this.http.get<Account>(`${this.API_URL}/${id}`);
  }

  create(request: CreateAccountRequest): Observable<Account> {
    return this.http.post<Account>(this.API_URL, request);
  }

  update(id: string, request: UpdateAccountRequest): Observable<Account> {
    return this.http.put<Account>(`${this.API_URL}/${id}`, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/${id}`);
  }
}
