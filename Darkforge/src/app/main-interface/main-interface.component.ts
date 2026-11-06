import { Component, ElementRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RendererComponent } from '../renderer/renderer.component';
import { ViewChild } from '@angular/core';
import { HostListener } from '@angular/core';
import { FileTransferComponent } from '../file_transfer/file-transfer.component';
import { FileTransferService } from '../file_transfer/file_service/file-transfer.service';
import { WebGLService } from '../renderer/webgl_service/web-gl.service';
import { ReactiveFormsModule} from '@angular/forms';
import { DashboardComponent } from '../../forms/dashboard/dashboard.component';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { alert_localized } from '../../utils/alerts';

@Component({
  selector: 'app-main-interface',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RendererComponent, FileTransferComponent, DashboardComponent, TranslatePipe],
  templateUrl: './main-interface.component.html',
  styleUrl: './main-interface.component.css',
  providers:[WebGLService,FileTransferService]
})
export class MainInterfaceComponent {

  public transfer:FileTransferService;
  @ViewChild('export') export_button!: ElementRef<HTMLButtonElement>;
  @ViewChild('file_input') input? : ElementRef<HTMLInputElement>;
  @ViewChild(FileTransferComponent) fileTransferComponent!: FileTransferComponent;


  constructor(public translate:TranslateService ,public fileService:FileTransferService, private el:ElementRef) {
    this.transfer=fileService;
    this.translate.addLangs(["en"]);
    this.translate.setDefaultLang("en");
    this.translate.use("en");
  }

  async begin_export():Promise<void>{

    this.export_button.nativeElement.disabled=true;


    await this.fileTransferComponent.export_files();


    this.export_button.nativeElement.disabled=false;
  }
  

  public about():void{
    alert_localized(this.translate,"alerts.about");
  }

  clear():void{
    this.transfer.Menu="NONE";
  }

  rst():void{
    this.transfer.reset();
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
        this.fileService.process_bmp(selected,selected.name);
      }
      else if(selected.name.endsWith('.wav')){              
        this.fileService.process_wav(selected,selected.name);        
      }
      else if(selected.name.endsWith('.glb')){              
        this.fileService.process_glb(selected,selected.name);        
      }

      else{
        
      }

    }

  }


}