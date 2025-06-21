export interface UpdateProductRequest {
  productId: number;
  productName: string;
  companyId: number;
  categoryId: number;
  productCategoryId?: number;
  description?: string;
  mrp: number;
  salesPrice: number;
  totalQuantity: number;
  isActive: boolean;
  taxType?: string;       // default "GST"
  barCode?: string;       // default null
  brandName?: string;     // default null
  taxPercent?: number;           // default 18
}
