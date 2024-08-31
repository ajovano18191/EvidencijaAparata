import { Route } from "@angular/router";
import { GMBaseService } from "./gm-base.service";
import { GMLocationService } from "./gm-location.service";

export const appRoutes: Route[] = [
  { path: 'gm-location', component: GMLocationService },
  { path: 'gm-base', component: GMBaseService },
  { path: '', redirectTo: 'gm-location', pathMatch: 'full' },
];
