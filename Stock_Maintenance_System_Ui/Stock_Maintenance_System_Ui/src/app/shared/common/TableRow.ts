export interface TableRow {
  id: string | number;
  [key: string]: any;
}

export interface TableColumn {
  key: string;
  label: string;
  align?: 'left' | 'center' | 'right';
  type?: string;
  isHidden: boolean;
}