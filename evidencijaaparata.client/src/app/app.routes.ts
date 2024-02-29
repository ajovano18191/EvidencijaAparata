import { Route } from "@angular/router";
import { GmBaseComponent } from "./gm-base.component";
import { GmLocationComponent } from "./gm-location.component";

export const appRoutes: Route[] = [
  { path: 'gm-location', component: GmLocationComponent },
  { path: 'gm-base', component: GmBaseComponent },
  { path: '', redirectTo: 'gm-location', pathMatch: 'full' },
];
