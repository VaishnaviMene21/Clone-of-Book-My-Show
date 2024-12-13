import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LocationComponent } from './location/location.component';
import { EventListComponent } from './event-list/event-list.component';
import { PaymentComponent } from './payment/payment.component';
import { TheaterComponent } from './theater/theater.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {path : 'register' , component: RegisterComponent},
  { path: 'eventlist', component: EventListComponent },
  {path : 'payment' , component :PaymentComponent },
  { path: 'location', component: LocationComponent },
  {path : 'theater' , component : TheaterComponent},
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
