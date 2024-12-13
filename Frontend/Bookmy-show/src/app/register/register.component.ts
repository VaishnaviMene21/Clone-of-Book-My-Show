import { Component ,OnInit } from '@angular/core';
import{FormBuilder , FormGroup , Validators} from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
registerForm!: FormGroup;

constructor(private fb: FormBuilder , private authService : AuthService , private router : Router)
{
  this.registerForm = this.fb.group({
    username: ['',Validators.required],
    password: ['',Validators.required]
  })
}

ngOnInit(): void {
  
}

onSubmit()
{
  if(this.registerForm.valid)
  {
    const{username , password} = this.registerForm.value;
    this.authService.login(this.registerForm.value).subscribe(
      response => {
        
        localStorage.setItem('username' ,username);
        this.checkUserLocation(username);
        alert("Login Succesfull")
       

        // this.router.navigate(['/location'])
      },
      error => {
        console.error('Login Failed' ,error)
        alert("Invalid username or password please try again")
      }
    )
  }else
  {
    console.log("form is invalid")
    alert('please fill in all fields')
  }
}

navigatetologin()
{
  this.router.navigate(['/login']);
}
private checkUserLocation(username: string) {
  this.authService.getUserLocation(username).subscribe(
    (res: any) => {
      if (res.location) {
        
        this.router.navigate(['/eventlist'], { queryParams: { location: res.location } });
      } else {
        
        this.router.navigate(['/location']);
      }
    },
    error => {
      console.error('Error fetching user location', error);
      alert('Could not retrieve location. Please select it manually.');
      this.router.navigate(['/location']);
    }
  );
}


}
