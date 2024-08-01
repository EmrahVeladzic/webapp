import { Component,  } from '@angular/core';
import { FileTransferService } from './file_service/file-transfer.service';
import { Injectable } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormControl,FormsModule,ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-file-transfer',
  standalone: true,
  imports: [HttpClientModule, ReactiveFormsModule],
  templateUrl: './file-transfer.component.html',
  styleUrl: './file-transfer.component.css',

  
})
export class FileTransferComponent {
private endpoint :string ;
private file:any;



changeFileType(type:string){
this.endpoint=type;
}


constructor(private transfer:FileTransferService) {
  this.endpoint="";

}

sendData(){
this.transfer.sendData(this.endpoint,this.file);


}


select_changed(){
  if(this.transfer){
    this.transfer.calculate_chunks();
  }
}






















}
