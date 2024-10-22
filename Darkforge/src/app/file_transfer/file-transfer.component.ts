import { Component, Input,  } from '@angular/core';
import { FileTransferService } from './file_service/file-transfer.service';
import { Injectable } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormControl,FormsModule,ReactiveFormsModule } from '@angular/forms';
import { style } from '@angular/animations';
import { RpfFormComponent } from "../../forms/rpf-form/rpf-form.component";



@Component({
  selector: 'app-file-transfer',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule, RpfFormComponent],
  templateUrl: './file-transfer.component.html',
  styleUrl: './file-transfer.component.css',

  
})
export class FileTransferComponent {
@Input() public Menu! :string;
private endpoint :string ;


changeFileType(type:string){
  this.endpoint=type;
}


constructor(public transfer:FileTransferService) {
  this.endpoint="";

}


}
