import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { CategoryService } from '../../../../core/services/category.service';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.css']
})
export class CategoryFormComponent implements OnInit {
  form: FormGroup;
  isLoading = false;
  isEditing = false;
  errorMessage = '';
  categoryId: string | null = null;

  predefinedIcons = [
    '🍔', '🛒', '🚗', '🏠', '💡', '🎮', '📚', '💰',
    '💼', '🎁', '🔄', '📱', '🎬', '✈️', '🏥', '🎓'
  ];

  predefinedColors = [
    '#EF4444', '#F97316', '#EAB308', '#22C55E',
    '#14B8A6', '#3B82F6', '#6366F1', '#A855F7'
  ];

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      icon: ['📦', Validators.required],
      color: ['#3B82F6', Validators.required],
      type: ['Expense', Validators.required]
    });
  }

  ngOnInit(): void {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    if (this.categoryId) {
      this.isEditing = true;
      this.loadCategory();
    }
  }

  loadCategory(): void {
    if (!this.categoryId) return;

    this.categoryService.getById(this.categoryId).subscribe({
      next: (category) => {
        this.form.patchValue({
          name: category.name,
          icon: category.icon,
          color: category.color,
          type: category.type
        });
      },
      error: () => {
        this.errorMessage = 'Erro ao carregar categoria.';
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

    const request = this.form.value;

    if (this.isEditing && this.categoryId) {
      this.categoryService.update(this.categoryId, request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/categorias']);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao atualizar categoria.';
        }
      });
    } else {
      this.categoryService.create(request).subscribe({
        next: () => {
          this.router.navigate(['/dashboard/categorias']);
        },
        error: (error) => {
          this.isLoading = false;
          this.errorMessage = error.error?.message || 'Erro ao criar categoria.';
        }
      });
    }
  }

  selectIcon(icon: string): void {
    this.form.patchValue({ icon });
  }

  selectColor(color: string): void {
    this.form.patchValue({ color });
  }
}
