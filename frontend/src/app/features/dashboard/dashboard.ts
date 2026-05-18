import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, CurrencyPipe, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DashboardService, DashboardSummary, Transaction } from '../../core/services/dashboard/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, CurrencyPipe, DatePipe],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  summary: DashboardSummary | null = null;
  recentTransactions: Transaction[] = [];
  isLoading = true;
  hasError = false;

  currentYear: number = new Date().getFullYear();
  currentMonth: number = new Date().getMonth() + 1;

  months = [
    { value: 1, label: 'Tháng 1' }, { value: 2, label: 'Tháng 2' },
    { value: 3, label: 'Tháng 3' }, { value: 4, label: 'Tháng 4' },
    { value: 5, label: 'Tháng 5' }, { value: 6, label: 'Tháng 6' },
    { value: 7, label: 'Tháng 7' }, { value: 8, label: 'Tháng 8' },
    { value: 9, label: 'Tháng 9' }, { value: 10, label: 'Tháng 10' },
    { value: 11, label: 'Tháng 11' }, { value: 12, label: 'Tháng 12' },
  ];

  constructor(
    private dashboardService: DashboardService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.isLoading = true;
    this.hasError = false;

    this.dashboardService.getSummary(this.currentYear, this.currentMonth).subscribe({
      next: (data) => {
        this.summary = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.hasError = true;
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });

    this.dashboardService.getRecentTransactions(8).subscribe({
      next: (data) => {
        this.recentTransactions = data;
        this.cdr.detectChanges();
      }
    });
  }

  onMonthChange(): void {
    this.loadData();
  }

  getProgressWidth(percentage: number): string {
    return `${Math.min(percentage, 100)}%`;
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
  }
}
