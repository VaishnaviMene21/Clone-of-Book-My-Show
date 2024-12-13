import { Component ,OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { PaymentService } from '../payment.service';



interface QRCodeOptions {
  errorCorrectionLevel: 'L' | 'M' | 'Q' | 'H';
  width: number;
  // height: number;
}

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {
  selectedLocation: string = '';
  selectedSeatsCount = 0;
  ticketPrice = 250;
  grandTotal = 0;
  discount = 0;
  totalAmount = 0;
  movieName : string = '';
  movieTime : string = '';
  qrData : string = '';
  paymentSuccessful : boolean = false;
  qrCodeOptions: QRCodeOptions = { // Initialize qrCodeOptions here
    errorCorrectionLevel: 'H',
    width: 256,
    // height: 256
  };
  showQRcode : Boolean = false;

  username : string = "";
 
  constructor(private route: ActivatedRoute,private paymentService: PaymentService ,private router : Router) {}

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.selectedSeatsCount = +params['count'];
      this.selectedLocation = params['location']; 
      this.movieName = params['movieName'];
      this.movieTime = params['time']; 
      this.calculateTotal();
    });
    this.username = localStorage.getItem('username') || '';
  }

  calculateTotal() {
    this.grandTotal = this.selectedSeatsCount * this.ticketPrice;
    // const discountAmount = this.discount > 100 ? 100 : this.discount;
    const discountAmount = Math.min(this.discount, this.grandTotal);
    this.totalAmount = this.grandTotal - discountAmount;
  }
  applyDiscount() {
    this.discount = 100; 
    this.calculateTotal(); 
  }

  makePayment() {
    
    this.paymentSuccessful = true;
    this.qrData = `Movie: ${this.movieName}, Seats: ${this.selectedSeatsCount}, Total: ${this.totalAmount} Rs`;
    const currentDateTime = new Date();
    const paymentData = {
      MovieName: this.movieName,
      movieTiming: currentDateTime.toISOString(), 
      AmountPaid: this.totalAmount,
      Username: this.username, 
      SeatsBooked: this.selectedSeatsCount 
    }
    this.paymentService.storePayment(paymentData).subscribe(response => {
      console.log('Payment stored successfully:', response);
      alert(`Payment Successful! Amount: ${this.totalAmount} Rs`);
      this.showQRcode = true;
  }, error => {
      console.error('Error storing payment:', error);
      alert('Payment failed. Please try again.');
  });

    
  }

  goBack() {
    const selectedLocation = localStorage.getItem('selectedLocation');
    this.router.navigate(['/eventlist'], { queryParams: { location: this.selectedLocation } }); 
  }
}
