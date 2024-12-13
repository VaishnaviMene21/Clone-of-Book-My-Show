import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

interface Event {
  title: string;
  image: string;
  time: string;
  genre: string;
}

@Component({
  selector: 'app-location',
  templateUrl: './location.component.html',
  styleUrls: ['./location.component.css']
})
export class LocationComponent implements OnInit {
  locations: string[] = ['Mumbai', 'Pune', 'Hyderabad', 'Chennai', 'Bangalore'];
  selectedLocation: string = '';
  events: Event[] = [];
  username: string = '' ;
  successMessage: string = '';

  
  // eventsMap: { [key: string]: Event[] } = {
  //   Mumbai: [
  //     { title: 'Mumbai Movie 1', image: 'path_to_mumbai_image1.jpg', time: '10:00 AM', genre: 'Action' },
  //     { title: 'Mumbai Movie 2', image: 'path_to_mumbai_image2.jpg', time: '2:00 PM', genre: 'Drama' }
  //   ],
  //   Pune: [
  //     { title: 'Pune Movie 1', image: 'path_to_pune_image1.jpg', time: '11:00 AM', genre: 'Comedy' },
  //     { title: 'Pune Movie 2', image: 'path_to_pune_image2.jpg', time: '3:00 PM', genre: 'Thriller' }
  //   ],
  //   Hyderabad: [
  //     { title: 'Pune Movie 1', image: 'path_to_pune_image1.jpg', time: '11:00 AM', genre: 'Comedy' },
  //     { title: 'Pune Movie 2', image: 'path_to_pune_image2.jpg', time: '3:00 PM', genre: 'Thriller' }
  //   ],
  //   Bangalore: [
  //     { title: 'Pune Movie 1', image: 'path_to_pune_image1.jpg', time: '11:00 AM', genre: 'Comedy' },
  //     { title: 'Pune Movie 2', image: 'path_to_pune_image2.jpg', time: '3:00 PM', genre: 'Thriller' }
  //   ],
  //   Chennai: [
  //     { title: 'Pune Movie 1', image: 'path_to_pune_image1.jpg', time: '11:00 AM', genre: 'Comedy' },
  //     { title: 'Pune Movie 2', image: 'path_to_pune_image2.jpg', time: '3:00 PM', genre: 'Thriller' }
  //   ],
   
  // };

  constructor(private authService: AuthService ,private router: Router) {}

  ngOnInit(): void {
    this.username  = localStorage.getItem('username') || '';
    localStorage.setItem('selectedLocation', this.selectedLocation);
  
    this.checkUserLocation();
  }

  checkUserLocation() {
    if (this.username) {
      this.authService.getUserLocation(this.username).subscribe(location => {
        if (location) {
          this.selectedLocation = location;
          // this.events = this.eventsMap[location] || [];
        }
      });
    }
  }

  submitLocation() {
    if (this.selectedLocation && this.username) {
      this.authService.updateLocation(this.username, this.selectedLocation).subscribe(
        response => {
          this.successMessage = `Location updated to ${this.selectedLocation}`;
          // this.events = this.eventsMap[this.selectedLocation] || [];
          localStorage.setItem('selectedLocation', this.selectedLocation);
          this.router.navigate(['/eventlist'], { queryParams: { location: this.selectedLocation } });
        },
        error => {
          console.error('Error updating location', error);
        }
      );
    }
  }
}
