import { Component, ElementRef} from '@angular/core';
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
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { alert_loclized } from '../utils/alerts';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RendererComponent, FileTransferComponent, DashboardComponent, TranslatePipe],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers:[FileTransferService, WebGLService]
  
})
export class AppComponent {
  title = 'Darkforge';
  public transfer:FileTransferService;
  
  @ViewChild('file_input') input? : ElementRef<HTMLInputElement>;


  constructor(public translate:TranslateService ,public fileService:FileTransferService, private el:ElementRef) {
    this.transfer=fileService;
    this.translate.addLangs(["en","bh","de"]);
    this.translate.setDefaultLang("en");
  }
 
  public about():void{
    alert_loclized(this.translate,"alerts.about");
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
