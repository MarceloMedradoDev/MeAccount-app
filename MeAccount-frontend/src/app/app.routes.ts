import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { DashboardHomeComponent } from './features/dashboard/components/dashboard-home/dashboard-home.component';
import { ContasComponent } from './features/dashboard/components/contas/contas.component';
import { ContaFormComponent } from './features/dashboard/components/contas/conta-form.component';
import { GastosComponent } from './features/dashboard/components/gastos/gastos.component';
import { InvestimentosComponent } from './features/dashboard/components/investimentos/investimentos.component';
import { ProjecoesComponent } from './features/dashboard/components/projecoes/projecoes.component';
import { CategoriasComponent } from './features/dashboard/components/categorias/categorias.component';
import { CategoryFormComponent } from './features/dashboard/components/categorias/category-form.component';
import { TransacoesComponent } from './features/dashboard/components/transacoes/transacoes.component';
import { TransactionFormComponent } from './features/dashboard/components/transacoes/transaction-form.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
    children: [
      { path: '', component: DashboardHomeComponent },
      { path: 'contas', component: ContasComponent },
      { path: 'contas/new', component: ContaFormComponent },
      { path: 'contas/edit/:id', component: ContaFormComponent },
      { path: 'categorias', component: CategoriasComponent },
      { path: 'categorias/new', component: CategoryFormComponent },
      { path: 'categorias/edit/:id', component: CategoryFormComponent },
      { path: 'transacoes', component: TransacoesComponent },
      { path: 'transacoes/new', component: TransactionFormComponent },
      { path: 'transacoes/edit/:id', component: TransactionFormComponent },
      { path: 'gastos', component: GastosComponent },
      { path: 'investimentos', component: InvestimentosComponent },
      { path: 'projecoes', component: ProjecoesComponent }
    ]
  },
  { path: '**', redirectTo: '/login' }
];
