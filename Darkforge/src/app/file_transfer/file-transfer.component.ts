import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FileTransferService } from './file_service/file-transfer.service';
import { Injectable } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormControl,FormsModule,ReactiveFormsModule } from '@angular/forms';
import { style } from '@angular/animations';
import { RpfFormComponent } from "../../forms/rpf-form/rpf-form.component";
import { WlFormComponent } from "../../forms/wl-form/wl-form.component";
import { GlbFormComponent } from "../../forms/glb-form/glb-form.component";
import { TranslateService,TranslatePipe, TranslateDirective} from '@ngx-translate/core';
import { PreferenceFormComponent } from '../../forms/preference-form/preference-form.component';
import {tex,sfx,ast} from '../../assets/global_assets';
import { alert_localized } from '../../utils/alerts';
import { base_url, export_actions } from '../http';
import { ExportDTO } from '../../models/models';
import { base64ToUint8Array } from '../../utils/decode_base64';

@Component({
  selector: 'app-file-transfer',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RpfFormComponent, WlFormComponent, GlbFormComponent, PreferenceFormComponent],
  templateUrl: './file-transfer.component.html',
  styleUrl: './file-transfer.component.css',

  
})
export class FileTransferComponent {
@Input() transfer!: FileTransferService;
@Input() public Menu! :string;
@ViewChild('download') download_link?: ElementRef<HTMLAnchorElement>;

public async write_to_local_storage(file: Uint8Array, name: string): Promise<void> {
    return new Promise((resolve) => {
        const blob = new Blob([file], { type: "application/octet-stream" });
        const url = window.URL.createObjectURL(blob);
        
        if (this.download_link?.nativeElement) {
            const link = this.download_link.nativeElement;
            link.href = url;
            link.download = name;
            link.click();
          
            setTimeout(() => {
                window.URL.revokeObjectURL(url);
                resolve();
            }, 1000); 
        } else {
            console.error("Download link is not available", name);
            resolve();
        }
    });
}



public async export_all_available(to_export:ExportDTO,name:string):Promise<void>{

  let ast_buffer = to_export.ast!= null ? base64ToUint8Array(to_export.ast) : null;
  let rpf_buffer = to_export.rpf!= null ? base64ToUint8Array(to_export.rpf) : null;
  let wl_buffer = to_export.wl!= null ? base64ToUint8Array(to_export.wl) : null;


  let count = 0;

  if(ast_buffer!=null){
    count++;
  }
  if(rpf_buffer!=null){
    count++;
  }

  if(wl_buffer!=null){
    count++;
  }

  if(count===1){

    if(ast_buffer!=null){

      await this.write_to_local_storage(ast_buffer,(name+'.AST'))

    }

    else if(rpf_buffer!=null){

      await this.write_to_local_storage(rpf_buffer,(name+'.RPF'))

    }

    else{

      await this.write_to_local_storage(wl_buffer!,(name+'.WL'))

    }

  }

  else{
    
  }
  
}

public async export_files():Promise<void>{

  const r_id:number|null = tex.id;
  const w_id:number|null = sfx.id;
  const a_id:number|null = ast.id;

  let name: string = "";

  if(this.transfer.model_name!="" && a_id!=0){
    name=this.transfer.model_name;
  }
  else if(this.transfer.image_name!="" && r_id!=0){
    name=this.transfer.image_name;
  }
  else if(this.transfer.sound_name!="" && w_id!=0){
    name=this.transfer.sound_name;
  }

  if(name!=""){

    name = name.toUpperCase().slice(0,name.length-4).slice(0,8).replace(' ','_');

    const full_url = `${base_url}${export_actions}`;

    const export_strings = await this.transfer.export_get(a_id,r_id,w_id,full_url) as ExportDTO;
    
    await this.export_all_available(export_strings,name);

    alert_localized(this.translate,'alerts.export-done');

  }

  else{
   
    alert_localized(this.translate,'alerts.export-null');



  }


}

constructor(public translate: TranslateService) {
 

}


}
