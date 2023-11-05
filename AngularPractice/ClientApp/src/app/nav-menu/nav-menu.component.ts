import { Component } from '@angular/core';

/* This component decorator for the NavMenuComponent class provides the metadata about how the component should behave
   The selector defines the components selector and how it will be used in the HTML markup of the application.
   In this case, it uses the selector <app-nav-menu>.
   templateUrl specifies the HTML template file associated with this component, Angular will render the HTML
   defined in the specified HTML file when this component is used.
   styleUrls specifies an array of css files that are associated with this component.
*/
@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
/* Like in React, we need to export this component */
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
