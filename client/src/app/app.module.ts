import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
// NEEDED TO HIT API ENDPOINTS
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    // NEEDED TO HIT API ENDPOINTS
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
