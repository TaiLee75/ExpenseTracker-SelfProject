import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService, Category, CategoryPayload } from '../../core/services/category/category.service';

type ModalMode = 'create' | 'edit';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './categories.html',
  styleUrl: './categories.css',
})
export class Categories implements OnInit {
  categories: Category[] = [];
  isLoading = true;
  isSaving = false;
  isDeleting = false;

  showModal = false;
  modalMode: ModalMode = 'create';
  editingId: string | null = null;
  form: FormGroup;

  showDeleteConfirm = false;
  deletingId: string | null = null;
  deletingCategory: Category | null = null;

  toastMessage = '';
  toastType: 'success' | 'error' = 'success';
  showToast = false;

  colorPresets = [
    '#3b82f6', '#8b5cf6', '#10b981', '#ef4444',
    '#f59e0b', '#06b6d4', '#ec4899', '#84cc16'
  ];

  iconPresets = [
    '💰', '🏠', '🍔', '🚗', '👕', '📱', '💊', '🎓',
    '✈️', '🎬', '🎮', '🛒', '💳', '📊', '🏋️', '🎁'
  ];

  constructor(
    private catService: CategoryService,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(100)]],
      type: ['1', Validators.required],  // String để match HTML select
      icon: ['💳'],
      color: ['#3b82f6']
    });
  }

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.catService.getAll().subscribe({
      next: (data) => {
        this.categories = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  get incomeCategories(): Category[] {
    return this.categories.filter(c => c.type === 1);
  }

  get expenseCategories(): Category[] {
    return this.categories.filter(c => c.type === 2);
  }

  openCreateModal(): void {
    this.modalMode = 'create';
    this.editingId = null;
    this.form.reset({ name: '', type: '1', icon: '💳', color: '#3b82f6' });
    this.showModal = true;
  }

  openEditModal(cat: Category): void {
    this.modalMode = 'edit';
    this.editingId = cat.id;
    this.form.patchValue({
      name: cat.name,
      type: cat.type.toString(), // Convert number -> string để select nhận đúng
      icon: cat.icon || '💳',
      color: cat.color || '#3b82f6'
    });
    this.showModal = true;
  }

  closeModal(): void {
    this.showModal = false;
  }

  selectIcon(icon: string): void {
    this.form.patchValue({ icon });
  }

  selectColor(color: string): void {
    this.form.patchValue({ color });
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.isSaving = true;

    const payload: CategoryPayload = {
      name: this.form.value.name,
      type: Number(this.form.value.type),
      icon: this.form.value.icon,
      color: this.form.value.color
    };

    const request$ = this.modalMode === 'create'
      ? this.catService.create(payload)
      : this.catService.update(this.editingId!, payload);

    request$.subscribe({
      next: () => {
        this.isSaving = false;
        this.showModal = false;
        this.loadCategories();
        this.showNotification(
          this.modalMode === 'create' ? 'Thêm danh mục thành công!' : 'Cập nhật danh mục thành công!',
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

  openDeleteConfirm(cat: Category): void {
    this.deletingId = cat.id;
    this.deletingCategory = cat;
    this.showDeleteConfirm = true;
  }

  confirmDelete(): void {
    if (!this.deletingId) return;
    this.isDeleting = true;
    this.catService.delete(this.deletingId).subscribe({
      next: () => {
        this.isDeleting = false;
        this.showDeleteConfirm = false;
        this.loadCategories();
        this.showNotification('Xóa danh mục thành công!', 'success');
      },
      error: () => {
        this.isDeleting = false;
        this.showNotification('Không thể xóa. Danh mục có thể đang được sử dụng.', 'error');
        this.cdr.detectChanges();
      }
    });
  }

  cancelDelete(): void {
    this.showDeleteConfirm = false;
  }

  showNotification(message: string, type: 'success' | 'error'): void {
    this.toastMessage = message;
    this.toastType = type;
    this.showToast = true;
    this.cdr.detectChanges();
    setTimeout(() => { this.showToast = false; this.cdr.detectChanges(); }, 3000);
  }
}
