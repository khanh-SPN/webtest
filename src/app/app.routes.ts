import { Routes } from '@angular/router';

import { HarvestAdd33603294 } from './harvest-add-33603294/harvest-add-33603294';
import { HarvestList33603294 } from './harvest-list-33603294/harvest-list-33603294';

export const routes: Routes = [
  { path: '', redirectTo: '33603294/harvests', pathMatch: 'full' },
  { path: '33603294/harvests', component: HarvestList33603294 },
  { path: '33603294/harvests/new', component: HarvestAdd33603294 },
];
