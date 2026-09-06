import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { AccountService } from '../../../../core/services/account.service';
import { ACCOUNT_TYPES } from '../../../../core/models/account.model';

@Component({
  selector: 'app-conta-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './conta-form.component.html',
  styleUrls: ['./conta-form.component.css']
})
export class ContaFormComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  isEditing = false;
  errorMessage = '';
  accountId: string | null = null;

  accountTypes = ACCOUNT_TYPES;

  constructor(
    private fb: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      type: ['Checking', Validators.required],
      initialBalance: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.accountId = this.route.snapshot.paramMap.get('id');
    if (this.accountId) {
      this.isEditing = true;
      this.form.get('initialBalance')?.clearValidators();
      this.form.get('initialBalance')?.updateValueAndValidity();
      this.loadAccount();
    }
  }

  loadAccount(): void {
    if (!this.accountId) return;

    this.accountService.getById(this.accountId).subscribe({
      next: (account) => {
        this.form.patchValue({
          name: account.name,
          type: account.type
        });
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar conta.';
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

    if (this.isEditing && this.accountId) {
      const request = {
        name: this.form.value.name,
        type: this.form.value.type
      };
      this.accountService.update(this.accountId, request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/contas']);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao atualizar conta.';
        }
      });
    } else {
      const request = {
        name: this.form.value.name,
        type: this.form.value.type,
        initialBalance: this.form.value.initialBalance
      };
      this.accountService.create(request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/contas']);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao criar conta.';
        }
      });
    }
  }
}
