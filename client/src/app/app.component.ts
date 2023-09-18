import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  // PROPERTIES
  title = 'Irrigation Management System';
  users: any;
  plants: any;
  // CONSTRUCTOR - this is needed to hit http endpoints
  constructor(private http: HttpClient) { }
  // METHODS
  ngOnInit(): void {
    this.http.get('https://localhost:5555/api/plants').subscribe({
      next: response => this.plants = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    })
  }

}
