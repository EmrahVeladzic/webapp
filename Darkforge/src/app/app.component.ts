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
import { appWidth, setAppDimensions} from '../utils/dynamic_html';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet,ReactiveFormsModule, RendererComponent,FileTransferComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers:[FileTransferService, WebGLService]
})
export class AppComponent {
  title = 'Darkforge';
  public Menu : string = "NONE";
  
  @ViewChild('file_input') input? : ElementRef<HTMLInputElement>;


  constructor(private fileService:FileTransferService, private el:ElementRef) {
    
  }

  @HostListener('window:resize')
  onResize() {
    setAppDimensions(this.el.nativeElement.offsetWidth,this.el.nativeElement.offsetHeight);
  }
  ngAfterViewInit() {
    setAppDimensions(this.el.nativeElement.offsetWidth,this.el.nativeElement.offsetHeight);
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
        
        this.Menu='rpf';
        this.fileService.process_bmp(selected);

      }
      else if(selected.name.endsWith('.wav')){
        
        this.Menu='wl';
        this.fileService.process_wav(selected);
       
      }
      else{
        
      }

    }

  }



  
}
