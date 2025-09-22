import { Component, ElementRef, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterOutlet } from '@angular/router';
import { RendererComponent } from '../renderer/renderer.component';
import { ViewChild } from '@angular/core';
import { HostListener } from '@angular/core';
import { FileTransferComponent } from '../file_transfer/file-transfer.component';
import { FileTransferService } from '../file_transfer/file_service/file-transfer.service';
import { WebGLService } from '../renderer/webgl_service/web-gl.service';
import { ReactiveFormsModule} from '@angular/forms';
import { Subject, Subscription, catchError, of } from 'rxjs';
import { DashboardComponent } from '../../forms/dashboard/dashboard.component';
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { base_url,pref_actions } from '../http';
import { alert_localized } from '../../utils/alerts';
import { emit_prefs_change, set_prefs, user_prefs, UserPreferences} from '../../assets/user_prefs';
import { force_reload, http_timeout, token_valid } from '../http';
import { HttpParams } from '@angular/common/http';
import { get_headers } from '../../utils/httpheaders';
import { ActivatedRoute } from '@angular/router';

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


  constructor(public translate:TranslateService ,public fileService:FileTransferService, private el:ElementRef, private router:Router, private route :ActivatedRoute) {
    this.transfer=fileService;
    this.translate.addLangs(["en","bh","de"]);
    this.translate.setDefaultLang("en");
  }

  async begin_export():Promise<void>{

    this.export_button.nativeElement.disabled=true;


    await this.fileTransferComponent.export_files();


    this.export_button.nativeElement.disabled=false;
  }
  

  ngOnInit(){

    let full_url =`${base_url}${pref_actions}`;  

    this.transfer.http.get<UserPreferences>(full_url,{headers:get_headers(),observe:"response",params:new HttpParams().set('id',user_prefs.userId)}).pipe(catchError($error=>{return of($error)})).subscribe($response=>{

      if($response.status>=400){

        if($response.status===401){
          alert_localized(this.translate,'alerts.timeout');
        }
        else{
          alert_localized(this.translate,'alerts.server_error');
        }
        
        force_reload();
      }

      else{      

      if($response.status===200){
      
        set_prefs($response.body as UserPreferences);

        this.translate.use(user_prefs.language);

        emit_prefs_change();        

      }

    }


    });

    const tokenInterval = setInterval(() => {
      
      const valid = token_valid();

      if(!valid){
        clearInterval(tokenInterval);
        alert_localized(this.translate,'alerts.timeout');
        force_reload();
      }      

    },5000);

  }
 
  public about():void{
    alert_localized(this.translate,"alerts.about");
  }

  public prefs():void{
    this.transfer.Menu="pref";
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
