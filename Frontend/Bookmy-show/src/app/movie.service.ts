import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
   apiUrl = 'https://localhost:7245/api/Movies/get-movies'

  constructor(private http : HttpClient) { }

  
  getMoviesByLocation(location:string): Observable<any[]>
  {
    const paymentData = {
      location:location
    }
    this.apiUrl = this.apiUrl
    return this.http.post<any[]>(`${this.apiUrl}`,paymentData);
  }
}
