export interface Transaction {
  id: string;
  accountId: string;
  accountName: string;
  categoryId: string | null;
  categoryName: string | null;
  type: 'Income' | 'Expense' | 'Transfer';
  amount: number;
  currency: string;
  date: Date;
  description: string;
  notes: string | null;
  createdAt: Date;
}

export interface CreateTransactionRequest {
  accountId: string;
  categoryId: string | null;
  type: 'Income' | 'Expense' | 'Transfer';
  amount: number;
  date: Date;
  description: string;
  notes: string | null;
}

export interface UpdateTransactionRequest {
  categoryId: string | null;
  type: 'Income' | 'Expense' | 'Transfer';
  amount: number;
  date: Date;
  description: string;
  notes: string | null;
}
