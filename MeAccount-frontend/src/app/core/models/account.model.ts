export interface Account {
  id: string;
  name: string;
  type: string;
  balance: number;
  currency: string;
}

export interface CreateAccountRequest {
  name: string;
  type: string;
  initialBalance: number;
}

export interface UpdateAccountRequest {
  name: string;
  type: string;
}

export const ACCOUNT_TYPES = [
  { value: 'Checking', label: 'Conta Corrente' },
  { value: 'Savings', label: 'Poupança' },
  { value: 'Wallet', label: 'Carteira' },
  { value: 'Investment', label: 'Investimento' },
  { value: 'CreditCard', label: 'Cartão de Crédito' }
] as const;
