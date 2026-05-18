import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

export interface Category {
  id: string;
  name: string;
  type: number; // 1 = Income, 2 = Expense
  icon?: string;
  color?: string;
}

export interface CategoryPayload {
  name: string;
  type: number;
  icon?: string;
  color?: string;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/Category`);
  }

  create(payload: CategoryPayload): Observable<Category> {
    return this.http.post<Category>(`${this.apiUrl}/Category`, payload);
  }

  update(id: string, payload: CategoryPayload): Observable<Category> {
    return this.http.put<Category>(`${this.apiUrl}/Category/${id}`, payload);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Category/${id}`);
  }
}
