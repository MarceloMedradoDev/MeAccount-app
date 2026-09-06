import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { TransactionService } from '../../../../core/services/transaction.service';
import { CategoryService } from '../../../../core/services/category.service';
import { AccountService } from '../../../../core/services/account.service';
import { Category } from '../../../../core/models/category.model';
import { Account } from '../../../../core/models/account.model';

@Component({
  selector: 'app-transaction-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink, CurrencyPipe],
  templateUrl: './transaction-form.component.html',
  styleUrls: ['./transaction-form.component.css']
})
export class TransactionFormComponent implements OnInit {
  form: FormGroup;
  categories: Category[] = [];
  accounts: Account[] = [];
  isLoading = false;
  isEditing = false;
  errorMessage = '';
  transactionId: string | null = null;

  constructor(
    private fb: FormBuilder,
    private transactionService: TransactionService,
    private categoryService: CategoryService,
    private accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {
    this.form = this.fb.group({
      accountId: ['', Validators.required],
      description: ['', [Validators.required, Validators.minLength(2)]],
      amount: [0, [Validators.required, Validators.min(0.01)]],
      type: ['Expense', Validators.required],
      date: [new Date().toISOString().split('T')[0], Validators.required],
      categoryId: [''],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadAccounts();
    this.loadCategories();
    this.transactionId = this.route.snapshot.paramMap.get('id');
    if (this.transactionId) {
      this.isEditing = true;
      this.loadTransaction();
    }
  }

  loadAccounts(): void {
    this.accountService.getAll().subscribe({
      next: (accounts: Account[]) => {
        this.accounts = accounts;
        this.cdr.detectChanges();
      },
      error: (_error: any) => {
        console.error('Erro ao carregar contas');
      }
    });
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (categories: Category[]) => {
        this.categories = categories;
        this.cdr.detectChanges();
      },
      error: (_error: any) => {
        console.error('Erro ao carregar categorias');
      }
    });
  }

  loadTransaction(): void {
    if (!this.transactionId) return;

    this.transactionService.getById(this.transactionId).subscribe({
      next: (transaction: any) => {
        this.form.patchValue({
          accountId: transaction.accountId || '',
          description: transaction.description,
          amount: transaction.amount,
          type: transaction.type,
          date: new Date(transaction.date).toISOString().split('T')[0],
          categoryId: transaction.categoryId || '',
          notes: transaction.notes || ''
        });
        this.cdr.detectChanges();
      },
      error: (_error: any) => {
        this.errorMessage = 'Erro ao carregar transação.';
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const formValue = this.form.value;
    const request = {
      ...formValue,
      date: new Date(formValue.date),
      categoryId: formValue.categoryId || null
    };

    if (this.isEditing && this.transactionId) {
      this.transactionService.update(this.transactionId, request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/transacoes']);
        },
        error: (error: any) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao atualizar transação.';
        }
      });
    } else {
      this.transactionService.create(request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/transacoes']);
        },
        error: (error: any) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao criar transação.';
        }
      });
    }
  }

  get filteredCategories(): Category[] {
    const type = this.form.get('type')?.value;
    return this.categories.filter(c => c.type === type);
  }
}
