import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth/auth.service';
import { UserService } from '../../../core/services/user/user.service';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule, FormsModule],
  templateUrl: './layout.html',
  styleUrl: './layout.css',
})
export class Layout implements OnInit {
  currentBalance: number | null = null;
  username: string = '';
  balanceVisible: boolean = false;

  // Edit balance modal
  showBalanceModal = false;
  newBalanceInput: number = 0;
  isSavingBalance = false;
  balanceError = '';

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    // Giải mã username từ JWT token
    const token = this.authService.getToken();
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.username = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
          || payload['name']
          || payload['sub']
          || 'User';
      } catch { }
    }

    // Subscribe vào balance stream — cập nhật tự động khi có thay đổi
    this.userService.balance$.subscribe(balance => {
      this.currentBalance = balance;
      this.cdr.detectChanges();
    });

    // Tải lần đầu
    this.userService.loadBalance();
  }

  toggleBalanceVisibility(): void {
    this.balanceVisible = !this.balanceVisible;
  }

  openBalanceModal(): void {
    this.newBalanceInput = this.currentBalance ?? 0;
    this.balanceError = '';
    this.showBalanceModal = true;
  }

  closeBalanceModal(): void {
    this.showBalanceModal = false;
  }

  saveBalance(): void {
    if (this.newBalanceInput < 0) {
      this.balanceError = 'Số dư không thể âm.';
      return;
    }
    this.isSavingBalance = true;
    this.balanceError = '';
    this.userService.updateBalance(this.newBalanceInput).subscribe({
      next: () => {
        this.isSavingBalance = false;
        this.showBalanceModal = false;
        this.userService.loadBalance(); // Refresh từ server
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isSavingBalance = false;
        this.balanceError = err.error?.message || 'Cập nhật thất bại!';
        this.cdr.detectChanges();
      }
    });
  }

  logout(): void {
    this.authService.removeToken();
    this.router.navigate(['/login']);
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
  }
}
