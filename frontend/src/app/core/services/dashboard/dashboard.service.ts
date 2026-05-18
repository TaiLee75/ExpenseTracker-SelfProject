import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface CategoryBreakdown {
  categoryId: string;
  categoryName: string;
  categoryColor?: string;
  totalAmount: number;
  percentage: number;
}

export interface DashboardSummary {
  totalIncome: number;
  totalExpense: number;
  netIncome: number;
  incomeBreakdown: CategoryBreakdown[];
  expenseBreakdown: CategoryBreakdown[];
}

export interface Transaction {
  id: string;
  amount: number;
  date: string;
  note?: string;
  categoryId: string;
  categoryName: string;
  type: number; // 1 = Income, 2 = Expense
  categoryIcon?: string;
  categoryColor?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getSummary(year: number, month: number): Observable<DashboardSummary> {
    const params = new HttpParams().set('year', year).set('month', month);
    return this.http.get<DashboardSummary>(`${this.apiUrl}/Dashboard/summary`, { params });
  }

  getRecentTransactions(count: number = 8): Observable<Transaction[]> {
    const params = new HttpParams().set('count', count);
    return this.http.get<Transaction[]>(`${this.apiUrl}/Transaction/recent`, { params });
  }
}
