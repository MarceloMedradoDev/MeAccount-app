export interface Category {
  id: string;
  name: string;
  icon: string;
  color: string;
  type: 'Income' | 'Expense';
  createdAt: Date;
}

export interface CreateCategoryRequest {
  name: string;
  icon: string;
  color: string;
  type: 'Income' | 'Expense';
}

export interface UpdateCategoryRequest {
  name: string;
  icon: string;
  color: string;
}
