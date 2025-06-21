import { TableRow } from "../../shared/common/TableRow";

export interface UserCreateRequest extends TableRow {
    userId?: string;
    firstName: string;
    lastName: string;
    userName: string;
    emailId: string;
    mobileNumber: string;
    password: string;
    role: string;
    isActive: boolean
    superAdmin: boolean;
}