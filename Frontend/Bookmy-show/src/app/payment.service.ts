
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:7245/api/Payment'; 

  constructor(private http: HttpClient) {}

  storePayment(paymentData: any): Observable<any> {
   
    return this.http.post<any>(this.apiUrl, paymentData);
  }
}
