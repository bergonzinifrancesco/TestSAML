import { Routes } from '@angular/router';
import { AccountInfoComponent } from './account-info/account-info.component';
import { EmptyPageComponent } from './empty-page/empty-page.component';

export const routes: Routes = [
  {path: 'info', component: AccountInfoComponent},
  {path: '', component: EmptyPageComponent},
];
