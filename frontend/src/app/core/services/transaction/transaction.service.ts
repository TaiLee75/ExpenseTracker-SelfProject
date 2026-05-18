import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Transaction } from '../dashboard/dashboard.service';

export interface CreateTransactionPayload {
  amount: number;
  date: string;
  note?: string;
  categoryId: string;
}

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getByMonth(year: number, month: number): Observable<Transaction[]> {
    const params = new HttpParams().set('year', year).set('month', month);
    return this.http.get<Transaction[]>(`${this.apiUrl}/Transaction`, { params });
  }

  create(payload: CreateTransactionPayload): Observable<Transaction> {
    return this.http.post<Transaction>(`${this.apiUrl}/Transaction`, payload);
  }

  update(id: string, payload: CreateTransactionPayload): Observable<Transaction> {
    return this.http.put<Transaction>(`${this.apiUrl}/Transaction/${id}`, payload);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Transaction/${id}`);
  }
}
