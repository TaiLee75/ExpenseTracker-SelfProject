import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl;

  // BehaviorSubject để chia sẻ trạng thái số dư toàn app
  private balanceSubject = new BehaviorSubject<number | null>(null);
  balance$ = this.balanceSubject.asObservable();

  constructor(private http: HttpClient) {}

  loadBalance(): void {
    this.http.get<{ currentBalance: number }>(`${this.apiUrl}/User/balance`).subscribe({
      next: (res) => this.balanceSubject.next(res.currentBalance)
    });
  }

  updateBalance(newBalance: number) {
    return this.http.put(`${this.apiUrl}/User/balance`, { newBalance });
  }
}
