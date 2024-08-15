import { Component, Input,  } from '@angular/core';
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

sendData(): void{
  this.transfer.sendData(this.endpoint,this.file);


}


calculateRGB($event : any): void{

 let r_out = document.getElementById("r_out") as HTMLOutputElement;
 let g_out = document.getElementById("g_out") as HTMLOutputElement;
 let b_out = document.getElementById("b_out") as HTMLOutputElement;

 let r = document.getElementById("r") as HTMLInputElement;
 let g = document.getElementById("g") as HTMLInputElement;
 let b = document.getElementById("b") as HTMLInputElement;


  r_out.value = r.value;
  g_out.value = g.value;
  b_out.value = b.value;

  let colour_out = document.getElementById("colour") as HTMLDivElement;

  colour_out.style.backgroundColor=`rgb(${r_out.value}, ${g_out.value}, ${b_out.value})`;
}


validateNumerical($event:any, $min : number, $max : number): void{


  if($event.target.value<$min){
    $event.target.value=$min;
  }

  else if($event.target.value>$max){
    $event.target.value=$max;
  }


}

postRPF($event : any): void{

this.transfer.post_image();
  
  

}
















}
