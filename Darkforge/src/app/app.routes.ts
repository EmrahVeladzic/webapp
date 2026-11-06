import { Routes } from '@angular/router';
import { MainInterfaceComponent } from './main-interface/main-interface.component';

export const routes: Routes = [
    {path: '', component: MainInterfaceComponent},
    {path: '**', redirectTo: ''}
];