import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CategoryService } from '../../../../core/services/category.service';
import { Category } from '../../../../core/models/category.model';

@Component({
  selector: 'app-categorias',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './categorias.component.html',
  styleUrls: ['./categorias.component.css']
})
export class CategoriasComponent implements OnInit {
  categories: Category[] = [];
  isLoading = true;
  errorMessage = '';
  filterType: 'all' | 'Income' | 'Expense' = 'all';

  constructor(private categoryService: CategoryService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    const type = this.filterType === 'all' ? undefined : this.filterType;

    this.categoryService.getAll(type).subscribe({
      next: (categories) => {
        console.log('[Categorias] Recebidas:', categories.length, 'categorias');
        this.categories = categories;
        this.isLoading = false;
        console.log('[Categorias] isLoading:', this.isLoading);
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('[Categorias] Erro:', error);
        this.isLoading = false;
        if (error.status === 0 || error.status === 502) {
          this.errorMessage = 'Servidor indisponível. Verifique se o backend está rodando.';
        } else {
          this.errorMessage = 'Erro ao carregar categorias.';
        }
        this.cdr.detectChanges();
      }
    });
  }

  deleteCategory(id: string): void {
    if (confirm('Tem certeza que deseja excluir esta categoria?')) {
      this.categoryService.delete(id).subscribe({
        next: () => {
          this.categories = this.categories.filter(c => c.id !== id);
        },
        error: () => {
          this.errorMessage = 'Erro ao excluir categoria.';
        }
      });
    }
  }

  onFilterChange(type: 'all' | 'Income' | 'Expense'): void {
    this.filterType = type;
    this.loadCategories();
  }
}
