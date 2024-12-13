import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http : HttpClient) { }
  private apiUrl = 'https://localhost:7245/api/Auth';

  register(Formvalues:any)
  {
    return this.http.post(`${this.apiUrl}/register`,Formvalues)
  }

  login(Formvalues:any)
  {
    return this.http.post(`${this.apiUrl}/login`,Formvalues)
    
  }

  updateLocation(username: string, location: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/update-location`, { username, location });
  }

  
  getUserLocation(username: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/get-location/${username}`);
  }
  getUserEvents(location: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/events?location=${location}`)
      
  }

 
}
