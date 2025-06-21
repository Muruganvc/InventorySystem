import { TableRow } from "../../shared/common/TableRow";


export interface ProductsResponse extends TableRow {
    productId: number;
    productName: string;
    productCategoryId?: number;
    productCategoryName?: string;
    categoryId: number;
    categoryName: string;
    companyId: number;
    companyName: string;
    description?: string;
    mrp: number;
    salesPrice: number;
    quantity: number;
    taxPercent: number;
    taxType?: string;
    barcode?: string;
    brandName?: string;
    isActive: boolean;
    userName?: string;
}