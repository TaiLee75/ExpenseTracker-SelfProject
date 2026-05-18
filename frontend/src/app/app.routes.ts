import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Layout } from './shared/components/layout/layout';
import { authGuard } from './core/guards/auth.guard';
import { Dashboard } from './features/dashboard/dashboard';
import { Transactions } from './features/transactions/transactions';
import { Categories } from './features/categories/categories';

export const routes: Routes = [
  { path: 'login', component: Login },
  { 
    path: '', 
    component: Layout,
    canActivate: [authGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: Dashboard },
      { path: 'transactions', component: Transactions },
      { path: 'categories', component: Categories },
    ]
  },
  { path: '**', redirectTo: '' }
];
