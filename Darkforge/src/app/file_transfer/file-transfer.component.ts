import { Component, Input,  } from '@angular/core';
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
private endpoint :string ;


changeFileType(type:string){
  this.endpoint=type;
}


constructor(public translate: TranslateService) {
  this.endpoint="";

}


}
