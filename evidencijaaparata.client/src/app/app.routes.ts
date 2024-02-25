import { Route } from "@angular/router";
import { AppComponent } from "./app.component";
import { GmBaseComponent } from "./gm-base.component";

export const appRoutes: Route[] = [
  { path: '', component: GmBaseComponent },
];
