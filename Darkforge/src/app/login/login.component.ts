import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { base_url, user_actions } from '../http';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

public username:string='';
public password:string='';

constructor(private router:Router, private http:HttpClient){

}

login(){ 

  /*
  this.http.get(base_url+user_actions,{observe:"response"}).subscribe($response=>{

    if($response.status===200){
      this.router.navigate([`/user/${200}`]);
    
    }
    else{
      this.router.navigate([`/user/${500}`]);
    
    }
    
  });
  */

  this.router.navigate([`/user/${1}`]);
    
}


}
