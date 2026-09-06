import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { finalize } from 'rxjs';
import { AuthService } from '../../../../core/services/auth.service';
import { AccountService } from '../../../../core/services/account.service';
import { TransactionService } from '../../../../core/services/transaction.service';
import { Transaction } from '../../../../core/models/transaction.model';
import { Account, ACCOUNT_TYPES } from '../../../../core/models/account.model';

@Component({
  selector: 'app-dashboard-home',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.css']
})
export class DashboardHomeComponent implements OnInit {
  userName = '';
  saldoTotal = 0;
  gastosMes = 0;
  receitasMes = 0;
  accounts: Account[] = [];
  ultimasTransacoes: Transaction[] = [];
  isLoading = true;

  selectedMonth: string = '';
  monthOptions: { value: string; label: string }[] = [];

  constructor(
    private authService: AuthService,
    private accountService: AccountService,
    private transactionService: TransactionService,
    private cdr: ChangeDetectorRef
  ) {
    const user = this.authService.getUser();
    this.userName = user?.fullName || 'Usuário';
    this.buildMonthOptions();
  }

  ngOnInit(): void {
    this.loadDashboardData();
  }

  private buildMonthOptions(): void {
    const now = new Date();
    const months = [
      'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
      'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
    ];

    this.monthOptions = [{ value: '', label: 'Todos os meses' }];
    this.selectedMonth = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`;

    for (let i = 0; i < 12; i++) {
      const d = new Date(now.getFullYear(), now.getMonth() - i, 1);
      const year = d.getFullYear();
      const month = d.getMonth();
      this.monthOptions.push({
        value: `${year}-${String(month + 1).padStart(2, '0')}`,
        label: `${months[month]} ${year}`
      });
    }
  }

  onMonthChange(event: Event): void {
    this.selectedMonth = (event.target as HTMLSelectElement).value;
    this.loadDashboardData();
  }

  private loadDashboardData(): void {
    this.isLoading = true;
    this.cdr.detectChanges();

    let startDate: Date;
    let endDate: Date;

    if (this.selectedMonth) {
      const [year, month] = this.selectedMonth.split('-').map(Number);
      startDate = new Date(year, month - 1, 1);
      endDate = new Date(year, month, 0, 23, 59, 59);
    } else {
      startDate = new Date(2020, 0, 1);
      endDate = new Date(2099, 11, 31, 23, 59, 59);
    }

    let completedCalls = 0;
    const totalCalls = 2;
    const markComplete = () => {
      completedCalls++;
      if (completedCalls === totalCalls) {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    };

    this.accountService.getAll().pipe(finalize(markComplete)).subscribe({
      next: (accounts) => {
        this.accounts = accounts;
        this.saldoTotal = accounts.reduce((sum, acc) => sum + acc.balance, 0);
      },
      error: (err) => {
        console.error('Erro ao carregar contas no dashboard', err);
      }
    });

    this.transactionService.getAll({ startDate, endDate }).pipe(finalize(markComplete)).subscribe({
      next: (transactions) => {
        this.gastosMes = transactions
          .filter(t => t.type === 'Expense')
          .reduce((sum, t) => sum + t.amount, 0);
        this.receitasMes = transactions
          .filter(t => t.type === 'Income')
          .reduce((sum, t) => sum + t.amount, 0);
        this.ultimasTransacoes = transactions
          .sort((a, b) => new Date(b.date).getTime() - new Date(a.date).getTime())
          .slice(0, 10);
      },
      error: (err) => {
        console.error('Erro ao carregar transações', err);
      }
    });
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('pt-BR');
  }

  getAccountTypeLabel(type: string): string {
    return ACCOUNT_TYPES.find(accountType => accountType.value === type)?.label ?? type;
  }
}
