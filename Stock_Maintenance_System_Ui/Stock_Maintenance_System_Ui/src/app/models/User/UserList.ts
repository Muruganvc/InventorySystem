import { TableRow } from "../../shared/common/TableRow";

export interface UserList extends TableRow {
    userId: number;
    userName: string;
    email: string;
    firstName: string;
    lastName: string;
    isActive: boolean;
    superAdmin: boolean;
    lastLogin: Date;
}
