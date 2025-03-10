import { Component,OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { base_url, force_reload, log_in, set_http_timeout, user_actions } from '../http';
import { jwtDecode } from 'jwt-decode';
import { animate } from '@angular/animations';
import { set_prefs, UserPreferences } from '../../assets/user_prefs';
import { catchError,of } from 'rxjs';


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

ngOnInit(){

  this.router.navigate(['/'], { replaceUrl: true });
  window.history.pushState(null, '', window.location.href);

}


login(){ 

    console.log(this.username,this.password);
  
  this.http.post<{ token: string }>(`${base_url}${user_actions}${log_in}`,{username:this.username,password:this.password},{observe:"response"}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{

    if($response.status===200){

      const token = $response.body?.token;

      this.username='';
      this.password='';

      if(token!=undefined){

        localStorage.setItem('authToken', token as string); 
  
        const decoded :any= jwtDecode(token);       

        const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

        set_http_timeout(decoded['exp'] as number);

        set_prefs(new UserPreferences(userId));
        
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
