import { NgModule  } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { LocationComponent } from './location/location.component';
import { EventListComponent } from './event-list/event-list.component';
import { PaymentComponent } from './payment/payment.component';
import { QRCodeModule } from 'angularx-qrcode';
import { TheaterComponent } from './theater/theater.component';
import { RegisterComponent } from './register/register.component';
// import { NgxQRCodeModule } from 'ngx-qrcode2';
@NgModule({
 
  declarations: [
    AppComponent,
    LoginComponent,
    LocationComponent,
    EventListComponent,
    PaymentComponent,
    TheaterComponent,
    RegisterComponent,
  
   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    QRCodeModule
    // NgxQRCodeModule
    //NgbModule
    
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
 