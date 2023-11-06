// Modules that are dependency packages that we inject into the components
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// Template Resolution
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ItemsComponent } from './items/items.component';

/* Is responsible for organisation of modules and components and the modules are the dependent packages.
   Components are the reusable UI elements like partial views. Template resolution is about finding the
   right path to the templates. */

// Components declaration
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ItemsComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    // Route mapping
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'items', component: ItemsComponent },
    ])
  ],
  providers: [],
  // Below indicates that it is the root component app-root
  bootstrap: [AppComponent]
})
export class AppModule { }
