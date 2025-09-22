import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, FormControlName } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { base_url, force_reload, log_in, set_http_timeout, user_actions } from '../http';
import { jwtDecode } from 'jwt-decode';
import { animate } from '@angular/animations';
import { set_prefs, UserPreferences } from '../../assets/user_prefs';
import { catchError,of } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
public form:FormGroup;


constructor(private router:Router, private http:HttpClient){
this.form = new FormGroup({
  username:new FormControl("",Validators.required),
  password:new FormControl("",Validators.required)

});
}

ngOnInit(){

  this.router.navigate(['/'], { replaceUrl: true });
  window.history.pushState(null, '', window.location.href);

}


login(){ 

  let username =this.form.get('username')?.value;
  let password =this.form.get('password')?.value;
  
  this.http.post<{ token: string }>(`${base_url}${user_actions}${log_in}`,{username:username,password:password},{observe:"response"}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{

    if($response.status===200){

      const token = $response.body?.token;
      
      if(token!=undefined){

        localStorage.setItem('DarkforgeAuthToken', token as string); 
  
        const decoded :any= jwtDecode(token);       

        const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        set_prefs(new UserPreferences(userId));

        set_http_timeout(decoded['exp']);
        
        this.router.navigate([`/user/${userId}`]);

      }
     
    
      
    }
   
    else{
      alert($response.status);
      force_reload();
    }
    
  });
  
    
}


}
