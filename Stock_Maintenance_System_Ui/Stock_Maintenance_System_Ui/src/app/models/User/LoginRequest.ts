export interface LoginRequest {
    userName: string;
    password: string;
}

export interface LoginResponse {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
    userName: string;
    token: string;
}


export type MenuItem = {
    icon: string;
    label: string;
    route?: string;
    subMenuItem?: MenuItem[];
};