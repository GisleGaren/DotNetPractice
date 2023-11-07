import { Pipe, PipeTransform } from "@angular/core";

/* A decorator that marks the class ConvertToCurrency as a pipe and assigns it a unique name
   "convertToCurrency". This name is used to invoke the pipe in the templates. */
@Pipe({
  name: 'convertToCurrency'
})

/* Defines the ConvertToCurrency class which implements PipeTransform interface, which forces us to
   use the transform() method. In the class, the method takes two input arguments and returns a string
   that concatenates the input arguments after converting the number to a string. Remember to implement
   this in the app.module.ts! */
export class ConvertToCurrency implements PipeTransform {
  transform(value: number, character: string): string {
    return character + ' ' + value.toString();
  }
}
