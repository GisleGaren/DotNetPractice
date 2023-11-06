import { Component } from '@angular/core';

@Component({
  selector: 'app-items-component',
  templateUrl: './items.component.html',
})

export class ItemsComponent {
  viewTitle: string = 'Table';
  displayImage: boolean = true;
  // For now the content of the array item is any, because we don't care about the type
  items: any[] = [
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

  // Method that inverts the boolean displayImage
  toggleImage(): void {
    this.displayImage = !this.displayImage;
  }
}
