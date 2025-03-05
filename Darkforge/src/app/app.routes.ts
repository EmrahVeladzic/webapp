import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { MainInterfaceComponent } from './main-interface/main-interface.component';

export const routes: Routes = [
    {path: '', redirectTo:'login',pathMatch:'full'},
    {path:'login',component:LoginComponent},
    {path:'user/:id',component:MainInterfaceComponent}

];

