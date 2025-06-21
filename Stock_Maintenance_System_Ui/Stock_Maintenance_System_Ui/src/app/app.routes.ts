import { Routes } from '@angular/router';
import { Login } from './components/login/login';
import { Layout } from './components/layout/layout';
import { AccessDenied } from './shared/components/access-denied/access-denied';
import { Dashboard } from './components/dashboard/dashboard';
import { ProductList } from './components/product/product-list/product-list';
import { Product } from './components/product/product';
import { Settings } from './components/settings/settings';
import { PasswordChange } from './components/settings/password-change/password-change';
import { UserPermission } from './components/settings/user-permission/user-permission';
import { NewUser } from './components/settings/new-user/new-user';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: Login },
    {
        path: '', component: Layout,
        children: [
            { path: 'dashboard', component: Dashboard },
            { path: 'product-list', component: ProductList },
            { path: 'new-product', component: Product },
            {
                path: 'settings', component: Settings, children: [
                    { path: '', redirectTo: 'change-password', pathMatch: 'full' },
                    { path: 'change-password', component: PasswordChange },
                    { path: 'user-permission', component: UserPermission },
                    { path: 'new-user', component: NewUser },
                ]
            },
        ]
    },
    { path: 'access-denied', component: AccessDenied },
    { path: '**', redirectTo: 'access-denied' }
];
