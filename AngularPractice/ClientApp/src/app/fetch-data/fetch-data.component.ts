/* Imports the component decorator and inject for dependency injections, which is what we use on on the
   constructor*/
import { Component, Inject } from '@angular/core';
/* Now we also need to import HttpClient in order to add that into the constructor as well so that
   we can make HTTP requests to the server. */
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  /* Below is a class constructor that takes in http, which is an instance of HttpClient as a parameter
     and the class is what we export to the html part of the component.
     The first argument injects http into the component and the other one uses the @Inject decorator to
     inject a value of 'BASE_URL' into the component. The BASE_URL is typically found in the configuration
     page of Angular, and we need this to specify the URL of the Server or it could be the URL of an API. */
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    /* This makes an HTTP get request by concatenating the baseurl with the endpoint 'weatherforecast'
       so we request data from the server. The subscribe method listens for two different events, when
       successful, we get a JSON object back (observable in Angular) which is found in result, if not
       successful, we throw an error exception, which is captured in the error object. */
    http.get<WeatherForecast[]>(baseUrl + 'weatherforecast').subscribe(result => {
      this.forecasts = result;
    }, error => console.error(error));
  }
}
/* This is the interface so that we can define what WeatherForecast should contain as attributes.
   Once we have made the GET request, we call the WeatherForecast model in the backend, which calls
   the WeatherForecastController and this generates the data which has four attributes per weather object. */

    
interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
