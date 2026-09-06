import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { AccountService } from '../../../../core/services/account.service';
import { Account } from '../../../../core/models/account.model';

@Component({
  selector: 'app-contas',
  standalone: true,
  imports: [RouterLink, CurrencyPipe],
  templateUrl: './contas.component.html',
  styleUrls: ['./contas.component.css']
})
export class ContasComponent implements OnInit {
  accounts: Account[] = [];
  isLoading = true;
  errorMessage = '';

  filterType: 'all' | 'Checking' | 'Savings' | 'Wallet' | 'Investment' | 'CreditCard' = 'all';

  constructor(private accountService: AccountService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadAccounts();
  }

  loadAccounts(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.accountService.getAll().subscribe({
      next: (accounts) => {
        this.accounts = accounts;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('[Contas] Erro:', error);
        this.isLoading = false;
        if (error.status === 0 || error.status === 502) {
          this.errorMessage = 'Servidor indisponível. Verifique se o backend está rodando.';
        } else {
          this.errorMessage = 'Erro ao carregar contas.';
        }
        this.cdr.detectChanges();
      }
    });
  }

  getAccountTypeLabel(type: string): string {
    const labels: Record<string, string> = {
      'Checking': 'Conta Corrente',
      'Savings': 'Poupança',
      'Wallet': 'Carteira',
      'Investment': 'Investimento',
      'CreditCard': 'Cartão de Crédito'
    };
    return labels[type] || type;
  }

  deleteAccount(id: string): void {
    if (confirm('Tem certeza que deseja excluir esta conta?')) {
      this.accountService.delete(id).subscribe({
        next: () => {
          this.accounts = this.accounts.filter(a => a.id !== id);
        },
        error: () => {
          this.errorMessage = 'Erro ao excluir conta.';
        }
      });
    }
  }

  onFilterChange(type: 'all' | 'Checking' | 'Savings' | 'Wallet' | 'Investment' | 'CreditCard'): void {
    this.filterType = type;
  }

  get filteredAccounts(): Account[] {
    if (this.filterType === 'all') {
      return this.accounts;
    }
    return this.accounts.filter(a => a.type === this.filterType);
  }
}
