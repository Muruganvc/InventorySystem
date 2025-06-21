import { TableRow } from "../../shared/common/TableRow";

export interface ProductResponse extends TableRow {
    productId?: string;
    productName: string;
    company: string;
    itemFullName: string;
    model: string;
    maximumRetailPrice: number;
    salesPrice: number;
    length: string;
    quantity: number;
    totalQuantity: number;
    purchaseDate?: Date;
    isWarranty: boolean;
}