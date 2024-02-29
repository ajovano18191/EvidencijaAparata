import { Route } from "@angular/router";
import { GmBaseComponent } from "./gm-base.component";

export const appRoutes: Route[] = [
  { path: 'gm-base', component: GmBaseComponent },
  { path: '', redirectTo: 'gm-base', pathMatch: 'full' },
];
