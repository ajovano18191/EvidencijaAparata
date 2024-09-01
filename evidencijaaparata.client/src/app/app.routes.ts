import { Route } from "@angular/router";
import { GMBaseComponent } from "./gm-base.component";
import { GMLocationComponent } from "./gm-location.component";

export const appRoutes: Route[] = [
  { path: 'gm-location', component: GMLocationComponent },
  { path: 'gm-base', component: GMBaseComponent },
  { path: '', redirectTo: 'gm-location', pathMatch: 'full' },
];
