import { Component ,OnInit} from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;

    constructor(private fb: FormBuilder, private authService: AuthService , private router: Router ) {
      this.loginForm = this.fb.group({
        username: ['', Validators.required],
        password: [
            '',
            [
                Validators.required,
                Validators.pattern(/^(?=.*[0-9])(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$/) 
            ]
        ]
    });
     }

    ngOnInit(): void {
      
    }

    onSubmit() {
        if (this.loginForm.valid) {
            const { Username, Password } = this.loginForm.value;
            this.authService.register(this.loginForm.value).subscribe(
                response => {
                    
                    localStorage.setItem('username' , this.loginForm.value.username);
                    alert('Registration successful');
                    this.router.navigate(['/register'])
                },
                error => {
                    console.error('Registration failed', error);
                    alert("username and password already exist")
                }
            );
        } else {
            console.error('Form is invalid');
        }
    }
}
