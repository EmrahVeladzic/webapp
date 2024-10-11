import { Component, Input,  } from '@angular/core';
import { FileTransferService } from './file_service/file-transfer.service';
import { Injectable } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormControl,FormsModule,ReactiveFormsModule } from '@angular/forms';
import { style } from '@angular/animations';
import { toggle_visibility } from '../../utils/dynamic_html';

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


calculateRGB($event : any, $numerical : boolean): void{

 let r_out = document.getElementById("r_out") as HTMLInputElement;
 let g_out = document.getElementById("g_out") as HTMLInputElement;
 let b_out = document.getElementById("b_out") as HTMLInputElement;

 let r = document.getElementById("r") as HTMLInputElement;
 let g = document.getElementById("g") as HTMLInputElement;
 let b = document.getElementById("b") as HTMLInputElement;

 if($numerical){

  if(Number(r_out.value)>255){
    r_out.value = "255";
  }
  else if(Number(r_out.value)<0){
    r_out.value = "0";
  }

  else{
    r_out.value=(Math.round(Number(r_out.value)).toString());
  }


  if(Number(g_out.value)>255){
    g_out.value = "255";
  }
  else if(Number(g_out.value)<0){
    g_out.value = "0";
  }

  else{
    g_out.value=(Math.round(Number(g_out.value)).toString());
  }


  if(Number(b_out.value)>255){
    b_out.value = "255";
  }
  else if(Number(b_out.value)<0){
    b_out.value = "0";
  }

  else{
    g_out.value=(Math.round(Number(g_out.value)).toString());
  }



  

  r.value=r_out.value;
  g.value=g_out.value;
  b.value=b_out.value;

 }

 else{

  if(Number(r.value)>255){
    r.value = "255";
  }
  else if(Number(r.value)<0){
    r.value = "0";
  }

  if(Number(g.value)>255){
    g.value = "255";
  }
  else if(Number(g.value)<0){
    g.value = "0";
  }

  if(Number(b.value)>255){
    b.value = "255";
  }
  else if(Number(b.value)<0){
    b.value = "0";
  }

  r_out.value = r.value;
  g_out.value = g.value;
  b_out.value = b.value;
 }





  let colour_out = document.getElementById("colour") as HTMLDivElement;

  colour_out.style.fill=`rgb(${r_out.value}, ${g_out.value}, ${b_out.value})`;
}


validateNumerical($event:any, $min : number, $max : number): void{


  if($event.target.value<$min){
    $event.target.value=$min;
  }

  else if($event.target.value>$max){
    $event.target.value=$max;
  }


}

additionalParams($event:any):void{

  var $Mode = document.getElementById("Mode") as HTMLSelectElement;
  var $P_BFR = document.getElementById("P_BFR") as HTMLDivElement;


  switch(Number($Mode.value)){

    case 1: {

      toggle_visibility("P_BFR",true);
      

    };break;

    default: {

      toggle_visibility("P_BFR",false);

    }; break;


  }

}

useAlpha($event:any):void{

  var $visible = $event.target.checked;

  toggle_visibility("RGB_OUT",$visible);

}

postRPF($event : any): void{

this.transfer.post_image();
  
  

}



}
