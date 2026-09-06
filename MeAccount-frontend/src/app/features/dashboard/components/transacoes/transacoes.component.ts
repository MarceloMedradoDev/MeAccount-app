import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TransactionService } from '../../../../core/services/transaction.service';
import { Transaction } from '../../../../core/models/transaction.model';

@Component({
  selector: 'app-transacoes',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './transacoes.component.html',
  styleUrls: ['./transacoes.component.css']
})
export class TransacoesComponent implements OnInit {
  transactions: Transaction[] = [];
  isLoading = true;
  errorMessage = '';

  filterType: 'all' | 'Income' | 'Expense' | 'Transfer' = 'all';
  startDate: string = '';
  endDate: string = '';

  constructor(private transactionService: TransactionService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const filters: { startDate?: Date; endDate?: Date; accountId?: string; categoryId?: string; type?: string } = {};

    if (this.filterType !== 'all') {
      filters.type = this.filterType;
    }
    if (this.startDate) {
      filters.startDate = new Date(this.startDate);
    }
    if (this.endDate) {
      filters.endDate = new Date(this.endDate);
    }

    this.transactionService.getAll(filters).subscribe({
      next: (transactions) => {
        this.transactions = transactions;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.isLoading = false;
        console.error('[TransacoesComponent] Erro ao carregar transações:', error);
        if (error.status === 0 || error.status === 502) {
          this.errorMessage = 'Servidor indisponível. Verifique se o backend está rodando.';
        } else {
          this.errorMessage = error.error?.message || 'Erro ao carregar transações.';
        }
        this.cdr.detectChanges();
      }
    });
  }

  deleteTransaction(id: string): void {
    if (confirm('Tem certeza que deseja excluir esta transação?')) {
      this.transactionService.delete(id).subscribe({
        next: () => {
          this.transactions = this.transactions.filter(t => t.id !== id);
        },
        error: () => {
          this.errorMessage = 'Erro ao excluir transação.';
        }
      });
    }
  }

  onFilterChange(): void {
    this.loadTransactions();
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('pt-BR');
  }

  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(amount);
  }
}
