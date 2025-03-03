import { Component, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { RendererComponent } from './renderer/renderer.component';
import { ViewChild } from '@angular/core';
import { HostListener } from '@angular/core';
import { FileTransferComponent } from './file_transfer/file-transfer.component';
import { FileTransferService } from './file_transfer/file_service/file-transfer.service';
import { WebGLService } from './renderer/webgl_service/web-gl.service';
import { ReactiveFormsModule } from '@angular/forms';
import { Subject, Subscription } from 'rxjs';
import { DashboardComponent } from "../forms/dashboard/dashboard.component";


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RendererComponent, FileTransferComponent, DashboardComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers:[FileTransferService, WebGLService]
})
export class AppComponent {
  title = 'Darkforge';
  public transfer:FileTransferService;
  
  @ViewChild('file_input') input? : ElementRef<HTMLInputElement>;


  constructor(public fileService:FileTransferService, private el:ElementRef) {
    this.transfer=fileService;
  }
 
  upload_click():void{

    if(this.input){
    this.input.nativeElement.click();
    }
    
  }
  
  file_logic($event:any):void{

   

    const selected:File = $event.target.files[0];

    if(selected!=null){
      if(selected.name.endsWith('.bmp')){      
        this.fileService.process_bmp(selected);
      }
      else if(selected.name.endsWith('.wav')){              
        this.fileService.process_wav(selected);        
      }
      else if(selected.name.endsWith('.glb')){              
        this.fileService.process_glb(selected,selected.name);        
      }

      else{
        
      }

    }

  }



  
}
