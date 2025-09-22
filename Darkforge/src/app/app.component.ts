import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { FileTransferService } from './file_transfer/file_service/file-transfer.service';
import { WebGLService } from './renderer/webgl_service/web-gl.service';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserPreferences, set_prefs } from '../assets/user_prefs';
import { jwtDecode } from 'jwt-decode';
import { set_http_timeout } from './http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,RouterOutlet, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  
})
export class AppComponent {
  title = 'Darkforge';
  
  constructor(private router:Router){
  

  }

  ngOnInit(){ 

    const token = localStorage.getItem('DarkforgeAuthToken');

    if(token!=undefined){

      const decoded :any= jwtDecode(token);       
      
      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

      set_prefs(new UserPreferences(userId));

      set_http_timeout(decoded['exp']);
        
      this.router.navigate([`/user/${userId}`]);


    }

    else if (window.location.pathname !== '/login'){
  
      this.router.navigateByUrl('/login');
      
    

    }
  }
 
  
}
