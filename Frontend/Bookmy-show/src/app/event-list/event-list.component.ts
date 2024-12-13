import { Component ,OnInit} from '@angular/core';
import { ActivatedRoute,  Router } from '@angular/router';
import { MovieService } from '../movie.service';

interface Event {
  title: string;
  image: string;
  time: string;
  genre: string;
}



@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {
  events: Event[] = [];
  selectedLocation: string = '';
  showPopup = false;
  selectedSeatsCount: number = 0; 
  selectedSeats: number[] = []; 
  maxSeats: number = 5;
  seatPrice: number = 250;
  seats: boolean[] = Array(20).fill(false); 
  selectedMovie : Event | null = null;

  // eventsMap: { [key: string]: Event[] } = {
    
  //   Mumbai: [
  //     { title: 'Yeh Jawani Hai deewani', image: 'https://th.bing.com/th/id/OIP.mDbFzSgS4eLougOA1r4j_AHaEK?w=322&h=181&c=7&r=0&o=5&dpr=1.3&pid=1.7', time: '10:00 AM', genre: 'Drama' },
  //     { title: 'Kuch Kuch hota hai', image: 'https://wallpapercave.com/wp/wp7489682.jpg', time: '2:00 PM', genre: 'Drama' }
  //   ],
  //   Pune: [
  //     { title: 'Bucket List', image: 'https://th.bing.com/th/id/OIP.ugOENtNuQuumwXSF2UIahwHaEK?rs=1&pid=ImgDetMain', time: '11:00 AM', genre: 'Comedy' },
  //     { title: 'Duniyadari', image: 'https://th.bing.com/th/id/OIP.HTOvq7FCN2yQJRx4zIY2xQHaEK?w=324&h=183&c=7&r=0&o=5&dpr=1.3&pid=1.7', time: '3:00 PM', genre: 'Drama' }
  //   ],
  //   Hyderabad: [
  //     { title: 'Devara', image: 'http://ts1.mm.bing.net/th?id=OIP.cPgEhAapSRWwEH97BHH7rgHaEK&pid=15.1', time: '11:00 AM', genre: 'Action' },
  //     { title: 'Maharshi', image: 'https://www.25cineframes.com/images/gallery/2018/08/mahesh-babu-ssmb25-movie-first-look-all-ultra-hd-posters-wallpapers/20-Mahesh-Babu-SSMB25-Movie-First-Look-ALL-ULTRA-HD-Posters-WallPapers.jpg', time: '3:00 PM', genre: 'Drama' }
  //   ],
  
  //   Chennai: [
  //     { title: 'Maharaja', image: 'https://img.onmanorama.com/content/dam/mm/en/entertainment/movie-reviews/images/2024/6/14/maharaja.jpg', time: '11:00 AM', genre: 'Action' },
  //     { title: '24', image: 'https://wallpapercave.com/wp/wp6579717.jpg', time: '3:00 PM', genre: 'Adventure' }
  //   ],
  //     Bangalore: [
  //     { title: 'Kantara', image: 'https://th.bing.com/th/id/OIP.9BF46wKerXkvGsM8Szb-PwHaEK?w=300&h=180&c=7&r=0&o=5&dpr=1.3&pid=1.7', time: '3:00 PM', genre: 'Action' },
  //     { title: 'K.G.F', image: 'https://th.bing.com/th/id/OIP.kp469KOO8bfEtuLUQK5lugHaJQ?w=208&h=260&c=7&r=0&o=5&dpr=1.3&pid=1.7', time: '3:00 PM', genre: 'Action' }
  //   ]
  // };    // we used when we did hardcoded

  constructor(private route: ActivatedRoute, private router: Router, private movieService : MovieService) {}

  ngOnInit(): void {
    this.resetState();
    this.route.queryParams.subscribe(params => {
      this.selectedLocation = params['location'] || localStorage.getItem('selectedLocation');
      if (!this.selectedLocation) {
        
        this.router.navigate(['/location']);
      } else {
        this.fetchMovies(); 
      }
      // this.fetchMovies();  // featching from backend the images
      
      // this.events = this.eventsMap[this.selectedLocation] || [];  // we used when we did hardcoded
      console.log('Events for selected location:', this.events);   // we used when we did hardcoded
    });
  }


  fetchMovies(): void {                                                             //new method feaching from backend
    this.movieService.getMoviesByLocation(this.selectedLocation).subscribe(
      movies => {
        this.events = movies.map(movie => ({
          title: movie.title,
          image: movie.image,
          time: movie.timing,
          genre: movie.genre
        }));
      },
      error => {
        console.error('Error fetching movies:', error);
        this.events = []; 
      }
    );
  }

  openPopup(movie : Event) {
    this.selectedMovie = movie;
    this.router.navigate(['/theater'],{queryParams : {movieTitle : movie.title , location : this.selectedLocation}})
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
    this.selectedSeats = []; 
    this.seats.fill(false); 
  }

  onSeatSelect(seatCount: number) {
    this.selectedSeatsCount = seatCount;
    this.showPopup = false; 
    this.openSeatSelection(); 
  }

  openSeatSelection() {
   
    this.showPopup = true; 
  }

  toggleSeatSelection(seatIndex: number) {
    const index = this.selectedSeats.indexOf(seatIndex);
    if (index === -1) {
      if (this.selectedSeats.length < this.selectedSeatsCount) {
        this.selectedSeats.push(seatIndex);
        this.seats[seatIndex] = true; 
      }
    } else { 
      this.selectedSeats.splice(index, 1);
      this.seats[seatIndex] = false; 
    }
  }

  calculateTotalPrice(): number {
    return this.selectedSeats.length * this.seatPrice; 
  }

  pay() {
    const total = this.calculateTotalPrice();
    if (total > 0 && this.selectedMovie) {
   
    this.router.navigate(['/payment'],{ queryParams: { count: this.selectedSeats.length ,
      // movieName : this.events[0].title,
      // time : this.events[0].time
      movieName: this.selectedMovie.title, // Use selectedMovie.title
      time: this.selectedMovie.time
    } });
   
      this.closePopup();
    } else {
      alert('Please select seats before proceeding to pay.');
    }
  }

  goBack() {
    this.router.navigate(['/location']);
  }
  resetState() {
    this.selectedSeatsCount = 0;
    this.selectedSeats = [];
    this.seats.fill(false);
    this.showPopup = false;
    this.selectedMovie = null;
  }
}
