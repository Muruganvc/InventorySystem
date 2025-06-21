export interface UpdateUserRequest {
    firstName: string;
    lastName: string;
    emailId: string;
    mobileNumber: string;
    isActive: boolean;
    isSuperAdmin: boolean;
}


export interface ChangePasswordRequest {
    userName: string;
    passwordHash: string;
    currentPassword: string;
    email: string;
}