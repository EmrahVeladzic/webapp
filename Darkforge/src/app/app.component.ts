import { Component, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { FileTransferService } from './file_transfer/file_service/file-transfer.service';
import { WebGLService } from './renderer/webgl_service/web-gl.service';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


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
    
    if (window.location.pathname !== '/login') {
      this.router.navigateByUrl('/login');
      
    }
  }
 
  
}
