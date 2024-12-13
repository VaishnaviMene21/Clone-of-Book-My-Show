import { Component , OnInit } from '@angular/core';
import { ActivatedRoute,Router } from '@angular/router';

interface Theater
{
  name : string;
  timings : string[];
}

interface Event {
  title: string;
  image: string;
  time: string;
  genre: string;
}

@Component({
  selector: 'app-theater',
  templateUrl: './theater.component.html',
  styleUrls: ['./theater.component.css']
})
export class TheaterComponent implements OnInit {
selectedMovies : string | null = null;
selectedLocation : string | null = null;
theaters : Theater[]=[];
showPopup = false;
selectedSeatsCount: number = 0; 
selectedSeats: number[] = []; 
maxSeats: number = 5;
seatPrice: number = 250;
seats: boolean[] = Array(20).fill(false); 
selectedShowtime: string | null = null;
seatSelectionVisible: boolean = true;


theaterMap: {[key:string]:{movie:string, theaters: Theater[]}[]} ={
  Mumbai:[
    {
         movie : 'Yeh Jawani Hai Deewani',
         theaters :[
          {name : 'Cinepolis : Dadar' ,timings:['10:00 AM','2:00 PM']},
          {name : 'PVR Juhu' ,timings:['12:00' , '4:00 PM']}
         ]  
    },
    {
      movie : 'kuch kuch hota hai',
      theaters:[
        {name : 'Regal Cinema Bandra' , timings:['1:00 PM' , '5:00']},
        {name : 'Inox Vasai' ,timings : ['3:00 PM' , '7:00 PM']}
      ]
    }
  ],
  Pune: [
    {
      movie: 'Bucket List',
      theaters: [
        { name: 'City Pride : Viman Nagar', timings: ['11:00 AM', '3:00 PM'] },
        { name: 'E Square : Karve Nagar', timings: ['1:00 PM', '5:00 PM'] }
      ]
    },
    {
      movie: 'Duniyadari',
      theaters: [
        { name: 'PVR : Shivaji Park', timings: ['12:30 PM', '4:30 PM'] },
        { name: 'K Cinema : PCMC', timings: ['2:30 PM', '6:30 PM'] }
      ]
    },
  ],
  
  Hyderabad: [
    {
      movie: 'Devara',
      theaters: [
        { name: 'INOX : Gachibowli', timings: ['10:00 AM', '1:00 PM'] },
        { name: 'AAA : Ameerpet', timings: ['2:30 PM', '5:30 PM'] }
      ]
    },
    {
      movie: 'Maharshi',
      theaters: [
        { name: 'PVR : Hi-tech City', timings: ['12:00 PM', '3:00 PM'] },
        { name: 'Satyam Cinemas : Madhapur', timings: ['4:30 PM', '7:30 PM'] }
      ]
    },
  ],
  
  Chennai: [
    {
      movie: 'Maharaja',
      theaters: [
        { name: 'Escape Cinemas :Arcod Road', timings: ['11:00 AM', '2:00 PM'] },
        { name: 'PVR : Dr. RK salai', timings: ['1:30 PM', '5:00 PM'] }
      ]
    },
    {
      movie: '24',
      theaters: [
        { name: 'Gaiety Galaxy : RSL ', timings: ['3:00 PM', '6:00 PM'] },
        { name: 'Sathyam Cinemas : Spectrum Mall', timings: ['2:00 PM', '7:00 PM'] }
      ]
    },
  ],
  
  Bangalore: [
    {
      movie: 'Kantara',
      theaters: [
        { name: 'PVR LULU MAll', timings: ['10:30 AM', '1:30 PM'] },
        { name: 'Cinepolis White-Filed', timings: ['3:00 PM', '6:00 PM'] }
      ]
    },
    {
      movie: 'K.G.F',
      theaters: [
        { name: 'INOX : Church Road', timings: ['12:00 PM', '4:00 PM'] },
        { name: 'Cineworld : Bengaluru', timings: ['2:00 PM', '7:00 PM'] }
      ]
    },
  ]
}

constructor(private route : ActivatedRoute , private router : Router){}

ngOnInit(): void {
  this.route.queryParams.subscribe(params =>{
    this.selectedMovies = params['movieTitle'];
    this.selectedLocation = params['location'];
    this.fetchTheaters();
  })
}


fetchTheaters()
{
  if(this.selectedLocation && this.selectedMovies)
  {
    const theaterData = this.theaterMap[this.selectedLocation].find(t => t.movie === this.selectedMovies);
    this.theaters = theaterData ? theaterData.theaters : [];
  }
}
selectShowtime(theater: string, showtime: string) {
  this.selectedShowtime = showtime;
  this.openSeatSelection();
}
openSeatSelection() {
  this.showPopup = true;
  this.resetSeatSelection();
}



selectSeatCount(seatCount: number) {
  if (seatCount <= this.maxSeats) {
    this.selectedSeatsCount = seatCount;
    this.seatSelectionVisible = false; 
  }
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
  if (total > 0 && this.selectedMovies) {
    this.router.navigate(['/payment'], {
      queryParams: {
        count: this.selectedSeats.length,
        movieName: this.selectedMovies,
        time: this.selectedShowtime
      }
    });
    this.closePopup(); // new one added
    localStorage.removeItem('location');
    this.closePopup();
  } else {
    alert('Please select seats before proceeding to pay.');
  }
}
closePopup() {
  this.showPopup = false;
  this.resetSeatSelection();
}

resetSeatSelection() {
  this.selectedSeatsCount = 0;
  this.selectedSeats = [];
  this.seats.fill(false);
  this.seatSelectionVisible = true;
}
}
