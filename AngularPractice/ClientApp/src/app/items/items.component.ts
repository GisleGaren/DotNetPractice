import { Component, OnInit} from '@angular/core';
import { IItem } from "./item";

// If we want to style our components, we can simply reference a css file.
@Component({
  selector: 'app-items-component',
  templateUrl: './items.component.html',
  styleUrls: ["./items.component.css"]
})

  // OnInit is a lifecycle hook that is called after Angular has initialised all data-bound properties of
  // a directive. This hook is commonly used for tasks like initialization, setting up data, or making
  // API calls when the component is first created.
export class ItemsComponent implements OnInit {
  viewTitle: string = 'Table';
  displayImage: boolean = true;
  // listFilter: string = "";
  /* For now the content of the array item is any, because we don't care about the type. UPDATE, we have
  now defined an interface that defines what attributes the IItem objects should have in item.ts which acts
  as our model / class. */

  // Create a private attribute so that we need getters and setters to access the attribute
  private _listFilter: string = "";

  get listFilter(): string {
    return this._listFilter;
  }

  set listFilter(value: string) {
    this._listFilter = value;
    console.log("In setter:", value);
    // Below calls the performFilter() method everytime the input element bound to listFilter is changed
    this.filteredItems = this.performFilter(value);
  }

  items: IItem[] = [
    {
      "ItemId": 1,
      "Name": "Pizza",
      "Price": 150,
      "Description": "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
      "ImageUrl": "assets/images/pizza.jpg"
    },
    {
      "ItemId": 2,
      "Name": "Fried Chicken Leg",
      "Price": 20,
      "Description": "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
      "ImageUrl": "assets/images/chickenleg.jpg"
    }
  ];
  // filteredItems is of type interface IItem[] array object, which is initiliazed as the items list above.
  // This is an attribute in the ItemsComponent class that should be up there with viewTitle, displayImage
  // and _listFilter, but it needs to be farther down because the items array object is defined above.
  filteredItems: IItem[] = this.items;

  /* Everytime we type something in the input box, we end up calling the performFilter() method, which takes
     in the input as a String and returns an IItem[] object array. first, we reassign our input string
     filterBy to lowercase letters and then we return the items object array that we defined above where
     we call the filter method which iterates over each IItem object in the array and checks that
     each lowercase name property of each IItem object in the array contains the lowercase filterBy
     string. */
  performFilter(filterBy: string): IItem[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.items.filter((item: IItem) =>
      item.Name.toLocaleLowerCase().includes(filterBy));
  }

  // Method that inverts the boolean displayImage
  toggleImage(): void {
    this.displayImage = !this.displayImage;
  }
  ngOnInit(): void {
    console.log("ItemsComponent Created")
    // Once we launch the program and this component is generated, Angular automatically calls this method
    // and any content in here will be executed first.
  }
}
