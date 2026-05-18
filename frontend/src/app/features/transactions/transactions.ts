import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TransactionService, CreateTransactionPayload } from '../../core/services/transaction/transaction.service';
import { CategoryService, Category } from '../../core/services/category/category.service';
import { Transaction } from '../../core/services/dashboard/dashboard.service';
import { UserService } from '../../core/services/user/user.service';

type ModalMode = 'create' | 'edit';

@Component({
  selector: 'app-transactions',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, DatePipe],
  templateUrl: './transactions.html',
  styleUrl: './transactions.css',
})
export class Transactions implements OnInit {
  transactions: Transaction[] = [];
  categories: Category[] = [];
  isLoading = true;
  isSaving = false;
  isDeleting = false;

  currentYear = new Date().getFullYear();
  currentMonth = new Date().getMonth() + 1;

  months = [
    { value: 1, label: 'Tháng 1' }, { value: 2, label: 'Tháng 2' },
    { value: 3, label: 'Tháng 3' }, { value: 4, label: 'Tháng 4' },
    { value: 5, label: 'Tháng 5' }, { value: 6, label: 'Tháng 6' },
    { value: 7, label: 'Tháng 7' }, { value: 8, label: 'Tháng 8' },
    { value: 9, label: 'Tháng 9' }, { value: 10, label: 'Tháng 10' },
    { value: 11, label: 'Tháng 11' }, { value: 12, label: 'Tháng 12' },
  ];

  // Modal state
  showModal = false;
  modalMode: ModalMode = 'create';
  editingId: string | null = null;
  form: FormGroup;

  // Delete confirm
  showDeleteConfirm = false;
  deletingId: string | null = null;
  deletingTx: Transaction | null = null;

  // Notification toast
  toastMessage = '';
  toastType: 'success' | 'error' = 'success';
  showToast = false;

  constructor(
    private txService: TransactionService,
    private catService: CategoryService,
    private userService: UserService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      categoryId: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(1)]],
      date: [this.todayISO(), Validators.required],
      note: ['']
    });
  }

  ngOnInit(): void {
    this.loadTransactions();
    this.catService.getAll().subscribe(cats => {
      this.categories = cats;
      this.cdr.detectChanges();
    });
  }

  todayISO(): string {
    return new Date().toISOString().split('T')[0];
  }

  loadTransactions(): void {
    this.isLoading = true;
    this.txService.getByMonth(this.currentYear, this.currentMonth).subscribe({
      next: (data) => {
        this.transactions = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  onMonthChange(): void {
    this.loadTransactions();
  }

  // --- Modal ---
  openCreateModal(): void {
    this.modalMode = 'create';
    this.editingId = null;
    this.form.reset({ date: this.todayISO(), categoryId: '', amount: '', note: '' });
    this.showModal = true;
  }

  openEditModal(tx: Transaction): void {
    this.modalMode = 'edit';
    this.editingId = tx.id;
    this.form.patchValue({
      categoryId: tx.categoryId,
      amount: tx.amount,
      date: tx.date.split('T')[0],
      note: tx.note || ''
    });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.isSaving = true;

    const payload: CreateTransactionPayload = {
      categoryId: this.form.value.categoryId,
      amount: Number(this.form.value.amount),
      date: this.form.value.date,
      note: this.form.value.note || undefined
    };

    const request$ = this.modalMode === 'create'
      ? this.txService.create(payload)
      : this.txService.update(this.editingId!, payload);

    request$.subscribe({
      next: () => {
        this.isSaving = false;
        this.showModal = false;
        this.loadTransactions();
        this.userService.loadBalance(); // Cập nhật số dư sidebar ngay lập tức
        this.showNotification(
          this.modalMode === 'create' ? 'Thêm giao dịch thành công!' : 'Cập nhật giao dịch thành công!',
          'success'
        );
      },
      error: (err) => {
        this.isSaving = false;
        this.showNotification(err.error?.message || 'Có lỗi xảy ra!', 'error');
        this.cdr.detectChanges();
      }
    });
  }

  // --- Delete ---
  openDeleteConfirm(tx: Transaction): void {
    this.deletingId = tx.id;
    this.deletingTx = tx;
    this.showDeleteConfirm = true;
  }

  confirmDelete(): void {
    if (!this.deletingId) return;
    this.isDeleting = true;
    this.txService.delete(this.deletingId).subscribe({
      next: () => {
        this.isDeleting = false;
        this.showDeleteConfirm = false;
        this.loadTransactions();
        this.userService.loadBalance(); // Cập nhật số dư sidebar ngay lập tức
        this.showNotification('Xóa giao dịch thành công!', 'success');
      },
      error: () => {
        this.isDeleting = false;
        this.showNotification('Không thể xóa giao dịch.', 'error');
        this.cdr.detectChanges();
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
    this.deletingId = null;
    this.deletingTx = null;
  }

  // --- Helpers ---
  get incomeCategories(): Category[] {
    return this.categories.filter(c => c.type === 1);
  }
  get expenseCategories(): Category[] {
    return this.categories.filter(c => c.type === 2);
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount);
  }

  totalIncome(): number {
    return this.transactions.filter(t => t.type === 1).reduce((s, t) => s + t.amount, 0);
  }
  totalExpense(): number {
    return this.transactions.filter(t => t.type === 2).reduce((s, t) => s + t.amount, 0);
  }

  showNotification(message: string, type: 'success' | 'error'): void {
    this.toastMessage = message;
    this.toastType = type;
    this.showToast = true;
    this.cdr.detectChanges();
    setTimeout(() => { this.showToast = false; this.cdr.detectChanges(); }, 3000);
  }
}
